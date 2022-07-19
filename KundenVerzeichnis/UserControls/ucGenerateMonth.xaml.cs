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
using Path = System.IO.Path;

namespace KundenVerzeichnis
{
    /// <summary>
    /// Interactionslogic for ucGenerateMonth.xaml
    /// </summary>
    public partial class ucGenerateMonth : UserControl
    {
        string[] months = new string[] { "Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember" };
        int[] monthNumbers = new int[] { 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12};
        List<int> yearList = new List<int>();
        List<Treatment> treatments = new List<Treatment>();
        private int setYear;
        decimal earn;
        int counter;
        /// <summary>
        /// Constructor fills the combobox after initializing
        /// </summary>
        public ucGenerateMonth()
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
        private void cbxMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            earn = 0;
            counter = 0;
            lblEarnings.Content = Convert.ToInt32(earn) + " CHF";
            lblPatients.Content = counter;
            lblMonth.Content = cbxMonth.SelectedItem;
            for (int i = 0; i < months.Length; i++)
            {
                if (Convert.ToString(cbxMonth.SelectedItem) == months[i])
                {
                    foreach (Treatment t in treatments)
                    {

                        if (t.TreatmentDate.Month == monthNumbers[i] && t.TreatmentDate.Year == setYear)
                        {
                            counter++;
                            earn += t.Price;
                        }
                    }
                    lblEarnings.Content = Convert.ToInt32(earn) + " CHF";
                    lblPatients.Content = counter;
                }
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
        /// Sets year from selected year combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

                setYear = Convert.ToInt32(cbxYear.SelectedItem);
                cbxMonth_SelectionChanged(sender, e);
            
        }

        /// <summary>
        /// Exports the monthly bill
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, RoutedEventArgs e) {

            List<Treatment> treatments;
            int selectedMonth = 0;
            for (int i = 0; i < months.Length; i++)
            {
                if (Convert.ToString(cbxMonth.SelectedItem) == months[i])
                {
                    selectedMonth = monthNumbers[i];
                    break;
                }
            }

            using (var db = new Context()) 
                treatments = db.Treatments.Where(c => c.TreatmentDate.Month == selectedMonth && c.TreatmentDate.Year == Convert.ToInt32(cbxYear.SelectedItem)).ToList();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Datei | *.pdf";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = Path.GetFullPath(saveFileDialog.FileName);
                BillPDF.generateMonth(treatments, cbxMonth.SelectedItem.ToString(), path);
                MessageBox.Show("Die Rechnung wurde unter '" + path + "' abgespeichert", "Monatsabrechnung Export");
            }
        }

        /// <summary>
        /// Fills the combobox with months and years
        /// </summary>
        public void FillCombobox()
        {
            cbxMonth.ItemsSource = months;
            foreach(Treatment t in treatments)
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
