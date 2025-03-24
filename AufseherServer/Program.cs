namespace AufseherServer
{
    public class Program
    {

        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
#if DEBUG
                    config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
#else
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
#endif

                    config.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:80", "https://*:443");
                    webBuilder.UseStartup<Startup>();
                });
    }
}