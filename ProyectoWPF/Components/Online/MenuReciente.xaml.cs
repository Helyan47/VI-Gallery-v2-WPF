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

namespace ProyectoWPF.Components {
    /// <summary>
    /// Lógica de interacción para MenuReciente.xaml
    /// </summary>
    public partial class MenuReciente : UserControl {
        private List<object> lista = new List<object>();
        private VIGallery main;
        public MenuReciente() {
            InitializeComponent();
            
        }

        public void setMain(VIGallery vi) {
            this.main = vi;
        }

        public void setList(List<object> lista) {
            if(lista.Count == 8) {
                this.lista = lista;
                int cont = 0;
                foreach(object o in lista) {
                    cont++;
                }
            }
        }

        public void clear() {
            /*img1.clear();
            img2.clear();
            img3.clear();
            img4.clear();
            img5.clear();
            img6.clear();
            img7.clear();
            img8.clear();*/
            lista = new List<object>();
        }
    }
}
