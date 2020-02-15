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
        public String tipo = "";
        private Grid gridCarpeta;
        private Button ButtonPrincipal;
        public string name { get; set; }
        public long menu { get; set; }

        public WrapPanelPrincipal()
        {
            InitializeComponent();
        }

        public void addCarpeta(Carpeta c) {
            c.Width = 250;
            c.Height = 400;
            c.Margin = new Thickness(40, 40, 40, 40);
            if (!c.getClass().img.Equals("")) {
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
            p.Width = 250;
            p.Height = 400;
            p.Margin = new Thickness(20, 20, 20, 20);
            if (!p.getClass().img.Equals("")) {
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

    }
}
