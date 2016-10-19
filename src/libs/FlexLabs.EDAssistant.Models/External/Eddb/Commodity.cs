namespace FlexLabs.EDAssistant.Models.External.Eddb
{
    public class Commodity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AveragePrice { get; set; }
        public bool IsRare { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
