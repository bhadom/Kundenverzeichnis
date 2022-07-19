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
    /// Interaktionslogik für ucChangeBill.xaml
    /// </summary>
    public partial class ucChangeBill : UserControl
    {
        int treatmentID;

        /// <summary>
        /// Constructor fills the combobox after initializing
        /// </summary>
        public ucChangeBill()
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
                    dgBills.ItemsSource = db.Treatments.Local.ToObservableCollection();
                }
                else
                    dgBills.ItemsSource = null;
            }
        }

        /// <summary>
        /// Doubleclick event to insert notes into textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Treatment treat = dgBills.SelectedItem as Treatment;
            treatmentID = treat.BID;
            using (var db = new Context())
            {
                Treatment treatment = db.Treatments.Where(d => d.BID == treatmentID).First();
                TextRange notes = new TextRange(tbNotes.Document.ContentStart, tbNotes.Document.ContentEnd);
                try
                {
                    Bill bill = db.Bills.Where(b => b.FK_Treatment == treatmentID).First();
                    notes.Text = bill.Notes;
                }
                catch(Exception)
                {
                    MessageBox.Show("Für diese Behandlung wurde noch keine Rechnung erstellt", "Keine Rechnung");
                    
                }
            }
        }

        /// <summary>
        /// Change bill from database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Context())
            {
                Bill bill = db.Bills.Where(b => b.FK_Treatment == treatmentID).First();
                TextRange notes = new TextRange(tbNotes.Document.ContentStart, tbNotes.Document.ContentEnd);
                bill.Notes = notes.Text;
                db.SaveChanges();
            }
            MessageBox.Show("Die Rechnung wurde erfolgreich angepasst", "Rechnung Anpassung");
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
