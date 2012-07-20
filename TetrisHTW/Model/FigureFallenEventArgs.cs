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
         public FigureFallenEventArgs(tools.Point[] previousPoints, tools.Point[] points, Color c)
        {
            this.figurePoints = points;
            this.previousFigurePoints = previousPoints;
            this.color = c;
        }


        public tools.Point[] figurePoints { get; set; }
        public tools.Point[] previousFigurePoints { get; set; }
        public Color color { get; set; }

        public bool PointsAreEqual()
        {
            for (int i = 0; i < figurePoints.Length; i++)
            {
                for (int j = 0; j < previousFigurePoints.Length; j++)
                {
                    if (figurePoints[i].X != figurePoints[j].X || figurePoints[i].Y != figurePoints[j].Y)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
