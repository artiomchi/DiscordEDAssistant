using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base
{
    public class Server
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }
        [StringLength(5)]
        public string CommandPrefix { get; set; }
        [MaxLength]
        public string WelcomeMessage { get; set; }
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
        public char Rating { get; set; }
        public byte? WeaponMode { get; set; }
        public byte? MissileType { get; set; }
        public float Mass { get; set; }
        public float Power { get; set; }
        [StringLength(255)]
        public string Ship { get; set; }
        public int GroupID { get; set; }
        public int CategoryID { get; set; }

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

    [Table("Systems", Schema = "eddb")]
    public class Eddb_System
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
        public char Rating { get; set; }
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
    }

    [Table("Eddb_Systems", Schema = "upload")]
    public class Upload_Eddb_System
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
}
