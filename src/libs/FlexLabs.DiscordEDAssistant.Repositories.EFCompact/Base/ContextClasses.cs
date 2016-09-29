using System;
using System.ComponentModel.DataAnnotations;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCompact.Base
{
    public class Server
    {
        [Key]
        public long ID { get; set; }
        public long ServerID { get; set; }
        public string CommandPrefix { get; set; }
    }
}
