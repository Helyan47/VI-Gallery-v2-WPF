using SeleccionarProfile.Components.Online;
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
using System.Windows.Threading;

namespace SeleccionarProfile.Components {
    /// <summary>
    /// Lógica de interacción para SerieComponent.xaml
    /// </summary>
    public partial class SerieComponent : UserControl {

        private DispatcherTimer _dispatcherTimer;
        private Serie _serie = null;
        private Canvas _defaultCanvas;
        private List<Temporada> _temporadas;
        private MenuComponent _menu;

        public SerieComponent(Serie s) {
            InitializeComponent();
            _defaultCanvas = canvasFolder;
            setSerie(s);
        }

        public void setSerie(Serie s) {
            if (s != null) {
                _serie = s;
                actualizar();
            }
            
        }

        public Serie getSerie() {
            return _serie;
        }

        public void setMenu(MenuComponent m) {
            _menu = m;
        }

        public MenuComponent getMenu() {
            return _menu;
        }

        public void setTemporadas(List<Temporada> temporadas) {
            _temporadas = temporadas;
        }

        public List<Temporada> getTemporadas() {
            return _temporadas;
        }

        public void setDefaultSource() {
            canvasFolder = _defaultCanvas;
            Img.Visibility = Visibility.Visible;
            bordeDesc.Visibility = Visibility.Visible;
            descripcion.Visibility = Visibility.Hidden;
        }

        public void actualizar() {
            if (!_serie.nombre.Equals("")) {
                Title.SetText(_serie.nombre);
                if (_serie.nombre.CompareTo("") != 0) {
                    setImg();
                }
                if (_serie.descripcion.CompareTo("") == 0) {
                    setDescripcion("Inserta una descripción");
                } else {
                    setDescripcion(_serie.descripcion);
                }
            }

        }

        public void setDescripcion(string d) {
            descripcion.Text = d;
        }

        public void setImg() {
            try {
                BitmapImage bm = new BitmapImage(new Uri(_serie.img, UriKind.RelativeOrAbsolute));
                ImageBrush ib = new ImageBrush(bm);
                ib.Stretch = Stretch.UniformToFill;

                ImgBorde.Background = ib;
                Img.Visibility = Visibility.Hidden;
            } catch (Exception e) {
                setDefaultSource();
                Console.WriteLine(e.Message);
            }
        }

        public void MouseClick(object sender, EventArgs e) {
            _menu.onSerieClick();
        }

        private void bordeDesc_MouseLeave(object sender, MouseEventArgs e) {
            if (_serie.img.Equals("")) {
                Img.Visibility = Visibility.Visible;
            } else {
                ImgBorde.Visibility = Visibility.Visible;
            }
            bordeDesc.Visibility = Visibility.Hidden;
            descripcion.Visibility = Visibility.Hidden;

        }

        private void img_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
            bordeDesc.Visibility = Visibility.Visible;
            Img.Visibility = Visibility.Hidden;
            ImgBorde.Visibility = Visibility.Hidden;
            descripcion.Opacity = 0.04;
            _dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            _dispatcherTimer.Start();
            descripcion.Visibility = Visibility.Visible;

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) {
            descripcion.Opacity += 0.04;
            if (descripcion.Opacity >= 1) {
                _dispatcherTimer.Stop();
            }
        }

        public void removeAll() {
            _menu.removeAll();
            _menu = null;
            _temporadas = null;
        }

        public bool checkGender(string s) {
            if (_serie.generos.Contains(s)) {
                return true;
            } else {
                return false;
            }
        }
    }
}
