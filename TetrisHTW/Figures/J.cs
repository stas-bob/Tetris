using System;
using System.Net;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TetrisHTW.Util;

namespace TetrisHTW.Figures
{
    public class J : Figure
    {
        public J(Model.DefaultBoardModel boardModel)
            : base(boardModel)
        {
            color = Colors.Blue;
            points[0] = new Point(boardModel.getColumns() / 2, 0);
            points[1] = new Point(boardModel.getColumns() / 2, 1);
            points[2] = new Point(boardModel.getColumns() / 2, 2);
            points[3] = new Point(boardModel.getColumns() / 2 - 1, 2);
        }

        protected override void doRotate()
        {

            Point[] newPoints = new Point[4];

            switch (rotateState)
            {
                case 0:
                    newPoints[3].X = points[3].X;
                    newPoints[3].Y = points[3].Y - 2;
                    newPoints[2].X = points[2].X - 1;
                    newPoints[2].Y = points[2].Y - 1;
                    newPoints[1].X = points[1].X;
                    newPoints[1].Y = points[1].Y;
                    newPoints[0].X = points[0].X + 1;
                    newPoints[0].Y = points[0].Y + 1;
                    break;
                case 1:
                    newPoints[3].X = points[3].X + 2;
                    newPoints[3].Y = points[3].Y;
                    newPoints[2].X = points[2].X + 1;
                    newPoints[2].Y = points[2].Y - 1;
                    newPoints[1].X = points[1].X;
                    newPoints[1].Y = points[1].Y;
                    newPoints[0].X = points[0].X - 1;
                    newPoints[0].Y = points[0].Y + 1;
                    break;
                case 2:
                    newPoints[3].X = points[3].X;
                    newPoints[3].Y = points[3].Y + 2;
                    newPoints[2].X = points[2].X + 1;
                    newPoints[2].Y = points[2].Y + 1;
                    newPoints[1].X = points[1].X;
                    newPoints[1].Y = points[1].Y;
                    newPoints[0].X = points[0].X - 1;
                    newPoints[0].Y = points[0].Y - 1;
                    break;
                case 3:
                    newPoints[3].X = points[3].X - 2;
                    newPoints[3].Y = points[3].Y;
                    newPoints[2].X = points[2].X - 1;
                    newPoints[2].Y = points[2].Y + 1;
                    newPoints[1].X = points[1].X;
                    newPoints[1].Y = points[1].Y;
                    newPoints[0].X = points[0].X + 1;
                    newPoints[0].Y = points[0].Y - 1;
                    break;
            }
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
            else
            {
                switch (rotateState)
                {
                    case 0:
                        Point[] newPointsHardRotated = hardRotate(rotateState);
                        for (int i = 0; i < newPointsHardRotated.Length; i++)
                        {
                            newPointsHardRotated[i].X -= 1;
                        }
                        fit = doPointsFit(newPointsHardRotated);
                        if (fit)
                        {
                            points = newPointsHardRotated;
                            rotateState = 1;
                            break;
                        }
                        break;
                    case 2:
                        if (!fit)
                        {
                            newPointsHardRotated = hardRotate(rotateState);
                            for (int i = 0; i < newPointsHardRotated.Length; i++)
                            {
                                newPointsHardRotated[i].X += 1;
                            }
                            fit = doPointsFit(newPointsHardRotated);
                            if (fit)
                            {
                                points = newPointsHardRotated;
                                rotateState = 3;
                                break;
                            }
                        }
                        break;
                }
            }
        }

        public Point[] hardRotate(int rotateState)
        {
            Point[] newPoints = new Point[4];
            switch (rotateState)
            {
                case 0:
                    newPoints[3].X = points[3].X;
                    newPoints[3].Y = points[3].Y - 2;
                    newPoints[2].X = points[2].X - 1;
                    newPoints[2].Y = points[2].Y - 1;
                    newPoints[1].X = points[1].X;
                    newPoints[1].Y = points[1].Y;
                    newPoints[0].X = points[0].X + 1;
                    newPoints[0].Y = points[0].Y + 1;
                    break;
                case 2:
                    newPoints[3].X = points[3].X;
                    newPoints[3].Y = points[3].Y + 2;
                    newPoints[2].X = points[2].X + 1;
                    newPoints[2].Y = points[2].Y + 1;
                    newPoints[1].X = points[1].X;
                    newPoints[1].Y = points[1].Y;
                    newPoints[0].X = points[0].X - 1;
                    newPoints[0].Y = points[0].Y - 1;
                    break;
            }
            return newPoints;
        }

        public override string toString()
        {
            return "J";
        }
    }
}
