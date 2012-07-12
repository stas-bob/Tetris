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
    public class O: Figure
    {
        public O(Model.DefaultBoardModel boardModel): base(boardModel)
        {
            color = Color.FromArgb(255, 255, 0, 0);
            points[0] = new Point(3, 0);
            points[1] = new Point(3, 1);
            points[2] = new Point(4, 0);
            points[3] = new Point(4, 1);
        }

        public override string toString()
        {
            return "square";
        }
    }
}
