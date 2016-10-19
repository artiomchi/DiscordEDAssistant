namespace FlexLabs.EDAssistant.Services.Integrations.Eddb.Models
{
    public class Body
    {
        public int id { get; set; }
        public int created_at { get; set; }
        public int updated_at { get; set; }
        public string name { get; set; }
        public int system_id { get; set; }
        public int group_id { get; set; }
        public string group_name { get; set; }
        public string type_id { get; set; }
        public string type_name { get; set; }
        public int distance_to_arrival { get; set; }
        public string full_spectral_class { get; set; }
        public string spectral_class { get; set; }
        public string spectral_sub_class { get; set; }
        public string luminosity_class { get; set; }
        public string luminosity_sub_class { get; set; }
        public int surface_temperature { get; set; }
        public bool is_main_star { get; set; }
        public int age { get; set; }
        public int solar_masses { get; set; }
        public int solar_radius { get; set; }
        public string catalogue_gliese_id { get; set; }
        public string catalogue_hipp_id { get; set; }
        public string catalogue_hd_id { get; set; }
        public string volcanism_type_id { get; set; }
        public string volcanism_type_name { get; set; }
        public string terraforming_state_id { get; set; }
        public float? earth_masses { get; set; }
        public float? radius { get; set; }
        public float? gravity { get; set; }
        public float? surface_pressure { get; set; }
        public float? orbital_period { get; set; }
        public float? semi_major_axis { get; set; }
        public float? orbital_eccentricity { get; set; }
        public float? orbital_inclination { get; set; }
        public float? arg_of_periapsis { get; set; }
        public float? rotational_period { get; set; }
        public bool is_rotational_period_titally_locked { get; set; }
        public float? axis_tilt { get; set; }
        public int? eg_id { get; set; }
        public string belt_moon_masses { get; set; }
        public BodyRings[] rings { get; set; }
        public BodyAtmosphereComposition[] atmosphere_composition { get; set; }
        public BodySolidComposition[] solid_composition { get; set; }
        public BodyMaterial[] materials { get; set; }

        public class BodyRings
        {
            public int id { get; set; }
            public int created_id { get; set; }
            public int updated_id { get; set; }
            public string name { get; set; }
            public float semi_major_axis { get; set; }
            public int ring_type_id { get; set; }
            public float ring_mass { get; set; }
            public float ring_inner_radius { get; set; }
            public float ring_outer_radius { get; set; }
            public string ring_type_name { get; set; }
        }

        public class BodyAtmosphereComposition
        {
            public int atmosphere_component_id { get; set; }
            public float share { get; set; }
            public string atmosphere_component_name { get; set; }
        }

        public class BodySolidComposition
        {
            public int solid_component_id { get; set; }
            public float share { get; set; }
            public string solid_component_name { get; set; }
        }

        public class BodyMaterial
        {
            public int material_id { get; set; }
            public string material_name { get; set; }
            public float share { get; set; }
        }
    }
}
