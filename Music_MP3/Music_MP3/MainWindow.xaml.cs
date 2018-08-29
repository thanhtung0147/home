using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<Song> listVN;
        private ObservableCollection<Song> listUS;
        private ObservableCollection<Song> listKO;

        public bool IsCheckVN { get => isCheckVN; set { isCheckVN = value; lsbTopSongs.ItemsSource = ListVN; isCheckEU = false; isCheckKO = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }
        public bool IsCheckEU { get => isCheckEU; set { isCheckEU = value; lsbTopSongs.ItemsSource = ListUS; isCheckVN = false; isCheckKO = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }
        public bool IsCheckKO { get => isCheckKO; set { isCheckKO = value; lsbTopSongs.ItemsSource = ListKO; isCheckEU = false; isCheckVN = false; OnPropertyChanged("IsCheckVN"); OnPropertyChanged("IsCheckEU"); OnPropertyChanged("IsCheckKO"); } }

        public ObservableCollection<Song> ListVN { get => listVN; set => listVN = value; }
        public ObservableCollection<Song> ListKO { get => listKO; set => listKO = value; }
        public ObservableCollection<Song> ListUS { get => listUS; set => listUS = value; }

        public MainWindow()
        {
            InitializeComponent();

            ucSong_Play.BackToMain += UcSong_Play_BackToMain;

            this.DataContext = this;


            ListVN = new ObservableCollection<Song>();
            ListUS = new ObservableCollection<Song>();
            ListKO = new ObservableCollection<Song>();

            IsCheckVN = true;

            CrawlBXH();
        }

        void CrawlBXH()
        {
            HttpRequest http = new HttpRequest();
            string htmlBXH_VN = http.Get(@"https://mp3.zing.vn/zing-chart-tuan/Bai-hat-Viet-Nam/IWZ9Z08I.html").ToString();
            string htmlBXH_US = http.Get(@"https://mp3.zing.vn/zing-chart-tuan/Bai-hat-US-UK/IWZ9Z0BW.html").ToString();
            string htmlBXH_KO = http.Get(@"https://mp3.zing.vn/zing-chart-tuan/Bai-hat-KPop/IWZ9Z0BO.html").ToString();

            AddSongToListSong(ListVN, htmlBXH_VN);
            AddSongToListSong(ListUS, htmlBXH_US);
            AddSongToListSong(ListKO, htmlBXH_KO);
        }

        void AddSongToListSong(ObservableCollection<Song> listSong, string html)
        {
            var bxh = Regex.Matches(html, @"<div class=""group po-r"">(.*?)</div>", RegexOptions.Singleline);

            for (int i = 0; i < bxh.Count; i++)
            {
                var songAndSinger = Regex.Matches(bxh[i].ToString(), @"<a(.*?)>", RegexOptions.Singleline);

                string songString = songAndSinger[1].ToString();
                int indexSong = songString.IndexOf("title=\"");
                string songName = Regex.Match(songString, "title=\"(.*?)\"", RegexOptions.Singleline).ToString().Replace("title=\"", "").Replace("\"", "");

                string singerString = songAndSinger[2].ToString();
                int indexSinger = singerString.IndexOf("title=\"");
                string singerName = singerString.Substring(singerString.IndexOf("title=\""), singerString.Length - indexSinger - 1).Replace("title=\"Nghệ sĩ ", "").Replace("\"", "");

                int indexURL = songString.IndexOf("href=\"");
                string URL = songString.Substring(indexURL, indexSong - indexURL - 2).Replace("href=\"", "");

                HttpRequest http = new HttpRequest();
                string htmlSong = http.Get(@"https://mp3.zing.vn" + URL).ToString();

                var lyrics = Regex.Matches(htmlSong, @"<p class=""fn-wlyrics fn-content""(.*?)</p>", RegexOptions.Singleline);

                string tempLyrics = "Chua co lyric";

                if (lyrics != null && lyrics.Count != 0)
                {
                    tempLyrics = lyrics[0].ToString();
                    string temp = tempLyrics.Substring(0, tempLyrics.IndexOf('>') + 1);
                    tempLyrics = tempLyrics.Replace(temp, "").Replace("<br>", "").Replace("<p>", "").Replace("</p>", "");
                }

                string getJsonURL = Regex.Match(htmlSong, @"div id=""zplayerjs-wrapper"" class=""player mt0"" data-xml=""(.*?)""", RegexOptions.Singleline).Value.Replace(@"div id=""zplayerjs - wrapper"" class=""player mt0"" data-xml=""", "").Replace("\"", "");
                string jsonInfo = http.Get(@"https://mp3.zing.vn" + getJsonURL).ToString();
                JObject jObject = JObject.Parse(jsonInfo);

                string downloadURL = jObject["data"][0]["source_list"].ToString();
                downloadURL = downloadURL.Substring(downloadURL.Substring(downloadURL.IndexOf("http"), downloadURL.IndexOf(",") - downloadURL.IndexOf("http") - 1));
                string photoURL = jObject["data"][0]["cover"].ToString();

                listSong.Add(new Song() { SingerName = singerName, SongName = songName, SongURL = URL, STT = i + 1, Lyric = tempLyrics });//, DownloadURL = downloadURL, PhotoURL = photoURL });
            }
        }

        private void UcSong_Play_BackToMain(object sender, EventArgs e)
        {
            gridTop10.Visibility = Visibility.Visible;
            ucSong_Play.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Song song = (sender as Button).DataContext as Song;

            gridTop10.Visibility = Visibility.Hidden;
            ucSong_Play.Visibility = Visibility.Visible;

            ucSong_Play.SongInfo = song;
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
