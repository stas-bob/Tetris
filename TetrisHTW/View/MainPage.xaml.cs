
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

        private bool gameStop;
        private BoardModel boardModel;
        private FallWorker fallWorker;
        private Timer timer;
        private Key lastKey;

        public MainPage()
        {
            
            this.boardModel = App.getInstance().getBoardModel();
            InitializeComponent();
            initBoard();
            this.KeyDown += new KeyEventHandler(Page_KeyDown);
            this.KeyUp += new KeyEventHandler(Page_KeyUp);

            boardModel.BoardChanged += new BoardChangedEventHandler(BoardChanged);
            boardModel.ScoreChanged += new ScoreChangedEventHandler(ScoreChanged);
            App.getInstance().GameOverEvent += new GameOverEventHandler(GameOver);
            App.getInstance().FigureFallenEvent += new FigureFallenEventHandler(FigureFallen);

            /*Circles Background*/
            anime.Begin();
        }

       


        void initBoard()
        {
            for (int i = 0; i < boardModel.getColumns(); i++)
            {
                boardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < boardModel.getRows(); i++)
            {
                boardGrid.RowDefinitions.Add(new RowDefinition());
            }
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
            for (int i = 0; i < previewGrid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < previewGrid.ColumnDefinitions.Count; j++)
                {
                    Rectangle rect = new Rectangle();
                    rect.SetValue(Grid.RowProperty, i);
                    rect.SetValue(Grid.ColumnProperty, j);
                    previewGrid.Children.Add(rect);
                }
            }
        }

        /*Circles Background*/
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
            if (!gameStop)
            {
                switch (e.Key)
                {
                    case Key.Up: boardModel.getCurrentFigure().rotate(); break;
                    case Key.Space: boardModel.getCurrentFigure().fallCompletely(); break;
                    default: 
                        if (timer == null)
                        {
                            lastKey = e.Key;
                            timer = new Timer(MoveFigure, null, 0, 150);
                        }
                        break;
                }
               
            }
        }

        void Page_KeyUp(object sender, KeyEventArgs e)
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        public void MoveFigure(Object stateInfo)
        {
            switch (lastKey)
            {
                case Key.Left: boardModel.getCurrentFigure().left(); break;
                case Key.Right: boardModel.getCurrentFigure().right(); break;
                case Key.Down: boardModel.getCurrentFigure().fall(); break;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            StopGame();
            boardModel.setScore(0);
            
            boardModel.clearBoard();
            Figure preview = boardModel.generateRandomFigure();
            Figure current = boardModel.generateRandomFigure();

            boardModel.setCurrentFigure(current);
            boardModel.setPreviewFigure(preview);

            boardModel.getCurrentFigure().newOnBoard();

            GameStart();
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
                    Color currentFigureColor = data[x, y];
                    if (currentFigureColor != boardModel.getColor())
                    {
                        Brush b = null;
                        if (currentFigureColor == Colors.Blue)
                        {
                            b = Application.Current.Resources["BluePointBrush"] as Brush;
                        } 
                        else if (currentFigureColor == Colors.Yellow)
                        {
                            b = Application.Current.Resources["YellowPointBrush"] as Brush;
                        } 
                        else if (currentFigureColor == Colors.Green)
                        {
                            b = Application.Current.Resources["GreenPointBrush"] as Brush;
                        } 
                        else if (currentFigureColor == Colors.Red)
                        {
                            b = Application.Current.Resources["RedPointBrush"] as Brush;
                        } 
                        else if (currentFigureColor == Colors.Purple)
                        {
                            b = Application.Current.Resources["PurplePointBrush"] as Brush;
                        } 
                        else if (currentFigureColor == Colors.Cyan)
                        {
                            b = Application.Current.Resources["CyanPointBrush"] as Brush;
                        } 
                        else if (currentFigureColor == Colors.Orange)
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
                tools.Point[] previewPoints = boardModel.getPreviewFigure().getPoints();

                List<Rectangle> previewRectangles = getPreviewBoardRectangles(previewPoints);

                foreach (FrameworkElement frameWorkElement in previewGrid.Children)
                {
                    Rectangle rect = (Rectangle)frameWorkElement;
                    rect.Fill = new SolidColorBrush(Colors.Transparent);
                }
                Color currentPreviewFigureColor = boardModel.getPreviewFigure().getColor();
                foreach (Rectangle rect in previewRectangles)
                {
                    Brush b = null;
                    if (currentPreviewFigureColor == Colors.Blue)
                    {
                        b = Application.Current.Resources["BluePointBrush"] as Brush;
                    } 
                    else if (currentPreviewFigureColor == Colors.Yellow)
                    {
                        b = Application.Current.Resources["YellowPointBrush"] as Brush;
                    } 
                    else  if (currentPreviewFigureColor == Colors.Green)
                    {
                        b = Application.Current.Resources["GreenPointBrush"] as Brush;
                    }
                    else if (currentPreviewFigureColor == Colors.Red)
                    {
                        b = Application.Current.Resources["RedPointBrush"] as Brush;
                    }
                    else if (currentPreviewFigureColor == Colors.Purple)
                    {
                        b = Application.Current.Resources["PurplePointBrush"] as Brush;
                    }
                    else if (currentPreviewFigureColor == Colors.Cyan)
                    {
                        b = Application.Current.Resources["CyanPointBrush"] as Brush;
                    }
                    else if (currentPreviewFigureColor == Colors.Orange)
                    {
                        b = Application.Current.Resources["OrangePointBrush"] as Brush;
                    }

                    rect.Fill = b;
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
                tools.Point[] points = ffea.figurePoints;
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
                    int xx = Grid.GetColumn(frameWorkElement) ;
                    int yy = Grid.GetRow(frameWorkElement);
                    if (xx == point.X && yy == point.Y)
                    {
                        rectangles.Add((Rectangle)frameWorkElement);
                    }
                }
            }
            return rectangles;
        }

        public List<Rectangle> getPreviewBoardRectangles(tools.Point[] points)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            foreach (tools.Point point in points)
            {
                foreach (FrameworkElement frameWorkElement in previewGrid.Children)
                {
                    int xx = Grid.GetColumn(frameWorkElement) + (boardGrid.ColumnDefinitions.Count/2 - previewGrid.ColumnDefinitions.Count/2);
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
                scoreText.Text = "game over";
                StopGame();
            });
        }

        public void StopGame()
        {
            if (fallWorker != null)
            {
                fallWorker.RequestStop();
            }
            gameStop = true;
        }

        public void GameStart()
        {
            gameStop = false;
            fallWorker = new FallWorker();
            new Thread(fallWorker.InvokeFalling).Start();
        }
    }
}
