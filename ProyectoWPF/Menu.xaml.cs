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
        Grid gridButtonPrincipal;
        Grid gridButtonBSecundario;
        WrapPanelPrincipal wrapCarpPrincipal;
        public Menu(Carpeta carpPrincipal) {
            InitializeComponent();
            carpeta = carpPrincipal;
        }

        public Carpeta getSerie() {
            return carpeta;
        }

        public void actualizar() {
            Title.Content = carpeta.getSerie().getTitle();
            Descripcion.Content = carpeta.getSerie().getDesc();
            String cadena = "";
            ICollection<String> generosAux = carpeta.getSerie().getGeneros();
            if (generosAux.Count != 0) {
                for (int i = 0; i < generosAux.Count; i++) {

                    if (i == (generosAux.Count - 1)) {
                        cadena += generosAux.ElementAt(i);
                    } else {
                        cadena += generosAux.ElementAt(i);
                        cadena += " | ";
                    }
                }
                listaGeneros.Content = cadena;
            } else {
                Generos.Visibility = Visibility.Hidden;
                listaGeneros.Visibility = Visibility.Hidden;
            }

            if (carpeta.getSerie().getDirImg() != "") {

                ImageBrush ib = new ImageBrush(new BitmapImage(new Uri(@carpeta.getSerie().getDirImg(), UriKind.Absolute)));
                Img.Background = ib;
                Img.Visibility = Visibility.Visible;
                ImgBorde.Visibility = Visibility.Hidden;
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

        public void changeTitle(String nombre) {
            Title.Content = nombre;
        }

        private void BReturn_Click(object sender, EventArgs e) {
            WrapPanelPrincipal p = carpeta.getListaCarpetas().getWrapCarptVisible();
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
    }
}
