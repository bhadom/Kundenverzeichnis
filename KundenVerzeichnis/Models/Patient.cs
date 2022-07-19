using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KundenVerzeichnis.Models
{
    /// <summary>
    /// Contains all properties for the model patient
    /// </summary>
    class Patient
    {
        [Key]
        public int PID { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Gender { get; set; }
        public bool? Diabetic { get; set; }
        [Column(TypeName = "varchar(70)")]
        public string Street { get; set; }
        [ForeignKey("City")]
        public int FK_City { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", LastName, FirstName);
        }
    }
}
