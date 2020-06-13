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

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para InputTextLoL.xaml
    /// </summary>
    public partial class InputTextLoL : UserControl {

        private string mode = "username";
        private string actualColor = "white";
        private string actualText = "username";
        private InputTextLoL otherInput = null;
        public bool hasFocus { get; set; }

        public InputTextLoL() {
            InitializeComponent();
        }

        public void lostFocus(bool flag) {
            if (getText().CompareTo("") != 0) {
                if (actualColor.CompareTo("white") == 0) {
                    userText.Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153));
                } else if (actualColor.CompareTo("black") == 0) {
                    userText.Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                }

                bordePrincipal.BorderBrush = Brushes.Transparent;
                if (flag == true) {
                    Keyboard.ClearFocus();
                }

            } else {
                bordePrincipal.BorderBrush = Brushes.Transparent;
                mouseLostGrid.SetValue(Grid.RowProperty, 0);
                mouseEnterGrid.SetValue(Grid.RowProperty, 1);
                gridParent.Focus();
            }
            if (actualColor.CompareTo("white") == 0) {
                bordePrincipal.Background = new SolidColorBrush(Color.FromRgb(237, 237, 237));
                bordePrincipal.BorderBrush = new SolidColorBrush(Color.FromRgb(237, 237, 237));
            } else if (actualColor.CompareTo("black") == 0) {
                bordePrincipal.Background = new SolidColorBrush(Color.FromRgb(27, 27, 27));
                bordePrincipal.BorderBrush = new SolidColorBrush(Color.FromRgb(27, 27, 27));
            }
            hasFocus = false;
            invButtonFocus.Visibility = Visibility.Visible;
        }

        public void gotFocus() {
            invertColors(actualColor);
            mouseLostGrid.SetValue(Grid.RowProperty, 1);
            mouseEnterGrid.SetValue(Grid.RowProperty, 0);
            if (actualColor.CompareTo("white") == 0) {
                bordePrincipal.Background = new SolidColorBrush(Color.FromRgb(237, 237, 237));
                bordePrincipal.BorderBrush = new SolidColorBrush(Color.FromRgb(27, 27, 27));
            } else if (actualColor.CompareTo("black") == 0) {
                bordePrincipal.Background = new SolidColorBrush(Color.FromRgb(27, 27, 27));
                bordePrincipal.BorderBrush = new SolidColorBrush(Color.FromRgb(237, 237, 237));
            }
            if (username.Visibility == Visibility.Visible) {
                if (!username.IsFocused) {
                    username.Focus();
                }

            } else if (username.Visibility == Visibility.Hidden) {
                if (!password.IsFocused) {
                    password.Focus();
                }

            }
            hasFocus = true;
            invButtonFocus.Visibility = Visibility.Hidden;
        }

        public void changeMode(string mode) {
            if (mode.CompareTo("username") == 0) {
                this.mode = mode;
                password.Visibility = Visibility.Hidden;
                username.Visibility = Visibility.Visible;
                Grid.SetColumnSpan(username, 2);
                hidePass.Visibility = Visibility.Hidden;
                showPass.Visibility = Visibility.Hidden;
                invButton.Visibility = Visibility.Hidden;
                userText.Content = "USERNAME";
                startText.Content = "USERNAME";
                actualText = "username";
            } else if (mode.CompareTo("password") == 0) {
                this.mode = mode;
                password.Visibility = Visibility.Visible;
                username.Visibility = Visibility.Hidden;
                Grid.SetColumnSpan(username, 1);
                username.SetValue(Grid.RowSpanProperty, 1);
                hidePass.Visibility = Visibility.Hidden;
                showPass.Visibility = Visibility.Visible;
                invButton.Visibility = Visibility.Visible;
                userText.Content = "PASSWORD";
                startText.Content = "PASSWORD";
                actualText = "password";
            }

        }

        public string getText() {
            if (actualText.CompareTo("username") == 0) {
                return username.Text.ToString();
            } else {
                return password.Password.ToString();
            }
        }

        public void setError() {
            bordePrincipal.BorderBrush = new SolidColorBrush(Color.FromRgb(224, 16, 16));
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Escape) {
                lostFocus(true);
            }
        }

        private void username_GotFocus(object sender, RoutedEventArgs e) {
            gotFocus();
        }

        private void showPass_MouseUp(object sender, MouseButtonEventArgs e) {
            username.Visibility = Visibility.Visible;
            password.Visibility = Visibility.Hidden;
            hidePass.Visibility = Visibility.Visible;
            showPass.Visibility = Visibility.Hidden;
            username.Text = password.Password.ToString();

        }

        private void hidePass_MouseUp(object sender, MouseButtonEventArgs e) {
            username.Visibility = Visibility.Hidden;
            password.Visibility = Visibility.Visible;
            hidePass.Visibility = Visibility.Hidden;
            showPass.Visibility = Visibility.Visible;
            password.Password = username.Text;

        }

        private void hideOrShow(object sender, EventArgs e) {
            if (username.Visibility == Visibility.Visible) {
                username.Visibility = Visibility.Hidden;
                password.Visibility = Visibility.Visible;
                hidePass.Visibility = Visibility.Hidden;
                showPass.Visibility = Visibility.Visible;
                password.Password = username.Text;
                actualText = "password";
                password.Focus();
            } else if (username.Visibility == Visibility.Hidden) {
                username.Visibility = Visibility.Visible;
                password.Visibility = Visibility.Hidden;
                hidePass.Visibility = Visibility.Visible;
                showPass.Visibility = Visibility.Hidden;
                username.Text = password.Password.ToString();
                actualText = "username";
                username.Focus();
                username.CaretIndex = username.Text.Length;
            }
        }

        public void invertColors(string s) {
            if (s.CompareTo("black") == 0) {
                userText.Foreground = new SolidColorBrush(Color.FromRgb(237, 237, 237));
                bordePrincipal.BorderBrush = new SolidColorBrush(Color.FromRgb(237, 237, 237));
                bordePrincipal.Background = new SolidColorBrush(Color.FromRgb(27, 27, 27));

                actualColor = s;
                username.Foreground = new SolidColorBrush(Color.FromRgb(237, 237, 237));
                password.Foreground = new SolidColorBrush(Color.FromRgb(237, 237, 237));
                username.CaretBrush = new SolidColorBrush(Color.FromRgb(237, 237, 237));
                password.CaretBrush = new SolidColorBrush(Color.FromRgb(237, 237, 237));

                ColorPath.Fill = new SolidColorBrush(Color.FromRgb(237, 237, 237));
                ColorPath2.Fill = new SolidColorBrush(Color.FromRgb(237, 237, 237));
            } else if (s.CompareTo("white") == 0) {
                userText.Foreground = new SolidColorBrush(Color.FromRgb(27, 27, 27));
                bordePrincipal.BorderBrush = new SolidColorBrush(Color.FromRgb(27, 27, 27));
                bordePrincipal.Background = new SolidColorBrush(Color.FromRgb(237, 237, 237));

                actualColor = s;
                username.Foreground = new SolidColorBrush(Color.FromRgb(27, 27, 27));
                password.Foreground = new SolidColorBrush(Color.FromRgb(27, 27, 27));
                username.CaretBrush = new SolidColorBrush(Color.FromRgb(27, 27, 27));
                password.CaretBrush = new SolidColorBrush(Color.FromRgb(27, 27, 27));

                ColorPath.Fill = new SolidColorBrush(Color.FromRgb(27, 27, 27));
                ColorPath2.Fill = new SolidColorBrush(Color.FromRgb(27, 27, 27));
            }
        }

        public PasswordBox getPaswwordBox() {
            return password;
        }

        public TextBox getUsernameInput() {
            return username;
        }

        public Button getInvButton() {
            return invButtonFocus;
        }

        public void setOtherInput(InputTextLoL input) {
            otherInput = input;
        }
    }
}
