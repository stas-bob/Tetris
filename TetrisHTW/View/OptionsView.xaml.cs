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
using System.Windows.Controls.Primitives;

namespace TetrisHTW.View
{
    public partial class OptionsView : UserControl
    {
        private Random rnd = new Random();
        private IndexView iv;
        private NormalTetrisView ntv;
        //private SpecialTetrisView stv;
        private int level = 0;

        public OptionsView(IndexView iv)
        {
            InitializeComponent();
            this.iv = iv;
            anleitung.Text = "Dies ist das orginal Tetris wie man es vom Gameboy kennt!"; 
        }

        private void Spielen_Click(object sender, RoutedEventArgs e)
        {
            
            int mode = 1;
            if (normalModeRadioButton.IsChecked == true)
            {
                mode = 1;
            }
            else if (spezialModeRadioButton.IsChecked == true)
            {
                mode = 2;
            }
            else if (kretschmerModeRadioButton.IsChecked == true)
            {
                mode = 3;
            }
            if (ntv == null)
            {
                ntv = new NormalTetrisView(this, iv);
            }
            bool unknown = false;
            if (playerNameTextBox.Text == null || playerNameTextBox.Text.Equals(""))
            {
                unknown = true;
            }
            if (!unknown)
            {
                ntv.setPlayerName(playerNameTextBox.Text);
            }
            ntv.setTempLines((level + 1) * 10);
            ntv.InitGame();
            ntv.setMode(mode);
            iv.rootContainer.Child = ntv;
        }

        

        private void Zurueck_Click(object sender, RoutedEventArgs e)
        {
            iv.rootContainer.Child = iv.LayoutRoot;
        }

        

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            tb0.IsChecked = false;
            tb1.IsChecked = false;
            tb2.IsChecked = false;
            tb3.IsChecked = false;
            tb4.IsChecked = false;
            tb5.IsChecked = false;
            tb6.IsChecked = false;
            tb7.IsChecked = false;
            tb8.IsChecked = false;
            tb9.IsChecked = false;

            switch (((ToggleButton)sender).Name)
            {
                case "tb0": tb0.IsChecked = true; level = 0; break;
                case "tb1": tb1.IsChecked = true; level = 1; break;
                case "tb2": tb2.IsChecked = true; level = 2; break;
                case "tb3": tb3.IsChecked = true; level = 3; break;
                case "tb4": tb4.IsChecked = true; level = 4; break;
                case "tb5": tb5.IsChecked = true; level = 5; break;
                case "tb6": tb6.IsChecked = true; level = 6; break;
                case "tb7": tb7.IsChecked = true; level = 7; break;
                case "tb8": tb8.IsChecked = true; level = 8; break;
                case "tb9": tb9.IsChecked = true; level = 9; break; 
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (normalModeRadioButton.IsChecked == true)
            {
                memoryKey.Visibility = Visibility.Collapsed;
                anleitung.Text = "Dies ist das orginal Tetris wie man es vom Gameboy kennt!"; 
            }
            else if (spezialModeRadioButton.IsChecked == true)
            {
                memoryKey.Visibility = Visibility.Visible;
                anleitung.Text = "Dies ist das orginal Tetris mit der Erweiterung, dads man Steine speichern kann."; 
            }
            else 
            {
                memoryKey.Visibility = Visibility.Collapsed;
                anleitung.Text = "Dies ist das orginal Tetris mit der Erweiterung, dads sich das Spielfeld nach entfernen einer Zeile dreht."; 
            }
        
        }
    }
}
