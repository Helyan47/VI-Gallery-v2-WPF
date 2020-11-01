﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ProyectoWPF.NewFolders;
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
        public const long LOADING_CONTROL = 8L;
        private static long actionMode = -1;
        
        private bool closed = false;
        public ActionPanel() {
            InitializeComponent();
            genderSelection._actionPanel = this;
            newFolder._actionPanel = this;
        }

        public void setMode(long mode, string rutaCarpeta, Dictionary<string, bool> filteredGenders, Carpeta c) {
            hideAll();
            if (mode == NEW_FOLDER_GENDER_MODE) {
                actionMode = mode;
                newFolder.Visibility = Visibility.Visible;
                
                filaDerecha.Height = new GridLength(0.5, GridUnitType.Star);
                filaIzquierda.Height = new GridLength(0.5, GridUnitType.Star);
                columnaSuperior.Width = new GridLength(1, GridUnitType.Star);
                columnaInferior.Width = new GridLength(1, GridUnitType.Star);
            } else if (mode == MODIFY_FOLDER_MODE) {
                actionMode = mode;
                updateFolder.Visibility = Visibility.Visible;
                updateFolder.setCarpeta(c);
                
                filaDerecha.Height = new GridLength(0.5, GridUnitType.Star);
                filaIzquierda.Height = new GridLength(0.5, GridUnitType.Star);
                columnaSuperior.Width = new GridLength(1, GridUnitType.Star);
                columnaInferior.Width = new GridLength(1, GridUnitType.Star);
            } else if (mode == MODIFY_FILE_MODE) {
                actionMode = mode;
            } else if (mode == FILTER_MODE) {
                actionMode = mode;
                genderSelection.setMode(mode, rutaCarpeta, filteredGenders);
                genderSelection.Visibility = Visibility.Visible;
                
                
            } else if (mode == NEW_SUBFOLDER) {
                actionMode = mode;
            } else if (mode == NEW_FILE) {
                actionMode = mode;
            } else if (mode == NEW_MENU) {
                actionMode = mode;
            } else if (mode == NEW_MULTI_FOLDER) {
                actionMode = mode;
            } else if (mode == LOADING_CONTROL) {
                actionMode = mode;
                filaDerecha.Height = new GridLength(1.5, GridUnitType.Star);
                filaIzquierda.Height = new GridLength(1.5, GridUnitType.Star);
                columnaSuperior.Width = new GridLength(3, GridUnitType.Star);
                columnaInferior.Width = new GridLength(3, GridUnitType.Star);
            }
        }

        public GenderSelection getGenderSelection() {
            return genderSelection;
        }

        public NewFolder getNewFolder() {
            return newFolder;
        }

        public void hideAll() {
            genderSelection.Visibility = Visibility.Hidden;
            newFolder.Visibility = Visibility.Hidden;
            updateFolder.Visibility = Visibility.Hidden;
        }

        public void close() {
            closed = true;
        }

        public bool isClosed() {
            return closed;
        }

        public void clearData() {
            newFolder.clearData();
            updateFolder.clearData();
        }
    }

}
