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
using System.Windows.Navigation;

namespace TetrisHTW.View
{
    /**
     * Klasse für den About-View
     */
    public partial class AboutView : UserControl
    {
        // Attribute
        private IndexView iv;

        /**
         * Konstruktor
         */
        public AboutView(IndexView iv)
        {
            this.iv = iv;
            InitializeComponent();
        }

        /**
         * Handler für Buttpn "Zurück"
         */
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            iv.rootContainer.Child = iv.LayoutRoot;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton button = (HyperlinkButton)sender;
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(button.Tag.ToString()), "_blank");
        }
    }
}
