using MySql.Data.MySqlClient;
using ProyectoWPF.Components;
using ProyectoWPF.Data;
using ProyectoWPF.NewFolders;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        /*private DispatcherTimer _dispatcherTimer;
        private WrapPanelPrincipal _wrapPanelAnterior;
        private Grid _gridPrincipal;
        private Grid _gridSecundario;
        private WrapPanelPrincipal _wrapCarpetaPropia;
        private Menu _menuCarpeta;
        private Grid _gridPadre;
        
        */
        private string rutaDirectorio = null;
        private DispatcherTimer _dispatcherTimer;
        private Grid _gridPrincipal;
        private Grid _gridSecundario;
        private Menu _menu;
        private CarpetaClass _carpeta;
        public WrapPanelPrincipal _primerPanel { get; set; }
        private Carpeta _carpetaPadre;
        private List<Carpeta> _carpetasHijo;
        private VIGallery _ventanaMain;
        private Canvas _defaultCanvas;
        public List<Archivo> _archivos { get; set; }
        private static long _mode = 0;
        public PerfilClass _profile { get; set; }

        public Carpeta(VIGallery ventana, WrapPanelPrincipal primerPanel, Menu menu, Carpeta carpetaPadre) {
            InitializeComponent();
            Title.SetText("");
            _carpetaPadre = carpetaPadre;
            _primerPanel = primerPanel;
            _menu = menu;
            _archivos = new List<Archivo>();
            _carpetasHijo = new List<Carpeta>();
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
            if (_carpeta.isFolder) {
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
        }

        private void bordeDesc_MouseLeave(object sender, MouseEventArgs e) {
            if (_carpeta.isFolder) {
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
            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) {
            descripcion.Opacity+=0.04;
            if (descripcion.Opacity >= 1) {
                _dispatcherTimer.Stop();
            }
        }

        public void setDescripcion(string d) {
            _carpeta.desc= d;
            descripcion.Text = d;
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
            return _menu;
        }

        public void addFile(Archivo ac) {
            _archivos.Add(ac);
        }

        public void addCarpetaHijo(Carpeta c) {
            if (!_carpetasHijo.Contains(c)) {
                _carpetasHijo.Add(c);
            }
        }

        public List<Carpeta> getCarpetasHijos() {
            if (_carpetasHijo.Count == 0) {
                return null;
            }
            return _carpetasHijo;
        }

        public void setRutaDirectorio(string ruta) {
            rutaDirectorio = ruta;
        }

        public string getRutaDirectorio() {
            return rutaDirectorio;
        }

#endregion

        public void actualizar() {
            if (!_carpeta.nombre.Equals("")) {
                Title.SetText(_carpeta.nombre);
                if (_carpeta.nombre.CompareTo("") != 0) {
                    setImg();
                }
                if (_carpeta.isFolder) {
                    if (_carpeta.desc.CompareTo("") == 0) {
                        setDescripcion("Inserta una descripción");
                    }
                } else {
                    bordeDesc.Visibility = Visibility.Hidden;
                    descripcion.Visibility = Visibility.Hidden;
                }
                
            }

        }

        public void click() {
            if (_menu != null) {
                _menu.actualizar(this);
                _menu.Visibility = Visibility.Visible;
                _primerPanel.Visibility = Visibility.Hidden;
            }


            _gridSecundario.SetValue(Grid.RowProperty, 0);
            _gridPrincipal.SetValue(Grid.RowProperty, 1);

            _ventanaMain.clearTextBoxAndSelection();

            _ventanaMain.ReturnVisibility(true);
           
        }

        public void clickEspecial() {
            if (_menu == null) {
                _menu.actualizar(this);
            }
            
            
        }

        public void clickInverso() {

            _gridSecundario.SetValue(Grid.RowProperty, 1);
            _gridPrincipal.SetValue(Grid.RowProperty, 0);
            
            if (_carpetaPadre == null) {
                _ventanaMain.ReturnVisibility(false);
                _primerPanel.Visibility = Visibility.Visible;
            } else {
                _menu.actualizar(_carpetaPadre);
                _ventanaMain.ReturnVisibility(true);
                
            }
            
        }

        private void MouseClick(object sender, EventArgs e) {
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
            try {
                Conexion.deleteFolder(_carpeta);
                if (_archivos != null) {
                    foreach (Archivo a in _archivos) {
                        removeFile(a);
                    }
                    _archivos = null;
                }

                if (_carpetasHijo != null) {
                    foreach (Carpeta c in _carpetasHijo) {
                        c.remove();
                    }
                    _carpetasHijo = null;
                }

                _primerPanel.removeFolder(this);

                Lista.removeCarpetaClass(_carpeta.id);
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
        }

        public bool checkGender(List<string> genders) {
            foreach(string gender in genders) {
                if (!_carpeta.generos.Contains(gender)) {
                    return false;
                }
            }
            return true;
        }

        public void changeName(string newName, string newDescripcion, string newImg, ICollection<string> generos) {
            try {
                _carpeta.nombre = newName;
                _carpeta.generos = generos;
                _carpeta.desc = newDescripcion;
                _carpeta.img = newImg;
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
                setImg();
                setDescripcion(newDescripcion);
                Lista.changeSubFoldersName(rutaAntigua, rutaNueva);
                if (_archivos != null) {
                    foreach (Archivo a in _archivos) {
                        a.updateRuta(rutaAntigua, rutaNueva);
                    }
                }
                Title.SetText(newName);
                Conexion.updateFolder(_carpeta);
                Lista.orderWrap(_primerPanel);
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
        }

        public void changeName(string newDescripcion, string newImg, ICollection<string> generos) {
            try {
                _carpeta.generos = generos;
                _carpeta.desc = newDescripcion;
                _carpeta.img = newImg;
                setImg();
                setDescripcion(newDescripcion);
                Conexion.updateFolder(_carpeta);
                Lista.orderWrap(_primerPanel);
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
        }

        public void changeName(string newName, string newImg) {
            try {
                _carpeta.nombre = newName;
                _carpeta.img = newImg;
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
                setImg();
                Lista.changeSubFoldersName(rutaAntigua, rutaNueva);
                if (_archivos != null) {
                    foreach (Archivo a in _archivos) {
                        a.updateRuta(rutaAntigua, rutaNueva);
                    }
                }
                Title.SetText(newName);
                Conexion.updateFolder(_carpeta);
                Lista.orderWrap(_primerPanel);
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
        }

        public void updateRuta(string rutaAntigua, string rutaNueva) {
            try {
                string rutaPadreAntigua = _carpeta.rutaPadre;
                _carpeta.rutaPadre = rutaNueva;
                string rutaAntiguaSub = _carpeta.ruta;
                _carpeta.ruta = _carpeta.ruta.Replace(rutaAntigua, rutaNueva);

                Conexion.updateFolder(_carpeta);
                Lista.changeSubFoldersName(rutaAntiguaSub, _carpeta.ruta);

                if (_archivos != null) {
                    foreach (Archivo a in _archivos) {
                        a.updateRuta(rutaAntiguaSub, _carpeta.ruta);
                    }
                }
            } catch (MySqlException exc) {
                throw exc;
            } catch (SQLiteException exc2) {
                throw exc2;
            }
        }

        public void showNewNamePanel(object sender, EventArgs e) {
            string folderRutaPadre = _carpeta.ruta.Split('/')[0] + "/";
            ChangeName cn = null;
            if (_carpetaPadre == null) {
               cn = new ChangeName(folderRutaPadre, true);
                cn.setDescripcion(_carpeta.desc);
                cn.changeGenderMode(ActionPanel.MODIFY_FOLDER_MODE,_carpeta.ruta, null);
            } else {
                cn = new ChangeName(folderRutaPadre, false);
            }

            cn.setName(_carpeta.nombre);
            
            cn.setImg(_carpeta.img);
            
            cn.ShowDialog();
            if (cn.getNewName() != null) {
                if (_carpetaPadre == null) {
                    if (cn.isNameChanged()) {
                        changeName(cn.getNewName(), cn.getDescripcion(), cn.getDirImg(), cn.getGeneros());
                    } else {
                        changeName(cn.getDescripcion(), cn.getDirImg(), cn.getGeneros());
                    }
                } else {
                    changeName(cn.getNewName(), cn.getDirImg());
                }
                
            }
        }

        private void deleteFolder(object sender, EventArgs e) {
            remove();
        }

        public void removeFile(Archivo a) {
            if (_archivos.Contains(a)) {
                _archivos.Remove(a);
                _menu.getWrap().removeFile(a);
            }
        }
    }
}