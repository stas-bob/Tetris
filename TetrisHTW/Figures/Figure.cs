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
using TetrisHTW.tools;
using System.Collections.Generic;
using System.Diagnostics;

namespace TetrisHTW.Figures
{
    public abstract class Figure
    {
        protected DefaultBoardModel board;
        protected Color color;
        protected Point[] points = new Point[4];
        protected int rotateState;
        private bool mutex;


        public Figure(DefaultBoardModel boardModel)
        {
            this.board = boardModel;
        }

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

        private void checkAfterFall(Point[] newPoints)
        {
            lock (App.myLock)
            {
                
                if (mutex)
                {
                    /*Diese Figur ist bereits nicht mehr relevant, da sie gefallen ist und eine neue Figur existiert*/
                    return;
                }
                board.clearPoints(points);
                bool fits = doPointsFit(newPoints);
                if (!fits)
                {
                    board.writeCell(points, color);
                    bool gameOver = false;
                    for (int i = 0; i < newPoints.Length; i++)
                    {
                        if (newPoints[i].Y < 0)
                        {
                            gameOver = true;
                            App.getInstance().gameOver();
                        }
                    }
                    if (!gameOver)
                    {
                        App.getInstance().NotifyFigureFallen(points);
                        List<int> linesToRemove = getLinesToRemove();
                        board.collapse(linesToRemove);

                        board.setCurrentFigure(board.getPreviewFigure());
                        board.setPreviewFigure(board.generateRandomFigure());
                        Debug.WriteLine("new on board");
                        board.getCurrentFigure().newOnBoard();
                        mutex = true;
                    }
                }
                else
                {
                    points = newPoints;
                    board.writeCell(points, color);
                }
            }
        }
        public void fall() 
        {
            lock (App.myLock)
            {
                Point[] newPoints = new Point[4];
            
                for (int i = 0; i < points.Length; i++)
                {
                    newPoints[i] = new Point(points[i].X, points[i].Y + 1);
                }
                checkAfterFall(newPoints);
            }
        }

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

        public void fallCompletely()
        {
            lock (App.myLock)
            {
                board.clearPoints(points);
                Point[] fallenPoints = simulatedFall();
                checkAfterFall(fallenPoints);
                fall();
            }
        }

        private Point[] simulatedFall()
        {

            Point[] newPoints = new Point[4];
            
            for (int i = 0; i < points.Length; i++)
            {
                newPoints[i] = new Point(points[i].X, points[i].Y + 1);
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
                bool doFit = doPointsFit(newPoints);
                if (doFit)
                {
                    points = newPoints;
                }
                board.writeCell(points, color);
                return doFit;
            }
        }

        public bool right()
        {
            lock (App.myLock)
            {
                board.clearPoints(points);
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
                board.writeCell(points, color);
                return doFit;
            }
        }

        
        public void newOnBoard()
        {
            lock (App.myLock)
            {
                
                bool fitsOnBoard = doPointsFit(points);
                board.writeCell(points, color);
                if (!fitsOnBoard)
                {
                    App.getInstance().gameOver();
                }
            }
        }

        public bool rotate()
        {
            //Wegen Fallthread
            lock (App.myLock)
            {
                return doRotate();
            }
        }

        protected virtual bool doRotate()
        {
            return false;
        }

        public abstract string toString();

        public Point[] getPoints()
        {
            return points;
        }

        public Color getColor()
        {
            return color;
        }
    }
}
