using System;
using KundenVerzeichnis.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using MessageBox = System.Windows.Forms.MessageBox;


namespace KundenVerzeichnis
{
    /// <summary>
    /// Interactionslogic for usAddTreatment.xaml
    /// </summary>
    public partial class ucAddTreatment : UserControl
    {
        /// <summary>
        /// Constructor fills the combobox after initializing
        /// </summary>
        public ucAddTreatment()
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
        /// Inserts the treatment into database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string[] content = cbxPatient.Text.Split();
            string firstName = content[1];
            string lastName = content[0];

            using (var db = new Context())
            {
                Treatment t = new Treatment();
                t.Title = txtTreatmentTitle.Text;
                
                t.TreatmentDate = Convert.ToDateTime(dpDate.Text);
                if ((bool)cbBill.IsChecked)
                {
                    t.Invoice = true;
                    t.Price = Convert.ToDecimal(txtPrice.Text)+5;
                }
                else
                {
                    t.Invoice = false;
                    t.Price = Convert.ToDecimal(txtPrice.Text);
                }

                TextRange notes = new TextRange(tbNotes.Document.ContentStart, tbNotes.Document.ContentEnd);
                t.Notes = notes.Text;
                var patientName = db.Patients.Where(p => p.LastName == lastName && p.FirstName == firstName).FirstOrDefault();
                t.FK_Patient = patientName.PID;
                db.Treatments.Add(t);
                db.SaveChanges();
                MessageBox.Show("Die Behandlung wurde erfolgreich erstellt", "Behandlung Erstellung");
            }
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
