using Mapster;

namespace Signals.App.Extensions
{
    public static class MapsterExtensions
    {
        public static IServiceCollection AddMapster(this IServiceCollection serviceCollection, Action<TypeAdapterSettings> configureAction)
        {
            configureAction?.Invoke(TypeAdapterConfig.GlobalSettings.Default.Settings);

            return serviceCollection;
        }
    }
}
