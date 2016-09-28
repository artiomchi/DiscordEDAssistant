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

        public void Start()
        {
            var heartBeatUrl = ConfigurationManager.AppSettings["System.Heartbeat.Url"];
            if (heartBeatUrl != null)
                Task.Factory.StartNew(() => MaintainHeartbeat(
                    heartBeatUrl,
                    ConfigurationManager.AppSettings["System.Heartbeat.Login"],
                    ConfigurationManager.AppSettings["System.Heartbeat.Password"]));

            var botToken = ConfigurationManager.AppSettings["Discord.Bot.Token"];
            var bot = new Bot();
            bot.Start(botToken); // this will never finish
            Console.ReadLine();
        }

        public void MaintainHeartbeat(String url, String login, String password)
        {
            while (true)
            {
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(login, password);
                    var result = client.DownloadString(url);
                }

                Thread.Sleep(Convert.ToInt32(TimeSpan.FromMinutes(5).TotalMilliseconds));
            }
        }
    }
}
