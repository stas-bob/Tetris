﻿using System;
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
    /**
     * Klasse für den Highscore View
     */
    public partial class HighScoresView : UserControl
    {
        // Attribute
        private SQLClient sqlClient = new SQLClient("http://stl-l-18.htw-saarland.de:8080/TetrisSQLProxy/SQLProxyServlet");
        private IndexView iv;
        private ScoresData tmpScoresData; //wird zur unterscheidung genutzt, ob man im gameover kontext ist oder im hauptmenü

        /**
         * Konstruktor
         */
        public HighScoresView(IndexView iv)
        {
            this.iv = iv;        

            InitializeComponent();
            
        }

        /**
         * Highscoreanzeige nach einem Spiel
         * 
         * Wird noch zusätzlich der erreichte Score angezeigt
         */
        public void update(ScoresData scoresData)
        {
            tmpScoresData = scoresData;
            switch (scoresData.mode)
            {
                case 1: normalModeRadioButton.IsChecked = true; break;
                case 2: spezialModeRadioButton.IsChecked = true; break;
                case 3: kretschmerModeRadioButton.IsChecked = true; break;
            }
            sqlClient.requestScoreEntry(callbackEntry, error, scoresData.score, scoresData.mode);
        }

        /**
         * Highscore nach dem Drücken des Highscore Button im Index View
         */
        public void update(int mode)
        {
            switch (mode)
            {
                case 1: normalModeRadioButton.IsChecked = true; break;
                case 2: spezialModeRadioButton.IsChecked = true; break;
                case 3: kretschmerModeRadioButton.IsChecked = true; break;
            }
        }

        /**
         * Callback zur Abfrage des Spielerergebnisses
         */
        public void callbackEntry(int rank)
        {
            Dispatcher.BeginInvoke(() =>
            {
                ownScoreBlock.Text = "Dein Ergebnis: Rang " + rank + " mit einem Score von " + tmpScoresData.score;
            });
        }

        /**
         * Füllen des Datagrid mit den Werten
         */
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
        
        /**
         * Error Handler
         */
        public void error(string msg)
        {
            Dispatcher.BeginInvoke(() =>
            {
                errorBlock.Text = msg;
            });
        }

        /**
         * Handler für das Dücken des "Zurück" Buttons
         */
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            tmpScoresData = null;
            normalModeRadioButton.IsChecked = false;
            spezialModeRadioButton.IsChecked = false;
            kretschmerModeRadioButton.IsChecked = false;
            errorBlock.Text = "";
            ownScoreBlock.Text = "";
            iv.rootContainer.Child = iv.LayoutRoot;
        }

        /**
         * Handler für den Moduswechsel
         */
        private void spezialModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (tmpScoresData == null)
            {
                sqlClient.requestScores(callback, error, 0, 2);
            }
            else
            {
                sqlClient.requestScores(callback, error, 50, 2);
            }
        }

        /**
         * Handler für den Moduswechsel
         */
        private void normalModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (tmpScoresData == null)
            {
                sqlClient.requestScores(callback, error, 0, 1);
            }
            else
            {
                sqlClient.requestScores(callback, error, 50, 1);
            }
        }

        /**
         * Handler für den Moduswechsel
         */
        private void kretschmerModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (tmpScoresData == null)
            {
                sqlClient.requestScores(callback, error, 0, 3);
            }
            else
            {
                sqlClient.requestScores(callback, error, 50, 3);
            }
        }
    }
}
