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
                ve.Width = 400;
                ve.Margin = new Thickness(0, 0, 20, 0);
            }
        }

        public void addVideoElement(VideoElement ve) {
            listaVideoElement.Add(ve);
            stackLista.Children.Add(ve);
            ve.Height = 200;
            ve.Width = 400;
            ve.Margin = new Thickness(0, 0, 20, 0);
        }
         
        public void clear() {
            stackLista.Children.Clear();
            listaVideoElement = new List<VideoElement>();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e) {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            if (e.Delta < 0) {
                scrollViewer.LineRight();
            } else {
                scrollViewer.LineLeft();
            }
            e.Handled = true;
        }
    }

}
