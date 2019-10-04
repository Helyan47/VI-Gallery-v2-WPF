using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para Carpeta.xaml
    /// </summary>
    public partial class Carpeta : UserControl {

        public String defaultSource = ".\\icons\\folder-ico_png.png";
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
    }
}
