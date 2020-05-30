using ProyectoWPF.Data.Online;
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

namespace ProyectoWPF.Components {
    /// <summary>
    /// Lógica de interacción para VideoReciente.xaml
    /// </summary>
    public partial class VideoReciente : UserControl {

        private Capitulo _capitulo = null;
        private Pelicula _pelicula = null;

        public double TitleSize {
            get { return title.FontSize; }
            set { title.FontSize = value; }
        }
        public VideoReciente() {
            InitializeComponent();
        }

        private void onClickReproducir(object sender, EventArgs e) {

        }

        public void setCapitulo(Capitulo c) {
            this._capitulo = c;
            actualizar();
        }

        public void setPelicula(Pelicula p) {
            this._pelicula = p;
            actualizar();
        }

        public void actualizar() {
            setImg();
            if (this._capitulo != null) {
                setTitle(_capitulo.nombre);
            } else if (_pelicula != null) {
                setTitle(_pelicula.nombre);
            }
        }

        private void setImg() {
            if (this._capitulo != null) {
                try {
                    Temporada t = ListaOnline.getTempByCap(this._capitulo);
                    if (t != null) {
                        if (t.img.CompareTo("") != 0) {
                            BitmapImage bm = new BitmapImage(new Uri(_pelicula.img, UriKind.RelativeOrAbsolute));
                            img.Source = bm;
                            if (bm.Width > bm.Height) {
                                img.Stretch = Stretch.UniformToFill;
                            }
                        }

                    } else if (ListaOnline.getSerieByTemp(t).img.CompareTo("") != 0) {
                        BitmapImage bm = new BitmapImage(new Uri(_pelicula.img, UriKind.RelativeOrAbsolute));
                        img.Source = bm;
                        if (bm.Width > bm.Height) {
                            img.Stretch = Stretch.UniformToFill;
                        }
                    }

                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            } else if (this._pelicula != null) {
                if (this._pelicula.img.CompareTo("") != 0) {
                    try {
                        BitmapImage bm = new BitmapImage(new Uri(_pelicula.img, UriKind.RelativeOrAbsolute));
                        img.Source = bm;
                        if (bm.Width > bm.Height) {
                            img.Stretch = Stretch.UniformToFill;
                        }
                    } catch (Exception e) {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        public void setTitle(string titulo) {
            title.Content = titulo;
        }
    }
}
