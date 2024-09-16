
using ElectronNET.API;

namespace Mailgram.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.WebHost.UseElectron(args);
            builder.Services.AddElectron();
            
            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {   
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            
            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            if (HybridSupport.IsElectronActive)
                CreateElectronWindow();
            
            app.Run();
        }
        
        static async void CreateElectronWindow()
        {
            BrowserWindow window = await Electron.WindowManager.CreateWindowAsync();
            window.OnClosed += () => Electron.App.Quit();
        }
    }
}
