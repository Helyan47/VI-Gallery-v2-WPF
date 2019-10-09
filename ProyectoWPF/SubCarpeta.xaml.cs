using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para SubCarpeta.xaml
    /// </summary>
    /// 

    public partial class SubCarpeta : UserControl {

        private String nombre;
        private SerieClass serie;
        private WrapPanelPrincipal wrapCarpAnterior;
        private Lista listas;
        private int numSubCarpetas;
        private WrapPanelPrincipal wrapCarpPropia;
        private Grid gridPadre;
        private int idHijo;
        private Menu menuCarpeta;
        private string ruta;
        private Canvas defaultCanvas;
        public SubCarpeta() {
            InitializeComponent();
            numSubCarpetas = 0;
            defaultCanvas = canvasFolder;
        }

        public void setDefaultSource() {
            canvasFolder = defaultCanvas;
            Img2.Visibility = Visibility.Hidden;
            Img.Visibility = Visibility.Visible;
        }

        public void setImg() {
            Bitmap bm = new Bitmap(serie.getDirImg());

            IntPtr hBitmap = bm.GetHbitmap();
            System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            Img2.Source = WpfBitmap;
            Img2.Stretch = System.Windows.Media.Stretch.Uniform;
            borde.Visibility = Visibility.Visible;
            Img2.Visibility = Visibility.Visible;
            Img.Visibility = Visibility.Hidden;
        }

        public void setRuta(string s) {
            ruta = s;
        }

        public string getRuta() {
            return ruta;
        }
        public void AddSubCarpetas() {
            numSubCarpetas++;
        }

        public int getNumSubCarp() {
            return numSubCarpetas;
        }

        public Grid GetGridCarpeta() {
            return gridPadre;
        }

        public void setIdHijo(int num) {
            idHijo = num;
        }

        public int getIdHijo() {
            return idHijo;
        }

        public WrapPanelPrincipal getPadreSerie() {
            return wrapCarpAnterior;
        }

        public void SetGridPadre(Grid p) {
            gridPadre = p;
            p.Children.Add(wrapCarpPropia);
        }

        public void setSerie(SerieClass newSerie) {

            serie = newSerie;
        }

        public Lista GetListaCarpetas() {
            return listas;
        }

        public void SetListaCarpetas(Lista listaSeries) {
            this.listas = listaSeries;
        }

        public void setMenuCarpeta(Menu m) {
            menuCarpeta = m;
        }

        public Menu GetMenuCarpeta() {
            return menuCarpeta;
        }

        public SerieClass getSerie() {

            return serie;
        }

        public WrapPanelPrincipal getWrapCarpPrincipal() {
            return wrapCarpPropia;
        }


        public Grid GetGridPadre() {
            return gridPadre;
        }

        public void actualizar() {
            if (serie.getTitle().Equals("")) {

            } else {
                if (serie.getDirImg() != "") {
                    setImg();
                }
            }

        }

        public void setDatos(SerieClass ser, WrapPanelPrincipal flowPadre, Lista listas, Grid gridP) {
            wrapCarpPropia = new WrapPanelPrincipal();

            serie = ser;
            wrapCarpAnterior = flowPadre;


            this.listas = listas;
            this.listas.addSubWrap(wrapCarpPropia);
            gridPadre = gridP;
            gridPadre.Children.Add(wrapCarpPropia);
            //flowCarpPropia.Dock = DockStyle.Fill;
            wrapCarpPropia.Visibility = System.Windows.Visibility.Hidden;
            wrapCarpPropia.setSubcarpeta(this);


        }

        public void click() {

            menuCarpeta.actualizar();
            menuCarpeta.changeTitle(nombre);
            wrapCarpAnterior.Visibility = System.Windows.Visibility.Hidden;
            wrapCarpPropia.Visibility = System.Windows.Visibility.Visible;
        }

        public void clickEspecial() {

            menuCarpeta.actualizar();
            menuCarpeta.changeTitle(nombre);

        }

        public void clickInverso() {
            wrapCarpAnterior.Visibility = System.Windows.Visibility.Visible;
            wrapCarpPropia.Visibility = System.Windows.Visibility.Hidden;

            if (wrapCarpAnterior.getSubCarpeta() != null) {
                menuCarpeta.Title.Content = wrapCarpAnterior.getSubCarpeta().getTitle();
            }else if(wrapCarpAnterior.getCarpeta() != null) {
                menuCarpeta.Title.Content = wrapCarpAnterior.getCarpeta().getTitle();
            }
        }

        private void OnClick(object sender,RoutedEventArgs e) {
            click();
        }


        public void setTitle(String titulo) {
            nombre = titulo;
            Title.Content = titulo;
        }

        public string getTitle() {
            return nombre;
        }

        private void SubCarpeta_MouseClick(object sender, MouseEventArgs e) {
            click();
        }

        public void chageFontSize(int size) {
            Title.FontSize = size;
        }
    }
}