﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_MP3
{
    public class Song
    {
        private string songName;
        private string singerName;
        private string songURL;
        private int sTT;
        private string lyric;

        public string SongName { get => songName; set => songName = value; }
        public string SingerName { get => singerName; set => singerName = value; }
        public string SongURL { get => songURL; set => songURL = value; }
        public int STT { get => sTT; set => sTT = value; }
        public string Lyric { get => lyric; set => lyric = value; }
    }
}
