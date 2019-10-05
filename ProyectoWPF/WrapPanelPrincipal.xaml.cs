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

namespace ProyectoWPF
{
    /// <summary>
    /// Lógica de interacción para WrapPanelPrincipal.xaml
    /// </summary>
    public partial class WrapPanelPrincipal : UserControl
    {

        private System.Windows.Media.Color colorGridPadre;
        private Carpeta carpeta;
        private SubCarpeta subcarpeta;
        private SubCarpeta subCarpetaPadre;
        public String nombre = "luis";
        private Grid gridCarpeta;

        public WrapPanelPrincipal()
        {
            InitializeComponent();
        }

        public void addCarpeta(Carpeta c) {
            c.Width = 250;
            c.Height = 400;
            c.Margin = new Thickness(10, 10, 10, 10);
            if (!c.getSerie().getDirImg().Equals("")) {
                c.setImg();
            } else {
                c.setDefaultSource();
            }
            //c.changeColor(System.Drawing.Color.Red);
            wrapPanel.Children.Add(c);
            
        }

        public void setColorGridPadre(Color grid) {
            this.colorGridPadre = grid;
        }

        public void setGridSubCarpetas(Grid p) {
            gridCarpeta = p;
        }

        public Grid GetGridSubCarpetas() {
            return gridCarpeta;
        }


        //public void setBackColor(Color c) {
        //    flowLayoutPanel1.BackColor = c;
        //}

        public void setCarpeta(Carpeta p) {
            carpeta = p;
        }

        public Carpeta getCarpeta() {
            return carpeta;
        }

        public void addSubCarpeta(SubCarpeta p) {
            p.Width = 187.5;
            p.Height = 300;
            p.chageFontSize(14);
            p.Margin = new Thickness(10, 10, 10, 10);
            if (!p.getSerie().getDirImg().Equals("")) {
                p.setImg();
            } else {
                p.setDefaultSource();
            }
            wrapPanel.Children.Add(p);
        }

        public SubCarpeta getSubCarpeta() {
            return subcarpeta;
        }

        public void setSubcarpeta(SubCarpeta p) {
            subcarpeta = p;
        }
        public SubCarpeta getSubCarpetaPadre() {
            return subCarpetaPadre;
        }

        public void setSubcarpetaPadre(SubCarpeta p) {
            subCarpetaPadre = p;
        }

    }
}
