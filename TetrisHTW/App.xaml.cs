﻿using System;
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
    // Delegates
    public delegate void BoardChangedEventHandler(object sender, BoardEventArgs e);
    public delegate void ScoreChangedEventHandler(object sender, ScoreEventArgs e);
    public delegate void LineChangedEventHandler(object sender, LineEventArgs e);
    public delegate void GameOverEventHandler(object sender, EventArgs e);
    public delegate void FigureFallenEventHandler(object sender, FigureFallenEventArgs e);

    /**
     * Hauptklasse App
     */
    public partial class App : Application
    {
        // Attribute
        private DefaultBoardModel boardModel = new DefaultBoardModel();
        public event FigureFallenEventHandler FigureFallenEvent;
        public event GameOverEventHandler GameOverEvent;

        // Static Attribute
        private static App instance;
        public static bool DEBUG = false;
        public static Lock myLock = new Lock();
        
        /**
         * Konstruktor
         */
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

        /**
         * Notifier für das Fallen einer Figur
         */
        public void NotifyFigureFallen(Util.Point[] previousPoints, Util.Point[] points, Color c)
        {
            if (FigureFallenEvent != null)
            {
                FigureFallenEventArgs ffea = new FigureFallenEventArgs(previousPoints, points, c);
                FigureFallenEvent(this, ffea);
            }
        }

        /**
         * GameOver Methode
         */
        public void gameOver()
        {
            if (GameOverEvent != null)
            {
                GameOverEvent(this, EventArgs.Empty);
            }
        }

        /**
         * Startmethode für die Anwenung
         */
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new IndexView();
        }

        /**
         * Schließen der Anwendung
         */
        private void Application_Exit(object sender, EventArgs e)
        {

        }

        /**
         * Handler fur nicht behandelete Exceptions
         */
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

        /**
         * Reportmethoden für Errors
         */
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

        /******************************************** Getter Methoden ***************************************************************/
        public static App getInstance()
        {
            return instance;
        }

        public DefaultBoardModel getBoardModel()
        {
            return boardModel;
        }
    }
}
