using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TetrisHTW.Model;
using System.Threading;
using System.Diagnostics;
using TetrisHTW.View;

namespace TetrisHTW
{
    public delegate void BoardChangedEventHandler(object sender, BoardEventArgs e);
    public delegate void ScoreChangedEventHandler(object sender, ScoreEventArgs e);
    public delegate void LineChangedEventHandler(object sender, LineEventArgs e);
    public delegate void GameOverEventHandler(object sender, GameOverEventArgs e);
    public delegate void FigureFallenEventHandler(object sender, FigureFallenEventArgs e);

    public partial class App : Application
    {
        

        public static Lock myLock = new Lock();
        private BoardModel boardModel = new DefaultBoardModel();
        
        private static App instance;

        public event FigureFallenEventHandler FigureFallenEvent;
        public event GameOverEventHandler GameOverEvent;
        public static bool DEBUG = true;
        
        public App()
        {
            if (DEBUG)
            {
                Application.Current.Host.Settings.EnableFrameRateCounter = true;
            }
            instance = this;
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
            

        }

        public static App getInstance()
        {
            return instance;
        }

        public BoardModel getBoardModel()
        {
            return boardModel;
        }

        public void NotifyFigureFallen(tools.Point[] previousPoints, tools.Point[] points, Color c)
        {
            if (FigureFallenEvent != null)
            {
                FigureFallenEventArgs ffea = new FigureFallenEventArgs(previousPoints, points, c);
                FigureFallenEvent(this, ffea);
            }
        }

        public void gameOver()
        {
            if (GameOverEvent != null)
            {
                GameOverEvent(this, new GameOverEventArgs());
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new IndexView();
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // Wenn die Anwendung außerhalb des Debuggers ausgeführt wird, melden Sie die Ausnahme mithilfe
            // des Ausnahmemechanismus des Browsers. In IE wird hier ein gelbes Warnsymbol in der 
            // Statusleiste angezeigt, Firefox zeigt einen Skriptfehler an.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // Hinweis: So kann die Anwendung weiterhin ausgeführt werden, nachdem eine Ausnahme ausgelöst, aber nicht
                // behandelt wurde. 
                // Bei Produktionsanwendungen sollte diese Fehlerbehandlung durch eine Anwendung ersetzt werden, die 
                // den Fehler der Website meldet und die Anwendung beendet.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
