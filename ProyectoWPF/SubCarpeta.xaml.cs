using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;

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
        public SubCarpeta() {
            InitializeComponent();
            numSubCarpetas = 0;
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

        public Lista getListaSeries() {
            return listas;
        }

        public void setListaSeries(Lista listaSeries) {
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

        public WrapPanelPrincipal getFlowCarpPrincipal() {
            return wrapCarpPropia;
        }


        public Grid GetGridPadre() {
            return gridPadre;
        }

        public void actualizar() {
            if (serie.getTitle().Equals("")) {

            } else {
                if (serie.getDirImg() != "") {
                    Bitmap bm = new Bitmap(serie.getDirImg());

                    IntPtr hBitmap = bm.GetHbitmap();
                    System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                    Img.Source = WpfBitmap;
                }
            }

        }

        public void setDatos(SerieClass ser, WrapPanelPrincipal flowPadre, Lista listas, Grid gridP) {
            wrapCarpPropia = new WrapPanelPrincipal();

            serie = ser;
            wrapCarpAnterior = flowPadre;


            this.listas = listas;
            this.listas.addWrapCarpeta(wrapCarpPropia);
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
        }

        private void Img_Click(object sender, EventArgs e) {

            click();
        }


        private void Borde_MouseClick(object sender, MouseEventArgs e) {
            /*
            fl.Parent = this.Parent.Parent;
            fl.setFlowPadre(padreSerie);
            this.Parent.Visible = false;
            fl.Dock = DockStyle.Fill;
            fl.Visible = true;
            */
            click();

        }

        private void Title_Click(object sender, EventArgs e) {
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
    }
}