using MySql.Data.MySqlClient;
using SeleccionarProfile.Data;
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


namespace LoginUser {
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        TextBox textUserBox;
        PasswordBox textPassBox;

        public MainWindow() {
            InitializeComponent();
            inputUser.changeMode("username");
            inputUser.invertColors("black");
            inputUser.lostFocus(true);
            textUserBox = inputUser.getUsernameInput();
            inputPass.changeMode("password");
            inputPass.invertColors("black");
            inputPass.lostFocus(true);
            textUserBox.KeyUp += inputUser_KeyUp;
            textUserBox.KeyDown += inputUser_KeyUp;
            textPassBox = inputPass.getPaswwordBox();

            inputUser.getInvButton().Click += clickedUser;
            inputPass.getInvButton().Click += clickedPass;

            newUser.changeMode("username");
            newUser.invertColors("black");
            newUser.lostFocus(true);

            newPass1.changeMode("password");
            newPass1.invertColors("black");
            newPass1.lostFocus(true);

            newPass2.changeMode("password");
            newPass2.invertColors("black");
            newPass2.lostFocus(true);

            newUser.getInvButton().Click += clickedNewUser;
            newPass1.getInvButton().Click += clickedNewPass1;
            newPass2.getInvButton().Click += clickedNewPass2;

        }

        public void clickedUser(object sender, RoutedEventArgs e) {
            inputPass.lostFocus(false);
            userError.Visibility = Visibility.Hidden;
        }

        public void clickedPass(object sender, RoutedEventArgs e) {
            inputUser.lostFocus(false);
            passError.Visibility = Visibility.Hidden;
        }

        public void clickedNewUser(object sender, RoutedEventArgs e) {
            newPass1.lostFocus(false);
            newPass2.lostFocus(false);
        }

        public void clickedNewPass1(object sender, RoutedEventArgs e) {
            newUser.lostFocus(false);
            newPass2.lostFocus(false);
        }
        public void clickedNewPass2(object sender, RoutedEventArgs e) {
            newUser.lostFocus(false);
            newPass1.lostFocus(false);
        }

        private void LoginClick(object sender, EventArgs e) {
            try {
                if (inputUser.getText().CompareTo("") != 0) {

                    if (inputPass.getText().CompareTo("") != 0) {
                        UsuarioClass user = Conexion.checkUser(inputUser.getText(), inputPass.getText());

                        if (user != null) {
                            MessageBox.Show("Conectado");
                            SeleccionarProfile.MainWindow selectProf = new SeleccionarProfile.MainWindow(true, user);
                            this.Hide();
                            selectProf.Show();
                            this.Close();
                        } else {
                            inputUser.setError();
                            inputPass.setError();
                            passError.Content = "El usuario o la contraseña son incorrectos";
                            passError.Visibility = Visibility.Visible;
                            //MessageBox.Show("Usuario o contraseña incorrectos");
                        }
                    } else {
                        inputPass.setError();
                        passError.Content = "La contraseña debe contener al menos un carácter";
                        passError.Visibility = Visibility.Visible;
                    }

                } else {
                    inputUser.setError();
                    userError.Content = "El usuario debe contener al menos un carácter";
                    userError.Visibility = Visibility.Visible;
                }
            } catch (MySqlException exc) {
                MessageBox.Show("Se ha producido un error al conectar con la base de datos");
            }
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

        private void Button_MouseEnter(object sender, MouseEventArgs e) {
            borderEnter.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e) {
            borderEnter.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
        }

        private void return_MouseEnter(object sender, MouseEventArgs e) {
            returnBorder.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        private void return_MouseLeave(object sender, MouseEventArgs e) {
            returnBorder.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
        }

        private void newUser_MouseEnter(object sender, MouseEventArgs e) {
            borderEnterNew.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        private void newUser_MouseLeave(object sender, MouseEventArgs e) {
            borderEnterNew.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
        }

        private void create_MouseEnter(object sender, MouseEventArgs e) {
            create.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void create_MouseLeave(object sender, MouseEventArgs e) {
            create.Foreground = new SolidColorBrush(Color.FromRgb(210, 210, 210));
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e) {
            Point p = Mouse.GetPosition(inputUser);
            Point h = Mouse.GetPosition(inputPass);
            if ((p.X >= 0 && p.X <= inputUser.ActualWidth) && (p.Y >= 0 && p.Y <= inputUser.ActualHeight)) {
                userError.Visibility = Visibility.Hidden;
                //inputPass.lostFocus();
            } else if ((h.X >= 0 && h.X <= inputPass.ActualWidth) && (h.Y >= 0 && h.Y <= inputPass.ActualHeight)) {
                passError.Visibility = Visibility.Hidden;
                //inputUser.lostFocus();
            } else {
                inputUser.lostFocus(true);
                inputPass.lostFocus(true);
            }

        }
        private void NewGrid_MouseUp(object sender, MouseButtonEventArgs e) {
            Point p = Mouse.GetPosition(newUser);
            Point h = Mouse.GetPosition(newPass1);
            Point q = Mouse.GetPosition(newPass2);
            if ((p.X >= 0 && p.X <= newUser.ActualWidth) && (p.Y >= 0 && p.Y <= newUser.ActualHeight)) {
                newUserNameError.Visibility = Visibility.Hidden;
                //inputPass.lostFocus();
            } else if ((h.X >= 0 && h.X <= newPass1.ActualWidth) && (h.Y >= 0 && h.Y <= newPass1.ActualHeight)) {
                newPass1Error.Visibility = Visibility.Hidden;
                //inputUser.lostFocus();
            } else if((q.X >= 0 && q.X <= newPass2.ActualWidth) && (q.Y >= 0 && q.Y <= newPass2.ActualHeight)){
                newPass2Error.Visibility = Visibility.Hidden;
            } else {
                newUser.lostFocus(true);
                newPass1.lostFocus(true);
                newPass2.lostFocus(true);
            }

        }

        private void inputUser_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Tab) {
                inputUser.lostFocus(true);
                inputPass.gotFocus();
            }
        }

        private void inputPass_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Tab) {
                inputPass.lostFocus(true);
                inputUser.gotFocus();
            }
        }

