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

namespace TetrisHTW.Figures
{
    /**
     * Klasse für die Figur "O"
     */
    public class O: Figure
    {
        /**
        * Konstruktor
        */
        public O(Model.DefaultBoardModel boardModel): base(boardModel)
        {
            color = Colors.Yellow;
            setInitPoints();
        }

        /**
        * Setzen der initialen Punkte der Figur
        */
        public override void setInitPoints()
        {
            rotateState = 0;
            points[0] = new Point(board.getColumns() / 2, 0);
            points[1] = new Point(board.getColumns() / 2, 1);
            points[2] = new Point(board.getColumns() / 2 + 1, 0);
            points[3] = new Point(board.getColumns() / 2 + 1, 1);
        }

        // toString Methode
        public override string toString()
        {
            return "square";
        }
    }
}
