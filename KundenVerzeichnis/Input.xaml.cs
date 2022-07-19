using System.Windows;

namespace KundenVerzeichnis
{
    /// <summary>
    /// Interactionslogic for Input.xaml
    /// </summary>
    public partial class Input : Window
    {
        MainWindow parent;

        /// <summary>
        /// Constructor with parameter to MainWindow
        /// </summary>
        /// <param name="main"></param>
        public Input(MainWindow main)
        {
            InitializeComponent();
            parent = main;
        }

        /// <summary>
        /// Get password for MainWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            parent.pwd = pwdBox.Password;
            this.Close();
        }
    }
}
