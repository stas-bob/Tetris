
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
using TetrisHTW.Util;
using TetrisHTW.View;

namespace TetrisHTW
{

    public partial class NormalTetrisView : UserControl
    {

        private DefaultBoardModel boardModel;
        private FallWorker fallWorker;
        private Timer timer;
        private Key lastKey;
        private int previousLevel;
        private Random rnd = new Random();
        private bool pause;
        private bool hardFall;
        private bool gameOver;
        private SQLClient sqlClient = new SQLClient("http://stl-l-18.htw-saarland.de:8080/TetrisSQLProxy/SQLProxyServlet");
        private string playerName;
        private long time;
        private List<long> timeList = new List<long>();
        private OptionsView ov;
        private IndexView iv;
        private int mod;

        public NormalTetrisView(OptionsView ov, IndexView iv)
        {
            this.iv = iv;
            this.ov = ov;
            this.boardModel = App.getInstance().getBoardModel();
            InitializeComponent();
            /*Hier werden die Grids initialisiert*/
            initBoard();
            
            /* Hier kommen die Event Listener fuers Spiel*/
            boardModel.BoardChanged += new BoardChangedEventHandler(OnBoardChanged);
            boardModel.ScoreChanged += new ScoreChangedEventHandler(OnScoreChanged);
            boardModel.LineChanged += new LineChangedEventHandler(OnLineChanged);
            App.getInstance().GameOverEvent += new GameOverEventHandler(OnGameOver);
            App.getInstance().FigureFallenEvent += new FigureFallenEventHandler(OnFigureFallen);
            playerName = "Unbekannt";
            mod = 1;
            
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


        public void setMode(int mode)
        {
            this.mod = mode;
        }


        /*KeyboardListener fuer druecken einer Taste*/
        void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (!gameOver)
            {
                if (e.Key == Key.P)
                {
                    if (!pause)
                    {
                        showHint("Pause", 40);
                    }
                    else
                    {
                        GameResume();
                    }
                }
                else
                {
                    if (e.Key == Key.Escape)
                    {
                        if (pause)
                            GameResume();
                    }
                    else
                    {
                        if (!pause)
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
                                        if (boardModel.getLevel() >= 9)
                                        {
                                            timer = new Timer(MoveFigure, null, 0, e.Key == Key.Down ? 75 : 125);
                                        }
                                        else if (boardModel.getLevel() == 8)
                                        {
                                            timer = new Timer(MoveFigure, null, 0, e.Key == Key.Down ? 75 : 130);
                                        }
                                        else if (boardModel.getLevel() == 7)
                                        {
                                            timer = new Timer(MoveFigure, null, 0, e.Key == Key.Down ? 75 : 135);
                                        }
                                        else
                                        {
                                            timer = new Timer(MoveFigure, null, 0, e.Key == Key.Down ? 75 : 140);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
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
            if (gameOver)
            {
                timer.Dispose();
                timer = null;
            }
            else
            {
                switch (lastKey)
                {
                    case Key.Left: boardModel.getCurrentFigure().left(); break;
                    case Key.Right: boardModel.getCurrentFigure().right(); break;
                    case Key.Down: boardModel.getCurrentFigure().fall(); break;
                }
            }
        }

        public void InitGame()
        {
            /*Das sind die Listener für das Keyboard. TODO Das muss doch auch anders gehn!! this.keydown geht net.
             Ich will dass dieses Objekt weggeraumt. dann braucht man kein onExit(), so muss ich nicht extra event -= machen*/
            App.getInstance().RootVisual.KeyDown += new KeyEventHandler(Page_KeyDown);
            App.getInstance().RootVisual.KeyUp += new KeyEventHandler(Page_KeyUp);

            Figure preview = boardModel.generateRandomFigure();
            Figure current = boardModel.generateRandomFigure();

            boardModel.setCurrentFigure(current);
            boardModel.setPreviewFigure(preview);

            boardModel.getCurrentFigure().newOnBoard();

            GameStart();
        }

        public void setPlayerName(string playerName)
        {
            this.playerName = playerName;
        }

        public Brush getBrushByColor(Color currentCellColor)
        {
            Brush b = null;
            if (currentCellColor == Colors.Blue)
            {
                b = Application.Current.Resources["BluePointBrush"] as Brush;
            }
            else if (currentCellColor == Colors.Yellow)
            {
                b = Application.Current.Resources["YellowPointBrush"] as Brush;
            }
            else if (currentCellColor == Colors.Green)
            {
                b = Application.Current.Resources["GreenPointBrush"] as Brush;
            }
            else if (currentCellColor == Colors.Red)
            {
                b = Application.Current.Resources["RedPointBrush"] as Brush;
            }
            else if (currentCellColor == Colors.Purple)
            {
                b = Application.Current.Resources["PurplePointBrush"] as Brush;
            }
            else if (currentCellColor == Colors.Cyan)
            {
                b = Application.Current.Resources["CyanPointBrush"] as Brush;
            }
            else if (currentCellColor == Colors.Orange)
            {
                b = Application.Current.Resources["OrangePointBrush"] as Brush;
            }
            else if (currentCellColor == boardModel.getFallenPreviewColor())
            {
                b = Application.Current.Resources["PreviewPointBrush"] as Brush;
            }
            return b;
        }

        public void OnBoardChanged(object sender, BoardEventArgs bea)
        {
            Dispatcher.BeginInvoke(delegate
            {
                if (iv.rootContainer.Child == this)
                {
                    Color[,] data = bea.boardData;
                    if (bea.removedLines.Count > 0)
                    {
                        if (mod == 3)
                        {
                            int random = rnd.Next(2);
                            if (random == 0)
                            {
                                animBoardRotate.To = 180 * (rnd.Next(7) + 3);
                            }
                            else
                            {
                                animBoardRotate.To = 90 * (rnd.Next(7) + 3);
                            }


                            boardRotateSB.Begin();
                        }
                        if (hardFall)
                        {
                            animateRemovedLinesHard(bea.removedLines);
                        }
                        else
                        {
                            animateRemovedLinesSoft(bea.removedLines);
                        }

                    }
                    foreach (FrameworkElement frameWorkElement in boardGrid.Children)
                    {
                        Rectangle rect = (Rectangle)frameWorkElement;
                        int x = Grid.GetColumn(frameWorkElement);
                        int y = Grid.GetRow(frameWorkElement);
                        Color currentCellColor = data[x, y];

                        if (currentCellColor == boardModel.getBoardColor())
                        {
                            rect.Fill = new SolidColorBrush(Colors.Transparent);
                        }
                        else if (currentCellColor == boardModel.getFallenPreviewColor())
                        {
                            rect.Fill = getBrushByColor(boardModel.getFallenPreviewColor());
                        }
                        else
                        {
                            rect.Fill = getBrushByColor(currentCellColor);
                        }

                    }
                    Util.Point[] previewPoints = boardModel.getPreviewFigure().getPoints();

                    List<Rectangle> previewRectangles = getPreviewBoardRectangles(previewPoints);

                    foreach (FrameworkElement frameWorkElement in previewGrid.Children)
                    {
                        Rectangle rect = (Rectangle)frameWorkElement;
                        rect.Fill = new SolidColorBrush(Colors.Transparent);
                    }
                    Color currentPreviewFigureColor = boardModel.getPreviewFigure().getColor();
                    foreach (Rectangle rect in previewRectangles)
                    {
                        rect.Fill = getBrushByColor(currentPreviewFigureColor);
                    }
                }
            });
        }

        private void animateRemovedLinesHard(List<int> removedLines)
        {
            Storyboard sb = new Storyboard();
            List<Rectangle> animatedRectangled = new List<Rectangle>();
            foreach (int y in removedLines)
            {
                for (int i = 0; i < boardModel.getColumns(); i++)
                {
                    Rectangle rectOnBoard = getRectangleAt(i, y);
                    Rectangle rect = new Rectangle();
                    rect.Height = rectOnBoard.ActualHeight;
                    rect.Width = rectOnBoard.ActualWidth;
                    rect.Fill = rectOnBoard.Fill;

                    System.Windows.Point p = rectOnBoard.TransformToVisual(this.LayoutRoot).Transform(new System.Windows.Point(0, 0));

                    canvas.Children.Add(rect);
                    Canvas.SetLeft(rect, p.X + Canvas.GetLeft(LayoutRoot));
                    Canvas.SetTop(rect, p.Y + Canvas.GetTop(LayoutRoot));


                    CompositeTransform ct = new CompositeTransform();
                    ct.CenterX = rect.ActualWidth / 2;
                    ct.CenterY = rect.ActualHeight / 2;
                    rect.RenderTransform = ct;
                   
                   

                    //Bewegung
                    DoubleAnimationUsingKeyFrames dauk = new DoubleAnimationUsingKeyFrames();
                    Storyboard.SetTargetProperty(dauk, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateY)"));
                    Storyboard.SetTarget(dauk, rect);


                    SplineDoubleKeyFrame sdk = new SplineDoubleKeyFrame();
                    sdk.SetValue(SplineDoubleKeyFrame.ValueProperty, (double)(-1 * rnd.Next(200)));
                    sdk.KeyTime = TimeSpan.FromMilliseconds(rnd.Next(400));
                    KeySpline ks = new KeySpline();
                    ks.ControlPoint1 = new System.Windows.Point(0, 0.5);
                    ks.ControlPoint2 = new System.Windows.Point(1, 1);
                    sdk.KeySpline = ks;
                    dauk.KeyFrames.Add(sdk);

                    SplineDoubleKeyFrame sdk0 = new SplineDoubleKeyFrame();
                    sdk0.SetValue(SplineDoubleKeyFrame.ValueProperty, (double)rnd.Next(474));
                    sdk0.KeyTime = TimeSpan.FromMilliseconds(1000);
                    KeySpline ks0 = new KeySpline();
                    ks0.ControlPoint1 = new System.Windows.Point(1, 0);
                    ks0.ControlPoint2 = new System.Windows.Point(1, 1);
                    sdk0.KeySpline = ks0;
                    dauk.KeyFrames.Add(sdk0);
                    //Drehung
                    //-------------------------------
                    DoubleAnimation da = new DoubleAnimation();
                    Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.Rotation)"));
                    Storyboard.SetTarget(da, rect);
                    da.Duration = TimeSpan.FromMilliseconds(200);
                    int fac = rnd.Next(2) == 0 ? -1 : 1;

                    da.To = fac*360.0;
                    da.RepeatBehavior = RepeatBehavior.Forever;


                    //--Opacity
                    DoubleAnimationUsingKeyFrames dauk2 = new DoubleAnimationUsingKeyFrames();
                    Storyboard.SetTarget(dauk2, rect);
                    Storyboard.SetTargetProperty(dauk2, new PropertyPath("Opacity"));

                    SplineDoubleKeyFrame sdk3 = new SplineDoubleKeyFrame();
                    sdk3.SetValue(SplineDoubleKeyFrame.ValueProperty, 0.0);
                    KeySpline ks3 = new KeySpline();
                    ks3.ControlPoint1 = new System.Windows.Point(0, 0.5);
                    ks3.ControlPoint2 = new System.Windows.Point(1, 0.5);
                    sdk3.KeySpline = ks3;
                    sdk3.KeyTime = TimeSpan.FromMilliseconds(1000);
                    dauk2.KeyFrames.Add(sdk3);


                    
                    sb.Children.Add(dauk);
                    sb.Children.Add(da);
                    sb.Children.Add(dauk2);
                    animatedRectangled.Add(rect);
                }
            }
            if (!LayoutRoot.Resources.Contains("unique_id"))
            {
                LayoutRoot.Resources.Add("unique_id", sb);
            }
            sb.Duration = TimeSpan.FromMilliseconds(1000);
            sb.Completed += new EventHandler((a, b) =>
            {
                foreach (var rect in animatedRectangled)
                {
                    canvas.Children.Remove(rect);
                }
            });
            sb.Begin();
        }

        private void animateRemovedLinesSoft(List<int> removedLines)
        {
            Storyboard sb = new Storyboard();
            
            List<Rectangle> animatedRectangled = new List<Rectangle>();
            foreach (int y in removedLines)
            {
                for (int i = 0; i < boardModel.getColumns(); i++)
                {
                    Rectangle rectOnBoard = getRectangleAt(i, y);
                    Rectangle rect = new Rectangle();
                    rect.Height = rectOnBoard.ActualHeight;
                    rect.Width = rectOnBoard.ActualWidth;
                    rect.Fill = rectOnBoard.Fill;

                    System.Windows.Point p = rectOnBoard.TransformToVisual(this.LayoutRoot).Transform(new System.Windows.Point(0, 0));

                    canvas.Children.Add(rect);
                    Canvas.SetLeft(rect, p.X + Canvas.GetLeft(LayoutRoot));
                    Canvas.SetTop(rect, p.Y + Canvas.GetTop(LayoutRoot));


                    CompositeTransform ct = new CompositeTransform();
                    ct.CenterX = rect.ActualWidth / 2;
                    ct.CenterY = rect.ActualHeight / 2;
                    rect.RenderTransform = ct;


                    //Bewegung
                    DoubleAnimationUsingKeyFrames dauk = new DoubleAnimationUsingKeyFrames();
                    Storyboard.SetTargetProperty(dauk, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateY)"));
                    Storyboard.SetTarget(dauk, rect);


                    SplineDoubleKeyFrame sdk = new SplineDoubleKeyFrame();
                    sdk.SetValue(SplineDoubleKeyFrame.ValueProperty, (double)(-1 * rnd.Next(10)));
                    sdk.KeyTime = TimeSpan.FromMilliseconds(rnd.Next(400));
                    KeySpline ks = new KeySpline();
                    ks.ControlPoint1 = new System.Windows.Point(0, 1);
                    ks.ControlPoint2 = new System.Windows.Point(1, 1);
                    sdk.KeySpline = ks;
                    dauk.KeyFrames.Add(sdk);

                    SplineDoubleKeyFrame sdk0 = new SplineDoubleKeyFrame();
                    sdk0.SetValue(SplineDoubleKeyFrame.ValueProperty, (double)rnd.Next(474));
                    sdk0.KeyTime = TimeSpan.FromMilliseconds(1000);
                    KeySpline ks0 = new KeySpline();
                    ks0.ControlPoint1 = new System.Windows.Point(1, 0);
                    ks0.ControlPoint2 = new System.Windows.Point(1, 1);
                    sdk0.KeySpline = ks0;
                    dauk.KeyFrames.Add(sdk0);
                    //Drehung
                    //-------------------------------
                    DoubleAnimation da = new DoubleAnimation();
                    Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.Rotation)"));
                    Storyboard.SetTarget(da, rect);
                    da.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
                    int fac = rnd.Next(2) == 0 ? -1 : 1;

                    da.To = fac * 360.0;
                    da.RepeatBehavior = RepeatBehavior.Forever;


                    //--Opacity
                    DoubleAnimationUsingKeyFrames dauk2 = new DoubleAnimationUsingKeyFrames();
                    Storyboard.SetTarget(dauk2, rect);
                    Storyboard.SetTargetProperty(dauk2, new PropertyPath("Opacity"));

                    SplineDoubleKeyFrame sdk3 = new SplineDoubleKeyFrame();
                    sdk3.SetValue(SplineDoubleKeyFrame.ValueProperty, 0.0);
                    KeySpline ks3 = new KeySpline();
                    ks3.ControlPoint1 = new System.Windows.Point(0, 0.5);
                    ks3.ControlPoint2 = new System.Windows.Point(1, 0.5);
                    sdk3.KeySpline = ks3;
                    sdk3.KeyTime = TimeSpan.FromMilliseconds(1000);
                    dauk2.KeyFrames.Add(sdk3);



                    sb.Children.Add(dauk);
                    sb.Children.Add(da);
                    sb.Children.Add(dauk2);
                    animatedRectangled.Add(rect);
                }
            }
            if (!LayoutRoot.Resources.Contains("unique_id"))
            {
                LayoutRoot.Resources.Add("unique_id", sb);
            }
            sb.Duration = TimeSpan.FromMilliseconds(1000);
            sb.Completed += new EventHandler((a, b) =>
            {
                foreach (var rect in animatedRectangled)
                {
                    canvas.Children.Remove(rect);
                }
            });
            sb.Begin();
        }

        public void OnScoreChanged(object sender, ScoreEventArgs bea)
        {
            Dispatcher.BeginInvoke(delegate
            {
                if (iv.rootContainer.Child == this)
                {
                    scoreText.Text = bea.score + "";
                    levelText.Text = bea.level + "";
                    if (previousLevel != bea.level)
                    {
                        fallWorker.setLevel(bea.level);
                        previousLevel = bea.level;
                        levelFontSizeSB.Begin();
                    }
                }
            });
            
        }

        public void OnLineChanged(object sender, LineEventArgs lea)
        {
            Dispatcher.BeginInvoke(delegate
            {
                linesText.Text = lea.lines + "";
                
            });

        }

        public void OnFigureFallen(object sender, FigureFallenEventArgs ffea)
        {
            Dispatcher.BeginInvoke(delegate
            {
                if (iv.rootContainer.Child == this)
                {
                    hardFall = false;
                    Util.Point[] points = ffea.figurePoints;
                    List<Rectangle> rectangles = getBoardRectangles(points);
                    Storyboard sb = new Storyboard();
                    sb.Duration = TimeSpan.FromMilliseconds(400);
                    foreach (Rectangle rectangle in rectangles)
                    {
                        Duration duration = TimeSpan.FromMilliseconds(200);
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
                        sb.Duration = TimeSpan.FromMilliseconds(3000);
                        hardFall = true;
                        int maxY = getMax(points, false);
                        int minY = getMin(ffea.previousFigurePoints, false);
                        int minX = getMin(points, true);
                        int maxX = getMax(points, true);

                        Rectangle upperLeftRect = getRectangleAt(minX, minY);
                        Rectangle bottomRightRect = getRectangleAt(maxX, maxY);
                        Rectangle bottomLeftRect = getRectangleAt(minX, maxY);
                        Rectangle upperRightRect = getRectangleAt(maxX, minY);


                        System.Windows.Point upperLeftPoint = upperLeftRect.TransformToVisual(this.LayoutRoot).Transform(new System.Windows.Point(0, 0));
                        System.Windows.Point bottomRightPoint = bottomRightRect.TransformToVisual(this.LayoutRoot).Transform(new System.Windows.Point(0, 0));
                        System.Windows.Point bottomLeftPoint = bottomLeftRect.TransformToVisual(this.LayoutRoot).Transform(new System.Windows.Point(0, 0));
                        System.Windows.Point upperRightPoint = upperRightRect.TransformToVisual(this.LayoutRoot).Transform(new System.Windows.Point(0, 0));


                        double width = Math.Sqrt(Math.Pow(bottomRightPoint.X - bottomLeftPoint.X, 2) + Math.Pow(bottomRightPoint.Y - bottomLeftPoint.Y, 2)) + upperLeftRect.ActualWidth;
                        double height = Math.Sqrt(Math.Pow(bottomRightPoint.X - upperRightPoint.X, 2) + Math.Pow(bottomRightPoint.Y - upperRightPoint.Y, 2));



                        Rectangle effectRectangle = new Rectangle();
                        effectRectangle.Width = width;
                        effectRectangle.Height = height;

                        CompositeTransform ct = (CompositeTransform)boardBorder.RenderTransform;
                        CompositeTransform ct2 = new CompositeTransform();
                        ct2.Rotation = ct.Rotation;
                        effectRectangle.RenderTransform = ct2;



                        LinearGradientBrush lgb = new LinearGradientBrush();
                        RotateTransform rt = new RotateTransform();
                        rt.Angle = 0;
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


                        Duration duration = TimeSpan.FromMilliseconds(3000);
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
                }

            });

        }


        public int getMax(Util.Point[] points, bool x)
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

        public int getMin(Util.Point[] points, bool x)
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

        public List<Rectangle> getBoardRectangles(Util.Point[] points)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            foreach (Util.Point point in points)
            {
                rectangles.Add(getRectangleAt(point.X, point.Y));
            }
            return rectangles;
        }

        public List<Rectangle> getPreviewBoardRectangles(Util.Point[] points)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            foreach (Util.Point point in points)
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

        public void OnGameOver(object sender, GameOverEventArgs goea)
        {
            Dispatcher.BeginInvoke(delegate
            {
                if (iv.rootContainer.Child == this)
                {
                    GameOver();
                }
            });
        }

        private void GameOver()
        {
            showHint("Game Over", 20);
            gameOver = true;
            scoreButton.Visibility = System.Windows.Visibility.Visible;
            timeList.Add(DateTime.Now.Ticks - time);
            time = 0;
            foreach (long t in timeList)
            {
                time += t;
            }
            sqlClient.writeScore(SQLClientError, playerName, boardModel.getScore(), boardModel.getLevel(), time, mod);
        }

        private void showHint(string msg, int fontSize)
        {
            pause = true;
            if (fallWorker != null)
            {
                fallWorker.RequestStop();
            }
            timeList.Add(DateTime.Now.Ticks - time);
            HintBoxTextBlock.FontSize = fontSize;
            HintBoxTextBlock.Text = msg;
            HintBoxOnSB.Begin();
        }
        private void SQLClientError(string msg)
        {
            Dispatcher.BeginInvoke(() =>
            {
                showHint(msg + ".Ihre Punktezahl ist eventuell verloren gegangen.", 8);   
            });
        }

        private void GameStart()
        {
            setFallWorker();
            new Thread(fallWorker.InvokeFalling).Start();
            time = DateTime.Now.Ticks;
        }

        private void GameResume()
        {
            if (!gameOver)
            {
                pause = false;
                setFallWorker();                        
                new Thread(fallWorker.InvokeFalling).Start();
                time = DateTime.Now.Ticks;
                HintBoxOffSB.Begin();
                
            }
        }

        private void setFallWorker()
        {
            fallWorker = new FallWorker(boardModel.getLevel());
        }

        private void ExitGame()
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (fallWorker != null)
                {
                    fallWorker.RequestStop();
                    fallWorker.setLevel(0);
                }
                timeList.Clear();
                time = 0;
                gameOver = false;
                pause = false;
                HintBox.Opacity = 0;
                boardModel.clearBoard();
                lastKey = 0;

                playerName = "Unbekannt";
                hardFall = false;
                mod = 1;
                CompositeTransform ct = new CompositeTransform();
                ct.Rotation = 0;
                ct.TranslateX = 0;
                ct.TranslateY = 0;
                ct.CenterX = 105;
                ct.CenterY = 200;
                boardBorder.RenderTransform = ct;
                animBoardRotate.To = 0;
                App.getInstance().RootVisual.KeyDown -= Page_KeyDown;
                App.getInstance().RootVisual.KeyUp -= Page_KeyUp;
                scoreButton.Visibility = System.Windows.Visibility.Collapsed;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExitGame();
            iv.rootContainer.Child = ov;
        }

        private void ScoreButton_Click(object sender, RoutedEventArgs e)
        {
            ExitGame();
            iv.getHighScoreView().update(boardModel.getScore(), mod);
            iv.rootContainer.Child = iv.getHighScoreView();
        }

        internal void setTempLines(int templines)
        {
            
            this.boardModel.setTempLines(templines);
            previousLevel = boardModel.getLevel();
        }
    }
}
