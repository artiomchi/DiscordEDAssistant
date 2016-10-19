namespace FlexLabs.EDAssistant.Repositories.EFCore.Base
{
    public class Eddb_Stations_SearchClosestByModuleResult
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int DistanceToStar { get; set; }
        public int SystemID { get; set; }
        public string SystemName { get; set; }
        public float DistanceToSystem { get; set; }
    }
}
