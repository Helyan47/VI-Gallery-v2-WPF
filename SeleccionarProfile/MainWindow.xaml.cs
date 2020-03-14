using ProyectoWPF;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeleccionarProfile {
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public static string loadNewProfile = "";
        public static bool conn;
        public List<PerfilClass> profiles = null;
        public static UsuarioClass usuario = null;
        public MainWindow(bool connection,UsuarioClass user) {
            InitializeComponent();
            conn = connection;
            if (connection) {
                usuario = user;
            } else {
                usuario = null;
            }
            if (conn) {
                profiles = Conexion.loadPerfiles(user.id);
                if (profiles != null) {
                    foreach (PerfilClassOnline p in profiles) {
                        addButton(p);
                    }
                }
            } else {
                profiles = ConexionOffline.LoadProfile();
                if (profiles.Count != 0) {
                    foreach (PerfilClass s in profiles) {

                        addButton(s);

                    }
                } else {
                    ConexionOffline.addProfile(new PerfilClass("Perfil 1"));
                    profiles = ConexionOffline.LoadProfile();
                    foreach (PerfilClass s in profiles) {

                        addButton(s);

                    }
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
            //b.Style = this.FindResource("CustomButtonStyle") as Style;
            b.Style = Application.Current.Resources["CustomButtonStyle"] as Style;
            b.Click += onClick;
            perfiles.Children.Add(b);
            Rectangle rect = new Rectangle();
            rect.HorizontalAlignment = HorizontalAlignment.Stretch;
            rect.Height = 2;
            rect.Fill = new SolidColorBrush(Color.FromRgb(60, 60, 60));
            perfiles.Children.Add(rect);
        }

        public void MinimizeApp(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }

        public void CerrarApp(object sender, RoutedEventArgs e) {
            this.Close();
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
            Button aux = (Button)sender;
            PerfilClass p = getProfile(aux.Content.ToString());
            if (p != null) {
                if (conn) {
                    VIGallery vi = new VIGallery(p, usuario, conn);
                    vi.loadDataConexion(p.id);
                    this.Hide();
                    vi.Show();
                    this.Close();
                } else {
                    VIGallery vi = new VIGallery(p, usuario, conn);
                    vi.LoadProfileOffline(p);
                    this.Hide();
                    vi.Show();
                    this.Close();
                }
            } else {
                MessageBox.Show("No se ha podido seleccionar el perfil");
            }
        }

        private PerfilClass getProfile(string s) {
            foreach(PerfilClass p in profiles) {
                if (p.nombre.CompareTo(s) == 0) {
                    return p;
                }
            }
            return null;
        }
    }
}
