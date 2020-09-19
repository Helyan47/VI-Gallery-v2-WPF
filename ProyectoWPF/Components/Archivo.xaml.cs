using System;
using System.Collections.Generic;
using System.IO;
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
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using ProyectoWPF.Data;
using ProyectoWPF.NewFolders;
using ProyectoWPF.Reproductor;

namespace ProyectoWPF.Components {
    /// <summary>
    /// Lógica de interacción para Archivo.xaml
    /// </summary>
    public partial class Archivo : UserControl {

        public ArchivoClass _archivoClass { get; set; }
        public Carpeta _carpetaPadre { get; set; }

        private WrapPanelPrincipal _wrapMenu;

        private VIGallery main;

        private Canvas _defaultCanvas;


        public Archivo(ArchivoClass archivo, VIGallery vi, WrapPanelPrincipal wrap) {
            InitializeComponent();
            this.main = vi;
            _wrapMenu = wrap;
            _archivoClass = archivo;
            _carpetaPadre = null;
            _defaultCanvas = canvasFile;
            Title.SetText(_archivoClass.nombre);
        }

        public void setCarpetaPadre(Carpeta p) {
            _archivoClass.idCarpeta = p.getClass().id;
            _carpetaPadre = p;
            actualizar();
        }

        public void actualizar() {
            if (!_archivoClass.nombre.Equals("")) {
                Title.SetText(_archivoClass.nombre);
                if (_archivoClass.nombre.CompareTo("") != 0) {
                    if (_carpetaPadre != null) {
                        setImgCarpeta();
                    }
                }
            }
        }

        public void setImgCarpeta() {
            try {
                BitmapImage bm = null;
                if (_archivoClass.img.CompareTo("") != 0) {
                    bm = new BitmapImage(new Uri(_archivoClass.img, UriKind.RelativeOrAbsolute));
                } else {
                    bm = new BitmapImage(new Uri(_carpetaPadre.getClass().img, UriKind.RelativeOrAbsolute));
                }
                
                ImageBrush ib = new ImageBrush(bm);
                if (bm.Width > bm.Height) {
                    ib.Stretch = Stretch.UniformToFill;
                }

                ImgBorde.Background = ib;
                Img.Visibility = Visibility.Hidden;
            } catch (Exception e) {
                setDefaultSource();
                //carpeta.img="";
                Console.WriteLine(e.Message);
            }
        }

        public void setImgSubCarpeta() {
            try {
                BitmapImage bm = null;
                if (_archivoClass.img.CompareTo("") != 0) {
                    bm = new BitmapImage(new Uri(_archivoClass.img, UriKind.RelativeOrAbsolute));
                }
                ImageBrush ib = new ImageBrush(bm);
                if (bm.Width > bm.Height) {
                    ib.Stretch = Stretch.UniformToFill;
                }

                ImgBorde.Background = ib;
                Img.Visibility = Visibility.Hidden;
            } catch (Exception e) {
                setDefaultSource();
                //carpeta.img="";
                Console.WriteLine(e.Message);
            }
        }

        public void setDefaultSource() {
            canvasFile = _defaultCanvas;
            Img.Visibility = Visibility.Visible;
        }

        private void onClick(object sender, EventArgs e) {
            
            if (_carpetaPadre != null) {
                VI_Reproductor reproductor = main.getReproductor();
                reproductor.Visibility = Visibility.Visible;
                List<Archivo> lista = _carpetaPadre._archivos;
                List<FileInfo> listaArchivos = new List<FileInfo>();
                List<string> listaNombres = new List<string>();
                int posicion = 0;
                int cont = 0;
                foreach(Archivo archivo in lista) {
                    if (archivo.Equals(this)) {
                        posicion = cont;
                    }
                    FileInfo f = new FileInfo(archivo._archivoClass.rutaSistema);
                    listaNombres.Add(archivo._archivoClass.nombre);
                    listaArchivos.Add(f);
                    cont++;
                }
                reproductor.setListaNombres(listaNombres.ToArray());
                reproductor.setLista(listaArchivos.ToArray(),posicion);
                reproductor.setVIGallery(main);
            } else {
                MessageBox.Show("No se ha podido abrir el archivo");
            }
        }

        private void changeName(string newName, string img) {
            _archivoClass.nombre = newName;
            _archivoClass.img = img;
            if (_carpetaPadre != null) {
                setImgCarpeta();
            }
            string[] splitted = _archivoClass.rutaPrograma.Split('/');
            splitted[splitted.Length - 1] = newName;
            string rutaNueva = "";
            for (int i = 0; i < splitted.Length; i++) {
                rutaNueva += splitted[i];
                if (i != splitted.Length - 1) {
                    rutaNueva += "/";
                }
            }
            _archivoClass.rutaPrograma = rutaNueva;
            Conexion.updateFile(_archivoClass);
            Title.SetText(newName);
            if (_carpetaPadre != null) {
                Lista.orderWrap(_wrapMenu);
            }

        }

        private void showNewNamePanel(object sender, EventArgs e) {
            List<ArchivoClass> files = null;
            if(_carpetaPadre != null) {
                files = new List<ArchivoClass>();
                foreach(Archivo a in _carpetaPadre._archivos) {
                    files.Add(a._archivoClass);
                }
            }
            if (files != null) {
                ChangeNameFile cn = new ChangeNameFile(files);
                cn.setName(_archivoClass.nombre);
                cn.setImg(_archivoClass.img);
                cn.ShowDialog();
                if (cn.getNewName() != null) {
                    changeName(cn.getNewName(),cn.getDirImg());
                }
            }
            
        }

        public void deleteFile(object sender, EventArgs e) {
            try {
                Conexion.deleteFile(_archivoClass);
                if (_carpetaPadre != null) {
                    _carpetaPadre.removeFile(this);
                }
                _archivoClass = null;
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            } catch (SQLiteException exc2) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
        }

        public void updateRuta(string rutaAntigua, string rutaNueva) {
            try {
                rutaAntigua = rutaAntigua.Replace("|C", "|F");
                _archivoClass.rutaPrograma = _archivoClass.rutaPrograma.Replace(rutaAntigua, rutaNueva);
                Conexion.updateFile(_archivoClass);
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
        }
    }
}
