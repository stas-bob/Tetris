using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TetrisHTW.Util
{
    public class ScoresData
    {
        public string playerName { get; set; }
        public int score { get; set; }
        public int level { get; set; }
        public int mode { get; set; }
        public string time { get; set; }
    }
}
