using System;
using System.Drawing;
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
    public partial class Carpeta : UserControl {

        private DispatcherTimer dispatcherTimer;
        private SerieClass serie;
        private WrapPanelPrincipal wrapPanelAnterior;
        private Grid gridPrincipal;
        private Grid gridSecundario;
        private Lista listas;
        private int numSubcarpetas;
        private WrapPanelPrincipal wrapCarpetaPropia;
        private Menu menuCarpeta;
        private Grid gridPadre;
        private string ruta;

        public Carpeta() {
            InitializeComponent();
            numSubcarpetas = 0;
            Title.Content = "";
        }



        public void setDefaultSource() {
            Bitmap bm = Properties.Resources.folder_ico_png1;

            IntPtr hBitmap = bm.GetHbitmap();
            System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            Img.Source = WpfBitmap;
            Img.Width = 250;
            Img.Height = 400;
            Img.Stretch = System.Windows.Media.Stretch.Uniform;

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

            Img.Source = WpfBitmap;
            Img.Width = 250;
            Img.Height = 400;
            Img.Stretch = System.Windows.Media.Stretch.Uniform;
        }

        private void img_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
            descripcion.Visibility = Visibility.Visible;
            SolidColorBrush sc = new SolidColorBrush(((SolidColorBrush)gridPadre.Background).Color);
            descripcion.Background = sc;
            descripcion.Opacity = 0.04;
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();

            Img.Visibility = Visibility.Hidden;
        }

        private void descripcion_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
            descripcion.Visibility = Visibility.Hidden;
            Img.Visibility = Visibility.Visible;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) {
            descripcion.Opacity+=0.04;
            if (descripcion.Opacity >= 1) {
                dispatcherTimer.Stop();
            }
        }


        public void setPadreSerie(WrapPanelPrincipal wrapPadre) {

            wrapPanelAnterior = wrapPadre;
        }

        public void SetGridPadre(Grid p) {
            gridPadre = p;
        }
        public Grid GetGridPadre() {
            return gridPadre;
        }

        public void setRuta(string s) {
            ruta = s;
        }

        public string getRuta() {
            return ruta;
        }

        public void setTitle(string title) {
            Title.Content = title;
        }

        public void AddSubCarpetas() {
            numSubcarpetas++;
        }

        public int getNumSubCarp() {
            return numSubcarpetas;
        }

        public void setSerie(SerieClass newSerie) {

            serie = newSerie;
        }

        public Lista getListaCarpetas() {
            return listas;
        }

        public void setListaCarpetas(Lista listaSeries) {
            this.listas = listaSeries;
        }

        public WrapPanelPrincipal GetWrapCarpPrincipal() {
            return wrapCarpetaPropia;
        }

        public Grid getGridButtonsPrincipal() {
            return gridPrincipal;
        }

        public Grid getGridButtonsSecundario() {
            return gridSecundario;
        }

        public void SetGridsOpciones(Grid principal, Grid secundario) {
            gridPrincipal = principal;
            gridSecundario = secundario;
        }

        public SerieClass getSerie() {

            return serie;
        }

        public void actualizar() {
            if (serie.getTitle().Equals("")) {

            } else {
                Title.Content = serie.getTitle();
                if (serie.getDirImg() != "") {
                    Bitmap bm = new Bitmap(serie.getDirImg());

                    IntPtr hBitmap = bm.GetHbitmap();
                    System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                    Img.Source = WpfBitmap;
                }

            }

        }

        public Menu GetMenuCarpeta() {
            return menuCarpeta;
        }

        public void click() {
            if (menuCarpeta == null) {
                menuCarpeta = new Menu(this);
                listas.addMenu(menuCarpeta);
                wrapCarpetaPropia = new WrapPanelPrincipal();
                listas.addSubWrap(wrapCarpetaPropia);
                wrapCarpetaPropia.setCarpeta(this);
                wrapCarpetaPropia.setSubcarpeta(null);
                gridPadre.Children.Add(menuCarpeta);
                menuCarpeta.SetFlowLayAnterior(wrapPanelAnterior);
                //menuCarpeta.Children.Add(menuCarpeta); //checkear padre
                wrapCarpetaPropia.setGridSubCarpetas(menuCarpeta.getWrapSubCarpetas());

                menuCarpeta.SetFlowCarpPrincipal(wrapCarpetaPropia);
                wrapCarpetaPropia.Visibility = Visibility.Hidden;
                menuCarpeta.Visibility = Visibility.Hidden;
            }
            menuCarpeta.actualizar();
            wrapPanelAnterior.Visibility = Visibility.Hidden;
            menuCarpeta.Visibility = Visibility.Visible;
            wrapCarpetaPropia.Visibility = Visibility.Visible;


            gridSecundario.SetValue(Grid.RowProperty, 0);
            gridPrincipal.SetValue(Grid.RowProperty, 1);
        }

        public void clickEspecial() {
            if (menuCarpeta == null) {
                menuCarpeta = new Menu(this);
                listas.addMenu(menuCarpeta);
                wrapCarpetaPropia = new WrapPanelPrincipal();
                listas.addSubWrap(wrapCarpetaPropia);
                wrapCarpetaPropia.setCarpeta(this);
                wrapCarpetaPropia.setSubcarpeta(null);
                gridPadre.Children.Add(menuCarpeta);
                menuCarpeta.SetFlowLayAnterior(wrapPanelAnterior);
                menuCarpeta.getWrapSubCarpetas().Children.Add(gridPadre); //checkear padre
                wrapCarpetaPropia.setGridSubCarpetas(menuCarpeta.getWrapSubCarpetas());

                menuCarpeta.SetFlowCarpPrincipal(wrapCarpetaPropia);
                wrapCarpetaPropia.Visibility = Visibility.Hidden;
                menuCarpeta.Visibility = Visibility.Hidden;
            }
            menuCarpeta.actualizar();
        }

        public void clickInverso() {
            menuCarpeta.Visibility = Visibility.Hidden;
            wrapPanelAnterior.Visibility = Visibility.Visible;
            wrapCarpetaPropia.Visibility = Visibility.Hidden;

            gridSecundario.SetValue(Grid.RowProperty, 1);
            gridPrincipal.SetValue(Grid.RowProperty, 0);
        }

        private void MouseClick(object sender, MouseButtonEventArgs e) {

            click();
        }

        public void changeTitle(String titulo) {
            Title.Content = titulo;
        }

        public string getTitle() {
            return (string)Title.Content;
        }

    }
}