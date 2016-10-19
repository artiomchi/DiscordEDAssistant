namespace FlexLabs.EDAssistant.Services.Integrations.Eddb.Models
{
    public class Module
    {
        public int id { get; set; }
        //public int? group_id { get; set; }
        public byte @class { get; set; }
        public char? rating { get; set; }
        public int? price { get; set; }
        public EDAssistant.Models.External.Eddb.ModuleWeaponMode? weapon_mode { get; set; }
        public byte? missile_type { get; set; }
        public string name { get; set; }
        //public string belongs_to { get; set; }
        //public int ed_id { get; set; }
        //public string ed_symbol { get; set; }
        public float mass { get; set; }
        public float power { get; set; }
        public string ship { get; set; }
        public ModuleGroup group { get; set; }

        public class ModuleGroup
        {
            public int id { get; set; }
            public int category_id { get; set; }
            public string name { get; set; }
            public string category { get; set; }
        }

        public EDAssistant.Models.External.Eddb.Module Translate()
            => new EDAssistant.Models.External.Eddb.Module
            {
                ID = id,
                Name = name,
                Price = price,
                Class = @class,
                Rating = rating,
                WeaponMode = weapon_mode,
                MissileType = missile_type,
                Mass = mass,
                Power = power,
                Ship = ship,
                GroupID = group.id,
                GroupName = group.name,
                CategoryID = group.category_id,
                CategoryName = group.category,
            };
    }
}
