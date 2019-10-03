using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProyectoWPF {
    class Lista {
        private ICollection<WrapPanelPrincipal> wrapsPrincipales;
        private ICollection<Button> buttons;

        public Lista(ICollection<WrapPanelPrincipal> wraps, ICollection<Button> buttons) {
            this.wrapsPrincipales = wraps;
            this.buttons = buttons;
        }

        public Lista() {
            this.wrapsPrincipales = new List<WrapPanelPrincipal>();
            this.buttons = new List<Button>();
        }


        public bool buttonInButtons(Button b) {
            foreach(Button bt in buttons) {
                if (bt == b) {
                    return true;
                }
            }
            return false;
        }

        public void showWrapFromButton(Button b) {
            int comp = 0;
            int cont = 0;
            foreach (Button bt in buttons) {
                if (bt == b) {
                    comp = cont;
                }
                cont++;
            }
            cont = 0;
            foreach (WrapPanelPrincipal wp in wrapsPrincipales) {
                if (comp == cont) {
                    wp.Visibility = System.Windows.Visibility.Visible;
                } else {
                    wp.Visibility = System.Windows.Visibility.Hidden;
                }
                cont++;
            }
        }

        public void hideAllWraps() {
            foreach (WrapPanelPrincipal wp in wrapsPrincipales) {
                wp.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public WrapPanelPrincipal getWrapVisible() {
            foreach(WrapPanelPrincipal wp in wrapsPrincipales) {
                if (wp.Visibility == System.Windows.Visibility.Visible) {
                    return wp;
                }
            }
            return null;
        }
    }
}
