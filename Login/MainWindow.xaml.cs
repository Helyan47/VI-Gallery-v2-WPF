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
using InputTextLoL;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Login {
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            inputUser.changeMode("username");
            inputUser.invertColors("black");
            inputUser.lostFocus();
            inputPass.changeMode("password");
            inputPass.invertColors("black");
            inputPass.lostFocus();
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
                this.Width = 1000;
                this.Height = 700;
            }

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e) {
            Console.WriteLine("Hola");
            
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e) {
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
            if((p.X >= 0 && p.X <= inputUser.ActualWidth) && (p.Y >= 0 && p.Y <= inputUser.ActualHeight)){
                inputPass.lostFocus();
            } else if ((h.X >= 0 && h.X <= inputPass.ActualWidth) && (h.Y >= 0 && h.Y <= inputPass.ActualHeight)) {
                inputUser.lostFocus();
            } else {
                inputUser.lostFocus();
                inputPass.lostFocus();
            }
               
        }
    }
}
