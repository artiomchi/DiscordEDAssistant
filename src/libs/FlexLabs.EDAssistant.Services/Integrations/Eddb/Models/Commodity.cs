namespace FlexLabs.EDAssistant.Services.Integrations.Eddb.Models
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

        public EDAssistant.Models.External.Eddb.Commodity Translate()
            => new EDAssistant.Models.External.Eddb.Commodity
            {
                ID = id,
                Name = name,
                AveragePrice = average_price,
                IsRare = is_rare == 1,
                CategoryID = category.id,
                CategoryName = category.name,
            };
    }
}
