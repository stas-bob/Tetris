using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace TetrisHTW.Model
{
    /**
     * Eventklasse für alle Boardevents
     */
    public class BoardEventArgs: EventArgs
    {
        // Attribute
        public List<int> removedLines { get; set; }
        public Color[,] boardData { get; set; }

        /**
         * Konstruktor
         */
        public BoardEventArgs(Color[,] boardData)
        {
            Color[,] newBoardData = new Color[boardData.GetLength(0), boardData.GetLength(1)];
            for (int i = 0; i < boardData.GetLength(1); i++)
            {
                for (int j = 0; j < boardData.GetLength(0); j++)
                {
                    newBoardData[j, i] = boardData[j, i];
                }
            }
            removedLines = new List<int>();
            this.boardData = newBoardData;
        }
    }
}
