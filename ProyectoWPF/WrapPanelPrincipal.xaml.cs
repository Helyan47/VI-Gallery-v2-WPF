using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoWPF
{
    /// <summary>
    /// Lógica de interacción para WrapPanelPrincipal.xaml
    /// </summary>
    public partial class WrapPanelPrincipal : UserControl
    {
        public WrapPanelPrincipal()
        {
            InitializeComponent();
        }

        public void addComponent(object c) {
            
            Rectangle r= (Rectangle) c;

            r.Width = 200;
            r.Height = 100;
            r.Margin = new Thickness(10, 10, 10, 10);

            wrapPanel.Children.Add(r);
            
        }
    }
}
