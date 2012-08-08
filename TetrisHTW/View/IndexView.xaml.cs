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
        private Random rnd = new Random();
        private OptionsView ov;
        private HighScoresView hv;
        private AboutView av;


        public IndexView()
        {
            InitializeComponent();

            /*hintergrundanim*/
            flyingAroundSB.Begin();
            rotatingSB.Begin();
        }


        private void Spielen_Click(object sender, RoutedEventArgs e)
        {
            if (ov == null)
            {
                ov = new OptionsView(this);
            }
            rootContainer.Child = ov;
        }

        public HighScoresView getHighScoreView()
        {
            if (hv == null)
            {
                hv = new HighScoresView(this);
            }
            return hv;
        }

        private void Highscore_Click(object sender, RoutedEventArgs e)
        {
            hv = getHighScoreView();
            hv.update(1);
            rootContainer.Child = getHighScoreView();
        }

        private void Ueber_Click(object sender, RoutedEventArgs e)
        {
            if (av == null)
            {
                av = new AboutView(this);
            }
            rootContainer.Child = av;
        }

        /*Zufällige Tos für Hintergrund*/
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

    }
}
