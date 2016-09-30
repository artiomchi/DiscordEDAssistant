namespace FlexLabs.DiscordEDAssistant.Services.Integrations.Eddb.Models
{
    public class Module
    {
        public int id { get; set; }
        public int? group_id { get; set; }
        public int @class { get; set; }
        public char? rating { get; set; }
        public int? price { get; set; }
        public string weapon_mode { get; set; }
        public string missile_type { get; set; }
        public string name { get; set; }
        public string belongs_to { get; set; }
        public int ed_id { get; set; }
        public string ed_symbol { get; set; }
        public string ship { get; set; }
        public ModuleGroup group { get; set; }

        public class ModuleGroup
        {
            public int id { get; set; }
            public int category_id { get; set; }
            public string name { get; set; }
            public string category { get; set; }
        }
    }
}
