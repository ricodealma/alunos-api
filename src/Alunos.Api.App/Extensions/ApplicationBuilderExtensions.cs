using Newtonsoft.Json;
using Alunos.Api.Domain.SeedWork;

namespace Alunos.Api.App.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static void FillEnvironmentKeys(EnvironmentKey environmentKey, IConfiguration configuration)
        {
            environmentKey.AppInformation.HeaderKey =
                EnvironmentKey.GetVariable<string>(Constant.HEADER_TOKEN, configuration);

            environmentKey.PostgresInformation.Server =
                EnvironmentKey.GetVariable<string>(Constant.SQL_SERVER, configuration);

            environmentKey.PostgresInformation.DataBase =
                EnvironmentKey.GetVariable<string>(Constant.SQL_DATABASE, configuration);

            environmentKey.PostgresInformation.UserId =
                EnvironmentKey.GetVariable<string>(Constant.SQL_USER, configuration);

            environmentKey.PostgresInformation.Password =
                EnvironmentKey.GetVariable<string>(Constant.SQL_PASSWORD, configuration);
        }

        public static void FillEnvironmentVariables(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            EnvironmentKey environmentKey = applicationBuilder.ApplicationServices.GetRequiredService<EnvironmentKey>();
            FillEnvironmentKeys(environmentKey, configuration);
            ValidateConfigurationBeforeStart(environmentKey, applicationBuilder.ApplicationServices);
        }

        private static void ValidateConfigurationBeforeStart(EnvironmentKey environmentKey, IServiceProvider serviceProvider)
        {
            if (!environmentKey.IsValid())
                throw new Exception(JsonConvert.SerializeObject(new { ErrorMessage = "Some environment variables are not configured", DetailedError = environmentKey }));
        }
    }
}
