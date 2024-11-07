
using ElectronNET.API;
using Mailgram.Server.Extensions;

namespace Mailgram.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
            
        builder.Services.AddControllers();
            
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddApplicationServices();
        builder.Services.AddRepositories();
            
        builder.WebHost.UseElectron(args);
        builder.Services.AddElectron();
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin", builder => // NOT RECOMMENDED FOR PRODUCTION
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
        
        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();
            
        app.MapControllers();

        app.MapFallbackToFile("/index.html");
        
        app.UseCors("AllowAnyOrigin");
        
        if (HybridSupport.IsElectronActive)
        {
            CreateElectronWindow();
        }

        app.Run();
    }
        
    static async void CreateElectronWindow()
    {
        var window = await Electron.WindowManager.CreateWindowAsync();
        window.SetAutoHideMenuBar(true);
        window.OnClosed += () => Electron.App.Quit();
    }
}
