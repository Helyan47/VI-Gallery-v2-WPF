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
using System.Windows.Shapes;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para newSubCarpeta.xaml
    /// </summary>
    public partial class NewSubCarpeta : Window {

        private SubCarpeta p;
        private string rutaPadre;
        public NewSubCarpeta(string s) {
            InitializeComponent();
            rutaPadre = s;
        }

        private void BAceptar_Click(object sender, EventArgs e) {
            if (newName.Text.CompareTo("") != 0) {
                if (!Lista.Contains(rutaPadre + "/" + newName.Text)) {
                    p.setTitle(newName.Text);
                    
                } else {
                    MessageBox.Show("La subcarpeta ya existe");
                }
                    
            } else {
                MessageBox.Show("No has introducido ningun titulo para la carpeta");
            }
            
            
            this.Close();
        }

        public void setSubCarpeta(SubCarpeta s) {
            p = s;
        }

        public SubCarpeta getSubCarpeta() {
            return p;
        }
    }
}
