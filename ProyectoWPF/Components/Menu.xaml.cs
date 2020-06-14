using ProyectoWPF.Components;
using ProyectoWPF.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para Menu.xaml
    /// </summary>
    public partial class Menu : UserControl {

        Carpeta carpeta;
        WrapPanelPrincipal wrapAnterior;
        WrapPanelPrincipal wrapCarpPrincipal;
        private Canvas _defaultCanvas;
        public Menu() {
            InitializeComponent();
            _defaultCanvas = canvasFolder;
        }

        public Carpeta getSerie() {
            return carpeta;
        }

        public void setDefaultSource() {
            canvasFolder = _defaultCanvas;
            ImgBorde.Visibility = Visibility.Visible;
            Img.Visibility = Visibility.Hidden;
        }

        public WrapPanelPrincipal getWrap() {
            return WrapSubCarpetas;
        }

        public void actualizar(Carpeta cp) {
            carpeta = cp;
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
                } catch (ArgumentException e) {
                    setDefaultSource();
                    Console.WriteLine(e.Message);
                }
            }
            WrapSubCarpetas.removeChildrens();

            List<Carpeta> carpetasHijo = carpeta.getCarpetasHijos();
            if (carpetasHijo != null) {
                foreach (Carpeta c in carpetasHijo) {
                    WrapSubCarpetas.addCarpeta(c);
                }
            }
            List<Archivo> archivos = carpeta._archivos;
            if (archivos != null) {
                foreach (Archivo a in archivos) {
                    WrapSubCarpetas.addFile(a);
                }
            }
            List<UIElement> hijos = OrderClass.orderChildOfWrap(WrapSubCarpetas.hijos);
            WrapSubCarpetas.getWrapPanel().Children.Clear();
            foreach (UIElement o in hijos) {
                WrapSubCarpetas.addUIElement(o);
            }
        }

        public Carpeta getCarpeta() {
            return carpeta;
        }

        public void SetFlowLayAnterior(WrapPanelPrincipal fl) {
            wrapAnterior = fl;
        }

        public void changeTitle(string nombre) {
            Title.Content = nombre;
        }
    }
}
