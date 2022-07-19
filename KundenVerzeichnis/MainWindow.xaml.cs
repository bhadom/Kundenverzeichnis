using KundenVerzeichnis.Models;
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace KundenVerzeichnis
{
    /// <summary>
    /// Interactionslogic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string pwd = "";
        public Input input;

        /// <summary>
        /// Constructor start input window to get password
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.Closed += mainClosed;
            input = new Input(this);
            input.Closed += inputClosed;
            this.Hide();
            input.Show();
        }

        /// <summary>
        /// Encrypts the connection string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainClosed(object sender, EventArgs e)
        {
            using (var db = new Context())
            {
                db.SaveChanges();
                foreach (Patient pt in db.Patients)
                {
                    Console.WriteLine(pt.LastName);
                }
            }
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings["DefaultConnection"].ConnectionString = "FhIoH4LtFQxLfTtbvTEyk01rAEAfjMtS+hAVikBkZK9dPwAZ0F0ooErh28XpVA4Pdj4PKxSD+g8Z0Xx8DZs83uCyTsW/0K/dmdko4N6NwtE7Q158BHQ8YMVkX8EnauaAQldv6CbpM+RKdrVIUKAYYVju35DGcCgxzCClizAjn2DATpGmLd8t/mfBq4/YLJKs";
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
            

        }

        /// <summary>
        /// More encryption for connectionstring
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputClosed(object sender, EventArgs e)
        {
            if(pwd == "")
            {
                Environment.Exit(0);
            }
            else
            {
                //Error handling stuff und so
                string decryptedString="";
                try
                {
                    decryptedString = decrypt(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, pwd);
                
                this.Show();

                //________________________________
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
                connectionStringsSection.ConnectionStrings["DefaultConnection"].ConnectionString = decryptedString;
                config.Save();
                ConfigurationManager.RefreshSection("connectionStrings");

                //________________________________
                
                    Context cont = new Context();
                }
                catch (Exception)
                {
                    pwd = "";
                    input = new Input(this);
                    input.Closed += inputClosed;
                    input.Show();
                    
                }
            }
        }

        /// <summary>
        /// Decrypt the password
        /// </summary>
        /// <param name="encryptedResult"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
        public static string decrypt(string encryptedResult, string passwd)
        {
            //call input field

            byte[] bytesToBeDecrypted = Convert.FromBase64String(encryptedResult);
            byte[] passwordBytesdecrypt = Encoding.UTF8.GetBytes(passwd);
            passwordBytesdecrypt = SHA256.Create().ComputeHash(passwordBytesdecrypt);
            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytesdecrypt);
            
            string decryptedResult = Encoding.UTF8.GetString(bytesDecrypted);

            return decryptedResult;
        }

        /// <summary>
        /// AES Decryption for password
        /// </summary>
        /// <param name="bytesToBeDecrypted"></param>
        /// <param name="passwordBytes"></param>
        /// <returns></returns>
        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            byte[] saltBytes = new byte[] { 3, 7, 5, 4, 2, 6, 4, 9 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
                return decryptedBytes;
            }
        }

        /// <summary>
        /// Shows the addPatient UC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            ucAddPatient addPatient = new ucAddPatient();
            UserControlGrid.Children.Clear();
            getUserControl(addPatient);
        }

        /// <summary>
        /// Shows the addTreatment UC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddTreatment_Click(object sender, RoutedEventArgs e)
        {
            ucAddTreatment addTreatment = new ucAddTreatment();
            UserControlGrid.Children.Clear();
            getUserControl(addTreatment);
            
        }

        /// <summary>
        /// Shows the changeTreatment UC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeTreatment_Click(object sender, RoutedEventArgs e)
        {
            ucChangeTreatment changeTreatment = new ucChangeTreatment();
            UserControlGrid.Children.Clear();
            getUserControl(changeTreatment);
        }

        /// <summary>
        /// Shows the generateBill UC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateBill_Click(object sender, RoutedEventArgs e)
        {
            ucGenerateBill generateBill = new ucGenerateBill();
            UserControlGrid.Children.Clear();
            getUserControl(generateBill);
        }

        /// <summary>
        /// Shows the changeBill UC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeBill_Click(object sender, RoutedEventArgs e)
        {
            ucChangeBill changeBill = new ucChangeBill();
            UserControlGrid.Children.Clear();
            getUserControl(changeBill);
        }

        /// <summary>
        /// Shows the generateMonth UC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateMonth_Click(object sender, RoutedEventArgs e)
        {
            ucGenerateMonth generateMonth = new ucGenerateMonth();
            UserControlGrid.Children.Clear();
            getUserControl(generateMonth);
        }

        /// <summary>
        /// Shows the generateYear UC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateYear_Click(object sender, RoutedEventArgs e)
        {
            ucGenerateYear generateYear= new ucGenerateYear();
            UserControlGrid.Children.Clear();
            getUserControl(generateYear);
        }

        /// <summary>
        /// Shows the according Usercontrol if one of the buttons is clicked
        /// </summary>
        /// <param name="userControl"></param>
        public void getUserControl(UserControl userControl)
        {
            userControl.Width = UserControlGrid.Width;
            userControl.Height = UserControlGrid.Height;
            userControl.HorizontalAlignment = HorizontalAlignment.Left;
            userControl.VerticalAlignment = VerticalAlignment.Top;
            UserControlGrid.Children.Add(userControl);
        }
    }
}
