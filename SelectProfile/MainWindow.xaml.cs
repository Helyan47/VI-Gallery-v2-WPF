using ProyectoWPF;
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

namespace SelectProfile {
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            List<string> profiles = SaveData.getProfiles();
            foreach(string s in profiles) {
                Button b = new Button();
                b.Content = s;
                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.VerticalAlignment = VerticalAlignment.Stretch;
                b.VerticalContentAlignment = VerticalAlignment.Center;
                b.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                b.FontSize = 50;
                b.Padding = new Thickness(0);
                b.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                //b.Style = this.FindResource("CustomButtonStyle") as Style;
                b.Style = Application.Current.Resources["CustomButtonStyle"] as Style;
                perfiles.Children.Add(b);
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

        private void onClick(object sender, RoutedEventArgs e) {

        }
    }
}
