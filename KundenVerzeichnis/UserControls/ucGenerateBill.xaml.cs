using KundenVerzeichnis.Models;
using KundenVerzeichnis.PDFs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using UserControl = System.Windows.Controls.UserControl;
using TextBox = System.Windows.Controls.TextBox;
using MessageBox = System.Windows.Forms.MessageBox;
using Path = System.IO.Path;

namespace KundenVerzeichnis
{
    /// <summary>
    /// Interactionslogic for ucGenerateBill.xaml
    /// </summary>
    public partial class ucGenerateBill : UserControl
    {
        int treatmentID;
        string notesHelper;
        Patient patientHelper;

        /// <summary>
        /// Constructor fills the combobox after initializing
        /// </summary>
        public ucGenerateBill()
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
        /// Doubleclick event to insert treatment attributes on UC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Treatment treat = dgTreatments.SelectedItem as Treatment;
            treatmentID = treat.BID;
            using (var db = new Context())
            {
                Treatment treatment = db.Treatments.Where(d => d.BID == treatmentID).First();
                TextRange notes = new TextRange(tbNotes.Document.ContentStart, tbNotes.Document.ContentEnd);
                string todaysDate = DateTime.Now.ToString("dd.MM.yyyy");
                int price = Convert.ToInt32(treatment.Price);
                int reducedPrice = price - 5;
                var treatDate = treatment.TreatmentDate.ToShortDateString();
                notes.Text = "Rechnung für: "+ treatment.Title
                            + " am " + treatDate + Environment.NewLine
                            + "Behandlung = " + reducedPrice  +".-" + Environment.NewLine
                            + "Rechnung = 5.-" + Environment.NewLine
                            + "Betrag von "  + price + ".-" + " bitte innert 30 Tagen bezahlen. " + Environment.NewLine
                            + "Vielen Dank" + Environment.NewLine + "C.Baumann" + Environment.NewLine + todaysDate;
                notesHelper = notes.Text;
            }
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
                    patientHelper = patientName;
                    db.Treatments.Where(t => t.FK_Patient == patientName.PID).Load();
                    this.DataContext = db.Treatments.Local;
                    dgTreatments.ItemsSource = db.Treatments.Local.ToObservableCollection();
                }
                else
                    dgTreatments.ItemsSource = null;
            }
        }

        /// <summary>
        /// Inserts bill into database and exports it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Context())
            {
                Bill b = new Bill();
                b.BillDate = DateTime.Now;
                b.Notes= notesHelper;
                b.FK_Treatment = treatmentID;
                db.Bills.Add(b);
                db.SaveChanges();

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF Datei | *.pdf";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = Path.GetFullPath(saveFileDialog.FileName);
                    BillPDF.generateBill(notesHelper, patientHelper, path);
                    MessageBox.Show("Die Rechnung wurde unter '" + path + "' abgespeichert", "Abrechnung Export");
                }
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
