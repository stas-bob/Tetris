using System;
using System.Net;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TetrisHTW.tools;

namespace TetrisHTW.Figures
{
    public class Z : Figure
    {
        public Z(Model.DefaultBoardModel boardModel)
            : base(boardModel)
        {
            color = Color.FromArgb(255, 255, 0, 0);
            points[0] = new Point(2, 0);
            points[1] = new Point(3, 0);
            points[2] = new Point(3, 1);
            points[3] = new Point(4, 1);
        }

        public override void doRotate()
        {

            Point[] newPoints = new Point[4];

            switch (rotateState)
            {
                case 0:
                    newPoints[3].X = points[3].X - 1;
                    newPoints[3].Y = points[3].Y + 1;
                    newPoints[2].X = points[2].X;
                    newPoints[2].Y = points[2].Y;
                    newPoints[1].X = points[1].X + 1;
                    newPoints[1].Y = points[1].Y + 1;
                    newPoints[0].X = points[0].X + 2;
                    newPoints[0].Y = points[0].Y;
                    break;
                case 1:
                    newPoints[3].X = points[3].X - 1;
                    newPoints[3].Y = points[3].Y - 1;
                    newPoints[2].X = points[2].X;
                    newPoints[2].Y = points[2].Y;
                    newPoints[1].X = points[1].X - 1;
                    newPoints[1].Y = points[1].Y + 1;
                    newPoints[0].X = points[0].X;
                    newPoints[0].Y = points[0].Y + 2;
                    break;
                case 2:
                    newPoints[3].X = points[3].X + 1;
                    newPoints[3].Y = points[3].Y - 1;
                    newPoints[2].X = points[2].X;
                    newPoints[2].Y = points[2].Y;
                    newPoints[1].X = points[1].X - 1;
                    newPoints[1].Y = points[1].Y - 1;
                    newPoints[0].X = points[0].X - 2;
                    newPoints[0].Y = points[0].Y;
                    break;
                case 3:
                    newPoints[3].X = points[3].X + 1;
                    newPoints[3].Y = points[3].Y + 1;
                    newPoints[2].X = points[2].X;
                    newPoints[2].Y = points[2].Y;
                    newPoints[1].X = points[1].X + 1;
                    newPoints[1].Y = points[1].Y - 1;
                    newPoints[0].X = points[0].X;
                    newPoints[0].Y = points[0].Y - 2;
                    break;
            }
            board.clearPoints(points);
            bool fit = doPointsFit(newPoints);

            if (fit)
            {
                points = newPoints;
                if (rotateState == 0)
                {
                    rotateState = 1;
                }
                else
                {
                    if (rotateState == 1)
                    {
                        rotateState = 2;
                    }
                    else
                    {
                        if (rotateState == 2)
                        {
                            rotateState = 3;
                        }
                        else
                        {
                            if (rotateState == 3)
                            {
                                rotateState = 0;
                            }
                        }

                    }
                }


            }
            board.writeCell(points, color);
        }

        public override string toString()
        {
            return "Z";
        }
    }
}
