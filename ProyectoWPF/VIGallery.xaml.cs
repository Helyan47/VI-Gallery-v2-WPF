using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using ProyectoWPF.NewFolders;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System.Net;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using ProyectoWPF.Data;
using ProyectoWPF.Components;
using ProyectoWPF.Components.Online;
using ProyectoWPF.Reproductor;
using System.Collections.ObjectModel;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para VIGallery.xaml
    /// </summary>
    public partial class VIGallery : Window {

        private ICollection<WrapPanelPrincipal> _wrapsPrincipales;
        private ICollection<ComboBoxItem> _botonesMenu;
        string[] _folders;
        private Carpeta _aux;
        private Carpeta _aux2;
        List<string> _rutas = new List<string>();
        private ItemCollection _botones;
        public static ComboBoxItem _activatedButton;
        public int firstFolder = 0;
        public static PerfilClass _profile = null;
        private static PerfilClass _newSelectedProfile = null;
        public static bool changedProfile = false;
        public static UsuarioClass _user { get; set; }
        public static bool isPrivateMode = false;
        private Dictionary<string, bool> filteredGenders = new Dictionary<string, bool>();
        public VIGallery(PerfilClass profile) {
            InitializeComponent();
            _profile = profile;
            Lista.clearListas();
            _botones = menu.Items;
            _newSelectedProfile = _profile;
            Metodos.setVIGallery(this);
            _botonesMenu = new List<ComboBoxItem>();
            changedProfile = false;
            _wrapsPrincipales = new List<WrapPanelPrincipal>();
            //menuReciente.setMain(this);
            reproductorControl.setVIGallery(this);
            actionPanel.getGenderSelection().getAcceptButton().Click += notifyGenderFilter;
        }

        /**
        * Evento que se lanza al pulsar alguno de los botones del menu
        * Oculta los paneles de cada menu y muestra el seleccionado
        */
        public void onClickButtonMenu(object sender,EventArgs e) {
            try {
                ComboBox b = (ComboBox)sender;
                ComboBoxItem selected = (ComboBoxItem)b.SelectedItem;
                if (selected != null) {
                    MenuClass mc = Lista.getMenuFromText(selected.Content.ToString());
                    WrapPanelPrincipal wp = Lista.getWrapVisible();
                    clearTextBoxAndSelection();
                    if (Lista.buttonInButtons(mc)) {
                        Lista.hideAll();
                        menuCarpetas.Visibility = Visibility.Hidden;
                        GridSecundario.SetValue(Grid.RowProperty, 1);
                        GridPrincipal.SetValue(Grid.RowProperty, 0);
                        Lista.showWrapFromMenu(mc);
                    }

                    _activatedButton = selected;
                    menu.SelectedItem = selected;
                    Return.Visibility = Visibility.Hidden;
                    borderEnter.Visibility = Visibility.Hidden;
                }
            } catch (Exception ) {
                ComboBoxItem selected = (ComboBoxItem)sender;
                if (selected != null) {
                    MenuClass mc = Lista.getMenuFromText(selected.Content.ToString());
                    WrapPanelPrincipal wp = Lista.getWrapVisible();
                    clearTextBoxAndSelection();
                    if (Lista.buttonInButtons(mc)) {
                        Lista.hideAll();
                        menuCarpetas.Visibility = Visibility.Hidden;
                        GridSecundario.SetValue(Grid.RowProperty, 1);
                        GridPrincipal.SetValue(Grid.RowProperty, 0);
                        Lista.showWrapFromMenu(mc);

                    }

                    _activatedButton = selected;
                    menu.SelectedItem = selected;
                    Return.Visibility = Visibility.Hidden;
                    borderEnter.Visibility = Visibility.Hidden;
                }
            }
            
            
        }

        /**
         * Evento que se lanza al ir a crear una subcarpeta
         */
        private void NewTemp_Click(object sender, EventArgs e) {
            addSubCarpeta();
        }

        /**
         * Muestra un explorador de carpetas que permite añadir varias carpetas a la aplicacion
         */
        private void Button_MouseLeftButtonUp(object sender, RoutedEventArgs e) {
            try {
                if (_activatedButton != null) {
                    string[] files = new string[0];
                    using (var folderDialog = new CommonOpenFileDialog()) {

                        folderDialog.IsFolderPicker = true;
                        firstFolder = 0;
                        if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok && !string.IsNullOrWhiteSpace(folderDialog.FileName)) {
                            clearTextBoxAndSelection();
                            Dispatcher.Invoke(new Action(() => {
                                _folders = OrderClass.orderArrayOfString(Directory.GetDirectories(folderDialog.FileName));
                                for (int i = 0; i < _folders.Length; i++) {
                                    _rutas.Add(_folders[i]);

                                    string[] aux = Directory.GetDirectories(_folders[i]);
                                    for (int j = 0; j < aux.Length; j++) {
                                        _rutas.Add(aux[j]);
                                    }
                                }
                            }));

                            Dispatcher.Invoke(new Action(() => {
                                if (_folders != null) {
                                    addText(_folders);
                                }
                            }));

                        }
                    }
                    Dispatcher.Invoke(new Action(() => {
                        Lista.modifyMode(_profile.mode);
                        Lista.orderWrap(menuCarpetas.getWrap());
                        WrapPanelPrincipal wp = Lista.getWrapVisible();
                        if (wp != null) {
                            Lista.orderWrap(wp);
                        }
                        Lista.hideAllExceptPrinc();
                        ReturnVisibility(false);
                    }));

                } else {
                    MessageBox.Show("No has creado ningún menú");
                }
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            } catch (SQLiteException exc2) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }

        }

        /**
         * Aáde una subcarpeta con el nombre que se la asigne
         */
        private void addSubCarpeta() {

            try {
                Carpeta padre = menuCarpetas.getCarpeta();
                Carpeta c = new Carpeta(this, Lista.getWrapVisible(), menuCarpetas, padre);
                NewSubCarpeta n = null;
                n = new NewSubCarpeta(padre.getClass().ruta);

                n.ShowDialog();
                if (n.getNombre() != "") {
                    clearTextBoxAndSelection();

                    CarpetaClass s = new CarpetaClass(n.getNombre(), "", true);
                    c.setClass(s);
                    c.getClass().idMenu = Lista.getMenuFromText(_activatedButton.Content.ToString()).id;
                    c.getClass().img = padre.getClass().img;
                    c.getClass().rutaPadre = padre.getClass().ruta;
                    c.setRutaPrograma(padre.getClass().ruta + "/" + c.getClass().nombre);
                    padre.addCarpetaHijo(c);
                    Lista.addCarpeta(c);
                    Conexion.saveSubFolder(c);

                    c.actualizar();
                    menuCarpetas.actualizar(padre);
                    c.Visibility = Visibility.Visible;
                    Lista.orderWrap(menuCarpetas.getWrap());




                } else {
                    c = null;
                }
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            } catch (SQLiteException exc2) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
            
        }


        /**
         * Añade un archivo a la carpeta que se le pase por argumentos
         */
        private void addFileCarpeta(string fileName, Carpeta c) {
            try {
                string ruta = _profile.nombre + "|F" + c.getClass().ruta.Split('|')[1].Substring(1) + "/" + System.IO.Path.GetFileName(fileName);
                ArchivoClass ac = new ArchivoClass(System.IO.Path.GetFileNameWithoutExtension(fileName), fileName, ruta, c.getClass().img, c.getClass().id);
                Archivo a = new Archivo(ac, this, null);
                clearTextBoxAndSelection();
                a.setCarpetaPadre(c);
                Conexion.saveFile(ac);
                c.addFile(a);
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            } catch (SQLiteException exc2) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
        }

        /**
         * Muestra un explorador de archivos que permite añadir varias archivos de video a una carpeta
         */
        private void newFile_Click(object sender, RoutedEventArgs e) {
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = "Video Files (*.mp4, *.avi, *.mkv, *.mpeg, *.wmv, *.flv, *.mov, *.wav | *.mp4; *.avi; *.mkv; *.mpeg; *.wmv; *.flv; *.mov; *.wav;";

            fileDialog.CheckFileExists = true;
            fileDialog.CheckPathExists = true;


            if (fileDialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(fileDialog.FileName)) {
                clearTextBoxAndSelection();
                List<string> _files = OrderClass.orderListOfString(fileDialog.FileNames.ToList());
                Carpeta c = menuCarpetas.getCarpeta();
                bool? flag = null;
                if (c != null) {
                    flag = true;
                }


                foreach (string file in _files) {
                    foreach (string s in Lista._extensiones) {
                        if (s.CompareTo(System.IO.Path.GetExtension(file)) == 0) {
                            if (flag == true) {
                                addFileCarpeta(file, c);
                                menuCarpetas.actualizar(c);
                            } else {
                                MessageBox.Show("No se ha podido añadir el archivo");
                            }

                        }
                    }
                    
                }
                if (flag == true) {
                    Lista.orderWrap(menuCarpetas.getWrap());
                }
                


            }
        }

        /**
         * Añade una carpeta de las multiples carpetas seleccionadas
         */
        private Carpeta addCarpetaCompleta(string filename) {
            try {
                Carpeta p1 = new Carpeta(this, Lista.getWrapVisible(), menuCarpetas, null);

                CarpetaClass s = new CarpetaClass(System.IO.Path.GetFileName(filename), "", true);
                p1.setClass(s);
                s.idMenu = Lista.getMenuFromText(_activatedButton.Content.ToString()).id;
                s.rutaPadre = "";
                p1.actualizar();

                string name = _activatedButton.Content.ToString();
                p1.getClass().rutaPadre = _profile.nombre + "|C/" + name;
                p1.setRutaPrograma(_profile.nombre + "|C/" + name + "/" + p1.getClass().nombre);
                bool checkIfExists = Lista.Contains(p1.getClass().ruta);
                if (!checkIfExists) {
                    Lista.addCarpeta(p1);

                    string[] files = System.IO.Directory.GetFiles(filename, "cover.*");
                    if (files.Length > 0) {
                        p1.getClass().img = files[0];
                    }

                    p1._primerPanel.addCarpeta(p1);

                    Conexion.saveFolder(p1);

                    p1.setRutaDirectorio(filename);

                    p1.SetGridsOpciones(GridPrincipal, GridSecundario);
                } else {
                    p1 = null;
                    s = null;
                }
                return p1;
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            } catch (SQLiteException exc2) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
            return null;
        }

        private Carpeta addSubCarpetaCompleta(Carpeta c, string filename) {
            try {
                Carpeta p1 = new Carpeta(this, Lista.getWrapVisible(), menuCarpetas, c);

                CarpetaClass s = new CarpetaClass(System.IO.Path.GetFileName(filename), "", false);
                p1.setClass(s);
                s.idMenu = Lista.getMenuFromText(_activatedButton.Content.ToString()).id;
                s.rutaPadre = "";
                p1.actualizar();

                string name = _activatedButton.Content.ToString();
                p1.getClass().rutaPadre = c.getClass().ruta;
                p1.setRutaPrograma(c.getClass().ruta + "/" + p1.getClass().nombre);
                bool checkIfExists = Lista.Contains(p1.getClass().ruta);
                if (!checkIfExists) {
                    Lista.addCarpeta(p1);

                    string[] files = System.IO.Directory.GetFiles(filename, "cover.*");
                    if (files.Length > 0) {
                        p1.getClass().img = files[0];
                    } else {
                        p1.getClass().img = c.getClass().img;
                    }

                    Conexion.saveFolder(p1);

                    p1.setRutaDirectorio(filename);

                    p1.SetGridsOpciones(GridPrincipal, GridSecundario);
                    c.addCarpetaHijo(p1);
                } else {
                    p1 = null;
                    s = null;
                }
                return p1;
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
            return null;
        }

        /**
         * Evento llamado al añadir varias carpetas a la aplicacion
         * Recorre todas las carpetas hijo de la carpeta seleccionada, asi como sus archivos
         * Recorre todo el arbol de carpetas hasta que no haya mas por leer
         */
        private void addText(string[] files) {

            for (int i = 0; i < files.Length; i++) {
                if (files == _folders) {
                    _aux = addCarpetaCompleta(files[i]);
                    if (_aux == null) {
                        if (i != files.Length - 1) {
                            i++;
                        }

                    } else {
                        _aux.clickEspecial();
                        string[] archivos = OrderClass.orderArrayOfString(Directory.GetFiles(files[i]));
                        for (int j = 0; j < archivos.Length; j++) {
                            foreach (string s in Lista._extensiones) {
                                if (s.CompareTo(System.IO.Path.GetExtension(archivos[j])) == 0) {
                                    addFileCarpeta(archivos[j], _aux);
                                }
                            }
                        }
                    }


                } else {
                    _aux2 = Lista.searchRuta(Directory.GetParent(files[i]).FullName);
                    if (!checkString(files[i])) {
                        if (_aux2 != null) {
                            _aux2 = addSubCarpetaCompleta(_aux2, files[i]);
                        }
                       
                    } else {
                        if (_aux != null) {
                            _aux2 = addSubCarpetaCompleta(_aux, files[i]);
                        }
                    }

                    string[] archivos = OrderClass.orderArrayOfString(Directory.GetFiles(files[i]));
                    if (_aux2 != null) {
                        for (int j = 0; j < archivos.Length; j++) {
                            foreach (string s in Lista._extensiones) {
                                if (s.ToLower().CompareTo(System.IO.Path.GetExtension(archivos[j]).ToLower()) == 0) {
                                    addFileCarpeta(archivos[j], _aux2);
                                    Console.WriteLine("Added: " + archivos[j]);
                                }
                            }
                        }
                    }
                }
                if (Directory.GetDirectories(files[i]) != null) {
                    string[] directorios = OrderClass.orderArrayOfString(Directory.GetDirectories(files[i]));
                    addText(directorios);
                }


            }
        }

        private void Button_AddCarpeta(object sender, EventArgs e) {
            try {
                if (_activatedButton != null) {
                    addCarpeta();

                } else {
                    MessageBox.Show("No has creado ninguno menú");
                }
            } catch (SQLiteException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }

        }



        /**
         * Añade una carpeta al menu
         */
        private void addCarpeta() {
            try {
                
                Carpeta p1 = new Carpeta(this, Lista.getWrapVisible(), menuCarpetas, null);

                AddCarpeta newSerie = new AddCarpeta(p1, _activatedButton);
                newSerie.ShowDialog();

                if (newSerie.createdSerie()) {
                    clearTextBoxAndSelection();
                    Lista.addCarpeta(p1);
                    WrapPanelPrincipal aux = Lista.getWrapVisible();

                    p1.actualizar();

                    string name = _activatedButton.Content.ToString();
                    p1.getClass().rutaPadre = _profile.nombre + "|C/" + name;
                    p1.setRutaPrograma(_profile.nombre + "|C/" + name + "/" + p1.getClass().nombre);
                    
                    Conexion.saveFolder(p1);

                    aux.addCarpeta(p1);
                    p1.SetGridsOpciones(GridPrincipal, GridSecundario);
                    Lista.orderWrap(aux);
                } else {
                    p1 = null;
                }
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            } catch (SQLiteException exc2) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }

        }

        

        /**
         * Añade una carpeta a partir de un registro en la base de datos
         */
        private void addCarpetaFromLoad(CarpetaClass cc) {
            Carpeta p1 = new Carpeta(this, Lista.getWrapVisible(), menuCarpetas, null);
            Lista.addCarpeta(p1);

            WrapPanelPrincipal aux = Lista.getWrapVisible();
            p1.setClass(cc);
            p1.actualizar();

            aux.addCarpeta(p1);

            p1.SetGridsOpciones(GridPrincipal, GridSecundario);

            p1.clickEspecial();
        }

        /**
         * Añade una subcarpeta a partir de un registro en la base de datos
         */
        private void addSubCarpetaFromLoad(CarpetaClass cc, Carpeta padre) {
            Carpeta c= new Carpeta(this, Lista.getWrapVisible(), menuCarpetas, padre);
            padre.addCarpetaHijo(c);
            Lista.addCarpeta(c);

            c.setClass(cc);

            c.SetGridsOpciones(GridPrincipal, GridSecundario);

            c.actualizar();

            c.Visibility = Visibility.Visible;
        }

        /**
         * Controla la opcion de volver atras en las subcarpetas
         */
        private void Return_MouseLeftButtonUp(object sender, EventArgs e) {
            Carpeta p1 = menuCarpetas.getCarpeta();
            if (p1 != null) {
                p1.clickInverso();
            } else {
                MessageBox.Show("null");
            }
        }

        /**
         * Carga los archivos de la base de datos online
         */
        private void loadFiles(CarpetaClass c) {
            try {
                Carpeta carpeta = Lista.getCarpetaById(c.id);
                List<ArchivoClass> archivos = OrderClass.orderListOfArchivoClass(Conexion.loadFiles(c.id));
                if (archivos != null) {
                    foreach (ArchivoClass ac in archivos) {
                        Archivo a = new Archivo(ac, this, menuCarpetas.getWrap());
                        carpeta.addFile(a);
                        a.setCarpetaPadre(carpeta);
                    }
                }
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
        }

        /**
         * Carga los datos de la base de datos online
         */
        public void loadDataConexion(long id) {
            try {
                List<MenuClass> menus = Conexion.loadMenus(_profile.id);
                if (menus != null) {
                    foreach (MenuClass m in menus) {
                        addMenuFromClass(m);
                        List<CarpetaClass> carpetas = OrderClass.orderListOfCarpetaClass(Conexion.loadFoldersFromMenu(m.id));
                        if (carpetas != null) {
                            foreach (CarpetaClass c in carpetas) {
                                addCarpetaFromLoad(c);
                                loadFiles(c);
                                loadSubCarpetas(c, m.id);
                            }
                        }
                    }
                }
                Lista.orderWrapsPrincipales();
                Lista.modifyMode(_profile.mode);
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
        }

        /**
         * Carga las subcarpetas de una carpeta de la base de datos online
         */
        public void loadSubCarpetas(CarpetaClass c, long idMenu) {
            try {
                Carpeta aux = Lista.getCarpetaById(c.id);
                List<CarpetaClass> carpetas = OrderClass.orderListOfCarpetaClass(Conexion.loadSubFoldersFromCarpeta(c, idMenu));
                if (carpetas != null) {
                    foreach (CarpetaClass cc in carpetas) {
                        addSubCarpetaFromLoad(cc,aux);
                        loadFiles(cc);
                        loadSubCarpetas(cc, idMenu);
                    }
                }
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
        }

        /**
         * Añade un menu a partir de un registro
         */
        public void addMenuFromClass(MenuClass m) {
            ComboBoxItem newButton = new ComboBoxItem();
            newButton.Content = m.nombre;;

            _botonesMenu.Add(newButton);
            Lista.addMenu(m);
            Lista.addButtonMenu(newButton);

            menu.Items.Add(newButton);

            string name = newButton.Content.ToString();
            WrapPanelPrincipal wp = new WrapPanelPrincipal();
            wp.Focusable = false;
            Grid.SetRow(wp, 1);
            wp.name = name;
            gridPrincipal.Children.Add(wp);
            wp.Visibility = Visibility.Visible;
            _activatedButton = newButton;
            _wrapsPrincipales.Add(wp);

            Lista.addWrapPrincipal(wp);
            wp.setButton(newButton);

            onClickButtonMenuEspecial(newButton);
        }

        /**
         * Evento que se añadira a un menu cargado
         */
        public void onClickButtonMenuEspecial(object sender) {
            ComboBoxItem b = (ComboBoxItem)sender;
            MenuClass mc = Lista.getMenuFromText(b.Content.ToString());
            if (Lista.buttonInButtons(mc)) {
                Lista.hideAll();
                GridSecundario.SetValue(Grid.RowProperty, 1);
                GridPrincipal.SetValue(Grid.RowProperty, 0);
                Lista.showWrapFromMenu(mc);

            }
            menu.SelectedItem = b;

            _activatedButton = b;
            Return.Visibility = Visibility.Hidden;
        }

        /**
         * Muestra un panel para agregar un menu a la aplicacion
         */
        private void addMenuClick(object sender, EventArgs e) {
            try {
                ComboBoxItem newButton = new ComboBoxItem();
                newButton.Content = "";
                AddButton a = new AddButton(newButton);
                a.ShowDialog();
                if (a.isAdded()) {
                    clearTextBoxAndSelection();
                    _botonesMenu.Add(newButton);
                    MenuClass mc = new MenuClass(newButton.Content.ToString(), _profile.id);
                    mc = Conexion.saveMenu(mc);
                    if (mc != null) {
                        Lista.addMenu(mc);
                        menu.Items.Add(newButton);
                        string name = newButton.Content.ToString();
                        WrapPanelPrincipal wp = new WrapPanelPrincipal();
                        wp.Focusable = false;
                        wp.name = name;
                        Grid.SetRow(wp, 1);
                        gridPrincipal.Children.Add(wp);
                        wp.Visibility = Visibility.Visible;
                        _activatedButton = newButton;
                        _wrapsPrincipales.Add(wp);

                        Lista.addWrapPrincipal(wp);
                        wp.setButton(newButton);

                        onClickButtonMenu(newButton, e);
                    } else {
                        MessageBox.Show("No se ha podido crear el Menu");
                    }

                }
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }

        }

        /**
         * Borra un menu y todos los elementos que contiene
         */
        private void removeMenu(object sender, EventArgs e) {
            try {
                if (_activatedButton != null) {
                    long id = Lista.getMenuFromText(_activatedButton.Content.ToString()).id;

                    WrapPanelPrincipal wp = Lista.getWrapFromMenu(Lista.getMenuFromText(_activatedButton.Content.ToString()));
                    if (id != 0) {
                        Conexion.deleteMenu(id);
                        Lista.removeMenu(_activatedButton.Content.ToString());

                        if (_botonesMenu.Contains(_activatedButton)) {
                            _botonesMenu.Remove(_activatedButton);
                        }
                        if (_botones.Contains(_activatedButton)) {
                            _botones.Remove(_activatedButton);
                        }
                        if (gridPrincipal.Children.Contains(wp)) {
                            gridPrincipal.Children.Remove(wp);
                        }
                        if (_botonesMenu.Count != 0) {
                            foreach (ComboBoxItem b in _botonesMenu) {
                                onClickButtonMenu(b, e);
                                break;
                            }
                        } else {
                            _activatedButton = null;
                        }
                        ReturnVisibility(false);
                    }

                }
                clearTextBoxAndSelection();
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
        }
        public bool checkString(string s) {
            foreach (string h in _rutas) {
                if (s.Equals(h)) {
                    return true;
                }
            }
            return false;
        }

        /**
         * Muestra o oculta el boton de return
         */
        public void ReturnVisibility(bool flag) {
            if (flag) {
                Return.Visibility = Visibility.Visible;
                borderEnter.Visibility = Visibility.Visible;
            } else {
                Return.Visibility = Visibility.Hidden;
                borderEnter.Visibility = Visibility.Hidden;
                menuCarpetas.Visibility = Visibility.Hidden;
            }
        }

        /**
         * Evento que se lanza al pulsar en la cruz superior derecha que cierra la aplicacion
         */
        public void CerrarApp(object sender, RoutedEventArgs e) {
            this.Close();
        }

        /**
         * Cambia entre el estado maximizado y normal al pulsar en el boton de maximizar
         */
        public void MaximizeApp(object sender, RoutedEventArgs e) {
            if (this.WindowState == WindowState.Normal) {
                this.WindowState = WindowState.Maximized;
            }else if (this.WindowState == WindowState.Maximized) {
                this.WindowState = WindowState.Normal;
                this.Width = 1000;
                this.Height = 700;
            }
            
        }

        /**
         * Minimiza la aplicacion
         */
        public void MinimizeApp(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }

        /**
         * Cambia el modo de las carpetas para que se muestren distinto
         */
        public void ChangeMode(object sender,RoutedEventArgs e) {
            Lista.changeMode();
        }

        /**
         * Actualiza el modo en la base de datos
         */
        public static void updateMode(long mode) {
            try {
                _profile.mode = mode;
                Conexion.updateMode(mode, _profile);
                
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
        }

        /**
         * Muestra el panel de opciones
         */
        private void showOptions(object sender, RoutedEventArgs e) {
            optionPanel.Visibility = Visibility.Visible;
            addProfilesOptions();
        }

        /**
         * Muestra la aplicacion principal
         */
        private void showMain(object sender, RoutedEventArgs e) {
            optionPanel.Visibility = Visibility.Hidden;
        }

        /**
         * Carga los perfiles que tiene el usuario
         */
        private void addProfilesOptions() {
            perfiles.Children.Clear();
            Lista.reloadProfiles();
            foreach (PerfilClass p in Lista.getProfiles()) {
                Button b = new Button();
                b.Content = p.nombre;
                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.VerticalAlignment = VerticalAlignment.Stretch;
                b.VerticalContentAlignment = VerticalAlignment.Center;
                b.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                b.FontSize = 40;
                b.Padding = new Thickness(0);
                b.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                b.Style = Application.Current.Resources["CustomButtonStyle"] as Style;
                b.Click += selectProfile;
                bool added = Lista.addButtonProfile(b);
                if (added) {
                    perfiles.Children.Add(b);
                } else {
                    b = null;
                }
                if (b.Content.ToString().CompareTo(_profile.nombre) == 0) {
                    selectProfile(b);
                }
            }
            
        }

        /**
         * Modifica el background del boton seleccionado
         */
        private void selectProfile(object sender,RoutedEventArgs e) {
            Button aux = (Button)sender;
            PerfilClass perfilSelected = Lista.getProfile(aux.Content.ToString());
            if (perfilSelected != null) {
                if (_newSelectedProfile != null & _newSelectedProfile.nombre.CompareTo(aux.Content.ToString()) != 0) {
                    _newSelectedProfile = perfilSelected;
                    Lista.clearBackProfile();
                    aux.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF595959"));
                }
            }
        }

        /**
         * Modifica el background del perfil seleccionado
         */
        private void selectProfile(Button b) {
            PerfilClass perfilSelected = Lista.getProfile(b.Content.ToString());
            if (perfilSelected != null) {
                Lista.clearBackProfile();
                b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF595959"));
            }
        }

        /**
         * Abre una nueva ventana con el nuevo perfil seleccionado y cierra la actual
         */
        private void changeProfile(object sender, RoutedEventArgs e) {
            if (_profile.nombre.CompareTo(_newSelectedProfile.nombre) != 0) {
                _profile = _newSelectedProfile;
                changedProfile = true;
                VIGallery vi = new VIGallery(_profile);
                
                vi.loadDataConexion(_profile.id);
                
                vi.Show();
                this.Close();
            } else {
                MessageBox.Show("El perfil ya está seleccionado");
            }

        }
        /**
         * Añade un nuevo perfil a la aplicacion
         */
        private void addProfile(object sender, RoutedEventArgs e) {
            NewProfile newProf = new NewProfile();
            newProf.ShowDialog();
            if (newProf.isAdded()) {
                Button b = new Button();
                b.Content = newProf.getName();
                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.VerticalAlignment = VerticalAlignment.Stretch;
                b.VerticalContentAlignment = VerticalAlignment.Center;
                b.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                b.FontSize = 40;
                b.Padding = new Thickness(0);
                b.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                b.Style = Application.Current.Resources["CustomButtonStyle"] as Style;
                b.Click += selectProfile;
                Lista.addButtonProfile(b);
                perfiles.Children.Add(b);
                
                MessageBox.Show("El perfil ha sido creado. Cambia al perfil desde el panel de opciones pulsando \"Cambiar Perfil\"");
                
            } else {
                MessageBox.Show("No se ha podido crear el perfil");
            }
            
        }

        /**
         * Devuelve el usuario
         */
        public static UsuarioClass getUser() {
            return _user;
        }

        /**
         * Borra el perfil seleccionado
         */
        private void removeProfile(object sender, RoutedEventArgs e) {
            try {
                if (_profile.nombre.CompareTo(_newSelectedProfile.nombre) != 0) {
                    Conexion.deleteProfile(_newSelectedProfile.id);
                    

                    Button b = Lista.getProfileButton(_newSelectedProfile.nombre);
                    if (perfiles.Children.Contains(b)) {
                        perfiles.Children.Remove(b);
                    }
                    Lista.removeProfile(_newSelectedProfile.nombre);
                    _newSelectedProfile = _profile;
                    Button aux = Lista.getProfileButton(_profile.nombre);
                    selectProfile(aux);
                } else {
                    MessageBox.Show("No puedes borrrar el perfil seleccionado");
                }
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
        }

        /**
         * Devuelve el gridPrincipal
         */
        public Grid getFirstGrid() {
            return firstPanel;
        }

        /**
         * Permite mover la ventana de la aplicacion
         */
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e) {
            if (this.WindowState == WindowState.Maximized) {
                this.WindowState = WindowState.Normal;
                this.Left = Mouse.GetPosition(this).X - 100;
                this.Top = Mouse.GetPosition(this).Y - 10;
            }
            this.DragMove();
        }

        /**
         * Cambia el fondo del boton return al entrar el raton
         */
        private void return_MouseEnter(object sender, MouseEventArgs e) {
            borderEnter.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        /**
         * Quita el fondo del boton return al salir de el
         */
        private void return_MouseLeave(object sender, MouseEventArgs e) {
            borderEnter.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
        }

        /**
         * Cambia el fondo del boton return al entrar el raton
         */
        private void return2_MouseEnter(object sender, MouseEventArgs e) {
            borderEnter2.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        /**
         * Quita el fondo del boton return al salir de el
         */
        private void return2_MouseLeave(object sender, MouseEventArgs e) {
            borderEnter2.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
        }

        /**
         * Filtra por genero el panel seleccionado
         */
        private void bButtonGender_Click(object sender, EventArgs e) {
            Button cb = (Button)sender;
            actionPanel.setMode(ActionPanel.FILTER_MODE,null,filteredGenders);
            actionPanel.Visibility = Visibility.Visible;
            actionPanel.getGenderSelection().loadGenders();
        }

        /**
         * Filtra por los datos de busqueda introducidos
         */
        private void onSearchValueChanged(object sender, KeyEventArgs e) { 
            TextBox textBox = (TextBox)sender;
            if (!textBox.Text.Equals("")) {
                if (buscadorOffline.Visibility == Visibility.Visible) {
                    if (textBox.Name.Equals("textOfflineMain")) {
                        WrapPanelPrincipal wp = Lista.getWrapVisible();
                        wp.showFoldersBySearch(textBox.Text);
                    } else if (textBox.Name.Equals("textOfflineSubFolder")) {
                        WrapPanelPrincipal wp = menuCarpetas.getWrap();
                        wp.showFoldersBySearch(textBox.Text);
                    }
                }
            } else {
                if (buscadorOffline.Visibility == Visibility.Visible) {
                    if (textBox.Name.Equals("textOfflineMain")) {
                        WrapPanelPrincipal wp = Lista.getWrapVisible();
                        wp.showAll();
                    } else if (textBox.Name.Equals("textOfflineSubFolder")) {
                        WrapPanelPrincipal wp = menuCarpetas.getWrap();
                        wp.showAll();
                    }
                }
            }
        }

        /**
         * Vacia el buscador
         */
        public void clearTextBoxAndSelection() {
            WrapPanelPrincipal wp = Lista.getWrapVisible();
            if(wp != null) {
                wp.showAll();
            }
            
            textOfflineMain.Text = "";
            textOfflineSubFolder.Text = "";
        }

        /**
         * Comprueba si hay conexion con el servidor
         */
        private bool checkConnection() {
            WebRequest request = WebRequest.Create("http://vigallery.helyan.com");
            request.Timeout = 4000;
            WebResponse response;
            try {
                response = request.GetResponse();
                response.Close();
                request = null;
                return true;
            }catch(Exception e) {
                request = null;
                return false;
            }
        }

        public void hideControl() {
            reproductorControl.Visibility = Visibility.Hidden;
        }

        public VI_Reproductor getReproductor() {
            return reproductorControl;
        }

        private void notifyGenderFilter(object sender, RoutedEventArgs e) {
            actionPanel.Visibility = Visibility.Hidden;
            filteredGenders = actionPanel.getGenderSelection().getGendersSelected();
            Lista.getWrapVisible().showFoldersByGender(filteredGenders.Keys.ToList<string>());
            actionPanel.getGenderSelection().clear();
        }
    }
}