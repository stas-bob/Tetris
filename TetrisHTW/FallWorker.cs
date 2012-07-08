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
        private App app;

        public FallWorker(App app)
        {
            this.app = app;
        }

        public void InvokeFalling()
        {
            shouldStop = false;
            while (!shouldStop)
            {
                Thread.Sleep(500);

                lock (App.myLock)
                {
                    if (shouldStop)
                    {
                        break;
                    }
                    app.getBoardModel().getCurrentFigure().fall(app);
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
