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
    /// Lógica de interacción para ActionPanel.xaml
    /// </summary>
    public partial class ActionPanel : UserControl {
        public const long NEW_FOLDER_GENDER_MODE = 0L;
        public const long MODIFY_FOLDER_MODE = 1L;
        public const long MODIFY_FILE_MODE = 2L;
        public const long FILTER_MODE = 3L;
        public const long NEW_SUBFOLDER = 4L;
        public const long NEW_FILE = 5L;
        public const long NEW_MENU = 6L;
        public const long NEW_MULTI_FOLDER = 7L;
        private static long actionMode = -1;
        public ActionPanel() {
            InitializeComponent();
        }

        public void setMode(long mode, string rutaCarpeta, Dictionary<string, bool> filteredGenders) {
            if (mode == NEW_FOLDER_GENDER_MODE) {
                actionMode = mode;
                genderSelection.setMode(mode, rutaCarpeta, filteredGenders);
                filaDerecha.Height = new GridLength(0, GridUnitType.Star);
                filaIzquierda.Height = new GridLength(0, GridUnitType.Star);
                columnaSuperior.Width = new GridLength(0, GridUnitType.Star);
                columnaInferior.Width = new GridLength(0, GridUnitType.Star);
            } else if (mode == MODIFY_FOLDER_MODE) {
                actionMode = mode;
                genderSelection.setMode(mode,rutaCarpeta,filteredGenders);
                filaDerecha.Height = new GridLength(0, GridUnitType.Star);
                filaIzquierda.Height = new GridLength(0, GridUnitType.Star);
                columnaSuperior.Width = new GridLength(0, GridUnitType.Star);
                columnaInferior.Width = new GridLength(0, GridUnitType.Star);
            } else if (mode == MODIFY_FILE_MODE) {
                actionMode = mode;
            } else if (mode == FILTER_MODE) {
                actionMode = mode;
                genderSelection.setMode(mode, rutaCarpeta, filteredGenders);
                filaDerecha.Height = new GridLength(1, GridUnitType.Star);
                filaIzquierda.Height = new GridLength(1, GridUnitType.Star);
                columnaSuperior.Width = new GridLength(1, GridUnitType.Star);
                columnaInferior.Width = new GridLength(1, GridUnitType.Star);
            } else if (mode == NEW_SUBFOLDER) {
                actionMode = mode;
            } else if (mode == NEW_FILE) {
                actionMode = mode;
            } else if (mode == NEW_MENU) {
                actionMode = mode;
            } else if (mode == NEW_MULTI_FOLDER) {
                actionMode = mode;
            }
        }

        public GenderSelection getGenderSelection() {
            return genderSelection;
        }
    }

}
