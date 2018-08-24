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
        private List<Song> listUS;
        private List<Song> listKO;

        public bool IsCheckVN { get => isCheckVN; set { isCheckVN = value; isCheckEU = false; isCheckKO = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }
        public bool IsCheckEU { get => isCheckEU; set { isCheckEU = value; isCheckVN = false; isCheckKO = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }
        public bool IsCheckKO { get => isCheckKO; set { isCheckKO = value; isCheckEU = false; isCheckVN = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }

        public List<Song> ListVN { get => listVN; set => listVN = value; }
        public List<Song> ListKO { get => listKO; set => listKO = value; }
        public List<Song> ListUS { get => listUS; set => listUS = value; }

        public MainWindow()
        {
            InitializeComponent();

            ucSong_Play.BackToMain += UcSong_Play_BackToMain;

            lsbTopSongs.ItemsSource = new List<string>() { "", "", "" };

            this.DataContext = this;

            IsCheckVN = true;

            ListVN = new List<Song>();
            ListUS = new List<Song>();
            ListKO = new List<Song>();

            CrawlBXH();
        }

        void CrawlBXH()
        {
            HttpRequest http = new HttpRequest();
            string htmlBXH_VN = http.Get(@"https://mp3.zing.vn/zing-chart-tuan/Bai-hat-Viet-Nam/IWZ9Z08I.html").ToString();
            string htmlBXH_US = http.Get(@"https://mp3.zing.vn/zing-chart-tuan/Bai-hat-US-UK/IWZ9Z0BW.html").ToString();
            string htmlBXH_KO = http.Get(@"https://mp3.zing.vn/zing-chart-tuan/Bai-hat-KPop/IWZ9Z0BO.html").ToString();

            AddSongToListSong(ListVN, htmlBXH_VN);
            AddSongToListSong(ListKO, htmlBXH_KO);
            AddSongToListSong(ListUS, htmlBXH_US);
        }

        void AddSongToListSong(List<Song> listSong, string html)
        {
            var bxh = Regex.Matches(html, @"<div class=""group po-r"">(.*?)</div>", RegexOptions.Singleline);

            for (int i = 0; i < bxh.Count; i++)
            {
                var songAndSinger = Regex.Matches(bxh[i].ToString(), @"<a\s\S*\stitle=""(.*?)""", RegexOptions.Singleline);

                string songString = songAndSinger[1].ToString();
                int indexSong = songString.IndexOf("title=\"");
                string songName = songString.Substring(songString.IndexOf("title=\""), songString.Length - indexSong - 1).Replace("title=\"", "");

                string singerString = songAndSinger[2].ToString();
                int indexSinger = singerString.IndexOf("title=\"");
                string singerName = singerString.Substring(singerString.IndexOf("title=\""), singerString.Length - indexSinger - 1).Replace("title=\"Nghệ sĩ ", "");

                int indexURL = songString.IndexOf("href=\"");
                string URL = songString.Substring(indexURL, indexSong - indexURL - 2).Replace("href=\"", "");

                listSong.Add(new Song() { SingerName = singerName, SongName = songName, SongURL = URL, STT = i + 1 });
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
