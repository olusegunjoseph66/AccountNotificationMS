using Notifications.Application.Interfaces.Messaging;

namespace Notifications.API.Extensions
{
    public static class ApplicaionBuilderExtension
    {
        public static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }

        public static IServiceScopeFactory ScopeFactory { get; set; }

        public static IApplicationBuilder UseAzureServiceBusConsume(this IApplicationBuilder app)
        {
            ScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
            using var scope = ScopeFactory.CreateScope();

            ServiceBusConsumer = scope.ServiceProvider.GetRequiredService<IAzureServiceBusConsumer>();
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLife.ApplicationStarted.Register(OnStart);

            return app;
        }

        private static void OnStop()
        {
        }

        private static void OnStart()
        {
            ServiceBusConsumer.StartAccountMsg();
        }
    }
}
