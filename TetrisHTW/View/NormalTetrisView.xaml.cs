﻿
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
   


    public partial class NormalTetrisView : UserControl
    {

        private bool gameStop;
        private BoardModel boardModel;
        private FallWorker fallWorker;
        private Timer timer;
        private Key lastKey;
        private int previousLevel;
        private Random rnd = new Random();

        public NormalTetrisView()
        {
            
            this.boardModel = App.getInstance().getBoardModel();
            InitializeComponent();
            /*Hier werden die Grids initialisiert*/
            initBoard();
            /*Das sind die Listener für das Keyboard*/
            App.getInstance().RootVisual.KeyDown += new KeyEventHandler(Page_KeyDown);
            App.getInstance().RootVisual.KeyUp += new KeyEventHandler(Page_KeyUp);
            /* Hier kommen die Event Listener fuers Spiel*/
            boardModel.BoardChanged += new BoardChangedEventHandler(BoardChanged);
            boardModel.ScoreChanged += new ScoreChangedEventHandler(ScoreChanged);
            boardModel.LineChanged += new LineChangedEventHandler(LineChanged);
            App.getInstance().GameOverEvent += new GameOverEventHandler(GameOver);
            App.getInstance().FigureFallenEvent += new FigureFallenEventHandler(FigureFallen);

            this.InitGame();
            /*Circles Background*/
            anime.Begin();
            anime2.Begin();

        }

        void initBoard()
        {
            /*Hier werden die Columns und Rows definiert*/
            for (int i = 0; i < boardModel.getColumns(); i++)
            {
                boardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < boardModel.getRows(); i++)
            {
                boardGrid.RowDefinitions.Add(new RowDefinition());
            }
            /* Die Rechtecke für das Grid werden hier erstellt*/
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
            /*Auch hier Rechtecke ins Grid-Vorschau*/
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
            dAnimeX.To = rnd.Next((int)boardGrid.ActualWidth);
            dAnimeY.To = rnd.Next((int)boardGrid.ActualHeight);
            dAnimeX2.To = rnd.Next((int)boardGrid.ActualWidth);
            dAnimeY2.To = rnd.Next((int)boardGrid.ActualHeight);
            dAnimeX3.To = rnd.Next((int)boardGrid.ActualWidth);
            dAnimeY3.To = rnd.Next((int)boardGrid.ActualHeight);
            dAnimeX4.To = rnd.Next((int)boardGrid.ActualWidth);
            dAnimeY4.To = rnd.Next((int)boardGrid.ActualHeight);
            dAnimeX5.To = rnd.Next((int)boardGrid.ActualWidth);
            dAnimeY5.To = rnd.Next((int)boardGrid.ActualHeight);
            dAnimeX6.To = rnd.Next((int)boardGrid.ActualWidth);
            dAnimeY6.To = rnd.Next((int)boardGrid.ActualHeight);
            dAnimeX7.To = rnd.Next((int)boardGrid.ActualWidth);
            dAnimeY7.To = rnd.Next((int)boardGrid.ActualHeight);
            dAnimeX8.To = rnd.Next((int)boardGrid.ActualWidth);
            dAnimeY8.To = rnd.Next((int)boardGrid.ActualHeight);
            anime.Begin();
        }

        /*KeyboardListener fuer druecken einer Taste*/
        void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (!gameStop)
            {
                switch (e.Key)
                {
                    case Key.Up: boardModel.getCurrentFigure().rotate(); break;
                    case Key.Space: boardModel.getCurrentFigure().fallCompletely(); break;
                    default: 
                        /* Dieser Timer ist fuer das fluessige links rechts und nach unten Bewegen */
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

        private void InitGame()
        {
            StopGame();
            FallWorker.Instance.setLevel(0);
            boardModel.setScore(0);
            boardModel.setLines(0);
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
                Color[,] data = bea.boardData;
                if (bea.removedLines.Count > 0)
                {
                    animateRemovedLines(bea.removedLines);
                    
                }
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

        private void animateRemovedLines(List<int> removedLines)
        {
            Storyboard sb = new Storyboard();
            foreach (int y in removedLines)
            {
                for (int i = 0; i < boardModel.getColumns(); i++)
                {
                    Rectangle rectOnBoard = getRectangleAt(i, y);
                    Rectangle rect = new Rectangle();
                    rect.Height = rectOnBoard.ActualHeight;
                    rect.Width = rectOnBoard.ActualWidth;
                    rect.Fill = rectOnBoard.Fill;

                    Point p = rectOnBoard.TransformToVisual(this.LayoutRoot).Transform(new Point(0, 0));

                    canvas.Children.Add(rect);
                    Canvas.SetLeft(rect, p.X + Canvas.GetLeft(LayoutRoot));
                    Canvas.SetTop(rect, p.Y + Canvas.GetTop(LayoutRoot));

                    DoubleAnimationUsingKeyFrames dauk = new DoubleAnimationUsingKeyFrames();
                    Duration duration = new Duration(TimeSpan.FromMilliseconds(1000));
                    dauk.Duration = duration;
                    Storyboard.SetTarget(dauk, rect);
                    Storyboard.SetTargetProperty(dauk, new PropertyPath("(Canvas.Top)"));

                    SplineDoubleKeyFrame sdk = new SplineDoubleKeyFrame();
                    sdk.SetValue(SplineDoubleKeyFrame.ValueProperty, p.Y + (Canvas.GetTop(boardGrid) + boardGrid.ActualHeight - p.Y) + rnd.Next(150));
                    KeyTime kt = TimeSpan.FromMilliseconds(1000);
                    KeySpline ks = new KeySpline();
                    ks.ControlPoint1 = new Point(0, 0.5);
                    ks.ControlPoint2 = new Point(1, 0.1);
                    sdk.KeySpline = ks;
                    sdk.KeyTime = kt;
                    dauk.KeyFrames.Add(sdk);

                    DoubleAnimationUsingKeyFrames dauk2 = new DoubleAnimationUsingKeyFrames();
                    dauk2.Duration = duration;
                    Storyboard.SetTarget(dauk2, rect);
                    Storyboard.SetTargetProperty(dauk2, new PropertyPath("Opacity"));

                    SplineDoubleKeyFrame sdk2 = new SplineDoubleKeyFrame();
                    sdk2.SetValue(SplineDoubleKeyFrame.ValueProperty, 0.0);
                    KeyTime kt2 = TimeSpan.FromMilliseconds(1000);
                    KeySpline ks2 = new KeySpline();
                    ks2.ControlPoint1 = new Point(0, 0.5);
                    ks2.ControlPoint2 = new Point(1, 0.5);
                    sdk2.KeySpline = ks2;
                    sdk2.KeyTime = kt2;
                    dauk2.KeyFrames.Add(sdk2);
                    sb.Children.Add(dauk);
                    sb.Children.Add(dauk2);
                }
            }
            if (!LayoutRoot.Resources.Contains("unique_id"))
            {
                LayoutRoot.Resources.Add("unique_id", sb);
            }
            sb.Begin();
        }

        public void ScoreChanged(object sender, ScoreEventArgs bea)
        {
            Dispatcher.BeginInvoke(delegate
            {
                scoreText.Text = bea.score + "";
                levelText.Text = bea.level + "";
                if (previousLevel != bea.level)
                {
                    FallWorker.Instance.setLevel(bea.level);
                    previousLevel = bea.level;
                }
            });
            
        }

        public void LineChanged(object sender, LineEventArgs lea)
        {
            Dispatcher.BeginInvoke(delegate
            {
                linesText.Text = lea.lines + "";
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
                    Duration duration = new Duration(TimeSpan.FromMilliseconds(200));
                    DoubleAnimation myDoubleAnimation = new DoubleAnimation();
                    myDoubleAnimation.Duration = duration;
                    myDoubleAnimation.AutoReverse = true;
                    Storyboard.SetTarget(myDoubleAnimation, rectangle);
                    Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath("Opacity"));
                    myDoubleAnimation.To = 0.5;
                    myDoubleAnimation.From = 1;
                    sb.Children.Add(myDoubleAnimation);
                }
                if (!ffea.PointsAreEqual())
                {
                    int maxY = getMax(points, false);
                    int minY = getMin(ffea.previousFigurePoints, false);
                    int minX = getMin(points, true);
                    int maxX = getMax(points, true);

                    Rectangle upperLeftRect = getRectangleAt(minX, minY);
                    Rectangle bottomRightRect = getRectangleAt(maxX, maxY);

                    Point upperLeftPoint = upperLeftRect.TransformToVisual(this.LayoutRoot).Transform(new Point(0, 0));
                    Point bottomRightPoint = bottomRightRect.TransformToVisual(this.LayoutRoot).Transform(new Point(0, 0));

                    int width = (int)(bottomRightPoint.X - upperLeftPoint.X) + (int)upperLeftRect.ActualWidth;
                    int height = (int)(bottomRightPoint.Y - upperLeftPoint.Y) + (int)upperLeftRect.ActualHeight;

                    Rectangle effectRectangle = new Rectangle();
                    effectRectangle.Width = width;
                    effectRectangle.Height = height;


                    LinearGradientBrush lgb = new LinearGradientBrush();
                    RotateTransform rt = new RotateTransform();
                    rt.Angle = 20;
                    lgb.Transform = rt;
                    GradientStop gs1 = new GradientStop();
                    gs1.Color = Colors.Transparent;
                    gs1.Offset = 0.0;
                    GradientStop gs2 = new GradientStop();
                    gs2.Color = Color.FromArgb(100, ffea.color.R, ffea.color.G, ffea.color.B);
                    gs2.Offset = 0.0;
                    lgb.GradientStops.Add(gs1);
                    lgb.GradientStops.Add(gs2);

                    effectRectangle.Fill = lgb;
                    canvas.Children.Add(effectRectangle);
                    Canvas.SetLeft(effectRectangle, upperLeftPoint.X + Canvas.GetLeft(LayoutRoot));
                    Canvas.SetTop(effectRectangle, upperLeftPoint.Y + Canvas.GetTop(LayoutRoot));

                    Duration duration = new Duration(TimeSpan.FromMilliseconds(3000));
                    DoubleAnimation myDoubleAnimation = new DoubleAnimation();
                    myDoubleAnimation.Duration = duration;
                    Storyboard.SetTarget(myDoubleAnimation, gs2);
                    Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath("Offset"));
                    myDoubleAnimation.To = 10.0;
                    sb.Children.Add(myDoubleAnimation);
                    sb.Completed += new EventHandler((a, b) =>
                    {
                        canvas.Children.Remove(effectRectangle);
                    });
                }
                if (!LayoutRoot.Resources.Contains("unique_id"))
                {
                    LayoutRoot.Resources.Add("unique_id", sb);
                }
                sb.Begin();

            });

        }


        public int getMax(tools.Point[] points, bool x)
        {
            if (!x)
            {

                int max = points[0].Y;
                for (int i = 0; i < points.Length; i++)
                {
                    if (points[i].Y > max)
                    {
                        max = points[i].Y;
                    }
                }
                return max;
            }
            else
            {
                int max = points[0].X;
                for (int i = 0; i < points.Length; i++)
                {
                    if (points[i].X > max)
                    {
                        max = points[i].X;
                    }
                }
                return max;
            }
        }

        public int getMin(tools.Point[] points, bool x)
        {
            if (!x)
            {
                int min = points[0].Y;
                for (int i = 0; i < points.Length; i++)
                {
                    if (points[i].Y < min)
                    {
                        min = points[i].Y;
                    }
                }
                return min;
            }
            else
            {
                int min = points[0].X;
                for (int i = 0; i < points.Length; i++)
                {
                    if (points[i].X < min)
                    {
                        min = points[i].X;
                    }
                }
                return min;

            }
        }

        public Rectangle getRectangleAt(int x, int y)
        {
            foreach (FrameworkElement frameWorkElement in boardGrid.Children)
            {
                int xx = Grid.GetColumn(frameWorkElement);
                int yy = Grid.GetRow(frameWorkElement);
                if (xx == x && yy == y)
                {
                    return (Rectangle)frameWorkElement;
                }
            }
            return null;
        }

        public List<Rectangle> getBoardRectangles(tools.Point[] points)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            foreach (tools.Point point in points)
            {
                rectangles.Add(getRectangleAt(point.X, point.Y));
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
            fallWorker = FallWorker.Instance;
            new Thread(fallWorker.InvokeFalling).Start();
        }

    }
}
