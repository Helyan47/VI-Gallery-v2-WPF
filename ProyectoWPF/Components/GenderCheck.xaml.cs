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
    /// Lógica de interacción para GenderCheck.xaml
    /// </summary>
    public partial class GenderCheck : UserControl {

        private string textName = "";
        public GenderCheck(string name, bool selection) {
            InitializeComponent();
            genero.Content = name;
            textName = name;
            genero.Tag = name;
            changeSelection(selection);
        }

        private void changeSelection(object sender, EventArgs e) {
            if (genero.IsChecked == true) {
                genero.IsChecked = false;
                genero.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            } else if (genero.IsChecked == false) {
                genero.IsChecked = true;
                genero.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }

        public void changeSelection(bool selection) {
            if (selection) {
                genero.IsChecked = selection;
                genero.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            } else {
                genero.IsChecked = selection;
                genero.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        public bool? isSelected() {
            return genero.IsChecked;
        }

        public string getName() {
            return genero.Content.ToString();
        }

        private void mouseEnter(object sender, EventArgs e) {
            if (genero.IsChecked == false) {
                genero.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                genero.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void mouseLeave(object sender, EventArgs e) {
            if (genero.IsChecked == false) {
                genero.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                genero.Background = new SolidColorBrush(Color.FromRgb(23, 23, 23));
            }
        }

        public string getText() {
            return textName;
        }

        private void checkedValue(object sender, EventArgs e) {
            genero.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        private void uncheckedValue(object sender, EventArgs e) {
            genero.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }
}
