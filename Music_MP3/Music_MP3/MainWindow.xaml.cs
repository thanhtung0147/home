using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
using xNet;

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

        private List<Song> listVN;
        private List<Song> listEU;
        private List<Song> listKO;

        public bool IsCheckVN { get => isCheckVN; set { isCheckVN = value; isCheckEU = false; isCheckKO = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }
        public bool IsCheckEU { get => isCheckEU; set { isCheckEU = value; isCheckVN = false; isCheckKO = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }
        public bool IsCheckKO { get => isCheckKO; set { isCheckKO = value; isCheckEU = false; isCheckVN = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }

        public List<Song> ListVN { get => listVN; set => listVN = value; }
        public List<Song> ListEU { get => listEU; set => listEU = value; }
        public List<Song> ListKO { get => listKO; set => listKO = value; }

        public MainWindow()
        {
            InitializeComponent();

            ucSong_Play.BackToMain += UcSong_Play_BackToMain;

            lsbTopSongs.ItemsSource = new List<string>() { "", "", "" };

            this.DataContext = this;

            IsCheckVN = true;

            ListVN = new List<Song>();
            ListEU = new List<Song>();
            ListKO = new List<Song>();

            CrawlBXH();
        }

        void CrawlBXH()
        {
            HttpRequest http = new HttpRequest();
            string htmlBXH = http.Get(@"https://mp3.zing.vn/zing-chart/bai-hat.html").ToString();
            string bxhPattern = @"<div class=""widget widget-tab"">(.*?)</ul>";
            var listBXH = Regex.Matches(htmlBXH, bxhPattern, RegexOptions.Singleline);

            string bxhVN = listBXH[0].ToString();
            var listSongHTML = Regex.Matches(bxhVN, @"<li>(.*?)</li>", RegexOptions.Singleline);

            foreach (var item in listSongHTML)
            {
                var songAndSinger = Regex.Matches(item.ToString(), @"<a\s\S*\stitle=""(.*?)""", RegexOptions.Singleline);

                string songString = songAndSinger[0].ToString();
                int indexSong = songString.IndexOf("title=\"");
                string songName = songString.Substring(indexSong, songString.Length - indexSong - 1).Replace("title =\"","");

                string singerString = songAndSinger[1].ToString();
                int indexSinger = singerString.IndexOf("title=\"");
                string singerName = singerString.Substring(indexSinger, singerString.Length - indexSinger - 1).Replace("title =\"", "");
            }
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