        private void OfflineMode_Click(object sender, MouseButtonEventArgs e) {
            SeleccionarProfile.MainWindow selectProf = new SeleccionarProfile.MainWindow(false, null);
            this.Hide();
            selectProf.Show();
            this.Close();
        }

        private void showCreateUser(object sender, EventArgs e) {
            gridLogin.Visibility = Visibility.Hidden;
            gridNewUser.Visibility = Visibility.Visible;
        }

        private void showLogin(object sender, EventArgs e) {
            gridLogin.Visibility = Visibility.Visible;
            gridNewUser.Visibility = Visibility.Hidden;
        }

        private void createUserClick(object sender, EventArgs e) {
            try {
                newUserNameError.Visibility = Visibility.Hidden;
                newPass1Error.Visibility = Visibility.Hidden;
                newPass2Error.Visibility = Visibility.Hidden;
                if (newUser.getText().CompareTo("") != 0) {
                    if (newPass1.getText().Length >= 8) {
                        if (newPass2.getText().CompareTo(newPass1.getText()) == 0) {
                            string message = Conexion.saveUser(newUser.getText(), newPass1.getText());
                            if (message.CompareTo("Usuario existe") == 0) {
                                newUserNameError.Content = "El nombre de usuario ya existe";
                                newUserNameError.Visibility = Visibility.Visible;
                            } else if (message.CompareTo("Usuario creado") == 0) {
                                MessageBox.Show("Se ha creado correctamente el usuario");
                                gridLogin.Visibility = Visibility.Visible;
                                gridNewUser.Visibility = Visibility.Hidden;
                            }
                        } else {
                            newPass2Error.Content = "No coinciden las contraseñas";
                            newPass2Error.Visibility = Visibility.Visible;
                        }
                    } else {
                        newPass1Error.Content = "La contraseña debe tener 8 caracteres como mínimo";
                        newPass1Error.Visibility = Visibility.Visible;
                    }
                } else {
                    newUserNameError.Content = "No has introducido un nombre de usuario";
                    newUserNameError.Visibility = Visibility.Visible;
                }
            } catch (MySqlException exc) {
                MessageBox.Show("Se ha producido un error al conectar con la base de datos");
            }
        }

    }
}
