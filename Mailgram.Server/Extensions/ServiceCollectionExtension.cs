using Mailgram.Server.Repositories;
using Mailgram.Server.Repositories.Interfaces;
using Mailgram.Server.Services;
using Mailgram.Server.Services.Interface;
using Mailgram.Server.Services.Interfaces;

namespace Mailgram.Server.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddSingleton<IMessagesRepository, MessagesRepository>()
            .AddSingleton<IAccountsRepository, AccountsRepository>();
    }
    
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<IEmailService, EmailService>();
    }
}