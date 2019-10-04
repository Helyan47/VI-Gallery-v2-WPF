using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para Carpeta.xaml
    /// </summary>
    public partial class Carpeta : UserControl {

        private System.Windows.Media.Color ColorgridPadre;
        private DispatcherTimer dispatcherTimer;
        public Carpeta() {
            InitializeComponent();

        }



        public void setDefaultSource() {
            Bitmap bm = Properties.Resources.folder_ico_png1;

            IntPtr hBitmap = bm.GetHbitmap();
            System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            img.Source = WpfBitmap;
            img.Width = 250;
            img.Height = 400;
            img.Stretch = System.Windows.Media.Stretch.Uniform;

            
        }

        public void changeColor(System.Drawing.Color c) {
            Bitmap bm = Properties.Resources.folder_ico_png1;

            for(int i = 0; i < bm.Width; i++) {
                for(int j = 0; j < bm.Height; j++) {
                    System.Drawing.Color aux = bm.GetPixel(i, j);
                    if (aux.A != 0) {
                        c = System.Drawing.Color.FromArgb(aux.A, c.R, c.G, c.B);
                        bm.SetPixel(i, j, c);
                    }
                }
            }

            IntPtr hBitmap = bm.GetHbitmap();
            System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            img.Source = WpfBitmap;
            img.Width = 250;
            img.Height = 400;
            img.Stretch = System.Windows.Media.Stretch.Uniform;
        }

        private void img_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
            descripcion.Visibility = Visibility.Visible;
            SolidColorBrush sc = new SolidColorBrush(ColorgridPadre);
            descripcion.Background = sc;
            descripcion.Opacity = 0.04;
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();

            img.Visibility = Visibility.Hidden;
        }

        private void descripcion_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
            descripcion.Visibility = Visibility.Hidden;
            img.Visibility = Visibility.Visible;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) {
            descripcion.Opacity+=0.04;
            if (descripcion.Opacity >= 1) {
                dispatcherTimer.Stop();
            }
        }


        public void setColorGridPadre(System.Windows.Media.Color grid) {
            this.ColorgridPadre = grid;
        }
    }
}
