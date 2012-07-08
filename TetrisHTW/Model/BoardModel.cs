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
using TetrisHTW.View;
using TetrisHTW.tools;

namespace TetrisHTW.Model
{
    public interface BoardModel
    {
        
        event EventHandler BoardChanged;

         void collapse(int[] linesToRemove);

         bool isCellColored(int x, int y);

         int getColumns();

         int getRows();

         void NotifyBoardChanged();

         void registerView(BoardView v);

         void writeCell(Point[] points, Color c);

         Color[,] getBoardData();

         Figure getCurrentFigure();

         int getScore();

         void setScore(int score);

         void generateRandomFigure();

         void clearPoints(Point[] points);

         void shiftToLine(int y);

         void clearBoard();
    }
}
