using KundenVerzeichnis.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;

namespace KundenVerzeichnis
{
    /// <summary>
    /// Interactionslogic for ucChangeTreatment.xaml
    /// </summary>
    public partial class ucChangeTreatment : UserControl
    {
        int treatmentID;
        bool billState;

        /// <summary>
        /// Constructor fills the combobox after initializing
        /// </summary>
        public ucChangeTreatment()
        {
            InitializeComponent();
            fillCombobox();
        }

        /// <summary>
        /// Focuses the search textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TextBox_Focus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_Focus;
        }

        /// <summary>
        /// Gets treatments from selected patient
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPatient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var db = new Context())
            {
                var patient = db.Patients.ToList();
                bool checker = false;
                cbxPatient.Text = Convert.ToString(cbxPatient.SelectedItem);

                for (int i = 0; i < patient.Count; i++)
                {
                    if (Convert.ToString(patient[i]) == Convert.ToString(cbxPatient.SelectedItem))
                        checker = true;
                }

                if (checker)
                {
                    string[] content = cbxPatient.Text.Split();
                    string firstName = content[1];
                    string lastName = content[0];
                    var patientName = db.Patients.Where(p => p.LastName == lastName && p.FirstName == firstName).FirstOrDefault();

                    db.Treatments.Where(t => t.FK_Patient == patientName.PID).Load();         

                    this.DataContext = db.Treatments.Local;
                    dgTreatments.ItemsSource = db.Treatments.Local.ToObservableCollection();
                }
                else
                    dgTreatments.ItemsSource = null;
            }
        }

        /// <summary>
        /// Doubleclick event to insert treatment attributes into textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Treatment treat = dgTreatments.SelectedItem as Treatment;
            treatmentID = treat.BID;
            int price;
            using (var db = new Context())
            {
                Treatment treatment = db.Treatments.Where(d => d.BID == treatmentID).First();
                price = Convert.ToInt32(treatment.Price);
                txtPrice.Text = price.ToString();
                cbBill.IsChecked = treatment.Invoice;
                billState = (bool)treatment.Invoice;
                TextRange notes = new TextRange(tbNotes.Document.ContentStart, tbNotes.Document.ContentEnd);
                notes.Text = treatment.Notes;
                dpDate.SelectedDate = treatment.TreatmentDate;
            }
        }

        /// <summary>
        /// Change treatment from database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Context())
            {
                Treatment treat = db.Treatments.Where(d => d.BID == treatmentID).First();
                TextRange notes = new TextRange(tbNotes.Document.ContentStart, tbNotes.Document.ContentEnd);
                treat.Notes = notes.Text;
                if (cbBill.IsChecked != billState)
                {
                    if ((bool)cbBill.IsChecked)
                    {
                        treat.Invoice = true;
                        treat.Price = Convert.ToDecimal(txtPrice.Text) + 5;
                    }
                    else
                    {
                        treat.Invoice = false;
                        treat.Price = Convert.ToDecimal(txtPrice.Text) - 5;
                    }
                }
                treat.Price = Convert.ToDecimal(txtPrice.Text);
                treat.TreatmentDate = Convert.ToDateTime(dpDate.Text);
                db.SaveChanges();
            }
            MessageBox.Show("Die Behandlung wurde erfolgreich angepasst", "Behandlung Anpassung");
        }

        /// <summary>
        /// Fills the combobox with patients
        /// </summary>
        public void fillCombobox()
        {
            using (var db = new Context())
            {
                cbxPatient.ItemsSource = db.Patients.ToList();
            }
        }
    }
}
