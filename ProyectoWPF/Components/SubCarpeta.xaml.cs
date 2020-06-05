using SeleccionarProfile.Components;
using SeleccionarProfile.Data;
using SeleccionarProfile.NewFolders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SeleccionarProfile {
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
        public List<Archivo> _archivos;
        private static long _mode = 0;
        private VIGallery _ventanaMain;
        public PerfilClass _profile { get; set; }
        public SubCarpeta(VIGallery ventanaMain) {
            InitializeComponent();
            _carpeta = new CarpetaClass("", "", false);
            _numSubCarpetas = 0;
            _archivos = new List<Archivo>();
            _defaultCanvas = canvasFolder;
            _profile = VIGallery._profile;
            changeMode(_profile.mode);
            _ventanaMain = ventanaMain;
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
                ImgBorde.Visibility = Visibility.Visible;
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

        public void AddSubCarpetas() {
            _numSubCarpetas++;
        }

        public int getNumSubCarp() {
            return _numSubCarpetas;
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

        #endregion

        public void actualizar() {
            if (_carpeta.img.Equals("")) {

            } else {
                if (_carpeta.img != "") {
                    setImg();
                }
            }

        }

        public void addFile(Archivo ac) {
            _archivos.Add(ac);
        }

        public void setDatos(CarpetaClass carpeta,WrapPanelPrincipal flowPadre, Grid gridP) {
            _wrapCarpPropia = new WrapPanelPrincipal();
            _wrapCarpAnterior = flowPadre;
            _carpeta = carpeta;

            Lista.addSubWrap(_wrapCarpPropia);
            _gridPadre = gridP;
            _gridPadre.Children.Add(_wrapCarpPropia);
            //flowCarpPropia.Dock = DockStyle.Fill;
            _wrapCarpPropia.Visibility = System.Windows.Visibility.Hidden;
            _wrapCarpPropia.setSubcarpeta(this);

        }

        public void click() {

            _menuCarpeta.actualizar();
            _menuCarpeta.changeTitle(_carpeta.nombre);
            _wrapCarpAnterior.Visibility = System.Windows.Visibility.Hidden;
            _wrapCarpPropia.Visibility = System.Windows.Visibility.Visible;
            _ventanaMain.clearTextBox(_wrapCarpAnterior);
        }

        public void clickEspecial() {

            _menuCarpeta.actualizar();
            _menuCarpeta.changeTitle(_carpeta.nombre);

        }

        public void clickInverso() {
            _wrapCarpAnterior.Visibility = System.Windows.Visibility.Visible;
            _wrapCarpPropia.Visibility = System.Windows.Visibility.Hidden;

            if (_wrapCarpAnterior.getSubCarpeta() != null) {
                _menuCarpeta.Title.Content = _wrapCarpAnterior.getSubCarpeta().getClass().nombre;
            } else if (_wrapCarpAnterior.getCarpeta() != null) {
                _menuCarpeta.Title.Content = _wrapCarpAnterior.getCarpeta().getTitle();
            }
            _ventanaMain.clearTextBox(_wrapCarpPropia);
        }

        private void OnClick(object sender, RoutedEventArgs e) {
            click();
        }


        public void setTitle(string titulo) {
            _nombre = titulo;
            _carpeta.nombre = titulo;
            Title.SetText(titulo);
        }

        public void setImg(string img) {
            _carpeta.img = img;
            setImg();
        }

        private void SubCarpeta_MouseClick(object sender, MouseEventArgs e) {
            click();
        }

        public void changeMode(long mode) {
            _mode = mode;
            _profile.mode = mode;

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

        public void remove() {
            if (VIGallery.conexionMode) {
                Conexion.deleteFolder(_carpeta);
            } else {
                ConexionOffline.deleteFolder(_carpeta.id);
            }
            _archivos = null;
            Lista.removeSubFolders(this);
            Lista.removeSubCarpeta(this);
            if (_wrapCarpAnterior != null) {
                _wrapCarpAnterior.removeSubFolder(this);
            }
            _wrapCarpPropia.removeChildrens();
            Lista.removeWrapPanelSecundario(_wrapCarpPropia);
            _wrapCarpPropia = null;
            Lista.removeCarpetaClass(_carpeta.id);
            _carpeta = null;
            
        }

        public void changeName(string newName) {
            string nombreAntiguo = _carpeta.nombre;
            _carpeta.nombre = newName;
            string[] splitted = _carpeta.ruta.Split('/');
            splitted[splitted.Length - 1] = newName;
            string rutaAntigua = _carpeta.ruta;
            string rutaNueva = "";
            for (int i = 0; i < splitted.Length; i++) {
                rutaNueva += splitted[i];
                if (i != splitted.Length - 1) {
                    rutaNueva += "/";
                }
            }
            _carpeta.ruta = rutaNueva;
            Console.WriteLine("Nombre antiguo: " + nombreAntiguo);
            Console.WriteLine("Nombre nuevo: " + newName);
            Console.WriteLine("Ruta antigua: " + rutaAntigua);
            Console.WriteLine("Ruta nueva: " + rutaNueva);
            Lista.changeSubFoldersName(rutaAntigua, rutaNueva);
            Title.SetText(newName);
            if (VIGallery.conexionMode) {
                Conexion.updateFolderName(_carpeta);
            } else {
                ConexionOffline.updateFolderName(_carpeta);
            }
            Lista.orderWrap(_wrapCarpAnterior);
        }

        public void updateRuta(string rutaAntigua, string rutaNueva) {
            string rutaPadreAntigua = _carpeta.rutaPadre;
            _carpeta.rutaPadre = rutaNueva;
            string rutaAntiguaSub = _carpeta.ruta;
            _carpeta.ruta = _carpeta.ruta.Replace(rutaAntigua, rutaNueva);
            Lista.changeSubFoldersName(rutaAntiguaSub, _carpeta.ruta);
            if (VIGallery.conexionMode) {
                Conexion.updateFolderName(_carpeta);
            } else {
                ConexionOffline.updateFolderName(_carpeta);
            }
            if (_archivos != null) {
                foreach (Archivo a in _archivos) {
                    a.updateRuta(rutaAntiguaSub, _carpeta.ruta);
                }
            }
            
        }

        private void showNewNamePanel(object sender, EventArgs e) {
            string folderRutaPadre = _carpeta.ruta.Split('/')[0] + "/";
            ChangeName cn = new ChangeName(folderRutaPadre);
            cn.ShowDialog();
            if (cn.getNewName() != null) {
                changeName(cn.getNewName());
            }
        }

        private void deleteFolder(object sender, EventArgs e) {
            remove();
        }

        public void removeFile(Archivo a) {
            if (_archivos.Contains(a)) {
                _archivos.Remove(a);
                _wrapCarpPropia.removeFile(a);
            }
        }
    }
}