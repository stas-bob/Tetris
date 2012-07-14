﻿using System;
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
    public class S : Figure
    {
        public S(Model.DefaultBoardModel boardModel)
            : base(boardModel)
        {
            color = Colors.Green;
            points[0] = new Point(boardModel.getColumns() / 2 + 1, 0);
            points[1] = new Point(boardModel.getColumns() / 2, 0);
            points[2] = new Point(boardModel.getColumns() / 2, 1);
            points[3] = new Point(boardModel.getColumns() / 2 - 1, 1);
        }

        public override bool doRotate()
        {

            Point[] newPoints = new Point[4];

            switch (rotateState)
            {
                case 0:
                    newPoints[3].X = points[3].X + 1;
                    newPoints[3].Y = points[3].Y - 1;
                    newPoints[2].X = points[2].X;
                    newPoints[2].Y = points[2].Y;
                    newPoints[1].X = points[1].X + 1;
                    newPoints[1].Y = points[1].Y + 1;
                    newPoints[0].X = points[0].X;
                    newPoints[0].Y = points[0].Y + 2;
                    break;
                
                case 1:
                    newPoints[3].X = points[3].X - 1;
                    newPoints[3].Y = points[3].Y + 1;
                    newPoints[2].X = points[2].X;
                    newPoints[2].Y = points[2].Y;
                    newPoints[1].X = points[1].X - 1;
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
                        rotateState = 0;
                    }
                }
            }
            else
            {
                switch (rotateState)
                {
                    case 1:
                        Point[] newPointsHardRotated = hardRotate();
                        for (int i = 0; i < newPointsHardRotated.Length; i++)
                        {
                            newPointsHardRotated[i].X += 1;
                        }
                        fit = doPointsFit(newPointsHardRotated);
                        if (fit)
                        {
                            points = newPointsHardRotated;
                            rotateState = 0;
                            break;
                        }
                        break;
                }
            }
            board.writeCell(points, color);
            return fit;
        }


        public Point[] hardRotate()
        {
            Point[] newPoints = new Point[4];
            newPoints[3].X = points[3].X - 1;
            newPoints[3].Y = points[3].Y + 1;
            newPoints[2].X = points[2].X;
            newPoints[2].Y = points[2].Y;
            newPoints[1].X = points[1].X - 1;
            newPoints[1].Y = points[1].Y - 1;
            newPoints[0].X = points[0].X;
            newPoints[0].Y = points[0].Y - 2;
            return newPoints;
        }


        public override string toString()
        {
            return "S";
        }
    }
}