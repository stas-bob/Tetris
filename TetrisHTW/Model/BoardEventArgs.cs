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

namespace TetrisHTW.Model
{
    public class BoardEventArgs: EventArgs
    {
        public BoardEventArgs()
        {
            removedLines = new int[0];
        }

        
        public int[] removedLines { get; set; }
        public bool collapse { get; set; }
    }
}
