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
    /// Lógica de interacción para CutomButton.xaml
    /// </summary>
    public partial class CutomButton : UserControl {
        public string Text {
            get {
                return button.Content.ToString();
            }
            set {
                button.Content = value;
            }
        }
        public Color BackColorOver {
            get { return (Color)GetValue(BackColorOverProperty); }
            set { SetValue(BackColorOverProperty, value); }
        }
        public static readonly DependencyProperty BackColorOverProperty =
        DependencyProperty.Register("BackColorOver", typeof(Color), typeof(Button), new PropertyMetadata((sender, args) => {
            
        }));

        public Color BackColor {
            get { return (Color)GetValue(BackColorProperty); }
            set { 
                SetValue(BackColorProperty, value);
                button.Background = new SolidColorBrush(value);
            }
        }

        public static readonly DependencyProperty BackColorProperty =
        DependencyProperty.Register("BackColor", typeof(Color), typeof(Button), new PropertyMetadata((sender, args) => {
            
        }));

        public Color ForColorOver {
            get { return (Color)GetValue(ForColorOverProperty); }
            set { SetValue(ForColorOverProperty, value); }
        }

        public static readonly DependencyProperty ForColorOverProperty =
        DependencyProperty.Register("ForColorOver", typeof(Color), typeof(Button), new PropertyMetadata((sender, args) => {
            
        }));

        public Color ForColor {
            get { return (Color)GetValue(ForColorProperty); }
            set { 
                SetValue(ForColorProperty, value);
                button.Foreground = new SolidColorBrush(value);
            }
        }

        public static readonly DependencyProperty ForColorProperty =
        DependencyProperty.Register("ForColor", typeof(Color), typeof(Button), new PropertyMetadata((sender, args) => {
            
        }));

        public Color BorderColor {
            get { return (Color)GetValue(BorderColorProperty); }
            set {
                SetValue(BorderColorProperty, value);
                button.BorderBrush = new SolidColorBrush(value); 
            }
        }

        public static readonly DependencyProperty BorderColorProperty =
        DependencyProperty.Register("BorderColor", typeof(Color), typeof(Button), new PropertyMetadata((sender, args) => {

        }));

        public CutomButton() {
            InitializeComponent();
        }

        public string getText() {
            return button.Content.ToString(); ;
        }

        public void setContent(string name) {
            button.Content = name;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e) {
            button.Foreground = new SolidColorBrush(ForColorOver);
            button.Background = new SolidColorBrush(BackColorOver);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e) {
            button.Foreground = new SolidColorBrush(ForColor);
            button.Background = new SolidColorBrush(BackColor);
        }

        public void addButtonEvent(RoutedEventHandler e) {
            button.Click += e;
        }
    }
}