using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MongoDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Aspose.Cells.License cellLicense = new Aspose.Cells.License();
            try
            {
                cellLicense.SetLicense("License.lic");
                Console.WriteLine("Licensee set success");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            CreateBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
