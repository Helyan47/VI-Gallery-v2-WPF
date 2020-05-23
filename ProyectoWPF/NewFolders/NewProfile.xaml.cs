using ProyectoWPF.Data;
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
    /// Lógica de interacción para NewProfile.xaml
    /// </summary>
    public partial class NewProfile : Window {

        private bool addedProfile = false;
        private string name = "";
        public NewProfile() {
            InitializeComponent();
        }

        public void onAceptar(object sender, RoutedEventArgs e) {
            if (newProfileText.Text.CompareTo("") != 0) {
                if (!Lista.checkProfile(newProfileText.Text)) {
                    
                    if (VIGallery.conexionMode) {
                        
                        PerfilClassOnline pfOnline = new PerfilClassOnline(newProfileText.Text.ToString(), VIGallery.getUser().id);
                        pfOnline = Conexion.saveProfile(pfOnline);
                        
                        if (pfOnline != null) {
                            addedProfile = true;
                            Lista.addProfile(pfOnline);
                        }
                    } else {
                        PerfilClass pf = new PerfilClass(newProfileText.Text.ToString());
                        ConexionOffline.startConnection();
                        pf = ConexionOffline.addProfile(pf);
                        ConexionOffline.closeConnection();
                        if (pf != null) {
                            addedProfile = true;
                            Lista.addProfile(pf);
                        }
                    }
                    name = newProfileText.Text;
                    this.Close();
                }
            } else {
                MessageBox.Show("No has introducido un nombre");
            }
        }

        private void onCancel(object sender, RoutedEventArgs e) {
            addedProfile = false;
            this.Close();
        }

        public bool isAdded() {
            return addedProfile;
        }

        public string getName() {
            return name;
        }
    }
}
