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
using System.Collections.Generic;

namespace TetrisHTW.Model
{



    public interface BoardModel
    {

        event BoardChangedEventHandler BoardChanged;
        event ScoreChangedEventHandler ScoreChanged;
        event LineChangedEventHandler LineChanged;

        void collapse(List<int> linesToRemove);

        bool isCellColored(int x, int y);

        int getColumns();

        int getRows();

        Color getBoardColor();

        Color getFallenPreviewColor();

        void NotifyBoardChanged(BoardEventArgs bea);

        void NotifyScoreChanged(ScoreEventArgs bea);

        void writeCell(Point[] points, Color c);


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

        void setLines(int lines);
    }
}
