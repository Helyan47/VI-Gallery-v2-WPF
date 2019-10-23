using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
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
        private MainWindow ventanaMain;
        private Canvas defaultCanvas;
        private int mode = 0;

        public Carpeta(MainWindow ventana) {
            InitializeComponent();
            numSubcarpetas = 0;
            Title2.SetText("");
            ventanaMain = ventana;
            defaultCanvas = canvasFolder;
        }


        public void setImg() {
            ImageBrush ib = new ImageBrush(new BitmapImage(
        new Uri(@serie.getDirImg(), UriKind.Absolute)));
            ImgBorde.Background = ib;
            Img.Visibility = Visibility.Hidden;
        }


        public void setDefaultSource() {
            canvasFolder = defaultCanvas;
            Img.Visibility = Visibility.Visible;
            bordeDesc.Visibility = Visibility.Visible;
            descripcion.Visibility = Visibility.Hidden;
        }

        public void changeColor(System.Windows.Media.Color c) {
            SolidColorBrush sb = new SolidColorBrush(c);
            ColorPath.Fill = sb;
        }

        private void img_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
            bordeDesc.Visibility = Visibility.Visible;
            Img.Visibility = Visibility.Hidden;
            ImgBorde.Visibility = Visibility.Hidden;
            descripcion.Opacity = 0.04;
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();
            descripcion.Visibility = Visibility.Visible;
            
        }

        private void bordeDesc_MouseLeave(object sender, MouseEventArgs e) {
            if (serie.getDirImg().Equals("")) {
                Img.Visibility = Visibility.Visible;
            } else {
                ImgBorde.Visibility = Visibility.Visible;
            }
            bordeDesc.Visibility = Visibility.Hidden;
            descripcion.Visibility = Visibility.Hidden;
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
            Title2.SetText(title);
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
                Title2.SetText(serie.getTitle());
                if (serie.getDirImg() != "") {
                    Bitmap bm = new Bitmap(serie.getDirImg());

                    IntPtr hBitmap = bm.GetHbitmap();
                    System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                    //Img.Source = WpfBitmap;
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
                wrapCarpetaPropia.setMode(this.mode);
            }
            menuCarpeta.actualizar();
            wrapPanelAnterior.Visibility = Visibility.Hidden;
            menuCarpeta.Visibility = Visibility.Visible;
            wrapCarpetaPropia.Visibility = Visibility.Visible;


            gridSecundario.SetValue(Grid.RowProperty, 0);
            gridPrincipal.SetValue(Grid.RowProperty, 1);

            ventanaMain.ReturnVisibility(true);
           
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
                //menuCarpeta.Children.Add(menuCarpeta); //checkear padre
                wrapCarpetaPropia.setGridSubCarpetas(menuCarpeta.getWrapSubCarpetas());

                menuCarpeta.SetFlowCarpPrincipal(wrapCarpetaPropia);
                wrapCarpetaPropia.Visibility = Visibility.Hidden;
                menuCarpeta.Visibility = Visibility.Hidden;
                wrapCarpetaPropia.setMode(this.mode);
            }
            menuCarpeta.actualizar();
            
        }

        public void clickInverso() {
            menuCarpeta.Visibility = Visibility.Hidden;
            wrapPanelAnterior.Visibility = Visibility.Visible;
            wrapCarpetaPropia.Visibility = Visibility.Hidden;

            gridSecundario.SetValue(Grid.RowProperty, 1);
            gridPrincipal.SetValue(Grid.RowProperty, 0);
            ventanaMain.ReturnVisibility(false);
        }

        private void MouseClick(object sender, MouseButtonEventArgs e) {

            click();
        }

        public void changeTitle(String titulo) {
            Title2.SetText(titulo);
        }

        public string getTitle() {
            return (string)Title2.GetText();
        }

        public void changeMode(int mode) {
            this.mode = mode;
            
            if (mode == 0) {
                Grid.SetRow(ImgBorde, 3);
                Grid.SetRowSpan(ImgBorde, 2);
                Grid.SetRow(Img, 3);
                Grid.SetRowSpan(Img, 2);
                Grid.SetRow(bordeDesc, 3);
                Grid.SetRowSpan(bordeDesc, 2);
                
                Title2.ChangeForegroundColor(System.Windows.Media.Color.FromRgb(0, 0, 0));
                Title2.SetSombraVisible(false);
                //Title2.VerticalContentAlignment = VerticalAlignment.Center;
                gridLabel.Height = new GridLength(0.25, GridUnitType.Star);
            } else if (mode == 1) {
                Grid.SetRow(ImgBorde, 1);
                Grid.SetRowSpan(ImgBorde,4);
                Grid.SetRow(Img, 1);
                Grid.SetRowSpan(Img, 4);
                Grid.SetRow(bordeDesc, 1);
                Grid.SetRowSpan(bordeDesc, 4);
                Title2.ChangeForegroundColor(System.Windows.Media.Color.FromRgb(255,255,255));
                //Title.VerticalContentAlignment = VerticalAlignment.Top;
                Title2.SetSombraVisible(true);
                gridLabel.Height = new GridLength(0.45,GridUnitType.Star);
            }
        }
    }
}