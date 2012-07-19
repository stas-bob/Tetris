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

namespace TetrisHTW.View
{
    public partial class OptionsView : UserControl
    {
        public OptionsView()
        {
            InitializeComponent();
        }

        private void Spielen_Click(object sender, RoutedEventArgs e)
        {
            NormalTetrisView ntv = new NormalTetrisView(); 
            this.Content = ntv;
            App.getInstance().RootVisual = ntv;
        }

        

        private void Zurueck_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new IndexView();
        }
    }
}
