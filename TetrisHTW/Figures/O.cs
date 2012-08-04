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
    public class O: Figure
    {
        public O(Model.DefaultBoardModel boardModel): base(boardModel)
        {
            color = Colors.Yellow;
            setInitPoints(boardModel);
        }

        public void setInitPoints(Model.DefaultBoardModel boardModel)
        {
            points[0] = new Point(boardModel.getColumns() / 2, 0);
            points[1] = new Point(boardModel.getColumns() / 2, 1);
            points[2] = new Point(boardModel.getColumns() / 2 + 1, 0);
            points[3] = new Point(boardModel.getColumns() / 2 + 1, 1);
        }

        public override string toString()
        {
            return "square";
        }
    }
}
