using ProyectoWPF.Data;
using System.Text.RegularExpressions;
using System.Windows;
using VIGallery.Data;

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
                Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())) + "]");
                if (!containsABadCharacter.IsMatch(newProfileText.Text)) {
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
                    } else {
                        MessageBox.Show("El perfil ya existe");
                    }

                } else {
                    MessageBox.Show("El nombre contiene caractéres no permitidos: " + new string(System.IO.Path.GetInvalidFileNameChars()));
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
