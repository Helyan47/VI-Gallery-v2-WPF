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

namespace ProyectoWPF.NewFolders {
    /// <summary>
    /// Lógica de interacción para AddButton.xaml
    /// </summary>
    public partial class AddButton : Window {
        private Button aux;
        public AddButton(Button b) {
            InitializeComponent();
            aux = b;
        }

        private void onClickAccept(object sender, EventArgs e) {
            if (Title.Text.CompareTo("") != 0) {
                aux.Content = Title.Text;
                this.Close();
            } else {
                MessageBox.Show("No has introducido un titulo");
            }
        }

        private void onClickCancel(object sender, EventArgs e) {
            this.Close();
        }
    }
}
