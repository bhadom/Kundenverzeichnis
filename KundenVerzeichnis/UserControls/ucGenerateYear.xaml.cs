using KundenVerzeichnis.Models;
using KundenVerzeichnis.PDFs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;
using MessageBox = System.Windows.Forms.MessageBox;

namespace KundenVerzeichnis
{
    /// <summary>
    /// Interactionslogic for ucGenerateYear.xaml
    /// </summary>
    public partial class ucGenerateYear : UserControl
    {
        List<int> yearList = new List<int>();
        List<Treatment> treatments = new List<Treatment>();
        int counter;
        decimal earnings;
        /// <summary>
        /// Constructor fills the combobox after initializing
        /// </summary>
        public ucGenerateYear()
        {
            InitializeComponent();
            getTreatments();
            FillCombobox();
        }

        /// <summary>
        /// Gets treatment quantity and earnings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            earnings = 0;
            counter = 0;
            lblYear.Content = cbxYear.SelectedItem;
            for (int i = 0; i < yearList.Count; i++)
            {
                foreach (Treatment t in treatments)
                {

                    if (t.TreatmentDate.Year == yearList[i])
                    {
                        counter++;
                        earnings += t.Price;
                    }
                }
                lblEarnings.Content = Convert.ToInt32(earnings) + " CHF";
                lblPatients.Content = counter;
            }
        } 
    
        /// <summary>
        /// Fills list with all Treatments
        /// </summary>
        public void getTreatments()
        {
            using (var db = new Context())
            {
                treatments = db.Treatments.ToList();
            }
        }

        /// <summary>
        /// Exports the yearly bill
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, RoutedEventArgs e) 
        {
            List<Treatment> treatments;
            int currentYear = Convert.ToInt32(cbxYear.SelectedItem);
            using (var db = new Context()) 
                treatments = db.Treatments.Where(c => c.TreatmentDate.Year == currentYear).ToList();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Datei | *.pdf";
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                string path = System.IO.Path.GetFullPath(saveFileDialog.FileName);
                BillPDF.generateYear(treatments, currentYear, path);
                MessageBox.Show("Die Rechnung wurde unter '" + path + "' abgespeichert", "Jahresabrechnung Export");
            }
        }

        /// <summary>
        /// Fills the combobox with years
        /// </summary>
        public void FillCombobox()
        {
            foreach (Treatment t in treatments)
            {
                if (!yearList.Contains(t.TreatmentDate.Year))
                {
                    yearList.Add(t.TreatmentDate.Year);
                }
            }
            cbxYear.ItemsSource = yearList;
        }
    }
}
