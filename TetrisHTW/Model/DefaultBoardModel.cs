using System;
using System.Net;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TetrisHTW.Figures;
using TetrisHTW.tools;
using TetrisHTW;

namespace TetrisHTW.Model
{
    public class DefaultBoardModel : BoardModel
    {
        private Random rnd = new Random();
        private Color boardColor = Color.FromArgb(255, 200, 200, 200);
        private int score;
        private Figure currentFigure;
        private Figure previewFigure;
        private const int columns = 10;
        private const int rows = 15;
        private volatile Color[,] board = new Color[columns, rows];
        
        public event BoardChangedEventHandler BoardChanged;
        public event ScoreChangedEventHandler ScoreChanged;

        public void clearBoard()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    board[j, i] = boardColor;
                }
            }
            score = 0;
        }

        public Color getColor()
        {
            return boardColor;
        }

        public DefaultBoardModel()
        {
            clearBoard();
        }

        public void collapse(int[] linesToRemove)
        {
            Array.Sort(linesToRemove);
            for (int i = 0; i < linesToRemove.Length; i++)
            {
                if (linesToRemove[i] != -1)
                {
                    shiftToLine(linesToRemove[i]);
                }
            }
            BoardEventArgs bae = new BoardEventArgs();
            bae.removedLines = linesToRemove;
            bae.collapse = true;
            NotifyBoardChanged(bae);
        }

        public bool isCellColored(int x, int y)
        {
            return board[x, y] != boardColor;
        }

        public int getColumns()
        {
            return columns;
        }

        public int getRows()
        {
            return rows;
        }
        public void NotifyBoardChanged(BoardEventArgs bea)
        {
            if (BoardChanged != null)
                BoardChanged(this, bea);
        }

        public void NotifyScoreChanged(ScoreEventArgs sea)
        {
            if (ScoreChanged != null)
            {
                ScoreChanged(this, sea);
            }
        }

        public void writeCell(Point[] points, Color c)
        {
            for (int i = 0; i < points.Length; i++)
            {
                board[(int)points[i].X, (int)points[i].Y] = c;
            }
            NotifyBoardChanged(new BoardEventArgs());
        }

        public Color[,] getBoardData()
        {
            return board;
        }

        public Figure getCurrentFigure()
        {
            return currentFigure;
        }

        public int getScore()
        {
            return score;
        }

        public void setScore(int score)
        {
            this.score = score;
            ScoreEventArgs sea = new ScoreEventArgs();
            sea.score = score;
            NotifyScoreChanged(sea);
        }

        public Figure generateRandomFigure()
        {
            int random = rnd.Next(7);
            Figure figure = null;
            switch (random)
            {
                case 0: figure = new I(this); break;
                case 1: figure = new O(this); break;
                case 2: figure = new T(this); break;
                case 3: figure = new Z(this); break;
                case 4: figure = new S(this); break;
                case 5: figure = new J(this); break;
                case 6: figure = new L(this); break;
            }
            return figure;
        }


        public void setPreviewFigure(Figure figure)
        {
            this.previewFigure = figure;
        }


        public void setCurrentFigure(Figure figure)
        {
            this.currentFigure = figure;
        }

        public void clearPoints(Point[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                board[(int)points[i].X, (int)points[i].Y] = boardColor;
            }
        }

        public Figure getPreviewFigure()
        {
            return previewFigure;
        }

        public void shiftToLine(int y)
        {
            for (int i = y; i > 0; i--)
            {
                for (int j = 0; j < columns; j++)
                {
                    board[j, i] = board[j, i - 1];
                }
            }
            for (int i = 0; i < columns; i++)
            {
                board[i, 0] = boardColor;
            }
        }

        
    }
}
