
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
using TetrisHTW.Model;
using System.Threading;
using System.Diagnostics;
using TetrisHTW.Figures;

namespace TetrisHTW
{
   


    public partial class MainPage : UserControl
    {

        private bool gameOver;
        private BoardModel boardModel;
        private FallWorker fallWorker;

        public MainPage()
        {
            this.boardModel = App.getInstance().getBoardModel();
            InitializeComponent();
            initBoard();
            this.KeyDown += new KeyEventHandler(Page_KeyDown);
            boardModel.BoardChanged += new BoardChangedEventHandler(BoardChanged);
            boardModel.ScoreChanged += new ScoreChangedEventHandler(ScoreChanged);
            App.getInstance().GameOverEvent += new GameOverEventHandler(GameOver);
            App.getInstance().FigureFallenEvent += new FigureFallenEventHandler(FigureFallen);

            anime.Begin();
        }



        void initBoard()
        {
            for (int i = 0; i < boardModel.getRows(); i++)
            {
                for (int j = 0; j < boardModel.getColumns(); j++)
                {
                    Rectangle rect = new Rectangle();
                    rect.SetValue(Grid.RowProperty, i);
                    rect.SetValue(Grid.ColumnProperty, j);
                    rect.Stroke = new SolidColorBrush(Color.FromArgb(50, 0, 0, 255));
                    DoubleCollection dc = new DoubleCollection();
                    dc.Add(10);
                    rect.StrokeDashArray = dc;
                    rect.StrokeThickness = 1;
                    boardGrid.Children.Add(rect);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Rectangle rect = new Rectangle();
                    rect.SetValue(Grid.RowProperty, i);
                    rect.SetValue(Grid.ColumnProperty, j);
                    previewGrid.Children.Add(rect);
                }
            }
        }

        void AnimCompleted(object sender, EventArgs e)
        {
            Random rnd = new Random();
            dAnimeX.To = rnd.Next(200);
            dAnimeY.To = rnd.Next(200);
            dAnimeX2.To = rnd.Next(200);
            dAnimeY2.To = rnd.Next(200);
            anime.Begin();
        }

        void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (!gameOver)
            {
                switch (e.Key)
                {
                    case Key.Left: boardModel.getCurrentFigure().left(); break;
                    case Key.Right: boardModel.getCurrentFigure().right(); break;
                    case Key.Up: boardModel.getCurrentFigure().rotate(); break;
                    case Key.Down: boardModel.getCurrentFigure().fall(); break;
                    case Key.Space: boardModel.getCurrentFigure().fallCompletely(); break;
                }
            }
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (fallWorker != null)
            {
                fallWorker.RequestStop();
            }
            gameOver = false;
            fallWorker = new FallWorker();
            boardModel.clearBoard();
            Figure preview = boardModel.generateRandomFigure();
            Figure current = boardModel.generateRandomFigure();

            boardModel.setCurrentFigure(current);
            boardModel.setPreviewFigure(preview);

            boardModel.getCurrentFigure().newOnBoard();
            
            new Thread(fallWorker.InvokeFalling).Start();
        } 

