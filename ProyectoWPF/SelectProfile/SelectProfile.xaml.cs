using ProyectoWPF.Data;
using ProyectoWPF.NewFolders;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VIGallery.Data;

namespace ProyectoWPF.SelectProfile {
    /// <summary>
    /// Lógica de interacción para SelectProfile.xaml
    /// </summary>
    public partial class SelectProfile : Window {

        public static string loadNewProfile = "";
        public static bool conn;
        public List<PerfilClass> _profiles = null;
        private List<Button> _buttons = new List<Button>();
        private PerfilClass _selectedProfile = null;
        public SelectProfile(bool connection, UsuarioClass user) {
            InitializeComponent();

            conn = connection;
            VIGallery.conexionMode = connection;

            if (connection) {
                VIGallery._user = user;
            } else {
                VIGallery._user = null;
            }
            if (conn) {
                _profiles = Conexion.loadProfiles(user.id);
                if (_profiles != null) {
                    foreach (PerfilClassOnline p in _profiles) {
                        addButton(p);
                        Lista.addProfile(p);
                    }
                }
            } else {
                ConexionOffline.startConnection();
                _profiles = ConexionOffline.LoadProfiles();
                if (_profiles.Count != 0) {
                    foreach (PerfilClass p in _profiles) {

                        addButton(p);
                        Lista.addProfile(p);

                    }
                } else {
                    ConexionOffline.addProfile(new PerfilClass("Perfil 1"));
                    _profiles = ConexionOffline.LoadProfiles();
                    foreach (PerfilClass p in _profiles) {

                        addButton(p);
                        Lista.addProfile(p);

                    }
                    ConexionOffline.closeConnection();
                }

            }
        }

        public void addButton(PerfilClass s) {
            Button b = new Button();
            b.Content = s.nombre;
            b.HorizontalAlignment = HorizontalAlignment.Stretch;
            b.VerticalAlignment = VerticalAlignment.Stretch;
            b.VerticalContentAlignment = VerticalAlignment.Center;
            b.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            b.FontSize = 40;
            b.Padding = new Thickness(0);
            b.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            b.Style = Application.Current.Resources["CustomButtonStyle"] as Style;
            b.Click += selectProfile;
            bool added = Lista.addButtonProfile(b);
            if (added) {
                perfiles.Children.Add(b);
            } else {
                b = null;
            }
            selectProfile(b);

        }

        public void MinimizeApp(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }

        public void CerrarApp(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void selectProfile(object sender, RoutedEventArgs e) {
            Button aux = (Button)sender;
            PerfilClass perfilSelected = Lista.getProfile(aux.Content.ToString());
            if (perfilSelected != null) {
                _selectedProfile = perfilSelected;
                Lista.clearBackProfile();
                aux.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF595959"));
            }
        }

        private void selectProfile(Button b) {
            PerfilClass perfilSelected = Lista.getProfile(b.Content.ToString());
            if (perfilSelected != null) {
                Lista.clearBackProfile();
                b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF595959"));
            }
        }

        public void MaximizeApp(object sender, RoutedEventArgs e) {
            if (this.WindowState == WindowState.Normal) {
                this.WindowState = WindowState.Maximized;
            } else if (this.WindowState == WindowState.Maximized) {
                this.WindowState = WindowState.Normal;
                this.Width = 600;
                this.Height = 800;
            }

        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e) {
            if (this.WindowState == WindowState.Maximized) {
                this.WindowState = WindowState.Normal;
                this.Left = Mouse.GetPosition(this).X - 100;
                this.Top = Mouse.GetPosition(this).Y - 10;
            }
            this.DragMove();
        }

        private void onClick(object sender, RoutedEventArgs e) {
            if (_selectedProfile != null) {
                Dispatcher.Invoke(new Action(() => {
                    if (conn) {
                        VIGallery vi = new VIGallery(_selectedProfile);
                        vi.loadDataConexion(_selectedProfile.id);
                        vi.Show();

                    } else {
                        VIGallery vi = new VIGallery(_selectedProfile);
                        vi.LoadProfileOffline(_selectedProfile);
                        vi.Show();

                    }
                }));
                //Task task1 = Task.Factory.StartNew(() => loadProfile());
                //Task.WaitAll(task1);
                this.Close();
            } else {
                MessageBox.Show("No has seleccionado un perfil");
            }
        }


        private PerfilClass getProfile(string s) {
            foreach (PerfilClass p in _profiles) {
                if (p.nombre.CompareTo(s) == 0) {
                    return p;
                }
            }
            return null;
        }

        private void addProfile(object sender, RoutedEventArgs e) {
            NewProfile newProf = new NewProfile();
            newProf.ShowDialog();
            if (newProf.isAdded()) {
                Button b = new Button();
                b.Content = newProf.getName();
                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.VerticalAlignment = VerticalAlignment.Stretch;
                b.VerticalContentAlignment = VerticalAlignment.Center;
                b.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                b.FontSize = 40;
                b.Padding = new Thickness(0);
                b.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                b.Style = Application.Current.Resources["CustomButtonStyle"] as Style;
                b.Click += selectProfile;
                Lista.addButtonProfile(b);
                perfiles.Children.Add(b);

                MessageBox.Show("El perfil se ha creado correctamente");

            } else {
                MessageBox.Show("No se ha podido crear el perfil");
            }

        }

        private void removeProfile(object sender, RoutedEventArgs e) {
            if (_selectedProfile != null) {
                if (conn) {
                    Conexion.deleteProfile(_selectedProfile.id);
                    Button b = Lista.getProfileButton(_selectedProfile.nombre);
                    Lista.removeProfile(_selectedProfile.nombre);
                    if (b != null) {
                        perfiles.Children.Remove(b);
                    }

                } else {
                    ConexionOffline.deleteProfile(_selectedProfile.id);
                    Button b = Lista.getProfileButton(_selectedProfile.nombre);
                    Lista.removeProfile(_selectedProfile.nombre);
                    if (b != null) {
                        perfiles.Children.Remove(b);
                    }
                }

            }
        }
    }
}
