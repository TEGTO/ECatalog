using Amazon.CDK;
using Amazon.CDK.AWS.ElasticBeanstalk;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.S3.Assets;
using Constructs;
using System.Collections.Generic;

namespace Cdk
{
    public class EBEnvProps : StackProps
    {
        public string MinSize { get; set; } = "1";
        public string MaxSize { get; set; } = "1";
        public string InstanceTypes { get; set; } = "t2.micro";
        public string SolutionStackName { get; set; } = "64bit Amazon Linux 2023 v3.2.2 running .NET 8";
        public required string AppName { get; set; }
        public required string EnvName { get; set; }
        public required string Path { get; set; }
        public List<CfnEnvironment.OptionSettingProperty> EnvironmentProperties { get; set; } = new List<CfnEnvironment.OptionSettingProperty>();
    }

    public class EBStack : Stack
    {
        public EBStack(Construct scope, string id, EBEnvProps props) : base(scope, id, props)
        {
            // Create an S3 asset from the source directory
            var webAppZipArchive = new Asset(this, "WebAppZip", new AssetProps
            {
                Path = props.Path
            });

            // Create an Elastic Beanstalk application
            var app = new CfnApplication(this, "Application", new CfnApplicationProps
            {
                ApplicationName = props.AppName
            });

            // Create an application version linked to the S3 asset
            var appVersionProps = new CfnApplicationVersion(this, "AppVersion", new CfnApplicationVersionProps
            {
                ApplicationName = props.AppName,
                SourceBundle = new CfnApplicationVersion.SourceBundleProperty
                {
                    S3Bucket = webAppZipArchive.S3BucketName,
                    S3Key = webAppZipArchive.S3ObjectKey
                }
            });

            // Ensure the application exists before the application version is created
            appVersionProps.AddDependency(app);

            // Create an IAM role for Elastic Beanstalk instances
            var myRole = new Role(this, $"{props.AppName}-aws-elasticbeanstalk-ec2-role", new RoleProps
            {
                AssumedBy = new ServicePrincipal("ec2.amazonaws.com")
            });

            myRole.AddManagedPolicy(ManagedPolicy.FromAwsManagedPolicyName("AWSElasticBeanstalkWebTier"));

            // Create an instance profile for the Elastic Beanstalk environment
            var myProfileName = $"{props.AppName}-InstanceProfile";
            var instanceProfile = new CfnInstanceProfile(this, myProfileName, new CfnInstanceProfileProps
            {
                InstanceProfileName = myProfileName,
                Roles = [myRole.RoleName]
            });

            // Configure Elastic Beanstalk environment options
            var optionSettingProperties = new List<CfnEnvironment.OptionSettingProperty>
            {
                new CfnEnvironment.OptionSettingProperty
                {
                    Namespace = "aws:autoscaling:launchconfiguration",
                    OptionName = "IamInstanceProfile",
                    Value = myProfileName
                },
                new CfnEnvironment.OptionSettingProperty
                {
                    Namespace = "aws:autoscaling:asg",
                    OptionName = "MinSize",
                    Value = props.MinSize
                },
                new CfnEnvironment.OptionSettingProperty
                {
                    Namespace = "aws:autoscaling:asg",
                    OptionName = "MaxSize",
                    Value = props.MaxSize
                },
                new CfnEnvironment.OptionSettingProperty
                {
                    Namespace = "aws:ec2:instances",
                    OptionName = "InstanceTypes",
                    Value = props.InstanceTypes
                }
            };

            foreach (var envProp in props.EnvironmentProperties)
            {
                optionSettingProperties.Add(new CfnEnvironment.OptionSettingProperty
                {
                    Namespace = "aws:elasticbeanstalk:application:environment",
                    OptionName = envProp.OptionName,
                    Value = envProp.Value
                });
            }

            // Create an Elastic Beanstalk environment
            var elbEnv = new CfnEnvironment(this, "Environment", new CfnEnvironmentProps
            {
                EnvironmentName = props.EnvName,
                ApplicationName = app.ApplicationName,
                SolutionStackName = props.SolutionStackName,
                OptionSettings = optionSettingProperties.ToArray(),
                VersionLabel = appVersionProps.Ref,
            });
        }
    }
}
