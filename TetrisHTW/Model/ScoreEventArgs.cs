﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TetrisHTW.Model
{
    /**
    * Eventklassse für den Score und das Level
    */
    public class ScoreEventArgs: EventArgs
    {
        
        public int score { get; set; }
        public int level { get; set; }
    }
}
