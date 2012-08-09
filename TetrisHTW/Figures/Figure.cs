using System;
using System.Net;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TetrisHTW.Model;
using TetrisHTW.Util;
using System.Collections.Generic;
using System.Diagnostics;

namespace TetrisHTW.Figures
{
    /**
     * Abstrakte Klasse für die verschiedenen Figuren
     */
    public abstract class Figure
    {
        // Board
        protected DefaultBoardModel board;
        // Farbe der Figur
        protected Color color;
        // Punkte der Figur im Board
        protected Point[] points = new Point[4];
        // Punkte der Vorschaufigur im Board
        private Point[] ghostPoints = new Point[4];
        // Status der Figur
        protected int rotateState;

        /**
         * Konstruktor
         */
        public Figure(DefaultBoardModel boardModel)
        {
            this.board = boardModel;
        }

        /**
         * Testen, ob die temporaere Figur in die Position passt
         */
        protected bool doPointsFit(Point[] points)
        {
            
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].X < 0 || points[i].X >= board.getColumns() || points[i].Y < 0 || points[i].Y >= board.getRows() || board.isCellColored((int)points[i].X, (int)points[i].Y))
                {
                    return false;
                }
            }
            return true;
        }

   
        /**
         * Figur kann nicht weiter fallen,
         * entweder wird neue Figur erzeugt oder man ist Gameover
         */
        private void FigureIsFallen(Point[] previousPoints)
        {
            ghostPoints = null;
            board.writeCell(points, color);
            bool gameOver = false;
            /*Pruefen ob GameOver*/
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].Y < 0)
                {
                    gameOver = true;
                    App.getInstance().gameOver();
                }
            }
            if (!gameOver)
            {
                App.getInstance().NotifyFigureFallen(previousPoints, points, color);
                
                List<int> linesToRemove = getLinesToRemove();
                /*Hier werden die zu loeschenden Zeilen geloescht*/
                board.collapse(linesToRemove);

                /*Setzt Vorschaufigur als aktuelle Figur und generiert neue Vorschau*/
                board.setCurrentFigure(board.getPreviewFigure());
                board.setPreviewFigure(board.generateRandomFigure());
                board.getCurrentFigure().newOnBoard();
            }
            
        }

        /**
         * Bewegt die Figur eine Position nach unten
         */
        public void fall() 
        {
            lock (App.myLock)
            {
                /* Punkte der temporaeren Figur*/
                Point[] newPoints = new Point[4];
            
                for (int i = 0; i < points.Length; i++)
                {
                    newPoints[i] = new Point(points[i].X, points[i].Y + 1);
                }
                board.clearPoints(points);
                bool fits = doPointsFit(newPoints);
                if (!fits)
                {
                    FigureIsFallen(points);
                }
                else
                {
                    /* temporaere Figur wird aktuelle Figur (Punkten)*/
                    points = newPoints;
                    board.writeCell(points, color);
                }
            }
        }

        /**
         * Diese Methode stellt fest welche Zeilen zu loeschen sind
         */
        private List<int> getLinesToRemove()
        {
            lock (App.myLock)
            {
                List<int> linesToRemove = new List<int>();
                for (int i = 0; i < points.Length; i++)
                {
                    int y = points[i].Y;
                    bool containsY = false;
                    for (int m = 0; m < linesToRemove.Count; m++)
                    {
                        if (linesToRemove[m] == y)
                        {
                            containsY = true;
                            break;
                        }
                    }
                    if (containsY)
                    {
                        continue;
                    }
                    Color[,] cells = board.getBoardData();
                    int cellsColored = 0;
                    for (int j = 0; j < board.getColumns(); j++)
                    {
                        if (board.isCellColored(j, y))
                        {
                            cellsColored++;
                        }
                    }
                    if (cellsColored == board.getColumns())
                    {
                        linesToRemove.Add(y);
                    }
                }
                return linesToRemove;
            }
        }

        /**
         * Hier wird die aktuelle Figur an den hoechst moeglichen y Wert gesetzt
         */
        public void fallCompletely()
        {
            lock (App.myLock)
            {
                board.clearPoints(points);
                Point[] fallenPoints = simulatedFall();
                Point[] previousPoints = new Point[4];
                for (int i = 0; i < points.Length; i++)
                {
                    previousPoints[i] = new Point(points[i].X, points[i].Y);
                }
                points = fallenPoints;
                FigureIsFallen(previousPoints);
            }
        }

        /**
         * Hier wird geprueft, wie weit die temporaere Figur (Punkte) fallen kann
         */
        private Point[] simulatedFall()
        {
            Point[] newPoints = new Point[4];
            
            for (int i = 0; i < points.Length; i++)
            {
                newPoints[i] = new Point(points[i].X, points[i].Y);
            }
            for (int i = 0; i < App.getInstance().getBoardModel().getRows(); i++)
            {
                for (int j = 0; j < newPoints.Length; j++)
                {
                    newPoints[j].Y += 1;
                }
                bool fit = doPointsFit(newPoints);
                if (!fit)
                {
                    for (int j = 0; j < newPoints.Length; j++)
                    {
                        newPoints[j].Y -= 1;
                    }
                    break;
                }
            }
            return newPoints;
        }

        /**
         * Es wird versucht, ob man die aktuelle Figur nach links verschieben kann
         */
        public bool left()
        {
            lock (App.myLock)
            {
                Point[] newPoints = new Point[4];
                for (int i = 0; i < points.Length; i++)
                {
                    newPoints[i] = new Point(points[i].X - 1, points[i].Y);
                }
                board.clearPoints(points);
                if (this.ghostPoints != null)
                {
                    board.clearPoints(this.ghostPoints);
                }
                
                bool doFit = doPointsFit(newPoints);
                if (doFit)
                {
                    points = newPoints;
                }
                Point[] fallenPoints = simulatedFall();
                this.ghostPoints = fallenPoints;
                board.writeCell(points, fallenPoints, color, board.getFallenPreviewColor());
                return doFit;
            }
        }

        /**
         * Es wird versucht, ob man die aktuelle Figur nach rechts verschieben kann
         */
        public bool right()
        {
            lock (App.myLock)
            {
                board.clearPoints(points);
                if (this.ghostPoints != null)
                {
                    board.clearPoints(this.ghostPoints);
                }
                Point[] newPoints = new Point[4];
                for (int i = 0; i < points.Length; i++)
                {
                    newPoints[i] = new Point(points[i].X + 1, points[i].Y);
                }

                bool doFit = doPointsFit(newPoints);
                if (doFit)
                {
                    points = newPoints;
                }
                Point[] fallenPoints = simulatedFall();
                this.ghostPoints = fallenPoints;
                board.writeCell(points, fallenPoints, color, board.getFallenPreviewColor());
                return doFit;
            }
        }

        
        /**
         * Wird geprüft, ob der neue Stein überhaupt auf das Board passt
         */
        public void newOnBoard()
        {
            lock (App.myLock)
            {
                bool fitsOnBoard = doPointsFit(points);
                Point[] fallenPoints = simulatedFall();
                this.ghostPoints = fallenPoints;
                board.writeCell(points, fallenPoints, color, board.getFallenPreviewColor());
                if (!fitsOnBoard)
                {
                    App.getInstance().gameOver();
                }
                else
                {
                    board.addScore(5);
                }
            }
        }

        /**
         * Prüft ob Figur gedreht werden kann
         */
        public void rotate()
        {
            //Wegen Fallthread
            lock (App.myLock)
            {
                if (this.ghostPoints != null)
                {
                    board.clearPoints(this.ghostPoints);
                }
                board.clearPoints(points);

                doRotate();
                
                Point[] fallenPoints = simulatedFall();
                this.ghostPoints = fallenPoints;
                board.writeCell(points, fallenPoints, color, board.getFallenPreviewColor());
            }
        }

        /**
         * Figur drehen
         */
        protected virtual void doRotate()
        {

        }

        /**
         * Gibt die Punkte der Figur zurück
         */
        public Point[] getPoints()
        {
            return points;
        }

        /**
         * Gibt die Farbe der Figur zurück
         */ 
        public Color getColor()
        {
            return color;
        }

        /**
         * Methode für den Special Mode, der die Vorscahufigur löscht beim Speichern der Figur
         */
        internal void removeFromBoard()
        {
            if (this.ghostPoints != null)
            {
                board.clearPoints(this.ghostPoints);
                this.ghostPoints = null;
               
            }
            board.clearPoints(points);
            if (App.DEBUG)
                Debug.WriteLine("removing " + toString());
        }

        // Abstrakte toString Methode
        public abstract string toString();

        // Abstrakte Methode, um die Figur auf die Startposition zu setzen
        public abstract void setInitPoints();
    }
}
