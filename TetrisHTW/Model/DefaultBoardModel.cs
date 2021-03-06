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
using TetrisHTW.Util;
using TetrisHTW;
using System.Collections.Generic;

namespace TetrisHTW.Model
{
    /**
     * Klasse die das Spielfeld zeichnet und auf Änderungen beim spielen reagiert
     * 
     * Wurde mit einer Kopie gelöst, da es sonst z.B. beim Zeilen löschen zu verspäteter Ausführung kam
     */
    public class DefaultBoardModel
    {
        // Attribute
        private Random rnd = new Random();
        private Color boardColor = Color.FromArgb(255, 200, 200, 200);
        private Color fallenPreviewColor = Color.FromArgb(255, 201, 201, 201);
        private int score;
        private int lines;
        private int tempLines;
        private Figure currentFigure;
        private Figure previewFigure;
        private Figure memoryFigure = null;
        private const int columns = 10;
        private const int rows = 20;
        private volatile Color[,] board = new Color[columns, rows];

        // Event-Handler
        public event BoardChangedEventHandler BoardChanged;
        public event ScoreChangedEventHandler ScoreChanged;
        public event LineChangedEventHandler LineChanged;

        /**
         * Löscht den Inhalt vom Board
         */
        public void clearBoard()
        {
            lock (App.myLock)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        board[j, i] = boardColor;
                    }
                }
                setLines(0);
                addScore(-1 * getScore());

