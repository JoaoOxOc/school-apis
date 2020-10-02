using Microsoft.Extensions.Configuration;

namespace SingleSignonPage.Util
{
    public static class Users
    {
        public static string GetUser(IConfiguration configuration)
        {
            return configuration.GetValue<string>("ApplicationSettings:DefaultUser") ?? "joao";
        }
        public static string GetPassword(IConfiguration configuration)
        {
            return configuration.GetValue<string>("ApplicationSettings:DefaultPass") ?? "Pa$$word123";
        }
        public static string GetEmail(IConfiguration configuration)
        {
            return configuration.GetValue<string>("ApplicationSettings:DefaultEmail") ?? "joao.almeida.viseu@gmail.com";
        }
    }
}
