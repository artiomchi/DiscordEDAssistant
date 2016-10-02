using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base
{
    public class KosSimpleRule
    {
        [Key]
        public int PK { get; set; }
        public long ServerID { get; set; }
        public string UserName { get; set; }
        public long AuthorID { get; set; }
        [Required, MaxLength]
        public string Rule { get; set; }
    }

    public class KosUserRule
    {
        [Key]
        public int PK { get; set; }
        public long ServerID { get; set; }
        public long UserID { get; set; }
        public long AuthorID { get; set; }
        [Required, MaxLength]
        public string Rule { get; set; }
    }

    public class Server
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }
        [StringLength(5)]
        public string CommandPrefix { get; set; }
        [MaxLength]
        public string WelcomeMessage { get; set; }
    }

    [Table("Commodities", Schema = "eddb")]
    public class Eddb_Commodity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public int AveragePrice { get; set; }
        public bool IsRare { get; set; }
        public int CategoryID { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public Eddb_Commodities_Category Category { get; set; }
    }

    [Table("Commodities_Categories", Schema = "eddb")]
    public class Eddb_Commodities_Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        [StringLength(255)]
        public string Name { get; set; }

        [InverseProperty(nameof(Eddb_Commodity.Category))]
        public IList<Eddb_Commodity> Modules { get; set; }
    }

    [Table("Modules", Schema = "eddb")]
    public class Eddb_Module
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public int? Price { get; set; }
        public byte Class { get; set; }
        public byte? Rating { get; set; }
        public byte? WeaponMode { get; set; }
        public byte? MissileType { get; set; }
        public float Mass { get; set; }
        public float Power { get; set; }
        [StringLength(255)]
        public string Ship { get; set; }
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        [StringLength(255)]
        public string FullName { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public Eddb_Modules_Category Category { get; set; }
        [ForeignKey(nameof(GroupID))]
        public Eddb_Modules_Group Group { get; set; }
    }

    [Table("Modules_Categories", Schema = "eddb")]
    public class Eddb_Modules_Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        [StringLength(255)]
        public string Name { get; set; }

        [InverseProperty(nameof(Eddb_Module.Category))]
        public IList<Eddb_Module> Modules { get; set; }
    }

    [Table("Modules_Groups", Schema = "eddb")]
    public class Eddb_Modules_Group
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        [StringLength(255)]
        public string Name { get; set; }

        [InverseProperty(nameof(Eddb_Module.Group))]
        public IList<Eddb_Module> Modules { get; set; }
    }

    [Table("Stations", Schema = "eddb")]
    public class Eddb_Station
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public int SystemID { get; set; }
        public byte? MaxLandingPadSize { get; set; }
        public int? DistanceToStar { get; set; }
        [StringLength(255)]
        public string Faction { get; set; }
        [StringLength(255)]
        public string Government { get; set; }
        [StringLength(255)]
        public string Allegiance { get; set; }
        [StringLength(255)]
        public string State { get; set; }
        public int? TypeID { get; set; }
        public bool HasBlackmarket { get; set; }
        public bool HasMarket { get; set; }
        public bool HasRefuel { get; set; }
        public bool HasRepair { get; set; }
        public bool HasRearm { get; set; }
        public bool HasOutfitting { get; set; }
        public bool HasShipyard { get; set; }
        public bool HasDocking { get; set; }
        public bool HasCommodities { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ShipyardUpdatedAt { get; set; }
        public DateTime? OutfittingUpdatedAt { get; set; }
        public DateTime? MarketUpdatedAt { get; set; }
        public bool IsPlanetary { get; set; }

        [ForeignKey(nameof(TypeID))]
        public Eddb_Stations_Type Type { get; set; }
        [InverseProperty(nameof(Eddb_Stations_SellingModule.Station))]
        public IList<Eddb_Stations_SellingModule> SellingModules { get; set; }
        [InverseProperty(nameof(Eddb_Stations_SellingShip.Station))]
        public IList<Eddb_Stations_SellingShip> SellingShips { get; set; }
    }

    [Table("Stations_SellingModules", Schema = "eddb")]
    public class Eddb_Stations_SellingModule
    {
        [Key]
        public int PK { get; set; }
        public int StationID { get; set; }
        public int ModuleID { get; set; }

        [ForeignKey(nameof(StationID))]
        public Eddb_Station Station { get; set; }
    }

    [Table("Stations_SellingShips", Schema = "eddb")]
    public class Eddb_Stations_SellingShip
    {
        [Key]
        public int PK { get; set; }
        public int StationID { get; set; }
        [StringLength(255)]
        public string Ship { get; set; }

        [ForeignKey(nameof(StationID))]
        public Eddb_Station Station { get; set; }
    }

    [Table("Stations_Types", Schema = "eddb")]
    public class Eddb_Stations_Type
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        [StringLength(255)]
        public string Name { get; set; }

        [InverseProperty(nameof(Eddb_Station.Type))]
        public IList<Eddb_Station> Stations { get; set; }
    }

    [Table("StarSystems", Schema = "eddb")]
    public class Eddb_StarSystem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        [StringLength(512)]
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public bool IsPopulated { get; set; }
    }

    [Table("Eddb_Commodities", Schema = "upload")]
    public class Upload_Eddb_Commodity
    {
        [Key]
        public int PK { get; set; }
        public int ID { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public int AveragePrice { get; set; }
        public bool IsRare { get; set; }
        public int CategoryID { get; set; }
        [StringLength(255)]
        public string CategoryName { get; set; }
    }

    [Table("Eddb_Modules", Schema = "upload")]
    public class Upload_Eddb_Module
    {
        [Key]
        public int PK { get; set; }
        public int ID { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public int? Price { get; set; }
        public byte Class { get; set; }
        public byte? Rating { get; set; }
        public byte? WeaponMode { get; set; }
        public byte? MissileType { get; set; }
        public float Mass { get; set; }
        public float Power { get; set; }
        public string Ship { get; set; }
        public int GroupID { get; set; }
        [StringLength(255)]
        public string GroupName { get; set; }
        public int CategoryID { get; set; }
        [StringLength(255)]
        public string CategoryName { get; set; }
        [StringLength(255)]
        public string FullName { get; set; }
    }

    [Table("Eddb_StarSystems", Schema = "upload")]
    public class Upload_Eddb_StarSystem
    {
        [Key]
        public int PK { get; set; }
        public int ID { get; set; }
        [StringLength(512)]
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public bool IsPopulated { get; set; }
    }

    [Table("Eddb_Stations", Schema = "upload")]
    public class Upload_Eddb_Station
    {
        [Key]
        public int PK { get; set; }
        public int ID { get; set; }
        [StringLength(512)]
        public string Name { get; set; }
        public int SystemID { get; set; }
        public byte? MaxLandingPadSize { get; set; }
        public int? DistanceToStar { get; set; }
        [StringLength(512)]
        public string Faction { get; set; }
        [StringLength(512)]
        public string Government { get; set; }
        [StringLength(512)]
        public string Allegiance { get; set; }
        [StringLength(512)]
        public string State { get; set; }
        public int? TypeID { get; set; }
        [StringLength(512)]
        public string TypeName { get; set; }
        public bool HasBlackmarket { get; set; }
        public bool HasMarket { get; set; }
        public bool HasRefuel { get; set; }
        public bool HasRepair { get; set; }
        public bool HasRearm { get; set; }
        public bool HasOutfitting { get; set; }
        public bool HasShipyard { get; set; }
        public bool HasDocking { get; set; }
        public bool HasCommodities { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ShipyardUpdatedAt { get; set; }
        public DateTime? OutfittingUpdatedAt { get; set; }
        public DateTime? MarketUpdatedAt { get; set; }
        public bool IsPlanetary { get; set; }
    }

    [Table("Eddb_Stations_SellingModules", Schema = "upload")]
    public class Upload_Eddb_Stations_SellingModule
    {
        [Key]
        public int PK { get; set; }
        public int StationID { get; set; }
        public int ModuleID { get; set; }
    }

    [Table("Eddb_Stations_SellingShips", Schema = "upload")]
    public class Upload_Eddb_Stations_SellingShip
    {
        [Key]
        public int PK { get; set; }
        public int StationID { get; set; }
        [StringLength(255)]
        public string Ship { get; set; }
    }
}
