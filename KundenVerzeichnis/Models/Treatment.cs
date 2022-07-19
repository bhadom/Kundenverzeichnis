using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KundenVerzeichnis.Models
{
    /// <summary>
    /// Contains all properties for the model treatment
    /// </summary>
    class Treatment
    {
        [Key]
        public int BID { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Title { get; set; }
        [ForeignKey("Patient")]
        public int FK_Patient { get; set; }
        public DateTime TreatmentDate { get; set; }
        [Column(TypeName ="Money")]
        public decimal Price { get; set; }
        [Column(TypeName = "varchar(400)")]
        public string Notes { get; set; }
        public bool? Invoice { get; set; }
    }
}
