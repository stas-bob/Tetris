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
        private bool gameOver;

        public HighScoresView(IndexView iv)
        {
            this.iv = iv;        

            InitializeComponent();
        }

        public void update(int score, int mode)
        {
            gameOver = true;
            switch (mode)
            {
                case 1: normalModeRadioButton.IsChecked = true; break;
                case 2: spezialModeRadioButton.IsChecked = true; break;
                case 3: kretschmerModeRadioButton.IsChecked = true; break;
            }
            sqlClient.requestScoreEntry(callbackEntry, error, score, mode);
        }

        public void update(int mode)
        {
            switch (mode)
            {
                case 1: normalModeRadioButton.IsChecked = true; break;
                case 2: spezialModeRadioButton.IsChecked = true; break;
                case 3: kretschmerModeRadioButton.IsChecked = true; break;
            }
        }

        public void callbackEntry(string playerName, int level, int score, string time, int mode, int rank)
        {
            Dispatcher.BeginInvoke(() =>
            {
                ownScoreBlock.Text = "dein ergebnis: rank: " + rank + " score: " + score + " level " + level + " time " + time + " mode " + mode;
            });
        }

        public void callback(System.Collections.Generic.List<string> playerNames,
            System.Collections.Generic.List<int> levels,
            System.Collections.Generic.List<int> scores,
            System.Collections.Generic.List<string> times,
            System.Collections.Generic.List<int> modes,
            System.Collections.Generic.List<int> ranks)
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
                        mode = modes[i],
                        rank = ranks[i]
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
            gameOver = false;
            normalModeRadioButton.IsChecked = false;
            spezialModeRadioButton.IsChecked = false;
            kretschmerModeRadioButton.IsChecked = false;
            iv.rootContainer.Child = iv.LayoutRoot;
        }

        private void spezialModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (gameOver)
            {
                sqlClient.requestScores(callback, error, 5, 2);
            }
            else
            {
                sqlClient.requestScores(callback, error, 2);
            }
        }

        private void normalModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (gameOver)
            {
                sqlClient.requestScores(callback, error, 5, 1);
            }
            else
            {
                sqlClient.requestScores(callback, error, 1);
            }
        }

        private void kretschmerModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (gameOver)
            {
                sqlClient.requestScores(callback, error, 5, 3);
            }
            else
            {
                sqlClient.requestScores(callback, error, 3);
            }
        }

    }
}
