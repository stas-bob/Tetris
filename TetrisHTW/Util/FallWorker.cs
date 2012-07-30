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
        private volatile bool shouldStop;
        private static object syncRoot = new Object();

        private int level = 0;

        public FallWorker(int level)
        {
            this.level = level;
        }

        public void setLevel(int level)
        {
            this.level = level;
        }

        /*Ist die Methode, die die Steine fallen laesst*/
        public void InvokeFalling()
        { 
            shouldStop = false;
            while (!shouldStop) {
                switch (level) {
                    case 0: Thread.Sleep(1000); break;
                    case 1: Thread.Sleep(900); break;
                    case 2: Thread.Sleep(800); break;
                    case 3: Thread.Sleep(700); break;
                    case 4: Thread.Sleep(600); break;
                    case 5: Thread.Sleep(500); break;
                    case 6: Thread.Sleep(400); break;
                    case 7: Thread.Sleep(300); break;
                    case 8: Thread.Sleep(200); break;
                    case 9: Thread.Sleep(160); break;
                }

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

        
    }
}
