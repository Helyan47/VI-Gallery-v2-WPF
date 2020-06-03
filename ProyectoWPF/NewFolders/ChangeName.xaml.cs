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
    /// Lógica de interacción para ChangeName.xaml
    /// </summary>
    public partial class ChangeName : Window {

        private string _rutaPadre = "";
        private string name = null;
        public ChangeName(string rutaPadre) {
            InitializeComponent();
            _rutaPadre = rutaPadre;
            Console.WriteLine("RutaPadre: ---" + rutaPadre);
        }

        private void BAceptar_Click(object sender, EventArgs e) {
            if (newName.Text.CompareTo("") != 0) {
                if (!Lista.Contains(_rutaPadre + "/" + newName.Text)) {
                    name = newName.Text;
                    this.Close();
                } else {
                    MessageBox.Show("Una carpeta con ese nombre ya existe");
                }

            } else {
                MessageBox.Show("No has introducido ningún nombre para la carpeta");
            }


            
        }

        public string getNewName() {
            return name;
        }
    }
}
