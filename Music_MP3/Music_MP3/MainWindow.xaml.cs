using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Music_MP3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool isCheckVN;
        private bool isCheckEU;
        private bool isCheckKO;

        public bool IsCheckVN { get => isCheckVN; set { isCheckVN = value; isCheckEU = false; isCheckKO = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }
        public bool IsCheckEU { get => isCheckEU; set { isCheckEU = value; isCheckVN = false; isCheckKO = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }
        public bool IsCheckKO { get => isCheckKO; set { isCheckKO = value; isCheckEU = false; isCheckVN = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }

        public MainWindow()
        {
            InitializeComponent();

            ucSong_Play.BackToMain += UcSong_Play_BackToMain;

            lsbTopSongs.ItemsSource = new List<string>() { "", "", "" };

            this.DataContext = this;
        }

        private void UcSong_Play_BackToMain(object sender, EventArgs e)
        {
            gridTop10.Visibility = Visibility.Visible;
            ucSong_Play.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gridTop10.Visibility = Visibility.Hidden;
            ucSong_Play.Visibility = Visibility.Visible;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }
    }
}
