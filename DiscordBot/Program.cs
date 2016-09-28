using Discord;
using System;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args) => new Program().Start();

        private DiscordClient _client;
        private String _welcomeMessage = null;

        public void Start()
        {
            var heartBeatUrl = ConfigurationManager.AppSettings["System.HearbeatUrl"];
            if (heartBeatUrl != null)
                Task.Factory.StartNew(MaintainHeartbeat, heartBeatUrl);

            var botToken = ConfigurationManager.AppSettings["Discord.Bot.Token"];
            var bot = new Bot();
            bot.Start(botToken); // this will never finish
        }

        public void MaintainHeartbeat(Object state)
        {
            var url = state as String;
            if (url == null)
                return;

            while (true)
            {
                using (var client = new WebClient())
                {
                    client.DownloadString(url);
                }

                Thread.Sleep(Convert.ToInt32(TimeSpan.FromMinutes(5).TotalMilliseconds));
            }
        }
    }
}
