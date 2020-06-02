using ProyectoWPF.Components;
using ProyectoWPF.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para Carpeta.xaml
    /// </summary>
    /// 

    [Serializable]
    public partial class Carpeta : UserControl {

        private DispatcherTimer _dispatcherTimer;
        private WrapPanelPrincipal _wrapPanelAnterior;
        private Grid _gridPrincipal;
        private Grid _gridSecundario;
        private WrapPanelPrincipal _wrapCarpetaPropia;
        private Menu _menuCarpeta;
        private Grid _gridPadre;
        private CarpetaClass _carpeta;
        private string _rutaDirectorio;
        private VIGallery _ventanaMain;
        private Canvas _defaultCanvas;
        public List<Archivo> _archivos { get; set; }
        private static long _mode = 0;
        public PerfilClass _profile { get; set; }

        public Carpeta(VIGallery ventana) {
            InitializeComponent();
            Title.SetText("");
            _archivos = new List<Archivo>();
            _ventanaMain = ventana;
            _defaultCanvas = canvasFolder;
            _profile = VIGallery._profile;
            changeMode(_profile.mode);
        }

        #region get/set

        public void setImg() {
            try {
                BitmapImage bm = new BitmapImage(new Uri(_carpeta.img, UriKind.RelativeOrAbsolute));
                ImageBrush ib = new ImageBrush(bm);
                if (bm.Width > bm.Height) {
                    ib.Stretch = Stretch.UniformToFill;
                }
                
                ImgBorde.Background = ib;
                Img.Visibility = Visibility.Hidden;
            }catch (Exception e) {
                setDefaultSource();
                //carpeta.img="";
                Console.WriteLine(e.Message);
            }
        }


        public void setDefaultSource() {
            canvasFolder = _defaultCanvas;
            Img.Visibility = Visibility.Visible;
            bordeDesc.Visibility = Visibility.Visible;
            descripcion.Visibility = Visibility.Hidden;
        }

        public void changeColor(System.Windows.Media.Color c) {
            SolidColorBrush sb = new SolidColorBrush(c);
            ColorPath.Fill = sb;
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
            if (_mode != 0) {
                Title.Visibility = Visibility.Hidden;
            }
            
        }

        private void bordeDesc_MouseLeave(object sender, MouseEventArgs e) {
            if (_carpeta.img.Equals("")) {
                Img.Visibility = Visibility.Visible;
            } else {
                ImgBorde.Visibility = Visibility.Visible;
            }
            bordeDesc.Visibility = Visibility.Hidden;
            descripcion.Visibility = Visibility.Hidden;
            if (_mode != 3) {
                Title.Visibility = Visibility.Visible;
            }
            
        }
        public void setMenuCarpeta(Menu m) {
            _menuCarpeta = m;
            m.SetFlowCarpPrincipal(_menuCarpeta.getFlowCarpPrincipal());
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) {
            descripcion.Opacity+=0.04;
            if (descripcion.Opacity >= 1) {
                _dispatcherTimer.Stop();
            }
        }


        public void setPadreSerie(WrapPanelPrincipal wrapPadre) {

            _wrapPanelAnterior = wrapPadre;
        }


        public void SetGridPadre(Grid p) {
            _gridPadre = p;
        }
        public Grid GetGridPadre() {
            return _gridPadre;
        }

        public void setDescripcion(string d) {
            _carpeta.desc= d;
            descripcion.Text = d;
        }

        public void setRutaDirectorio(string s) {
            _rutaDirectorio = s;
        }

        public string getRutaDirectorio() {
            return _rutaDirectorio;
        }

        public void setRutaPrograma(string s) {
            _carpeta.ruta = s;
        }

        public void AddSubCarpetas() {
            _carpeta.increaseSubCarpetas();
        }

        public void setClass(CarpetaClass newCarpeta) {

            _carpeta = newCarpeta;
            Title.SetText(_carpeta.nombre);
            descripcion.Text = _carpeta.desc;
        }

        public WrapPanelPrincipal GetWrapCarpPrincipal() {
            return _wrapCarpetaPropia;
        }

        public Grid getGridButtonsPrincipal() {
            return _gridPrincipal;
        }

        public Grid getGridButtonsSecundario() {
            return _gridSecundario;
        }

        public void SetGridsOpciones(Grid principal, Grid secundario) {
            _gridPrincipal = principal;
            _gridSecundario = secundario;
        }

        public CarpetaClass getClass() {

            return _carpeta;
        }

        public void changeTitle(String titulo) {
            Title.SetText(titulo);
            _carpeta.nombre = titulo;
        }

        public string getTitle() {
            return (string)Title.GetText();
        }

        public Menu GetMenuCarpeta() {
            return _menuCarpeta;
        }

        public void addFile(Archivo ac) {
            _archivos.Add(ac);
        }

#endregion

        public void actualizar() {
            if (!_carpeta.nombre.Equals("")) {
                Title.SetText(_carpeta.nombre);
                if (_carpeta.nombre.CompareTo("") != 0) {
                    setImg();
                }
                if (_carpeta.desc.CompareTo("") == 0) {
                    setDescripcion("Inserta una descripción");
                }
            }

        }

        public void click() {
            if (_menuCarpeta == null) {
                _menuCarpeta = new Menu(this);
                Lista.addMenu(_menuCarpeta);
                _wrapCarpetaPropia = new WrapPanelPrincipal();
                Lista.addSubWrap(_wrapCarpetaPropia);
                _wrapCarpetaPropia.setCarpeta(this);
                _wrapCarpetaPropia.setSubcarpeta(null);
                _gridPadre.Children.Add(_menuCarpeta);
                _menuCarpeta.SetFlowLayAnterior(_wrapPanelAnterior);
                //menuCarpeta.Children.Add(menuCarpeta); //checkear padre
                _wrapCarpetaPropia.setGridSubCarpetas(_menuCarpeta.getWrapSubCarpetas());

                _menuCarpeta.SetFlowCarpPrincipal(_wrapCarpetaPropia);
                _wrapCarpetaPropia.Visibility = Visibility.Hidden;
                _menuCarpeta.Visibility = Visibility.Hidden;
            }
            _menuCarpeta.actualizar();
            _wrapPanelAnterior.Visibility = Visibility.Hidden;
            _menuCarpeta.Visibility = Visibility.Visible;
            _wrapCarpetaPropia.Visibility = Visibility.Visible;


            _gridSecundario.SetValue(Grid.RowProperty, 0);
            _gridPrincipal.SetValue(Grid.RowProperty, 1);

            _ventanaMain.ReturnVisibility(true);
           
        }

        public void clickEspecial() {
            if (_menuCarpeta == null) {
                _menuCarpeta = new Menu(this);
                Lista.addMenu(_menuCarpeta);
                _wrapCarpetaPropia = new WrapPanelPrincipal();
                Lista.addSubWrap(_wrapCarpetaPropia);
                _wrapCarpetaPropia.setCarpeta(this);
                _wrapCarpetaPropia.setSubcarpeta(null);
                _gridPadre.Children.Add(_menuCarpeta);
                _menuCarpeta.SetFlowLayAnterior(_wrapPanelAnterior);
                //menuCarpeta.Children.Add(menuCarpeta); //checkear padre
                _wrapCarpetaPropia.setGridSubCarpetas(_menuCarpeta.getWrapSubCarpetas());

                _menuCarpeta.SetFlowCarpPrincipal(_wrapCarpetaPropia);
                _wrapCarpetaPropia.Visibility = Visibility.Hidden;
                _menuCarpeta.Visibility = Visibility.Hidden;
            }
            _menuCarpeta.actualizar();
            
        }

        public void clickInverso() {
            _menuCarpeta.Visibility = Visibility.Hidden;
            _wrapPanelAnterior.Visibility = Visibility.Visible;
            _wrapCarpetaPropia.Visibility = Visibility.Hidden;

            _gridSecundario.SetValue(Grid.RowProperty, 1);
            _gridPrincipal.SetValue(Grid.RowProperty, 0);
            _ventanaMain.ReturnVisibility(false);
        }

        private void MouseClick(object sender, MouseButtonEventArgs e) {

            click();
        }


        public void changeMode(long mode) {
            _mode = mode;
            _profile.mode = mode;
            
            if (mode == 0) {
                Grid.SetRowSpan(ImgBorde, 2);
                Grid.SetRowSpan(Img, 2);
                Grid.SetRowSpan(bordeDesc, 2);
                Grid.SetRowSpan(borde, 3);
                
                Title.ChangeForegroundColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                Title.SetSombraVisible(false);
                this.Height = 400;

                Title.Visibility = Visibility.Visible;
                borde.BorderThickness = new Thickness(5);
                borde2.BorderThickness = new Thickness(5);
                Title.Width = 239;
                Title.Margin = new Thickness(0,0,5,0);
                Title.SetRadius(new CornerRadius(0,0,10,10));
                backgroundGrid.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255,255,255));
                Grid.SetRowSpan(Title, 1);
                ImgBorde.CornerRadius = new CornerRadius(15);

            } else if (mode == 1) {
                Grid.SetRowSpan(ImgBorde, 4);
                Grid.SetRowSpan(Img, 4);
                Grid.SetRowSpan(bordeDesc, 4);
                Grid.SetRowSpan(borde, 5);
                Title.ChangeForegroundColor(System.Windows.Media.Color.FromRgb(255,255,255));
                Title.SetSombraVisible(true);
                this.Height = 352;
            } else if (mode == 2) {
                Grid.SetRowSpan(ImgBorde, 4);
                Grid.SetRowSpan(Img, 4);
                Grid.SetRowSpan(bordeDesc, 4);

                Title.SetSombraVisible(true);
                this.Height = 352;
                Title.ChangeForegroundColor(System.Windows.Media.Color.FromRgb(255, 255, 255));

                borde.BorderThickness = new Thickness(0);
                borde2.BorderThickness = new Thickness(0);
                Title.Width = 250;
                Title.Margin = new Thickness(0,1,0,0);
                ImgBorde.CornerRadius = new CornerRadius(10);
                Grid.SetRowSpan(Title,2);
                backgroundGrid.Background= new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#00000000"));
            } else if (mode == 3) {
                Grid.SetRowSpan(ImgBorde, 4);
                Grid.SetRowSpan(Img, 4);
                Grid.SetRowSpan(bordeDesc, 4);
                this.Height = 352;

                Title.Visibility = Visibility.Hidden;
                borde.BorderThickness = new Thickness(0);
                borde2.BorderThickness = new Thickness(0);
                ImgBorde.CornerRadius = new CornerRadius(10);
                backgroundGrid.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#00000000"));
            }
        }

        public void remove() {
            _archivos = null;
            _wrapPanelAnterior.removeFolder(this);
            Lista.removeSubFolders(this);
            if (_wrapCarpetaPropia != null) {
                _wrapCarpetaPropia.removeChildrens();
                Lista.removeWrapPanelSecundario(_wrapCarpetaPropia);
                _wrapCarpetaPropia = null;
            }

            if (_menuCarpeta != null) {
                _menuCarpeta.remove();
                _menuCarpeta = null;
            }
            
            Lista.removeCarpetaClass(_carpeta.id);
        }
    }
}