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

namespace SeleccionarProfile.Components.Online {
    /// <summary>
    /// Lógica de interacción para TemporadaComponent.xaml
    /// </summary>
    public partial class TemporadaComponent : UserControl {
        private Temporada _temporada;
        private Canvas _defaultCanvas;
        private MenuComponent _menu;
        private List<ArchivoComponent> _archivos = new List<ArchivoComponent>();
        private WrapPanelPrincipal _panelArchivos;
        public TemporadaComponent(MenuComponent menu, Temporada t) {
            InitializeComponent();
            _menu = menu;
            setTemporada(t);
            
        }

        public void setTemporada(Temporada t) {
            if (t != null) {
                _temporada = t;
                actualizar();
                _defaultCanvas = canvasFolder;
            }
            
        }

        public void setPanelArchivos(WrapPanelPrincipal panel) {
            _panelArchivos = panel;
        }

        public WrapPanelPrincipal getPanelArchivos() {
            return _panelArchivos;
        }

        public void setArchivos(List<ArchivoComponent> archivos) {
            if (archivos != null) {
                _archivos = archivos;
                foreach(ArchivoComponent ac in _archivos) {
                    _panelArchivos.addEpisodio(ac);
                }
                
            }
        }

        public Temporada getTemporada() {
            return _temporada;
        }

        public void setDefaultSource() {
            canvasFolder = _defaultCanvas;
            Img.Visibility = Visibility.Visible;
        }

        public void actualizar() {
            Title.SetText("Temporada "+_temporada.numTemporada);
            setImg();
        }

        public void setImg() {
            if (!_temporada.img.Equals("")) {
                try {
                    BitmapImage bm = new BitmapImage(new Uri(_temporada.img, UriKind.RelativeOrAbsolute));
                    ImageBrush ib = new ImageBrush(bm);

                    ib.Stretch = Stretch.UniformToFill;

                    ImgBorde.Background = ib;
                    Img.Visibility = Visibility.Hidden;
                } catch (Exception e) {
                    setDefaultSource();
                    Console.WriteLine(e.Message);
                }
            } else {
                Serie s = ListaOnline.getSerieByTemp(_temporada);
                if (!s.img.Equals("")) {
                    try {
                        BitmapImage bm = new BitmapImage(new Uri(s.img, UriKind.RelativeOrAbsolute));
                        ImageBrush ib = new ImageBrush(bm);

                        ib.Stretch = Stretch.UniformToFill;

                        ImgBorde.Background = ib;
                        Img.Visibility = Visibility.Hidden;
                    } catch (Exception e) {
                        setDefaultSource();
                        Console.WriteLine(e.Message);
                    }
                } else {
                    setDefaultSource();
                }
            }
           
        }

        public void MouseClick(object sender, EventArgs e) {
            _menu.onTempCompClick(this);
        }

        public void removeAll() {
            _archivos = null;
            _panelArchivos.removeChildrens();
        }
    }
}
