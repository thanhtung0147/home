using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for Song_PlayUC.xaml
    /// </summary>
    public partial class Song_PlayUC : UserControl, INotifyPropertyChanged
    {
        private Song songInfo;
        public Song SongInfo { get { return songInfo; } set { songInfo = value; DownloadSong(songInfo); OnPropertyChanged("SongInfo");this.DataContext = SongInfo; } }
        public Song_PlayUC()
        {
            InitializeComponent();
            this.DataContext = SongInfo;
        }

        private event EventHandler backToMain;
        public event EventHandler BackToMain
        {
            add { backToMain += value; }
            remove { backToMain -= value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (backToMain != null)
                backToMain(this, new EventArgs());
        }

        void DownloadSong(Song songInfo)
        {
            string songName = AppDomain.CurrentDomain.BaseDirectory + "Song/" + songInfo.SongName + ".mp3";

            if (!File.Exists(songName))
            {
                WebClient wb = new WebClient();
                wb.DownloadFile(SongInfo.DownloadURL, songName);
            }
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
