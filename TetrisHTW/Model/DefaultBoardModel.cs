﻿using System;
using System.Net;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TetrisHTW.Figures;
using TetrisHTW.View;
using TetrisHTW.tools;

namespace TetrisHTW.Model
{
    public class DefaultBoardModel : BoardModel
    {
        
        private Color boardColor = Color.FromArgb(255, 200, 200, 200);
        private int score;
        private Figure currentFigure;
        private Figure previewFigure;
        private const int columns = 7;
        private const int rows = 15;
        private volatile Color[,] board = new Color[columns, rows];

        public event EventHandler BoardChanged;


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
            NotifyBoardChanged();
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
        public void NotifyBoardChanged()
        {
            if (BoardChanged != null)
                BoardChanged(this, EventArgs.Empty);
        }


        public void registerView(BoardView v)
        {
            BoardChanged += new EventHandler(delegate
            {
                v.updateBoard();
            });
        }

        public void writeCell(Point[] points, Color c)
        {
            for (int i = 0; i < points.Length; i++)
            {
                board[(int)points[i].X, (int)points[i].Y] = c;
            }
            NotifyBoardChanged();
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
            NotifyBoardChanged();
        }

        public Figure generateRandomFigure()
        {
            int random = new Random().Next(2);
            Figure figure = null;
            switch (random)
            {
                case 0: figure = new Square(this); break;
                case 1: figure = new Bar(this); break;
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
