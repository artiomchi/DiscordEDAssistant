namespace FlexLabs.DiscordEDAssistant.Services.Integrations.Eddb.Models
{
    public class System
    {
        public int id { get; set; }
        public int edsm_id { get; set; }
        public string name { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public string faction { get; set; }
        public int population { get; set; }
        public string government { get; set; }
        public string allegiance { get; set; }
        public string state { get; set; }
        public string security { get; set; }
        public string primary_economy { get; set; }
        public string power { get; set; }
        public string power_state { get; set; }
        public bool needs_permit { get; set; }
        public int updated_at { get; set; }
        public string simbad_ref { get; set; }
        public bool is_populated { get; set; }
        public string reserve_type { get; set; }
    }
}
