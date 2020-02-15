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
            textPassBox = inputPass.getPaswwordBox();

            inputUser.getInvButton().Click += clickedUser;
            inputPass.getInvButton().Click += clickedPass;
        }

        public void clickedUser(object sender, RoutedEventArgs e) {
            inputPass.lostFocus(false);
            userError.Visibility = Visibility.Hidden;
        }

        public void clickedPass(object sender, RoutedEventArgs e) {
            inputUser.lostFocus(false);
            passError.Visibility = Visibility.Hidden;
        }

        private void LoginClick(object sender, EventArgs e) {

            if (inputUser.getText().CompareTo("") != 0) {

                if (inputPass.getText().CompareTo("") != 0) {
                    bool connected = false;//Conexion.checkUserPass(inputUser.getText(), inputPass.getText());
                    if (connected) {
                        MessageBox.Show("Conectado");
                        //SeleccionarProfile.MainWindow selectProf = new SeleccionarProfile.MainWindow(true);
                        this.Hide();
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
            for (int i = 0; i < 10; i++) {
                borderEnter.Background = new SolidColorBrush(Color.FromArgb((byte)(25.5 * i), 255, 255, 255));
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e) {
            borderEnter.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
        }

        private void create_MouseEnter(object sender, MouseEventArgs e) {
            create.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void create_MouseLeave(object sender, MouseEventArgs e) {
            create.Foreground = new SolidColorBrush(Color.FromRgb(210, 210, 210));
        }

        private void forgotPass_MouseEnter(object sender, MouseEventArgs e) {
            forgotPass.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void forgotPass_MouseLeave(object sender, MouseEventArgs e) {
            forgotPass.Foreground = new SolidColorBrush(Color.FromRgb(210, 210, 210));
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e) {
            Point p = Mouse.GetPosition(inputUser);
            Point h = Mouse.GetPosition(inputPass);
            //Console.WriteLine(p.X + " || "+p.Y + " |||||| " + inputUser.ActualWidth + " || " + inputUser.ActualHeight);
            //Console.WriteLine(h.X + " || "+h.Y + " |||||| " + inputPass.ActualWidth + " || " + inputPass.ActualHeight);
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
    }
}
