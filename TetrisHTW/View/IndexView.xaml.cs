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

namespace TetrisHTW.View
{
    public partial class IndexView : UserControl
    {
        public IndexView()
        {
            InitializeComponent();
        }


        private void Spielen_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new OptionsView();
        }

        private void Highscore_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Ueber_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
