using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Lógica de interacción para Menu.xaml
    /// </summary>
    public partial class Menu : UserControl {

        Carpeta carpeta;
        WrapPanelPrincipal wrapAnterior;
        WrapPanelPrincipal wrapCarpPrincipal;
        private Canvas _defaultCanvas;
        public Menu(Carpeta carpPrincipal) {
            InitializeComponent();
            _defaultCanvas = canvasFolder;
            carpeta = carpPrincipal;
        }

        public Carpeta getSerie() {
            return carpeta;
        }

        public void setDefaultSource() {
            canvasFolder = _defaultCanvas;
            ImgBorde.Visibility = Visibility.Visible;
            Img.Visibility = Visibility.Hidden;
        }

        public void actualizar() {
            Title.Content = carpeta.getClass().nombre;
            Descripcion.Content = carpeta.getClass().desc;
            ICollection<string> generosAux = carpeta.getClass().generos;
            if (generosAux.Count != 0) {
                listaGeneros.Content = carpeta.getClass().getGeneros();
            } else {
                Generos.Visibility = Visibility.Hidden;
                listaGeneros.Visibility = Visibility.Hidden;
            }

            if (carpeta.getClass().img != "") {
                try {
                    BitmapImage bm = new BitmapImage(new Uri(@carpeta.getClass().img, UriKind.Absolute));
                    ImageBrush ib = new ImageBrush(bm);
                    if (bm.Width > bm.Height) {
                        ib.Stretch = Stretch.UniformToFill;
                    }
                    Img.Background = ib;
                    Img.Visibility = Visibility.Visible;
                    ImgBorde.Visibility = Visibility.Hidden;
                }catch(ArgumentException e) {
                    setDefaultSource();
                    Console.WriteLine(e.Message);
                }
            }

        }

        public void SetFlowCarpPrincipal(WrapPanelPrincipal wrapCarp) {
            this.wrapCarpPrincipal = wrapCarp;

            WrapSubCarpetas.Children.Add(this.wrapCarpPrincipal);
        }

        public WrapPanelPrincipal getFlowCarpPrincipal() {
            return wrapCarpPrincipal;
        }

        public Carpeta getCarpeta() {
            return carpeta;
        }

        public void SetFlowLayAnterior(WrapPanelPrincipal fl) {
            wrapAnterior = fl;
        }

        public Grid getWrapSubCarpetas() {
            return WrapSubCarpetas;
        }

        public void changeTitle(string nombre) {
            Title.Content = nombre;
        }

        private void BReturn_Click(object sender, EventArgs e) {
            WrapPanelPrincipal p = Lista.getWrapCarptVisible();
            if (p.getCarpeta() == null) {
                SubCarpeta c = p.getSubCarpeta();
                //c.getPadreSerie().Visible = true;

                c.clickInverso();
            } else {
                p.getCarpeta().clickInverso();

                //flowLayAnterior.Visible = true;
                //panelBPrincipal = carpeta.getPrincipal();
                //panelBSecundario = carpeta.getSecundario();
                //panelBPrincipal.Visible = true;
                //panelBSecundario.Visible = false;
                //this.Visible = false;
            }

        }

        public void remove() {
            Lista.removeWrapPanelSecundario(wrapCarpPrincipal);
            wrapCarpPrincipal.removeChildrens();
            wrapCarpPrincipal = null;
        }
    }
}
