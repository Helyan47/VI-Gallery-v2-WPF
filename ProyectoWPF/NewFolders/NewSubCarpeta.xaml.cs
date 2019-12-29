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
        public NewSubCarpeta() {
            InitializeComponent();
        }

        private void BAceptar_Click(object sender, EventArgs e) {
            p.setTitle(newName.Text);
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
