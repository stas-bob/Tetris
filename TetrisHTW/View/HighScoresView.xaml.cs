using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using TetrisHTW.Util;
using System.Diagnostics;

namespace TetrisHTW.View
{
    public partial class HighScoresView : UserControl
    {
        private SQLClient sqlClient = new SQLClient("http://stl-l-18.htw-saarland.de:8080/TetrisSQLProxy/SQLProxyServlet");

        public HighScoresView()
        {
            

            InitializeComponent();

            sqlClient.requestScores(callback, error, 100);
        }

        public void callback(System.Collections.Generic.List<string> playerNames, System.Collections.Generic.List<int> levels, System.Collections.Generic.List<int> scores, System.Collections.Generic.List<string> times, System.Collections.Generic.List<int> mods)
        {
            Dispatcher.BeginInvoke(() => {
                for (int i = 0; i < playerNames.Count; i++)
                {
                    TextBlock tb = new TextBlock();
                    tb.Text = playerNames[i] + " " + scores[i] + " " + levels[i] + " " + times[i] + " " + mods[i];
                    stackPanel1.Children.Add(tb);
                }
            });
        }

        public void error(string msg)
        {
            Dispatcher.BeginInvoke(() =>
            {
                TextBlock tb = new TextBlock();
                tb.Text = msg;
                stackPanel1.Children.Add(tb);
            });
        }

    }
}
