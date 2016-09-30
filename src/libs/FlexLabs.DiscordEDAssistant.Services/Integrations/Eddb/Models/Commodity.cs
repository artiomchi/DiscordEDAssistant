namespace FlexLabs.DiscordEDAssistant.Services.Integrations.Eddb.Models
{
    public class Commodity
    {
        public int id { get; set; }
        public string name { get; set; }
        public int category_id { get; set; }
        public int average_price { get; set; }
        public byte is_rare { get; set; }
        public CommodityCategory category { get; set; }

        public class CommodityCategory
        {
            public int id { get; set; }
            public string name { get; set; }
        }
    }
}
