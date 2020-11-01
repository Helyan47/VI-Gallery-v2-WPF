using ProyectoWPF.Data;
using ProyectoWPF.NewFolders;
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
    /// Lógica de interacción para GenderSelection.xaml
    /// </summary>
    public partial class GenderSelection : UserControl {

        private Dictionary<string, bool> genders;
        private List<GenderCheck> checkGroup;
        private Dictionary<string, bool> gendersFiltered;
        private Dictionary<string, bool> gendersSelected;
        private string rutaFolder = "";
        private long genderMode = -1;
        public ActionPanel _actionPanel { get; set; }
        public GenderSelection() {
            InitializeComponent();
            loadGenders();
            bAddGender.addButtonEvent(addGenderWindow);
        }

        public void loadGenders() {
            genders = new Dictionary<string, bool>();
            checkGroup = new List<GenderCheck>();
            if (genderMode == ActionPanel.NEW_FOLDER_GENDER_MODE) {
                genders = Conexion.loadAllGenders();
            } else if (genderMode == ActionPanel.MODIFY_FOLDER_MODE) {
                if(gendersFiltered != null) {
                    genders = Conexion.loadAllGenders();
                } else {
                    genders = Conexion.loadGenders(rutaFolder);
                }
                
            } else if (genderMode == ActionPanel.FILTER_MODE) {
                genders = Conexion.loadAllGenders();
            }
            
            if (genders != null && genders.Count > 0) {
                foreach (KeyValuePair<string, bool> gender in genders) {
                    GenderCheck gb = new GenderCheck(gender.Key, gender.Value);
                    gb.Margin = new Thickness(5);
                    if(genderMode == ActionPanel.FILTER_MODE) {
                        foreach(KeyValuePair<string, bool> gen in gendersFiltered) {
                            if (gen.Key.Equals(gender.Key)) {
                                gb.changeSelection(true);
                            }
                        }
                    }else if((genderMode == ActionPanel.NEW_FOLDER_GENDER_MODE || genderMode == ActionPanel.MODIFY_FOLDER_MODE) && (gendersFiltered != null)) {
                        foreach (KeyValuePair<string, bool> gen in gendersFiltered) {
                            if (gen.Key.Equals(gender.Key)) {
                                gb.changeSelection(true);
                            }
                        }
                    }
                    checkGroup.Add(gb);
                }
                reloadButtons(checkGroup);
            }
        }

        private void addGenderWindow(object sender, EventArgs e) {
            AddGender ag = new AddGender();
            ag.ShowDialog();
            if (ag.isAdded() == true) {
                loadGenders();
            }
        }

        private void reloadButtons(List<GenderCheck> genderButtons) {
            wrapPanel.Children.Clear();
            if(genderButtons != null) {
                foreach(GenderCheck gb in genderButtons) {
                    if(gb != null) {
                        //gb.getButtonClick().Click += changeSelection;
                        wrapPanel.Children.Add(gb);
                    }
                }
                checkGroup = genderButtons;
            }
        }

        private void changeSelection(object sender, EventArgs e) {
            Button button = (Button)sender;
            GenderCheck gb = getButtonByTag(button.Tag.ToString());
            if (gb != null) {
                if (gb.isSelected() == true) {
                    gb.changeSelection(false);
                } else if(gb.isSelected() == false){
                    gb.changeSelection(true);
                }
            }
            
        }

        private GenderCheck getButtonByTag(string name) {
            foreach(GenderCheck gb in checkGroup) {
                if (gb.getText().Equals(name)) {
                    return gb;
                }
            }
            return null;
        }

        public void setMode(long mode, string rutaCarpeta, Dictionary<string,bool> filteredGenders) {
            if (mode == ActionPanel.NEW_FOLDER_GENDER_MODE) {
                this.genderMode = mode;
                this.gendersFiltered = filteredGenders;
                bAccept.Content = "Accept";
            } else if (mode == ActionPanel.MODIFY_FOLDER_MODE) {
                this.genderMode = mode;
                this.rutaFolder = rutaCarpeta;
                bAccept.Content = "Accept";
            } else if (mode == ActionPanel.FILTER_MODE) {
                this.genderMode = mode;
                this.gendersFiltered = filteredGenders;
                bAccept.Text = "Filter";
            }
        }

        public Dictionary<string,bool> getGendersSelected() {
            gendersSelected = new Dictionary<string, bool>();
            foreach(GenderCheck gb in checkGroup) {
                if (gb.isSelected()) {
                    gendersSelected.Add(gb.getText(), gb.isSelected());
                }
                
            }
            return gendersSelected;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e) {
            bAccept.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            bAccept.Background = new SolidColorBrush(Colors.White);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e) {
            bAccept.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bAccept.Background = new SolidColorBrush(Color.FromRgb(23, 23, 23));
        }

        public void clearData() {
            gendersSelected = null;
            wrapPanel.Children.Clear();
        }

        public void addAcceptButtonEvent(RoutedEventHandler e) {
            bAccept.Click += e;
        }

        public void addCancelButtonEvent(RoutedEventHandler e) {
            bCancel.Click += e;
        }

        private void bClearValues_Click(object sender, RoutedEventArgs e) {
            foreach(GenderCheck gc in checkGroup) {
                gc.changeSelection(false);
            }
        }

        private void bCancel_Click(object sender, RoutedEventArgs e) {
            if (addGender.Visibility == Visibility.Hidden && this.genderMode == ActionPanel.NEW_FOLDER_GENDER_MODE) {
                Metodos.notifyNewGendersSelected();
            } else if (addGender.Visibility == Visibility.Hidden && this.genderMode == ActionPanel.FILTER_MODE) {
                Metodos.notifyCanceled();
            }
        }

        private void bAccept_Click(object sender, RoutedEventArgs e) {
            if (addGender.Visibility == Visibility.Hidden && this.genderMode == ActionPanel.NEW_FOLDER_GENDER_MODE) {
                Metodos.notifyNewGendersSelected();
            } else if (addGender.Visibility == Visibility.Hidden && this.genderMode == ActionPanel.FILTER_MODE) {
                Metodos.notifyGenderFilter();
            }
        }
    }
}
