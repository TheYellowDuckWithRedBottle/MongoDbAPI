using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
