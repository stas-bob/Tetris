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

namespace TetrisHTW.Model
{
    public class FigureFallenEventArgs: EventArgs
    {
        public FigureFallenEventArgs(Util.Point[] previousPoints, Util.Point[] points, Color c)
        {
            this.figurePoints = points;
            this.previousFigurePoints = previousPoints;
            this.color = c;
        }


         public Util.Point[] figurePoints { get; set; }
         public Util.Point[] previousFigurePoints { get; set; }
        public Color color { get; set; }

        public bool PointsAreEqual()
        {
            for (int i = 0; i < figurePoints.Length; i++)
            {
                if (figurePoints[i].X != previousFigurePoints[i].X || figurePoints[i].Y != previousFigurePoints[i].Y)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
