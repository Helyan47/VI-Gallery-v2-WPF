using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProyectoWPF.Components {
    public class VIGalleryButton : Button{
        static VIGalleryButton() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VIGalleryButton),
               new FrameworkPropertyMetadata(typeof(VIGalleryButton)));
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string),
                            typeof(VIGalleryButton), new PropertyMetadata(string.Empty));

        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Color BackColorOver {
            get { return (Color)GetValue(BackColorOverProperty); }
            set { SetValue(BackColorOverProperty, value); }
        }
        public static readonly DependencyProperty BackColorOverProperty =
        DependencyProperty.Register("BackColorOver", typeof(Color), typeof(VIGalleryButton), new PropertyMetadata((sender, args) => {

        }));

        public Color BackColor {
            get { return (Color)GetValue(BackColorProperty); }
            set {
                SetValue(BackColorProperty, value);
                this.Background = new SolidColorBrush(value);
            }
        }

        public static readonly DependencyProperty BackColorProperty =
        DependencyProperty.Register("BackColor", typeof(Color), typeof(VIGalleryButton), new PropertyMetadata((sender, args) => {

        }));

        public Color ForColorOver {
            get { return (Color)GetValue(ForColorOverProperty); }
            set { SetValue(ForColorOverProperty, value); }
        }

        public static readonly DependencyProperty ForColorOverProperty =
        DependencyProperty.Register("ForColorOver", typeof(Color), typeof(VIGalleryButton), new PropertyMetadata((sender, args) => {

        }));

        public Color ForColor {
            get { return (Color)GetValue(ForColorProperty); }
            set {
                SetValue(ForColorProperty, value);
                this.Foreground = new SolidColorBrush(value);
            }
        }

        public static readonly DependencyProperty ForColorProperty =
        DependencyProperty.Register("ForColor", typeof(Color), typeof(VIGalleryButton), new PropertyMetadata((sender, args) => {

        }));

        public static readonly DependencyProperty PathDataProperty = DependencyProperty.Register(nameof(PathData), typeof(Geometry),
                   typeof(VIGalleryButton), new PropertyMetadata(Geometry.Empty));



        public Geometry PathData {
            get { return (Geometry)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }

        protected override void OnMouseEnter(MouseEventArgs e) {
            base.OnMouseEnter(e);
            this.Foreground = new SolidColorBrush(ForColorOver);
            this.Background = new SolidColorBrush(BackColorOver);
        }

        protected override void OnMouseLeave(MouseEventArgs e) {
            base.OnMouseLeave(e);
            this.Foreground = new SolidColorBrush(ForColor);
            this.Background = new SolidColorBrush(BackColor);
        }
    }
}
