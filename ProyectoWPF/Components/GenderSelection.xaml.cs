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
            bAddGender.getButton().Click += addGenderWindow;
        }

        public void loadGenders() {
            genders = new Dictionary<string, bool>();
            checkGroup = new List<GenderCheck>();
            if (genderMode == 0L) {
                genders = Conexion.loadAllGenders(false);
            } else if (genderMode == 1L) {
                genders = Conexion.loadGenders(false, rutaFolder);
            } else if (genderMode == 3L) {
                genders = Conexion.loadAllGenders(false);
            }
            
            if (genders != null && genders.Count > 0) {
                foreach (KeyValuePair<string, bool> gender in genders) {
                    GenderCheck gb = new GenderCheck(gender.Key, gender.Value);
                    gb.Margin = new Thickness(5);
                    if(genderMode == 2) {
                        foreach(KeyValuePair<string, bool> gen in gendersFiltered) {
                            if (gen.Equals(gender.Key)) {
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
            } else if (mode == ActionPanel.MODIFY_FOLDER_MODE) {
                this.genderMode = mode;
                this.rutaFolder = rutaCarpeta;
            } else if (mode == ActionPanel.FILTER_MODE) {
                this.genderMode = mode;
                this.gendersFiltered = filteredGenders;
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

        public void clear() {
            gendersSelected = null;
            wrapPanel.Children.Clear();
        }

        public Button getAcceptButton() {
            return bAccept.getButton();
        }
    }
}
