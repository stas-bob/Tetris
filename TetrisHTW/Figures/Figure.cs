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

namespace TetrisHTW.Figures
{
    public abstract class Figure
    {
        protected DefaultBoardModel board;
        protected Color color;
        protected Point[] points = new Point[4];
        protected int rotateState;
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

        
        public void fall(App app)
        {
            Point[] newPoints = new Point[4];
            board.clearPoints(points);
            for (int i = 0; i < points.Length; i++)
            {
                newPoints[i] = new Point(points[i].X, points[i].Y + 1);
            }
            
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
                        app.gameOver();
                    }
                }
                if (!gameOver)
                {
                    int[] linesToRemove = getLinesToRemove();
                    board.collapse(linesToRemove);
                    int score = 0;
                    for (int i = 0; i < linesToRemove.Length; i++)
                    {
                        if (linesToRemove[i] != -1)
                        {
                            score++;
                        }
                    }
                    board.setScore(board.getScore() + score);
                    board.setCurrentFigure(board.getPreviewFigure());
                    board.setPreviewFigure(board.generateRandomFigure());
                    board.getCurrentFigure().newOnBoard(app);
                }
            }
            else
            {
                points = newPoints;
                board.writeCell(points, color);
            }
            
        }

        private int[] getLinesToRemove()
        {
            int[] linesToRemove = new int[4];
            for (int i = 0; i < linesToRemove.Length; i++)
            {
                linesToRemove[i] = -1;
            }
            for (int i = 0; i < points.Length; i++)
            {
                int y = points[i].Y;
                bool containsY = false;
                for (int m = 0; m < linesToRemove.Length; m++)
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
                    linesToRemove[i] = y;
                }
            }
            return linesToRemove;
        }

        public void fallFaster()
        {
            lock (App.myLock)
            {
            }
        }

        public void fallCompletely()
        {
            lock (App.myLock)
            {
            }
        }

        public void left()
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
            }
        }

        public void right()
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
            }
        }

        
        public void newOnBoard(App app)
        {
            lock (App.myLock)
            {
                
                bool fitsOnBoard = doPointsFit(points);
                board.writeCell(points, color);
                if (!fitsOnBoard)
                {
                    app.gameOver();
                }
            }
        }

        public void rotate()
        {
            //Wegen Fallthread
            lock (App.myLock)
            {
                doRotate();
            }
        }

        public virtual void doRotate()
        {

        }

        public abstract string toString();
    }
}
