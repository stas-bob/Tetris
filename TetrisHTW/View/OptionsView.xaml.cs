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

        public OptionsView()
        {
            InitializeComponent();
            flyingAroundSB.Begin();
            rotatingSB.Begin();
        }

        private void Spielen_Click(object sender, RoutedEventArgs e)
        {
            bool unknown = false;
            if (playerNameTextBox.Text == null || playerNameTextBox.Text.Equals(""))
            {
                unknown = true;
            }
            NormalTetrisView ntv = new NormalTetrisView(unknown ? "Unbekannt" : playerNameTextBox.Text); 
            this.Content = ntv;
            App.getInstance().RootVisual = ntv;
        }

        

        private void Zurueck_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new IndexView();
        }

        void AnimCompleted(object sender, EventArgs e)
        {
            Point p = canvas.TransformToVisual(App.getInstance().RootVisual).Transform(new Point(0, 0));
            animXO.To = rnd.Next((int)App.getInstance().RootVisual.RenderSize.Width) - p.X;
            animYO.To = rnd.Next((int)App.getInstance().RootVisual.RenderSize.Height) - p.Y;
            animXT.To = rnd.Next((int)App.getInstance().RootVisual.RenderSize.Width) - p.X;
            animYT.To = rnd.Next((int)App.getInstance().RootVisual.RenderSize.Height) - p.Y;
            animXL.To = rnd.Next((int)App.getInstance().RootVisual.RenderSize.Width) - p.X;
            animYL.To = rnd.Next((int)App.getInstance().RootVisual.RenderSize.Height) - p.Y;
            animXJ.To = rnd.Next((int)App.getInstance().RootVisual.RenderSize.Width) - p.X;
            animYJ.To = rnd.Next((int)App.getInstance().RootVisual.RenderSize.Height) - p.Y;
            animXI.To = rnd.Next((int)App.getInstance().RootVisual.RenderSize.Width) - p.X;
            animYI.To = rnd.Next((int)App.getInstance().RootVisual.RenderSize.Height) - p.Y;
            animXS.To = rnd.Next((int)App.getInstance().RootVisual.RenderSize.Width) - p.X;
            animYS.To = rnd.Next((int)App.getInstance().RootVisual.RenderSize.Height) - p.Y;
            animXZ.To = rnd.Next((int)App.getInstance().RootVisual.RenderSize.Width) - p.X;
            animYZ.To = rnd.Next((int)App.getInstance().RootVisual.RenderSize.Height) - p.Y;

            flyingAroundSB.Begin();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
         

                switch (((ToggleButton)sender).Name)
                {
                    case "tb0": tb0.IsChecked= true; tb1.IsChecked= false; tb2.IsChecked= false; tb3.IsChecked= false; tb4.IsChecked= false; tb5.IsChecked= false; tb6.IsChecked= false; tb7.IsChecked= false; tb8.IsChecked= false; tb9.IsChecked= false; break; 
                    case "tb1": tb0.IsChecked= false; tb1.IsChecked= true; tb2.IsChecked= false; tb3.IsChecked= false; tb4.IsChecked= false; tb5.IsChecked= false; tb6.IsChecked= false; tb7.IsChecked= false; tb8.IsChecked= false; tb9.IsChecked= false; break; 
                    case "tb2": tb0.IsChecked= false; tb1.IsChecked= false; tb2.IsChecked= true; tb3.IsChecked= false; tb4.IsChecked= false; tb5.IsChecked= false; tb6.IsChecked= false; tb7.IsChecked= false; tb8.IsChecked= false; tb9.IsChecked= false; break; 
                    case "tb3": tb0.IsChecked= false; tb1.IsChecked= false; tb2.IsChecked= false; tb3.IsChecked= true; tb4.IsChecked= false; tb5.IsChecked= false; tb6.IsChecked= false; tb7.IsChecked= false; tb8.IsChecked= false; tb9.IsChecked= false; break; 
                    case "tb4": tb0.IsChecked= false; tb1.IsChecked= false; tb2.IsChecked= false; tb3.IsChecked= false; tb4.IsChecked= true; tb5.IsChecked= false; tb6.IsChecked= false; tb7.IsChecked= false; tb8.IsChecked= false; tb9.IsChecked= false; break; 
                    case "tb5": tb0.IsChecked= false; tb1.IsChecked= false; tb2.IsChecked= false; tb3.IsChecked= false; tb4.IsChecked= false; tb5.IsChecked= true; tb6.IsChecked= false; tb7.IsChecked= false; tb8.IsChecked= false; tb9.IsChecked= false; break; 
                    case "tb6": tb0.IsChecked= false; tb1.IsChecked= false; tb2.IsChecked= false; tb3.IsChecked= false; tb4.IsChecked= false; tb5.IsChecked= false; tb6.IsChecked= true; tb7.IsChecked= false; tb8.IsChecked= false; tb9.IsChecked= false; break; 
                    case "tb7": tb0.IsChecked= false; tb1.IsChecked= false; tb2.IsChecked= false; tb3.IsChecked= false; tb4.IsChecked= false; tb5.IsChecked= false; tb6.IsChecked= false; tb7.IsChecked= true; tb8.IsChecked= false; tb9.IsChecked= false; break; 
                    case "tb8": tb0.IsChecked= false; tb1.IsChecked= false; tb2.IsChecked= false; tb3.IsChecked= false; tb4.IsChecked= false; tb5.IsChecked= false; tb6.IsChecked= false; tb7.IsChecked= false; tb8.IsChecked= true; tb9.IsChecked= false; break; 
                    case "tb9": tb0.IsChecked= false; tb1.IsChecked= false; tb2.IsChecked= false; tb3.IsChecked= false; tb4.IsChecked= false; tb5.IsChecked= false; tb6.IsChecked= false; tb7.IsChecked= false; tb8.IsChecked= false; tb9.IsChecked= true; break; 
                   
                }
        }
    }
}
