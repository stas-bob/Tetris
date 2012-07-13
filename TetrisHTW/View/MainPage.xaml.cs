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
using TetrisHTW.Figures;

namespace TetrisHTW
{
    public partial class MainPage : UserControl
    {

        private BoardModel boardModel;
        private FallWorker fallWorker;

        public MainPage()
        {
            this.boardModel = App.getInstance().getBoardModel();
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Page_KeyDown);
            boardModel.BoardChanged += new BoardChangedEventHandler(BoardChanged);
            boardModel.ScoreChanged += new ScoreChangedEventHandler(ScoreChanged);
            App.getInstance().GameOverEvent += new GameOverEventHandler(GameOver);
        }

        void Page_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left: boardModel.getCurrentFigure().left(); break;
                case Key.Right: boardModel.getCurrentFigure().right(); break;
                case Key.Up: boardModel.getCurrentFigure().rotate(); break;
            }
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (fallWorker != null)
            {
                fallWorker.RequestStop();
            }
            
            fallWorker = new FallWorker();
            boardModel.clearBoard();
            Figure preview = boardModel.generateRandomFigure();
            Figure current = boardModel.generateRandomFigure();

            boardModel.setCurrentFigure(current);
            boardModel.setPreviewFigure(preview);

            boardModel.getCurrentFigure().newOnBoard();
            
            new Thread(fallWorker.InvokeFalling).Start();
        }


        public void BoardChanged(object sender, BoardEventArgs bea)
        {
            Dispatcher.BeginInvoke(delegate
            {
                string s = "";
                Color[,] data = boardModel.getBoardData();
                for (int i = 0; i < boardModel.getRows(); i++)
                {
                    for (int j = 0; j < boardModel.getColumns(); j++)
                    {
                        s += boardModel.isCellColored(j, i) ? " # " : " ‒ ";
                    }
                    s += "\n";
                }
                label1.Content = s;
                label3.Content = boardModel.getPreviewFigure().toString();
            });
        }

        public void ScoreChanged(object sender, ScoreEventArgs bea)
        {
            Dispatcher.BeginInvoke(delegate
            {
                label2.Content = bea.score;
            });
            
        }

        public void GameOver(object sender, GameOverEventArgs goea)
        {
            Dispatcher.BeginInvoke(delegate
            {
                Debug.WriteLine("game over");
                fallWorker.RequestStop();
            });

        }

    }
}
