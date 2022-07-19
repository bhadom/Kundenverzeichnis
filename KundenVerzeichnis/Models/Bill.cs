using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KundenVerzeichnis.Models
{
    /// <summary>
    /// Contains all properties for the model bill
    /// </summary>
    class Bill
    {
        [Key]
        public int RID { get; set; }
        public DateTime BillDate { get; set; }
        [ForeignKey("Treatment")]
        public int FK_Treatment { get; set; }
        [Column(TypeName ="varchar(250)")]
        public string Notes { get; set; }
    }
}
