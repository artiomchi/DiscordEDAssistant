using System;

namespace FlexLabs.EDAssistant.Models
{
    public class Settings
    {
        public static Settings Instance { get; private set; }
        public Settings()
        {
            if (Instance != null) throw new InvalidOperationException();
            Instance = this;
        }

        public BotFrameworkSettings BotFramework { get; set; }
        public DiscordSettings Discord { get; set; }
        public InaraSettings Inara { get; set; }
        public LuisSettings Luis { get; set; }

        public class BotFrameworkSettings
        {
            public string BotId { get; set; }
            public string MicrosoftAppId { get; set; }
            public string MicrosoftAppPassword { get; set; }
        }

        public class DiscordSettings
        {
            public string ClientID { get; set; }
            public string AuthToken { get; set; }
        }

        public class InaraSettings
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }

        public class LuisSettings
        {
            public string AppId { get; set; }
            public string SubscriptionKey { get; set; }
        }
    }
}
