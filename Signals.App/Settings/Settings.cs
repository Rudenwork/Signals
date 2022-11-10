namespace Signals.App.Settings
{
    public class Settings
    {
        public IdentitySettings Identity { get; set; }

        public class IdentitySettings
        {
            public string Authority { get; set; }
        }
    }
}
