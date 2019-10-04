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
        private System.Windows.Media.Color colorGridPadre;
        public WrapPanelPrincipal()
        {
            InitializeComponent();
        }

        public void addComponent(Carpeta c) {

            c.setColorGridPadre(colorGridPadre);
            c.Width = 250;
            c.Height = 400;
            c.Margin = new Thickness(10, 10, 10, 10);
            c.setDefaultSource();
            //c.changeColor(System.Drawing.Color.Red);
            wrapPanel.Children.Add(c);
            
        }

        public void setColorGridPadre(Color grid) {
            this.colorGridPadre = grid;
        }
    }
}
