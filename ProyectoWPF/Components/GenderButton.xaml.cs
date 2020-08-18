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

        private bool selected = false;
        public GenderButton(string name) {
            InitializeComponent();
            text.Content = name;
        }

        private void changeSelection(object sender, EventArgs e) {
            if (selected) {
                selected = false;
                text.Foreground = new SolidColorBrush(Colors.White);
                border.Background = new SolidColorBrush(Color.FromRgb(23, 23, 23));
            } else {
                selected = true;
                text.Foreground = new SolidColorBrush(Colors.Black);
                border.Background = new SolidColorBrush(Colors.White);
            }
        }

        public bool isSelected() {
            return selected;
        }

        public string getName() {
            return text.Content.ToString();
        }
    }
}
