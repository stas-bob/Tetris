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
    public class ScoreEventArgs: EventArgs
    {
        public delegate void ScoreChangedEventHandler(object sender, ScoreEventArgs e);
        public int score { get; set; }
    }
}
