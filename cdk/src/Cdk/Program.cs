using Amazon.CDK;
using Amazon.CDK.AWS.ElasticBeanstalk;
using System.Collections.Generic;

namespace Cdk
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();

            var esbStack = new EBStack(app, "ECatalogProductApiStack", new EBEnvProps
            {
                Env = new Amazon.CDK.Environment
                {
                    Account = System.Environment.GetEnvironmentVariable("AWS_ACCOUNT_ID"),
                    Region = System.Environment.GetEnvironmentVariable("AWS_REGION"),
                },
                AppName = System.Environment.GetEnvironmentVariable("APPNAME"),
                EnvName = System.Environment.GetEnvironmentVariable("ENVNAME"),
                Path = System.Environment.GetEnvironmentVariable("PUBLISH_PATH"),
                EnvironmentProperties = new List<CfnEnvironment.OptionSettingProperty>()
                {
                     new CfnEnvironment.OptionSettingProperty
                     {
                         OptionName = "ASPNETCORE_ENVIRONMENT",
                         Value = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                     },
                     new CfnEnvironment.OptionSettingProperty
                     {
                         OptionName = "AllowedCORSOrigins",
                         Value = System.Environment.GetEnvironmentVariable("AllowedCORSOrigins")
                     },
                    new CfnEnvironment.OptionSettingProperty
                    {
                        OptionName = "UseCORS",
                        Value = System.Environment.GetEnvironmentVariable("UseCORS")
                    },
                    new CfnEnvironment.OptionSettingProperty
                    {
                        OptionName = "EFCreateDatabase",
                        Value = System.Environment.GetEnvironmentVariable("EFCreateDatabase")
                    },
                    new CfnEnvironment.OptionSettingProperty
                    {
                        OptionName = "ConnectionStrings__Db",
                        Value = System.Environment.GetEnvironmentVariable("ConnectionStrings__Db")
                    },
                }
            });

            app.Synth();
        }
    }
}