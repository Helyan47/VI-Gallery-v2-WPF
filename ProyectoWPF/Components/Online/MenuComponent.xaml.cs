using ProyectoWPF;
using SeleccionarProfile.Data.Online;
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

namespace ProyectoWPF.Components.Online {
    /// <summary>
    /// Lógica de interacción para MenuComponent.xaml
    /// </summary>
    public partial class MenuComponent : UserControl {
        private WrapPanelPrincipal _wrapSeries;
        private SerieComponent _serie;
        private List<TemporadaComponent> _tempComponents = new List<TemporadaComponent>();
        private Canvas _defaultCanvas;
        private VIGallery _ventanaMain;
        private Grid _gridPrincipal;
        public bool isVisible { get; set; }

        public MenuComponent(WrapPanelPrincipal wp, SerieComponent s, VIGallery vi, Grid gridPrincipal) {
            InitializeComponent();
            _defaultCanvas = canvasFolder;
            _wrapSeries = wp;
            setSerie(s);
            isVisible = false;
            _ventanaMain = vi;
            _gridPrincipal = gridPrincipal;
        }

        public void setSerie(SerieComponent s) {
            if (s != null) {
                _serie = s;
                wrapTemporadas.setSerie(_serie);
                actualizar();
            }
        }

        public void actualizar() {
            Title.Content = _serie.getSerie().nombre;
            Descripcion.Content = _serie.getSerie().descripcion;
            string cadena = "";
            ICollection<string> generosAux = _serie.getSerie().generos;
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

            if (_serie.getSerie().img != "") {
                try {
                    BitmapImage bm = new BitmapImage(new Uri(@_serie.getSerie().img, UriKind.Absolute));
                    ImageBrush ib = new ImageBrush(bm);
                    ib.Stretch = Stretch.UniformToFill;
                    Img.Background = ib;
                    Img.Visibility = Visibility.Visible;
                    ImgBorde.Visibility = Visibility.Hidden;
                } catch (ArgumentException e) {
                    setDefaultSource();
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void setDefaultSource() {
            canvasFolder = _defaultCanvas;
            ImgBorde.Visibility = Visibility.Visible;
            Img.Visibility = Visibility.Hidden;
        }

        public void setTempComponents(List<TemporadaComponent> temporadaComponents) {
            _tempComponents = temporadaComponents;
            foreach(TemporadaComponent tc in _tempComponents) {
                WrapPanelPrincipal wp = new WrapPanelPrincipal();
                wp.setTemporada(tc);
                tc.setPanelArchivos(wp);
                wrapTemporadas.addTemporada(tc);
                gridTemporadas.Children.Add(wp);
                wp.Visibility = Visibility.Hidden;
            }
        }

        public void addTempComponent(TemporadaComponent tempComp) {
            _tempComponents.Add(tempComp);
            WrapPanelPrincipal wp = new WrapPanelPrincipal();
            wp.setTemporada(tempComp);
            tempComp.setPanelArchivos(wp);
            wrapTemporadas.addTemporada(tempComp);
            gridTemporadas.Children.Add(wp);
            wp.Visibility = Visibility.Hidden;
        }

        public void onSerieClick() {
            _wrapSeries.Visibility = Visibility.Hidden;
            this.Visibility = Visibility.Visible;
            isVisible = true;
            _ventanaMain.ReturnVisibility(true);
        }

        public void onTempCompClick(TemporadaComponent temporadaComponent) {
            wrapTemporadas.Visibility = Visibility.Hidden;
            temporadaComponent.getPanelArchivos().Visibility = Visibility.Visible;
            Title.Content = _serie.getSerie().nombre + " Temporada " + temporadaComponent.getTemporada().numTemporada;
        }

        public bool onReturn() {
            if (wrapTemporadas.Visibility == Visibility.Visible) {
                _wrapSeries.Visibility = Visibility.Visible;
                this.Visibility = Visibility.Hidden;
                isVisible = false;
                _ventanaMain.clearTextBox(wrapTemporadas);
                return false;
            } else {
                wrapTemporadas.Visibility = Visibility.Visible;
                foreach(TemporadaComponent tc in _tempComponents) {
                    tc.getPanelArchivos().Visibility = Visibility.Hidden;
                }
                Title.Content = _serie.getSerie().nombre;
                _ventanaMain.clearTextBox(getWrapVisible());
                return true;
            }
            
        }

        public void removeAll() {
            foreach(TemporadaComponent tc in _tempComponents) {
                tc.removeAll();
            }
            if (_gridPrincipal.Children.Contains(this)) {
                _gridPrincipal.Children.Remove(this);
            }
            wrapTemporadas.removeChildrens();
            _wrapSeries.removeChildrens();
        }

        public WrapPanelPrincipal getWrapVisible() {
            if(wrapTemporadas.Visibility == Visibility.Visible) {
                return wrapTemporadas;
            } else {
                foreach(TemporadaComponent t in _tempComponents) {
                    if (t.getPanelArchivos().Visibility == Visibility.Visible) {
                        return t.getPanelArchivos();
                    }
                }
            }
            return null;
        }
    }
}
