using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KundenVerzeichnis.Models
{
    /// <summary>
    /// Contains all properties for the model city
    /// </summary>
    class City
    {
        [Key]
        public int CID { get; set; }
        [Column(TypeName = "int")]
        public int PostalCode { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Town { get; set; }
    }
}
