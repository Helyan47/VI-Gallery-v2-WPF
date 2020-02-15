using ProyectoWPF.Data;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para SubCarpeta.xaml
    /// </summary>
    /// 
    [Serializable]
    public partial class SubCarpeta : UserControl {

        private String _nombre;
        private CarpetaClass _carpeta;
        private WrapPanelPrincipal _wrapCarpAnterior;
        private int _numSubCarpetas;
        private WrapPanelPrincipal _wrapCarpPropia;
        private Grid _gridPadre;
        private Menu _menuCarpeta;
        private string _rutaDirectorio;
        private string _rutaPrograma;
        private Canvas _defaultCanvas;
        private int _mode = 0;
        public PerfilClass _profile { get; set; }
        public SubCarpeta() {
            InitializeComponent();
            _numSubCarpetas = 0;
            _defaultCanvas = canvasFolder;
            _profile = VIGallery._profile;
        }

        #region get/set

        public void setDefaultSource() {
            canvasFolder = _defaultCanvas;
            ImgBorde.Visibility = Visibility.Hidden;
            Img.Visibility = Visibility.Visible;
        }

        public void setImg() {
            try {
                BitmapImage bm = new BitmapImage(new Uri(_carpeta.img, UriKind.Absolute));
                ImageBrush ib = new ImageBrush(bm);
                if (bm.Width > bm.Height) {
                    ib.Stretch = Stretch.UniformToFill;
                }
                ImgBorde.Background = ib;
                Img.Visibility = Visibility.Hidden;
            }catch(Exception e) {
                setDefaultSource();
                Console.WriteLine(e.Message);
            }
        }

        public void setRutaDirectorio(string s) {
            _rutaDirectorio = s;
        }

        public string getRutaDirectorio() {
            return _rutaDirectorio;
        }

        public void setRutaPrograma(string s) {
            _rutaPrograma = s;
        }

        public void AddSubCarpetas() {
            _numSubCarpetas++;
        }

        public int getNumSubCarp() {
            return _numSubCarpetas;
        }

        public string getDirImg() {
            return _carpeta.img;
        }

        public Grid GetGridCarpeta() {
            return _gridPadre;
        }

        public WrapPanelPrincipal getPadreSerie() {
            return _wrapCarpAnterior;
        }

        public void SetGridPadre(Grid p) {
            _gridPadre = p;
            p.Children.Add(_wrapCarpPropia);
        }

        public void setClass(CarpetaClass newSerie) { 
            _carpeta = newSerie;
            Title.SetText(_carpeta.nombre);
        }

        public void setMenuCarpeta(Menu m) {
            _menuCarpeta = m;
        }

        public Menu GetMenuCarpeta() {
            return _menuCarpeta;
        }

        public CarpetaClass getClass() {

            return _carpeta;
        }

        public WrapPanelPrincipal getWrapCarpPrincipal() {
            return _wrapCarpPropia;
        }


        public Grid GetGridPadre() {
            return _gridPadre;
        }

        public string getTitle() {
            return _nombre;
        }

        #endregion

        public void actualizar() {
            if (_carpeta.img.Equals("")) {

            } else {
                if (_carpeta.img != "") {
                    setImg();
                }
            }

        }

        public void setDatos(CarpetaClass carpetaClass, WrapPanelPrincipal flowPadre, Grid gridP) {
            _wrapCarpPropia = new WrapPanelPrincipal();

            _carpeta = carpetaClass;
            _wrapCarpAnterior = flowPadre;

            Lista.addSubWrap(_wrapCarpPropia);
            _gridPadre = gridP;
            _gridPadre.Children.Add(_wrapCarpPropia);
            //flowCarpPropia.Dock = DockStyle.Fill;
            _wrapCarpPropia.Visibility = System.Windows.Visibility.Hidden;
            _wrapCarpPropia.setSubcarpeta(this);

        }

        public void click() {

            _menuCarpeta.actualizar();
            _menuCarpeta.changeTitle(_nombre);
            _wrapCarpAnterior.Visibility = System.Windows.Visibility.Hidden;
            _wrapCarpPropia.Visibility = System.Windows.Visibility.Visible;
        }

        public void clickEspecial() {

            _menuCarpeta.actualizar();
            _menuCarpeta.changeTitle(_nombre);

        }

        public void clickInverso() {
            _wrapCarpAnterior.Visibility = System.Windows.Visibility.Visible;
            _wrapCarpPropia.Visibility = System.Windows.Visibility.Hidden;

            if (_wrapCarpAnterior.getSubCarpeta() != null) {
                _menuCarpeta.Title.Content = _wrapCarpAnterior.getSubCarpeta().getTitle();
            } else if (_wrapCarpAnterior.getCarpeta() != null) {
                _menuCarpeta.Title.Content = _wrapCarpAnterior.getCarpeta().getTitle();
            }
        }

        private void OnClick(object sender, RoutedEventArgs e) {
            click();
        }


        public void setTitle(String titulo) {
            _nombre = titulo;
            Title.SetText(titulo);
        }

        

        private void SubCarpeta_MouseClick(object sender, MouseEventArgs e) {
            click();
        }

        public void changeMode(int mode) {
            this._mode = mode;

            if (mode == 0) {
                Grid.SetRowSpan(ImgBorde, 2);
                Grid.SetRowSpan(Img, 2);
                Grid.SetRowSpan(borde, 3);

                Title.ChangeForegroundColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                Title.SetSombraVisible(false);
                this.Height = 400;

                Title.Visibility = Visibility.Visible;
                borde.BorderThickness = new Thickness(5);
                borde2.BorderThickness = new Thickness(5);
                Title.Width = 239;
                Title.Margin = new Thickness(0, 0, 5, 0);
                Title.SetRadius(new CornerRadius(0, 0, 10, 10));
                backgroundGrid.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
                Grid.SetRowSpan(Title, 1);
                ImgBorde.CornerRadius = new CornerRadius(15);

            } else if (mode == 1) {
                Grid.SetRowSpan(ImgBorde, 4);
                Grid.SetRowSpan(Img, 4);
                Grid.SetRowSpan(borde, 5);

                Title.ChangeForegroundColor(System.Windows.Media.Color.FromRgb(255, 255, 255));
                Title.SetSombraVisible(true);
                this.Height = 352;
            } else if (mode == 2) {
                Grid.SetRowSpan(ImgBorde, 4);
                Grid.SetRowSpan(Img, 4);

                Title.SetSombraVisible(true);
                this.Height = 352;
                Title.ChangeForegroundColor(System.Windows.Media.Color.FromRgb(255, 255, 255));

                borde.BorderThickness = new Thickness(0);
                borde2.BorderThickness = new Thickness(0);
                Title.Width = 250;
                Title.Margin = new Thickness(0, 1, 0, 0);
                ImgBorde.CornerRadius = new CornerRadius(10);
                Grid.SetRowSpan(Title, 2);
                backgroundGrid.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#00000000"));
            } else if (mode == 3) {
                Grid.SetRowSpan(ImgBorde, 4);
                Grid.SetRowSpan(Img, 4);
                this.Height = 352;

                Title.Visibility = Visibility.Hidden;
                borde.BorderThickness = new Thickness(0);
                borde2.BorderThickness = new Thickness(0);
                ImgBorde.CornerRadius = new CornerRadius(10);
                backgroundGrid.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#00000000"));
            }
        } 
    }
}