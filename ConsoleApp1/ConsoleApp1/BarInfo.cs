﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class BarInfo
    {
        public  string BarName { get; set; }
        public  double Lat { get; set; }
        public  double Lng { get; set; }
        public string Phone { get; set; }
        public string WorkTime { get; set; }
        public  List<string> PictureLinks { get; set; } = new List<string>();

    }
}