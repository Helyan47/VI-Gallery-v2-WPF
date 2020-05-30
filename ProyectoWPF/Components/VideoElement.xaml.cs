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

namespace ProyectoWPF.Components
{
    /// <summary>
    /// Lógica de interacción para VideoElement.xaml
    /// </summary>
    public partial class VideoElement : UserControl
    {

        private Capitulo _capitulo = null;
        private Pelicula _pelicula = null;
        private Serie _serie = null;
        public VideoElement()
        {
            InitializeComponent();
            
        }

        private void reproduceButton_MouseEnter(object sender, EventArgs e)
        {
            gridMouseOver.Visibility = Visibility.Visible;
        }

        private void reproduceButton_MouseLeave(object sender, EventArgs e)
        {
            gridMouseOver.Visibility = Visibility.Hidden;
        }

        public void setCapitulo(Capitulo c)
        {
            this._capitulo = c;
            actualizar();
        }

        public void setPelicula(Pelicula p)
        {
            this._pelicula = p;
            actualizar();
        }

        public void setSerie(Serie s) {
            this._serie = s;
            actualizar();
        }

        public void actualizar()
        {
            setImg();
            if(this._capitulo != null) {
                setTitle(_capitulo.nombre);
            }else if(_pelicula != null) {
                setTitle(_pelicula.nombre);
            }else if(_serie != null) {
                setTitle(_serie.nombre);
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
                            if (bm.Width > bm.Height) {
                                ib.Stretch = Stretch.UniformToFill;
                            }

                            bordeImg.Background = ib;
                        }

                    } else if (ListaOnline.getSerieByTemp(t).img.CompareTo("") != 0) {
                        BitmapImage bm = new BitmapImage(new Uri(ListaOnline.getSerieByTemp(t).img, UriKind.RelativeOrAbsolute));
                        ImageBrush ib = new ImageBrush(bm);
                        if (bm.Width > bm.Height) {
                            ib.Stretch = Stretch.UniformToFill;
                        }

                        bordeImg.Background = ib;
                    }

                } catch (Exception e) {
                    gridMouseOver.Visibility = Visibility.Visible;
                    Console.WriteLine(e.Message);
                }
            } else if (this._pelicula != null) {
                if (this._pelicula.img.CompareTo("") != 0) {
                    try {
                        BitmapImage bm = new BitmapImage(new Uri(_pelicula.img, UriKind.RelativeOrAbsolute));
                        ImageBrush ib = new ImageBrush(bm);
                        if (bm.Width > bm.Height) {
                            ib.Stretch = Stretch.UniformToFill;
                        }

                        bordeImg.Background = ib;
                    } catch (Exception e) {
                        gridMouseOver.Visibility = Visibility.Visible;
                        Console.WriteLine(e.Message);
                    }
                }
            }else if (this._serie != null) {
                try {
                    BitmapImage bm = new BitmapImage(new Uri(_serie.img, UriKind.RelativeOrAbsolute));
                    ImageBrush ib = new ImageBrush(bm);
                    if (bm.Width > bm.Height) {
                        ib.Stretch = Stretch.UniformToFill;
                    }

                    bordeImg.Background = ib;
                } catch (Exception e) {
                    gridMouseOver.Visibility = Visibility.Visible;
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void setTitle(string title) {
            videoTitle.Content = title;
        }
    }
}
