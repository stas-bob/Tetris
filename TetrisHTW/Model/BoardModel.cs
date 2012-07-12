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

namespace TetrisHTW.Model
{



    public interface BoardModel
    {

        event TetrisHTW.Model.BoardEventArgs.BoardChangedEventHandler BoardChanged;
        event TetrisHTW.Model.ScoreEventArgs.ScoreChangedEventHandler ScoreChanged;

        void collapse(int[] linesToRemove);

        bool isCellColored(int x, int y);

        int getColumns();

        int getRows();

        void NotifyBoardChanged(BoardEventArgs bea);

        void NotifyScoreChanged(ScoreEventArgs bea);

        void writeCell(Point[] points, Color c);

        Color[,] getBoardData();

        Figure getCurrentFigure();

        Figure getPreviewFigure();

        int getScore();

        void setScore(int score);

        Figure generateRandomFigure();

        void setPreviewFigure(Figure figure);

        void setCurrentFigure(Figure figure);

        void clearPoints(Point[] points);

        void shiftToLine(int y);

        void clearBoard();
    }
}
