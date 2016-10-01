namespace FlexLabs.DiscordEDAssistant.Models.External.Eddb
{
    public class Module
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? Price { get; set; }
        public byte Class { get; set; }
        public char Rating { get; set; }
        public ModuleWeaponMode? WeaponMode { get; set; }
        public byte? MissileType { get; set; }
        public float Mass { get; set; }
        public float Power { get; set; }
        public string Ship { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
