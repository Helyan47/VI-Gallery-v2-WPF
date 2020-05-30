using ProyectoWPF.Data.Online;
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
    /// Lógica de interacción para ListaArchivos.xaml
    /// </summary>
    public partial class ListaArchivos : UserControl {

        private List<VideoElement> listaVideoElement = new List<VideoElement>();

        public string Titulo {
            get { return title.Content.ToString(); }
            set { title.Content = value; }
        }
        public ListaArchivos() {
            InitializeComponent();
        }

        public void setLista(List<VideoElement> lista) {
            listaVideoElement = lista;
            foreach(VideoElement ve in lista) {
                stackLista.Children.Add(ve);
                this.VerticalAlignment = VerticalAlignment.Stretch;
                this.HorizontalAlignment = HorizontalAlignment.Stretch;
            }
        }

        public void addVideoElement(VideoElement ve) {
            listaVideoElement.Add(ve);
            stackLista.Children.Add(ve);
            this.VerticalAlignment = VerticalAlignment.Stretch;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
        }
    }
}
