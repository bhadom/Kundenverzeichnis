using KundenVerzeichnis.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MessageBox = System.Windows.Forms.MessageBox;

namespace KundenVerzeichnis
{
    /// <summary>
    /// Interactionslogic for ucPatientErfassen.xaml
    /// </summary>
    public partial class ucAddPatient : UserControl
    {
        /// <summary>
        /// Constructor fills the combobox after initializing
        /// </summary>
        public ucAddPatient()
        {
            InitializeComponent();
            cbxGender.ItemsSource = new string[] { "Männlich", "Weiblich" };
        }

        /// <summary>
        /// Inserts the patient into the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Context())
            {
                Patient p = new Patient();
                City city = new City();
                p.Gender = cbxGender.Text;
                p.FirstName = txtFirstName.Text;
                p.LastName = txtLastName.Text;
                p.Street = txtStreet.Text;

                if ((bool)cbDiabetic.IsChecked)
                    p.Diabetic = true;
                else
                    p.Diabetic = false;
                
                if (!db.Cities.Any(c => c.PostalCode == Convert.ToInt32(txtPostalCode.Text)))
                {
                    city.PostalCode = Convert.ToInt32(txtPostalCode.Text);
                    city.Town = txtCity.Text;
                    db.Cities.Add(city);
                    db.SaveChanges();
                }

                var cityPostalCode = db.Cities.Where(c => c.PostalCode == Convert.ToInt32(txtPostalCode.Text)).First();
                p.FK_City = cityPostalCode.CID;
                //db.Patients.Add(p);
                db.Entry(p).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.SaveChanges();
                foreach(Patient pt in db.Patients)
                {
                    Console.WriteLine(pt.LastName);
                }
                
                
                
                MessageBox.Show("Der Patient wurde erfolgreich erfasst", "Patient Erfassung");
            }
        }
    }
}
