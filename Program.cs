using Microsoft.Owin.Cors;
using Owin;
using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;

namespace SignalR_WithoutIIS_sample
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:5001";
            using (WebApp.Start<Startup>(url))
            {
                Console.Read();
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
            string exeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string webFolder = Path.Combine(exeFolder, "Web");
            app.UseFileServer(new FileServerOptions()
            {
                RequestPath = PathString.Empty,
                FileSystem = new PhysicalFileSystem(webFolder),
                EnableDirectoryBrowsing = false
            });
        }
    }

    public class MyHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }
    }
}