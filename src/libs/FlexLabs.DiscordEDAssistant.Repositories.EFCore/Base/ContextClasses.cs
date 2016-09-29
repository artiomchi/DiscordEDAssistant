using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base
{
    public class Server
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }
        [StringLength(5)]
        public string CommandPrefix { get; set; }
        [MaxLength]
        public string WelcomeMessage { get; set; }
    }
}
