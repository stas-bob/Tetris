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
    public partial class MainPage : UserControl, BoardView
    {

        private BoardModel boardModel;
        private FallWorker fallWorker;

        public MainPage(BoardModel boardModel, FallWorker fallWorker)
        {
            this.boardModel = boardModel;
            this.fallWorker = fallWorker;
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Page_KeyDown);
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

        public void updateBoard()
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
            });
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            new Thread(fallWorker.InvokeFalling).Start();
        }
    }
}
