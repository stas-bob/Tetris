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
        private IndexView iv;
        private int tmpScore;

        public HighScoresView(IndexView iv)
        {
            this.iv = iv;        

            InitializeComponent();
        }

        public void update(int score)
        {
            tmpScore = score;
            sqlClient.requestScores(callbackWithScore, error, score, 10);
        }

        public void update()
        {
            sqlClient.requestScores(callback, error);
        }

        public void callbackWithScore(System.Collections.Generic.List<string> playerNames, System.Collections.Generic.List<int> levels, System.Collections.Generic.List<int> scores, System.Collections.Generic.List<string> times, System.Collections.Generic.List<int> mods)
        {
            Dispatcher.BeginInvoke(() => {

                List<ScoresData> source = new List<ScoresData>();

                for (int i = 0; i < playerNames.Count; i++)
                {
                    source.Add(new ScoresData()
                    {
                        playerName = playerNames[i],
                        score = scores[i],
                        level = levels[i],
                        time = times[i],
                        mode = mods[i]
                    });
                }
                dataGrid1.ItemsSource = source;
            });
            
        }

        public void callback(System.Collections.Generic.List<string> playerNames, System.Collections.Generic.List<int> levels, System.Collections.Generic.List<int> scores, System.Collections.Generic.List<string> times, System.Collections.Generic.List<int> mods)
        {
            Dispatcher.BeginInvoke(() => {

                List<ScoresData> source = new List<ScoresData>();

                for (int i = 0; i < playerNames.Count; i++)
                {
                    source.Add(new ScoresData()
                    {
                        playerName = playerNames[i],
                        score = scores[i],
                        level = levels[i],
                        time = times[i],
                        mode = mods[i]
                    });
                }
                dataGrid1.ItemsSource = source;
            });
            
        }


        public void error(string msg)
        {
            Dispatcher.BeginInvoke(() =>
            {
                TextBlock tb = new TextBlock();
                tb.Text = msg;
                LayoutRoot.Children.Add(tb);
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            iv.rootContainer.Child = iv.LayoutRoot;
        }

    }
}
