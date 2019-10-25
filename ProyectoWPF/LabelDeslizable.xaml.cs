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
using System.Windows.Threading;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para LabelDeslizable.xaml
    /// </summary>
    public partial class LabelDeslizable : UserControl {

        private Color principalColor;
        private Color secondaryColor;
        private Color defaultColorShadow;
        private Color customPrincipalColor;
        private Color customSecondaryColor;
        private Color customColorShadow;
        private DispatcherTimer tm;
        private Thickness normalMargin;
        private int margin;
        private int widthUserControl;
        private int numLetras;
        private double intervalo;
        public LabelDeslizable() {
            InitializeComponent();
            defaultColorShadow = shadowColor.Color;
            SolidColorBrush sb = Title.Foreground as SolidColorBrush;
            principalColor = sb.Color;
            secondaryColor = Color.FromRgb(255, 255, 255);
            normalMargin = new Thickness(0);
            margin = 0;
            widthUserControl = (int)Sombra.Width;
        }

        public void ChangeShadowColor(Color c) {
            customColorShadow = c;
            defaultColorShadow = c;
        }

        public void ChangePrincipalColor(Color c) {
            customPrincipalColor = c;
            principalColor = c;
        }

        public void ChangeSecondaryColor(Color c) {
            customSecondaryColor = c;
            secondaryColor = c;
        }

        public void ChangeForegroundColor(Color c) {
            Title.Foreground = new SolidColorBrush(c);
        }

        public void SetText(string text) {
            Title.Content = text;
        }

        public string GetText() {
            return Title.Content.ToString();
        }

        private void Title_MouseEnter(object sender, MouseEventArgs e) {
            Console.WriteLine("mouseenter");
            tm = new DispatcherTimer();
            tm.Tick += Tm_Tick;
            tm.Interval = new TimeSpan(0, 0, 0, 0, 10);
            widthUserControl = (int)Title.ActualWidth;
            numLetras = Title.Content.ToString().Length;
            if (numLetras >= 18){
                if(numLetras <= 22) {
                    intervalo = (numLetras * 7.5) / ((-(numLetras / 18) * 82.5) + 1000);
                } else if(numLetras<=40){
                    intervalo = (numLetras * 7.5) / ((-(numLetras / 18) * 82.5) + 450);
                } else {
                    intervalo = (numLetras * 7.5) / ((-(numLetras / 18) * 82.5) + 400);
                }
                
            } else {
                intervalo = 0;
            }

            Color c = limiteColor.Color;
            c.A = 255;
            limiteColor.Color = c;
            c = limiteColor2.Color;
            c.A = 255;
            limiteColor2.Color = c;
            c = limiteColor3.Color;
            c.A = 255;
            limiteColor3.Color = c;
            //if (numLetras <= 17) {
            //    intervalo = 0;
            //} else if(numLetras >=50){
            //    intervalo = (numLetras * 7.5) / 200;
            //} else if(numLetras >=30){
            //    intervalo = (numLetras * 7.5) / 250;
            //} else {
            //    intervalo = (numLetras * 7.5) / 350;
            //}

            tm.Start();
        }

        private void Tm_Tick(object sender, EventArgs e) {
            margin--;
            borde.Margin = new Thickness(margin, 0, 0, 0);
            int cont = margin * -1;

            if (cont % 7 == 0) {
                limiteColor.Offset += 0.0016667;
                
                limiteColor2.Offset += 0.00083;
                limiteColor3.Offset += 0.0005;
            }

            if (cont > (widthUserControl * intervalo)) {
                borde.Margin = normalMargin;
                margin = 0;
                limiteColor.Offset = 0.765;
                limiteColor2.Offset += 0.871;
                limiteColor3.Offset = 0.935;
            }
        }

        private void Title_MouseLeave(object sender, MouseEventArgs e) {
            if (tm.IsEnabled) {
                tm.Stop();
                borde.Margin = normalMargin;
                margin = 0;
                Color c = (Color)ColorConverter.ConvertFromString("#FF000000");
                limiteColor.Color = c;
                c = (Color)ColorConverter.ConvertFromString("#4C737373");
                limiteColor2.Color = c;
                c = (Color)ColorConverter.ConvertFromString("#02FFFFFF");
                limiteColor3.Color = c;
            }

        }

        public void SetBorderCorners(CornerRadius cn) {
            borde.CornerRadius = cn;
        }
        public void SetWidth(int Width) {
            widthUserControl = Width;
        }

        public void SetSombraVisible(bool flag) {
            if (!flag) {
                Sombra.Visibility = Visibility.Hidden;
            } else {
                Sombra.Visibility = Visibility.Visible;
            }
        }
    }
}