        public void BoardChanged(object sender, BoardEventArgs bea)
        {
            Dispatcher.BeginInvoke(delegate
            {
                Color[,] data = boardModel.getBoardData();
                foreach(FrameworkElement frameWorkElement in boardGrid.Children)
                {
                    Rectangle rect = (Rectangle)frameWorkElement;
                    int x = Grid.GetColumn(frameWorkElement);
                    int y = Grid.GetRow(frameWorkElement);
                    if (!data[x, y].Equals(boardModel.getColor()))
                    {
                        Brush b = null;
                        if (data[x, y] == Colors.Blue)
                        {
                            b = Application.Current.Resources["BluePointBrush"] as Brush;
                        }
                        if (data[x, y] == Colors.Yellow)
                        {
                            b = Application.Current.Resources["YellowPointBrush"] as Brush;
                        }
                        if (data[x, y] == Colors.Green)
                        {
                            b = Application.Current.Resources["GreenPointBrush"] as Brush;
                        }
                        if (data[x, y] == Colors.Red)
                        {
                            b = Application.Current.Resources["RedPointBrush"] as Brush;
                        }
                        if (data[x, y] == Colors.Purple)
                        {
                            b = Application.Current.Resources["PurplePointBrush"] as Brush;
                        }
                        if (data[x, y] == Colors.Cyan)
                        {
                            b = Application.Current.Resources["CyanPointBrush"] as Brush;
                        }
                        if (data[x, y] == Colors.Orange)
                        {
                            b = Application.Current.Resources["OrangePointBrush"] as Brush;
                        }
                         

                        rect.Fill = b;
                    }
                    else
                    {
                        rect.Fill = new SolidColorBrush(Colors.Transparent);
                    }

                }
                foreach (FrameworkElement frameWorkElement in previewGrid.Children)
                {
                        Rectangle rect = (Rectangle)frameWorkElement;
                        rect.Fill = new SolidColorBrush(Colors.White);
                }
                tools.Point[] previewPoints = boardModel.getPreviewFigure().getPoints();
                foreach (FrameworkElement frameWorkElement in previewGrid.Children)
                {
                    Rectangle rect = (Rectangle)frameWorkElement;
                    rect.Fill = new SolidColorBrush(Colors.White);
                }
                for (int i = 0; i < previewPoints.Length; i++)
                {
                    foreach (FrameworkElement frameWorkElement in previewGrid.Children)
                    {
                        int x = Grid.GetColumn(frameWorkElement) + 4;
                        int y = Grid.GetRow(frameWorkElement);
                        if (previewPoints[i].X == x && previewPoints[i].Y == y)
                        {
                            Rectangle rect = (Rectangle)frameWorkElement;
                            LinearGradientBrush brush = new LinearGradientBrush();
                            brush.GradientStops = new GradientStopCollection();
                            GradientStop gs = new GradientStop();
                            gs.Color = boardModel.getPreviewFigure().getColor();
                            gs.Offset = 0;
                            brush.GradientStops.Add(gs);
                            GradientStop gs2 = new GradientStop();
                            gs.Color = Colors.White;
                            gs.Offset = 0.4;
                            brush.GradientStops.Add(gs2);
                            GradientStop gs3 = new GradientStop();
                            gs.Color = Colors.White;
                            gs.Offset = 0.45;
                            brush.GradientStops.Add(gs3);
                            GradientStop gs4 = new GradientStop();
                            gs.Color = boardModel.getPreviewFigure().getColor();
                            gs.Offset = 1;
                            brush.GradientStops.Add(gs4);

                            rect.Fill = brush;
                        }
                    }
                }
            });
        }

        public void ScoreChanged(object sender, ScoreEventArgs bea)
        {
            Dispatcher.BeginInvoke(delegate
            {
                scoreText.Text = bea.score + "";
            });
            
        }

        public void FigureFallen(object sender, FigureFallenEventArgs ffea)
        {
            Dispatcher.BeginInvoke(delegate
            {
                tools.Point[] points = ffea.points;
                List<Rectangle> rectangles = getBoardRectangles(points);
                Storyboard sb = new Storyboard();
                foreach (Rectangle rectangle in rectangles)
                {
                    Duration duration = new Duration(TimeSpan.FromMilliseconds(100));
                    DoubleAnimation myDoubleAnimation = new DoubleAnimation();
                    myDoubleAnimation.Duration = duration;
                    myDoubleAnimation.AutoReverse = true;
                    Storyboard.SetTarget(myDoubleAnimation, rectangle);
                    Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath("Opacity"));
                    myDoubleAnimation.To = 0.5;
                    myDoubleAnimation.From = 1;
                    sb.Children.Add(myDoubleAnimation);
                }
                if (!LayoutRoot.Resources.Contains("unique_id"))
                {
                    LayoutRoot.Resources.Add("unique_id", sb);
                }
                sb.Begin();
            });

        }

        public List<Rectangle> getBoardRectangles(tools.Point[] points)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            foreach (tools.Point point in points)
            {
                foreach (FrameworkElement frameWorkElement in boardGrid.Children)
                {
                    int xx = Grid.GetColumn(frameWorkElement);
                    int yy = Grid.GetRow(frameWorkElement);
                    if (xx == point.X && yy == point.Y)
                    {
                        rectangles.Add((Rectangle)frameWorkElement);
                    }
                }
            }
            return rectangles;
        }

        public void GameOver(object sender, GameOverEventArgs goea)
        {
            Dispatcher.BeginInvoke(delegate
            {
                Debug.WriteLine("game over");
                fallWorker.RequestStop();
                scoreText.Text = "";
                gameOver = true;
            });

        }

    }
}
