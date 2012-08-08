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

namespace TetrisHTW
{
    /*Der Fallworker und main thread(space, oben), Timer thread(links, rechts, runter)
         * können gleichzeitig auf die Figurpunkte ändernd zugreifen. das wird mit dem lock unterbunden*/
    public class Lock
    {

    }
}
