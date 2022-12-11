using MassTransit;

namespace Signals.App.Extensions
{
    public static class MassTransitExtensions
    {
        public static void SetNetEndpointNameFormatter(this IBusRegistrationConfigurator config)
        {
            config.SetEndpointNameFormatter(new NetEndpointNameFormatter());
        }
    }

    public class NetEndpointNameFormatter : IEndpointNameFormatter
    {
        public string Separator => "-";

        public string Message<T>() where T : class
        {
            var type = typeof(T);

            return type.FullName
                .Remove(type.Namespace.Length, 1)
                .Insert(type.Namespace.Length, ":")
                .Replace("+", Separator);
        }

        public string Consumer<T>() where T : class, IConsumer
        {
            var type = typeof(T);

            return type.FullName
                .Remove(type.Namespace.Length, 1)
                .Insert(type.Namespace.Length, ":")
                .Replace("+", Separator);
        }

        public string SanitizeName(string name)
        {
            return name;
        }

        public string TemporaryEndpoint(string tag)
        {
            throw new NotImplementedException();
        }

        string IEndpointNameFormatter.CompensateActivity<T, TLog>()
        {
            throw new NotImplementedException();
        }

        string IEndpointNameFormatter.ExecuteActivity<T, TArguments>()
        {
            throw new NotImplementedException();
        }

        string IEndpointNameFormatter.Saga<T>()
        {
            throw new NotImplementedException();
        }
    }
}
