using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlexLabs.DiscordEDAssistant.Models
{
    public class ServerPermission
    {
        public int ID { get; set; }
        public long ServerID { get; set; }

        public Server Server { get; set; }
    }
}
