﻿using System;
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
    /**
     * Klasse für die Figur "I"
     */
    public class I: Figure
    {
        /**
         * Konstruktor
         */
        public I(Model.DefaultBoardModel boardModel) : base(boardModel)
        {
            color = Colors.Cyan;
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
            points[2] = new Point(board.getColumns() / 2, 2);
            points[3] = new Point(board.getColumns() / 2, 3);
        }

        /**
         * Figurspezifische Drehung
         */
        protected override void doRotate()
        {
            Point[] newPoints = new Point[4];

            switch (rotateState)
            {
                case 0:
                    newPoints[3].X = points[3].X - 2;
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
                    newPoints[3].Y = points[3].Y + 2;
                    newPoints[2].X = points[2].X + 1;
                    newPoints[2].Y = points[2].Y + 1;
                    newPoints[1].X = points[1].X;
                    newPoints[1].Y = points[1].Y;
                    newPoints[0].X = points[0].X - 1;
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
                        rotateState = 0;
                    }
                }
            }
            else
            {
                if (rotateState == 0)
                {
                    Point[] newPointsHardRotated = hardRotate();
                    //links
                    for (int j = 0; j < 2; j++) //j schleife wegen 2 herausstechenden punkten beim I
                    {
                        for (int i = 0; i < newPointsHardRotated.Length; i++)
                        {
                            newPointsHardRotated[i].X += 1;
                        }
                        fit = doPointsFit(newPointsHardRotated);
                        if (fit)
                        {
                            points = newPointsHardRotated;
                            rotateState = 1;
                            break;
                        }
                    }
                    if (!fit)
                    {
                        //rechts
                        newPointsHardRotated = hardRotate();
                        for (int i = 0; i < newPointsHardRotated.Length; i++)
                        {
                            newPointsHardRotated[i].X -= 1;
                        }
                        fit = doPointsFit(newPointsHardRotated);
                        if (fit)
                        {
                            points = newPointsHardRotated;
                            rotateState = 1;
                        }
                    }
                }
            }
            
        }

        /**
         * Es wird probiert die Figur nach links/rechts zu verschieben, 
         * damit die Drehung doch kalppt, auch wenn kein platz da ist. 
         * 
         * siehe doRotate()
         */ 
        public Point[] hardRotate()
        {
            Point[] newPoints = new Point[4];
            newPoints[3].X = points[3].X - 2;
            newPoints[3].Y = points[3].Y - 2;
            newPoints[2].X = points[2].X - 1;
            newPoints[2].Y = points[2].Y - 1;
            newPoints[1].X = points[1].X;
            newPoints[1].Y = points[1].Y;
            newPoints[0].X = points[0].X + 1;
            newPoints[0].Y = points[0].Y + 1;
                
            return newPoints;
        }

        // toString Methode
        public override string toString()
        {
            return "bar";
        }
    }
}
