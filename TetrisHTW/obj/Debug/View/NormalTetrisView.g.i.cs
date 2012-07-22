﻿#pragma checksum "C:\Users\bline\Documents\Visual Studio 2010\Projects\Tetris\TetrisHTW\View\NormalTetrisView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "85C909E556AF8C02179383EAD4F14B18"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.269
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace TetrisHTW {
    
    
    public partial class NormalTetrisView : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Canvas canvas;
        
        internal System.Windows.Media.Animation.Storyboard flyingAroundSB;
        
        internal System.Windows.Media.Animation.DoubleAnimation animXT;
        
        internal System.Windows.Media.Animation.DoubleAnimation animYT;
        
        internal System.Windows.Media.Animation.DoubleAnimation animXL;
        
        internal System.Windows.Media.Animation.DoubleAnimation animYL;
        
        internal System.Windows.Media.Animation.DoubleAnimation animXJ;
        
        internal System.Windows.Media.Animation.DoubleAnimation animYJ;
        
        internal System.Windows.Media.Animation.DoubleAnimation animXO;
        
        internal System.Windows.Media.Animation.DoubleAnimation animYO;
        
        internal System.Windows.Media.Animation.DoubleAnimation animXI;
        
        internal System.Windows.Media.Animation.DoubleAnimation animYI;
        
        internal System.Windows.Media.Animation.DoubleAnimation animXS;
        
        internal System.Windows.Media.Animation.DoubleAnimation animYS;
        
        internal System.Windows.Media.Animation.DoubleAnimation animXZ;
        
        internal System.Windows.Media.Animation.DoubleAnimation animYZ;
        
        internal System.Windows.Media.Animation.Storyboard rotatingSB;
        
        internal System.Windows.Media.Animation.DoubleAnimation animRotateT;
        
        internal System.Windows.Media.Animation.DoubleAnimation animRotateI;
        
        internal System.Windows.Media.Animation.DoubleAnimation animRotateZ;
        
        internal System.Windows.Media.Animation.DoubleAnimation animRotateS;
        
        internal System.Windows.Media.Animation.DoubleAnimation animRotateO;
        
        internal System.Windows.Media.Animation.DoubleAnimation animRotateL;
        
        internal System.Windows.Media.Animation.DoubleAnimation animRotateJ;
        
        internal System.Windows.Media.Animation.Storyboard pauseOnSB;
        
        internal System.Windows.Media.Animation.DoubleAnimation animPauseOn;
        
        internal System.Windows.Media.Animation.Storyboard pauseOffSB;
        
        internal System.Windows.Media.Animation.DoubleAnimation animPauseOff;
        
        internal System.Windows.Media.Animation.Storyboard levelFontSizeSB;
        
        internal System.Windows.Media.Animation.DoubleAnimation animLevelFontScale;
        
        internal System.Windows.Shapes.Path T;
        
        internal System.Windows.Media.RotateTransform rotateTransformT;
        
        internal System.Windows.Shapes.Path O;
        
        internal System.Windows.Media.RotateTransform rotateTransformO;
        
        internal System.Windows.Shapes.Path L;
        
        internal System.Windows.Media.RotateTransform rotateTransformL;
        
        internal System.Windows.Shapes.Path J;
        
        internal System.Windows.Media.RotateTransform rotateTransformJ;
        
        internal System.Windows.Shapes.Path I;
        
        internal System.Windows.Media.RotateTransform rotateTransformI;
        
        internal System.Windows.Shapes.Path Z;
        
        internal System.Windows.Media.RotateTransform rotateTransformZ;
        
        internal System.Windows.Shapes.Path S;
        
        internal System.Windows.Media.RotateTransform rotateTransformS;
        
        internal System.Windows.Controls.Border levelHint;
        
        internal System.Windows.Controls.TextBlock levelHintTextBlock;
        
        internal System.Windows.Controls.Border layoutBorder;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock scoreText;
        
        internal System.Windows.Controls.TextBlock linesText;
        
        internal System.Windows.Controls.Grid previewGrid;
        
        internal System.Windows.Controls.TextBlock levelText;
        
        internal System.Windows.Media.ScaleTransform levelTextFontScale;
        
        internal System.Windows.Controls.Grid boardGrid;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/TetrisHTW;component/View/NormalTetrisView.xaml", System.UriKind.Relative));
            this.canvas = ((System.Windows.Controls.Canvas)(this.FindName("canvas")));
            this.flyingAroundSB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("flyingAroundSB")));
            this.animXT = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animXT")));
            this.animYT = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animYT")));
            this.animXL = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animXL")));
            this.animYL = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animYL")));
            this.animXJ = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animXJ")));
            this.animYJ = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animYJ")));
            this.animXO = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animXO")));
            this.animYO = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animYO")));
            this.animXI = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animXI")));
            this.animYI = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animYI")));
            this.animXS = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animXS")));
            this.animYS = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animYS")));
            this.animXZ = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animXZ")));
            this.animYZ = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animYZ")));
            this.rotatingSB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("rotatingSB")));
            this.animRotateT = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animRotateT")));
            this.animRotateI = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animRotateI")));
            this.animRotateZ = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animRotateZ")));
            this.animRotateS = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animRotateS")));
            this.animRotateO = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animRotateO")));
            this.animRotateL = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animRotateL")));
            this.animRotateJ = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animRotateJ")));
            this.pauseOnSB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("pauseOnSB")));
            this.animPauseOn = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animPauseOn")));
            this.pauseOffSB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("pauseOffSB")));
            this.animPauseOff = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animPauseOff")));
            this.levelFontSizeSB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("levelFontSizeSB")));
            this.animLevelFontScale = ((System.Windows.Media.Animation.DoubleAnimation)(this.FindName("animLevelFontScale")));
            this.T = ((System.Windows.Shapes.Path)(this.FindName("T")));
            this.rotateTransformT = ((System.Windows.Media.RotateTransform)(this.FindName("rotateTransformT")));
            this.O = ((System.Windows.Shapes.Path)(this.FindName("O")));
            this.rotateTransformO = ((System.Windows.Media.RotateTransform)(this.FindName("rotateTransformO")));
            this.L = ((System.Windows.Shapes.Path)(this.FindName("L")));
            this.rotateTransformL = ((System.Windows.Media.RotateTransform)(this.FindName("rotateTransformL")));
            this.J = ((System.Windows.Shapes.Path)(this.FindName("J")));
            this.rotateTransformJ = ((System.Windows.Media.RotateTransform)(this.FindName("rotateTransformJ")));
            this.I = ((System.Windows.Shapes.Path)(this.FindName("I")));
            this.rotateTransformI = ((System.Windows.Media.RotateTransform)(this.FindName("rotateTransformI")));
            this.Z = ((System.Windows.Shapes.Path)(this.FindName("Z")));
            this.rotateTransformZ = ((System.Windows.Media.RotateTransform)(this.FindName("rotateTransformZ")));
            this.S = ((System.Windows.Shapes.Path)(this.FindName("S")));
            this.rotateTransformS = ((System.Windows.Media.RotateTransform)(this.FindName("rotateTransformS")));
            this.levelHint = ((System.Windows.Controls.Border)(this.FindName("levelHint")));
            this.levelHintTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("levelHintTextBlock")));
            this.layoutBorder = ((System.Windows.Controls.Border)(this.FindName("layoutBorder")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.scoreText = ((System.Windows.Controls.TextBlock)(this.FindName("scoreText")));
            this.linesText = ((System.Windows.Controls.TextBlock)(this.FindName("linesText")));
            this.previewGrid = ((System.Windows.Controls.Grid)(this.FindName("previewGrid")));
            this.levelText = ((System.Windows.Controls.TextBlock)(this.FindName("levelText")));
            this.levelTextFontScale = ((System.Windows.Media.ScaleTransform)(this.FindName("levelTextFontScale")));
            this.boardGrid = ((System.Windows.Controls.Grid)(this.FindName("boardGrid")));
        }
    }
}

