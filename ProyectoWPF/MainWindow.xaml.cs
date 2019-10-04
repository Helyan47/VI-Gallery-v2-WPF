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

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private ICollection<WrapPanelPrincipal> wrapsPrincipales;
        private ICollection<Button> botonesMenu;
        private Lista lista;
        public MainWindow() {
            InitializeComponent();
            UIElementCollection botones = buttonStack.Children;
            wrapsPrincipales = new List<WrapPanelPrincipal>();
            botonesMenu = new List<Button>();
            foreach (Button b in botones) {
                botonesMenu.Add(b);
                string name = b.Content.ToString();
                
                WrapPanelPrincipal wp = new WrapPanelPrincipal();
                wp.Name = name;
                gridPrincipal.Children.Add(wp);
                if (name.Equals("Anime")) {
                    wp.Visibility = Visibility.Visible;
                } else {
                    wp.Visibility = Visibility.Hidden;
                }
                wrapsPrincipales.Add(wp);
            }
            lista = new Lista(wrapsPrincipales, botonesMenu);
        }


        public void onClickButtonMenu(object sender,EventArgs e) {
            Button b = (Button)sender;

            if (lista.buttonInButtons(b)) {
                lista.showWrapFromButton(b);
            }
        }

        private void Button_MouseLeftButtonUp(object sender, RoutedEventArgs e) {
            Carpeta r = new Carpeta();
            WrapPanelPrincipal wp = lista.getWrapVisible();
            System.Windows.Media.Color c = ((SolidColorBrush)gridPrincipal.Background).Color;
            wp.setColorGridPadre(c);
            if (wp != null) {
                wp.addComponent(r);
            }
        }
    }
}
