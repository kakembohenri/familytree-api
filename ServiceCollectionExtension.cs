using familytree_api.Events.Emails;
using familytree_api.Events;
using familytree_api.Workers.Emails;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using familytree_api.Services.Email;
using familytree_api.Dtos.Emails;

namespace familytree_api
{
    public static class ServiceCollectionExtension
    {
        public static void AddServicesFromAssembly(this IServiceCollection services, Assembly assembly)
        {

            var types = assembly.GetTypes()
    .Where(t =>
        t.IsClass &&
        !t.IsAbstract &&
        !t.IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false)) // 👈 FILTER HERE
    .Select(t => new
    {
        Implementation = t,
        Interface = t.GetInterfaces().FirstOrDefault()
    })
    .Where(t => t.Interface != null && !t.Interface.Name.Contains("IAsyncStateMachine")); // 👈 EXTRA SAFETY

            foreach (var type in types)
            {
                if (type.Implementation == typeof(EmailQueue) || type.Implementation == typeof(EmailWorker))
                {
                    // Handle special cases and add them as Singleton or Hosted Service
                    services.AddSingleton(type.Interface, type.Implementation);
                }
                else
                {
                    // Add all other classes as Scoped services
                    services.AddScoped(type.Interface, type.Implementation);
                }
            }

            // Explicitly register the specific services as Singleton or Hosted Service
            services.AddSingleton<IEmailQueue, EmailQueue>();
            //services.AddSingleton<IEmailServices, EmailServices>();
            services.AddScoped<IEmailServices, EmailServices>();
            services.AddHostedService<EmailWorker>();

            services.AddScoped<IEventDispatcher, EventDispatcher>();

            // Re register services that implement IEventHandler<EmailMessage>
            services.RemoveAll<IEventHandler<EmailMessage>>();
            services.AddScoped<IEventHandler<EmailMessage>, EmailVerificationEvent>();

            services.RemoveAll<IEventHandler<PasswordReset>>();
            services.AddScoped<IEventHandler<PasswordReset>, PasswordResetEvent>();

        }

    }
}
