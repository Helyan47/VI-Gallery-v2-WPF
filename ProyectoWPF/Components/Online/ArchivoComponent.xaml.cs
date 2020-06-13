using SeleccionarProfile.Data.Online;
using Reproductor;
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
using ProyectoWPF.Data.Online;

namespace ProyectoWPF.Components.Online {
    /// <summary>
    /// Lógica de interacción para ArchivoComponent.xaml
    /// </summary>
    public partial class ArchivoComponent : UserControl {

        private TemporadaComponent _tempComponent = null;
        private Capitulo _capitulo = null;
        private Pelicula _pelicula = null;
        private Canvas _defaultCanvas;
        private VIGallery _main;

        public ArchivoComponent(object c,VIGallery vi) {
            InitializeComponent();
            if(c is Capitulo) {
                _capitulo = (Capitulo)c;
                actualizar();
            }else if(c is Pelicula) {
                _pelicula = (Pelicula)c;
                actualizar();
            }
            _defaultCanvas = canvasFile;
            _main = vi;
        }

        public void setTempComponent(TemporadaComponent tc) {
            _tempComponent = tc;
        }

        public void actualizar() {
            if (this._capitulo != null) {
                Temporada t = ListaOnline.getTempByCap(this._capitulo);
                Serie s = ListaOnline.getSerieByTemp(t);
                setTitle(s.nombre + " Temporada "+ t.numTemporada+ " " + _capitulo.nombre);
                setImg();
            } else if (_pelicula != null) {
                setTitle(_pelicula.nombre);
                setImg();
            }
        }

        private void setImg() {
            if (this._capitulo != null) {
                try {
                    Temporada t = ListaOnline.getTempByCap(this._capitulo);
                    if (t != null) {
                        if (t.img.CompareTo("") != 0) {
                            BitmapImage bm = new BitmapImage(new Uri(t.img, UriKind.RelativeOrAbsolute)); 
                            ImageBrush ib = new ImageBrush(bm);

                            ib.Stretch = Stretch.UniformToFill;

                            ImgBorde.Background = ib;
                            Img.Visibility = Visibility.Hidden;
                        }

                    } else if (ListaOnline.getSerieByTemp(t).img.CompareTo("") != 0) {
                        BitmapImage bm = new BitmapImage(new Uri(t.img, UriKind.RelativeOrAbsolute));
                        ImageBrush ib = new ImageBrush(bm);
                        ib.Stretch = Stretch.UniformToFill;

                        ImgBorde.Background = ib;
                        Img.Visibility = Visibility.Hidden;
                    } else {
                        setDefaultSource();
                    }

                } catch (Exception e) {
                    setDefaultSource();
                    Console.WriteLine(e.Message);
                }
            } else if (this._pelicula != null) {
                if (this._pelicula.img.CompareTo("") != 0) {
                    try {
                        BitmapImage bm = new BitmapImage(new Uri(_pelicula.img, UriKind.RelativeOrAbsolute));
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

        public void setDefaultSource() {
            canvasFile = _defaultCanvas;
            Img.Visibility = Visibility.Visible;
        }

        private void setTitle(string s) {
            Title.SetText(s);
        }

        private void onClick(object sender, EventArgs e) {

            VI_Reproductor reproductor = new VI_Reproductor(true);
            _main.getFirstGrid().Children.Add(reproductor);
            List<Uri> listaArchivos = new List<Uri>();
            List<long> listaId = new List<long>();
            List<string> listaNombre= new List<string>();
            if (_capitulo != null) {
                int posicion = 0;
                int cont = 0;
                Temporada t = ListaOnline.getTempByCap(this._capitulo);
                Serie s = ListaOnline.getSerieByTemp(t);
                foreach (Capitulo c in ListaOnline.getCapitulosFromTemporada(_tempComponent.getTemporada().id)) {
                    Uri u = new Uri(c.rutaWeb);
                    if(c.id == _capitulo.id) {
                        posicion = cont;
                    }
                    listaArchivos.Add(u);
                    listaId.Add(c.id);
                    string nombre = s.nombre + " - Temporada " + t.numTemporada + " " + c.nombre;
                    listaNombre.Add(nombre);
                    cont++;
                    
                }
                reproductor.setListaNombres(listaNombre.ToArray());
                reproductor.setListaCapitulos(listaArchivos.ToArray(), listaId.ToArray(), posicion);
                reproductor.setVIGallery(_main.getFirstGrid());
            }else if (_pelicula != null) {
                Uri u = new Uri(_pelicula.rutaWeb);
                listaArchivos.Add(u);
                string nombre = _pelicula.nombre;
                listaNombre.Add(nombre);
                reproductor.setListaNombres(listaNombre.ToArray());
                reproductor.setListaPeliculas(listaArchivos.ToArray(),listaId.ToArray());
                reproductor.setVIGallery(_main.getFirstGrid());
            }
        }

        public bool checkGender(string s) {
            if (_pelicula != null) {
                if (_pelicula.generos.Contains(s)) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
            
        }

        public string getNombre() {
            if (_capitulo != null) {
                return _capitulo.nombre;
            }else if (_pelicula != null) {
                return _pelicula.nombre;
            } else {
                return null;
            }
        }
    }
}
