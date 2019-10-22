using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        private Menu menuCarpeta;
        private string ruta;
        private Canvas defaultCanvas;
        private int mode;
        public SubCarpeta() {
            InitializeComponent();
            numSubCarpetas = 0;
            defaultCanvas = canvasFolder;
        }

        public void setDefaultSource() {
            canvasFolder = defaultCanvas;
            ImgBorde.Visibility = Visibility.Hidden;
            Img.Visibility = Visibility.Visible;
        }

        public void setImg() {
            ImageBrush ib = new ImageBrush(new BitmapImage(
        new Uri(@serie.getDirImg(), UriKind.Absolute)));
            ImgBorde.Background = ib;
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
            wrapCarpPropia.setMode(this.mode);

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

        public void changeMode(int mode) {
            this.mode = mode;
            if (mode == 0) {
                Grid.SetRow(ImgBorde, 3);
                Grid.SetRowSpan(ImgBorde, 2);
                Grid.SetRow(Img, 3);
                Grid.SetRowSpan(Img, 2);
                lbTitle.Visibility = Visibility.Visible;
            } else if (mode == 1) {
                Grid.SetRow(ImgBorde, 1);
                Grid.SetRowSpan(ImgBorde, 4);
                Grid.SetRow(Img, 1);
                Grid.SetRowSpan(Img, 4);
                lbTitle.Visibility = Visibility.Hidden;
            }
        }
    }
}