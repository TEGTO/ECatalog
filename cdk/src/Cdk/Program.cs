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

            var esbStack = new EBStack(app, "ECatalogStack", new EBEnvProps
            {
                Env = new Amazon.CDK.Environment
                {
                    Account = "982081081153",
                    Region = "eu-central-1"
                },
                AppName = "eb-productapi-eucentral1-001",
                EnvName = "Development",
                Path = "../src/ECatalog.Backend/ProductApi/publish",
                EnvironmentProperties = new List<CfnEnvironment.OptionSettingProperty>()
                {
                     new CfnEnvironment.OptionSettingProperty
                     {
                         OptionName = "ASPNETCORE_ENVIRONMENT",
                         Value = "Development"
                     },
                     new CfnEnvironment.OptionSettingProperty
                     {
                         OptionName = "AllowedCORSOrigins",
                         Value = ""
                     },
                    new CfnEnvironment.OptionSettingProperty
                    {
                        OptionName = "UseCORS",
                        Value = "true"
                    },
                    new CfnEnvironment.OptionSettingProperty
                    {
                        OptionName = "EFCreateDatabase",
                        Value = "true"
                    },
                    new CfnEnvironment.OptionSettingProperty
                    {
                        OptionName = "ConnectionStrings__Db",
                        Value = ""
                    },
                }
                //System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"),
            });

            app.Synth();
        }
    }
}
