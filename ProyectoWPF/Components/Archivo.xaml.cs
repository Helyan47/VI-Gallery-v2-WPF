using ProyectoWPF.Data;
using Reproductor;
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

namespace ProyectoWPF.Components {
    /// <summary>
    /// Lógica de interacción para Archivo.xaml
    /// </summary>
    public partial class Archivo : UserControl {

        public ArchivoClass _archivoClass { get; set; }
        public Carpeta _carpetaPadre { get; set; }
        public SubCarpeta _subCarpetaPadre { get; set; }

        private VIGallery main;

        private Canvas _defaultCanvas;


        public Archivo(ArchivoClass archivo,VIGallery vi) {
            InitializeComponent();
            this.main = vi;
            _archivoClass = archivo;
            _subCarpetaPadre = null;
            _carpetaPadre = null;
            _defaultCanvas = canvasFile;
            Title.SetText(_archivoClass.nombre);
        }

        public void setCarpetaPadre(Carpeta p) {
            _archivoClass.idCarpeta = p.getClass().id;
            _carpetaPadre = p;
            actualizar();
        }

        public void setSubCarpetaPadre(SubCarpeta p) {
            _archivoClass.idCarpeta = p.getClass().id;
            _subCarpetaPadre = p;
            actualizar();
        }

        public void actualizar() {
            if (_archivoClass.nombre.Equals("")) {

            } else {
                Title.SetText(_archivoClass.nombre);
                if (_archivoClass.nombre.CompareTo("") != 0) {
                    if (_carpetaPadre != null) {
                        setImgCarpeta();
                    }else if(_subCarpetaPadre!=null){
                        setImgSubCarpeta();
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
                } else {
                    bm = new BitmapImage(new Uri(_subCarpetaPadre.getClass().img, UriKind.RelativeOrAbsolute));
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
                VI_Reproductor reproductor = new VI_Reproductor();
                main.getFirstGrid().Children.Add(reproductor);
                List<Archivo> lista = _carpetaPadre._archivos;
                List<FileInfo> listaArchivos = new List<FileInfo>();
                int posicion = 0;
                int cont = 0;
                foreach(Archivo archivo in lista) {
                    if (archivo.Equals(this)) {
                        posicion = cont;
                    }
                    FileInfo f = new FileInfo(archivo._archivoClass.rutaSistema);
                    listaArchivos.Add(f);
                    cont++;
                }
                reproductor.setLista(listaArchivos.ToArray(),posicion);
                reproductor.setVIGallery(main.getFirstGrid());
            } else if (_subCarpetaPadre != null) {
                VI_Reproductor reproductor = new VI_Reproductor();
                main.getFirstGrid().Children.Add(reproductor);
                List<Archivo> lista = _subCarpetaPadre._archivos;
                List<FileInfo> listaArchivos = new List<FileInfo>();
                int posicion = 0;
                int cont = 0;
                foreach (Archivo archivo in lista) {
                    if (archivo.Equals(this)) {
                        posicion = cont;
                    }
                    FileInfo f = new FileInfo(archivo._archivoClass.rutaSistema);
                    listaArchivos.Add(f);
                    cont++;
                }
                reproductor.setLista(listaArchivos.ToArray(), posicion);
                reproductor.setVIGallery(main.getFirstGrid());
            } else {
                MessageBox.Show("No se ha podido abrir el archivo");
            }
        }
    }
}
