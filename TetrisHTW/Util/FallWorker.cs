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
using System.Threading;
using System.Diagnostics;

namespace TetrisHTW
{
    public class FallWorker
    {
        /*Ist die Methode, die die Steine fallen laesst*/
        public void InvokeFalling()
        {
            shouldStop = false;
            while (!shouldStop)
            {
                Thread.Sleep(400);

                lock (App.myLock)
                {
                    if (shouldStop)
                    {
                        break;
                    }
                    App.getInstance().getBoardModel().getCurrentFigure().fall();
                }

            }
        }

        public void RequestStop()
        {
            shouldStop = true;
        }

        private volatile bool shouldStop;
    }
}