                currentFigure = null;
                previewFigure = null;
                memoryFigure = null;
            }
        }

        /**
         * Errechnet die zu löschenden Zeilen und löscht sie
         */
        public void collapse(List<int> linesToRemove)
        {
            linesToRemove.Sort(delegate(int a, int b)
            {
                return a.CompareTo(b);
            });
            for (int i = 0; i < linesToRemove.Count; i++)
            {
                shiftToLine(linesToRemove[i]);
            }
            for (int i = 0; i < linesToRemove.Count; i++)
            {
                lines++;
            }
            BoardEventArgs bae = new BoardEventArgs(getBoardData());
            bae.removedLines = linesToRemove;
            NotifyBoardChanged(bae);
            if (linesToRemove.Count > 0)
            {
                addScore(calcScore(linesToRemove));
                setLines(lines);
            }
        }

        /*
         * Errechnet den Score anhand der Anzahl gelöschten Zeile und der Höhe
         */
        private int calcScore(List<int> linesToRemove)
        {
            linesToRemove.Reverse();
            double tmpScore = 0;

            for (int i = 1; i <= linesToRemove.Count; i++)
            {
                switch (getLevel())
                {
                    case 0: tmpScore += Math.Pow(50 + calcHeight(linesToRemove[i - 1]), calcLines(i)); break;
                    case 1: tmpScore += Math.Pow(100 + calcHeight(linesToRemove[i - 1]), calcLines(i)); break;
                    case 2: tmpScore += Math.Pow(150 + calcHeight(linesToRemove[i - 1]), calcLines(i)); break;
                    case 3: tmpScore += Math.Pow(200 + calcHeight(linesToRemove[i - 1]), calcLines(i)); break;
                    case 4: tmpScore += Math.Pow(250 + calcHeight(linesToRemove[i - 1]), calcLines(i)); break;
                    case 5: tmpScore += Math.Pow(300 + calcHeight(linesToRemove[i - 1]), calcLines(i)); break;
                    case 6: tmpScore += Math.Pow(350 + calcHeight(linesToRemove[i - 1]), calcLines(i)); break;
                    case 7: tmpScore += Math.Pow(400 + calcHeight(linesToRemove[i - 1]), calcLines(i)); break;
                    case 8: tmpScore += Math.Pow(450 + calcHeight(linesToRemove[i - 1]), calcLines(i)); break;
                    case 9: tmpScore += Math.Pow(500 + calcHeight(linesToRemove[i - 1]), calcLines(i)); break;
                }
            }
            return (int)tmpScore;
        }

        /**
         * Gibt einen Wert zurück, so dass der Score anhand der auf einmal gelöschten Zeilen ansteigt
         */
        private double calcLines(int lines)
        {
            switch (lines)
            {
                case 1: return 1;
                case 2: return 1.025;
                case 3: return 1.05;
                case 4: return 1.075;
            }
            return 0.0;
        }

        /*
         * Gibt einen Wert zurück, um den Score nach der Höhe der Zeile zu berrechnen die gelöscht wird
         */
        private int calcHeight(int height)
        {
            return ((height + 1) - 20) * -1;

        }

        public bool isCellColored(int x, int y)
        {
            return board[x, y] != boardColor && board[x, y] != fallenPreviewColor;
        }

        /**
         * Erstellt die Punkte der Figur auf dem Board
         */
        public void writeCell(Point[] points, Color c)
        {
            for (int i = 0; i < points.Length; i++)
            {
                board[(int)points[i].X, (int)points[i].Y] = c;
            }
            NotifyBoardChanged(new BoardEventArgs(getBoardData()));
        }

        /**
         * Erstellt die Punkte der Figur auf dem Board, wo sie hinkommen würde
         */
        public void writeCell(Point[] points, Point[] fallenPreviewPoints, Color pointsC, Color fppC)
        {
            for (int i = 0; i < fallenPreviewPoints.Length; i++)
            {
                board[(int)fallenPreviewPoints[i].X, (int)fallenPreviewPoints[i].Y] = fppC;
            }
            for (int i = 0; i < points.Length; i++)
            {
                board[(int)points[i].X, (int)points[i].Y] = pointsC;
            }
            
            NotifyBoardChanged(new BoardEventArgs(getBoardData()));
        }

        /**
         * Fügt die erzielten Punkte des Schrittes zum Gesamtscore dazu
         */
        public void addScore(int score)
        {
            this.score += score;
            ScoreEventArgs sea = new ScoreEventArgs();
            sea.score = this.score;

            sea.level = getLevel();
            NotifyScoreChanged(sea);
        }     

        /**
         * Generiert eine neue Figur
         */
        public Figure generateRandomFigure()
        {
            int random = rnd.Next(7);
            Figure figure = null;
            switch (random)
            {
                case 0: figure = new I(this); break;
                case 1: figure = new J(this); break;
                case 2: figure = new T(this); break;
                case 3: figure = new O(this); break;
                case 4: figure = new S(this); break;
                case 5: figure = new Z(this); break;
                case 6: figure = new L(this); break;
            }
            return figure;
        }

        /**
         * Löscht die Punkte einer Figur
         */
        public void clearPoints(Point[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                board[(int)points[i].X, (int)points[i].Y] = boardColor;
            }
        }

        /**
         * Löscht eien oder meherer Zeilen
         */
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

        /************************************************************** Notifier **************************************************************/
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

        public void NotifyLineChanged(LineEventArgs sea)
        {
            if (ScoreChanged != null)
            {
                LineChanged(this, sea);
            }
        }

        /************************************************************** Getter  und Setter Methoden **************************************************************/
        public Color getBoardColor()
        {
            return boardColor;
        }

        public Color getFallenPreviewColor()
        {
            return fallenPreviewColor;
        }

        public DefaultBoardModel()
        {
            clearBoard();
        }

        public int getColumns()
        {
            return columns;
        }

        public int getRows()
        {
            return rows;
        }

        public int getTempLines()
        {
            return tempLines;
        }
        
        public Figure getPreviewFigure()
        {
            return previewFigure;
        }

        public int getLines()
        {
            return lines;
        }

        public Color[,] getBoardData()
        {
            return board;
        }

        public Figure getCurrentFigure()
        {
            return currentFigure;
        }

        public Figure getMemoryFigure()
        {
            return memoryFigure;
        }

        public int getScore()
        {
            return score;
        }

        public int getLevel()
        {
            if (lines >= tempLines)
            {
                if ((lines / 10) > 9)
                {
                    return 9;
                }
                return (lines / 10);
            }
            else
            {
                return (tempLines / 10) - 1;
            }
        }

        public void setLines(int lines)
        {
            this.lines = lines;
            LineEventArgs sea = new LineEventArgs();
            sea.lines = lines;
            NotifyLineChanged(sea);
        }

        public void setTempLines(int lines)
        {
            this.tempLines = lines;
        }

        public void setPreviewFigure(Figure figure)
        {
            this.previewFigure = figure;
        }


        public void setCurrentFigure(Figure figure)
        {
            this.currentFigure = figure;
        }

        public void setMemoryFigure(Figure figure)
        {
            this.memoryFigure = figure;
        }
    }
}
