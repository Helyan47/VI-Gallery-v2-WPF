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

namespace ProyectoWPF.Components {
    /// <summary>
    /// Lógica de interacción para GenderButton.xaml
    /// </summary>
    public partial class GenderButton : UserControl {

        public GenderButton() {
            InitializeComponent();
        }

        public string getText() {
            return button.Content.ToString(); ;
        }

        public void setContent(string name) {
            button.Content = name;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e) {
            button.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            button.Background = new SolidColorBrush(Colors.White);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e) {
            button.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            button.Background = new SolidColorBrush(Color.FromRgb(23, 23, 23));
        }

        public Button getButton() {
            return button;
        }
    }
}
