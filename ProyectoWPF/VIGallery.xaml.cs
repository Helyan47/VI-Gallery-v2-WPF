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
using VIGallery.Data;
using ProyectoWPF.Components;
using ProyectoWPF.Data.Online;
using ProyectoWPF.Components.Online;
using ProyectoWPF.Reproductor;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para VIGallery.xaml
    /// </summary>
    public partial class VIGallery : Window {

        private ICollection<WrapPanelPrincipal> _wrapsPrincipales;
        private ICollection<Button> _botonesMenu;
        string[] _folders;
        private Carpeta _aux;
        private Carpeta _aux2;
        List<string> _rutas = new List<string>();
        private UIElementCollection _botones;
        private Button _activatedButton;
        public int firstFolder = 0;
        //Establece si se ha iniciado con conexion o sin conexion
        public static bool conexionMode = false;
        public static PerfilClass _profile = null;
        private static PerfilClass _newSelectedProfile = null;
        public static bool changedProfile = false;
        public static UsuarioClass _user { get; set; }
        private bool panelPrincSelected = true;
        public VIGallery(PerfilClass profile) {
            InitializeComponent();
            _profile = profile;
            Lista.clearListas();
            _botones = buttonStack.Children;
            _newSelectedProfile = _profile;
            _botonesMenu = new List<Button>();
            changedProfile = false;
            _wrapsPrincipales = new List<WrapPanelPrincipal>();
            menuReciente.setMain(this);
            if (!conexionMode) {
                rowOnline.Height = new GridLength(0,GridUnitType.Star);
            }
            reproductorControl.setVIGallery(this);
        }

        /**
        * Evento que se lanza al pulsar alguno de los botones del menu
        * Oculta los paneles de cada menu y muestra el seleccionado
        */
        public void onClickButtonMenu(object sender,EventArgs e) {
            Button b = (Button)sender;
            MenuClass mc = Lista.getMenuFromButton(b);
            WrapPanelPrincipal wp = Lista.getWrapVisible();
            clearTextBox();
            if (Lista.buttonInButtons(mc)) {
                Lista.hideAll();
                menuCarpetas.Visibility = Visibility.Hidden;
                GridSecundario.SetValue(Grid.RowProperty, 1);
                GridPrincipal.SetValue(Grid.RowProperty, 0);
                Lista.showWrapFromMenu(mc);

            }
            foreach(Button h in _botones) {
                h.ClearValue(Button.BackgroundProperty);
            }
            _activatedButton = b;
            b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF595959"));
            Return.Visibility = Visibility.Hidden;
            borderEnter.Visibility = Visibility.Hidden;
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
                                    if (!conexionMode) {
                                        ConexionOffline.startConnection();
                                    }
                                    addText(_folders);
                                    if (!conexionMode) {
                                        ConexionOffline.closeConnection();
                                    }
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
                    menuButtons.BorderThickness = new Thickness(5);
                    MessageBox.Show("No has creado ningún menú");
                    menuButtons.BorderThickness = new Thickness(0);
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
                    CarpetaClass s = new CarpetaClass(n.getNombre(), "", true);
                    c.setClass(s);
                    c.getClass().idMenu = Lista.getMenuFromButton(_activatedButton).id;
                    c.getClass().img = padre.getClass().img;
                    c.getClass().rutaPadre = padre.getClass().ruta;
                    c.setRutaPrograma(padre.getClass().ruta + "/" + c.getClass().nombre);
                    padre.addCarpetaHijo(c);
                    Lista.addCarpeta(c);
                    if (conexionMode) {
                        Conexion.saveSubFolder(c);
                    } else {
                        ConexionOffline.startConnection();
                        ConexionOffline.addCarpeta(c.getClass());
                        ConexionOffline.closeConnection();
                    }

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

                a.setCarpetaPadre(c);

                if (conexionMode) {
                    Conexion.saveFile(ac);
                } else {
                    ConexionOffline.addArchivo(ac);
                }
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
                s.idMenu = Lista.getMenuFromButton(_activatedButton).id;
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


                    if (conexionMode) {
                        Conexion.saveFolder(p1);
                    } else {
                        ConexionOffline.addCarpeta(p1.getClass());
                    }

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
                s.idMenu = Lista.getMenuFromButton(_activatedButton).id;
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


                    if (conexionMode) {
                        Conexion.saveFolder(p1);
                    } else {
                        ConexionOffline.addCarpeta(p1.getClass());
                    }

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
            } catch (SQLiteException exc2) {
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
                    ConexionOffline.startConnection();
                    addCarpeta();
                    ConexionOffline.closeConnection();

                } else {
                    menuButtons.BorderThickness = new Thickness(5);
                    MessageBox.Show("No has creado ninguno menú");
                    menuButtons.BorderThickness = new Thickness(0);
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
                    Lista.addCarpeta(p1);
                    WrapPanelPrincipal aux = Lista.getWrapVisible();

                    p1.actualizar();

                    string name = _activatedButton.Content.ToString();
                    p1.getClass().rutaPadre = _profile.nombre + "|C/" + name;
                    p1.setRutaPrograma(_profile.nombre + "|C/" + name + "/" + p1.getClass().nombre);

                    if (conexionMode) {
                        Conexion.saveFolder(p1);
                    } else {
                        ConexionOffline.addCarpeta(p1.getClass());
                    }

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

            c.actualizar();

            c.Visibility = Visibility.Visible;
        }

        /**
         * Controla la opcion de volver atras en las subcarpetas
         */
        private void Return_MouseLeftButtonUp(object sender, EventArgs e) {
            if (menuOnline.Visibility == Visibility.Visible) {
                if (!panelPrincSelected) {
                    MenuComponent mc = ListaOnline.getMenuVisible();
                    ReturnVisibility(mc.onReturn());
                }
            } else {
                Carpeta p1 = menuCarpetas.getCarpeta();
                if (p1 != null) {
                    p1.clickInverso();
                } else {
                    MessageBox.Show("null");
                }
            }
                
            
            
            
        }

        /**
         * Carga un perfil de la base de datos offline
         */
        public void LoadProfileOffline(PerfilClass perfil) {
            try {
                List<MenuClass> menus = ConexionOffline.LoadMenus(perfil);

                foreach (MenuClass m in menus) {
                    addMenuFromClass(m);
                    List<CarpetaClass> carpetas = ConexionOffline.LoadCarpetasFromMenu(m);
                    if (carpetas != null) {
                        foreach (CarpetaClass c in carpetas) {
                            addCarpetaFromLoad(c);
                            loadFilesOffline(c);
                            loadSubCarpetasOffline(c);
                        }
                    }
                }
                Lista.orderWrapsPrincipales();
                Lista.modifyMode(_profile.mode);
                Lista.orderWrap(menuCarpetas.getWrap());
            } catch (SQLiteException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
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
         * Carga los archivos de la base de datos offline
         */
        private void loadFilesOffline(CarpetaClass c) {
            try {
                Carpeta carpeta = Lista.getCarpetaById(c.id);
                List<ArchivoClass> archivos = OrderClass.orderListOfArchivoClass(ConexionOffline.loadFiles(c));
                if (archivos != null) {
                    foreach (ArchivoClass ac in archivos) {
                        Archivo a = new Archivo(ac, this, menuCarpetas.getWrap());
                        carpeta.addFile(a);
                        a.setCarpetaPadre(carpeta);
                    }
                }
            } catch (SQLiteException exc) {
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
         * Carga las subcarpetas de una carpeta de la base de datos online
         */
        public void loadSubCarpetasOffline(CarpetaClass c) {
            try {
                Carpeta aux = Lista.getCarpetaById(c.id);
                List<CarpetaClass> carpetas = OrderClass.orderListOfCarpetaClass(ConexionOffline.loadSubCarpetasFromCarpeta(c));
                if (carpetas != null) {
                    foreach (CarpetaClass cc in carpetas) {
                        addSubCarpetaFromLoad(cc,aux);
                        loadFilesOffline(cc);
                        loadSubCarpetasOffline(cc);
                    }
                }
            } catch (SQLiteException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }
        }

        /**
         * Añade un menu a partir de un registro
         */
        public void addMenuFromClass(MenuClass m) {
            Button newButton = new Button();
            newButton.Content = m.nombre;
            newButton.Height = 100;
            newButton.FontSize = 30;
            newButton.BorderThickness = new System.Windows.Thickness(0);
            newButton.FontWeight = FontWeights.Bold;
            newButton.Foreground = Brushes.White;
            newButton.Visibility = Visibility.Visible;
            newButton.Style = (Style)Application.Current.Resources["CustomButtonStyle"];
            newButton.Click += onClickButtonMenu;


            _botonesMenu.Add(newButton);
            Lista.addMenu(m);
            Lista.addButtonMenu(newButton);
            buttonStack.Children.Add(newButton);
            string name = newButton.Content.ToString();
            WrapPanelPrincipal wp = new WrapPanelPrincipal();
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
            Button b = (Button)sender;
            MenuClass mc = Lista.getMenuFromButton(b);
            if (Lista.buttonInButtons(mc)) {
                Lista.hideAll();
                GridSecundario.SetValue(Grid.RowProperty, 1);
                GridPrincipal.SetValue(Grid.RowProperty, 0);
                Lista.showWrapFromMenu(mc);

            }
            foreach (Button h in _botones) {
                h.ClearValue(Button.BackgroundProperty);
            }
            _activatedButton = b;
            b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF595959"));
            Return.Visibility = Visibility.Hidden;
        }

        /**
         * Muestra un panel para agregar un menu a la aplicacion
         */
        private void addMenuClick(object sender, EventArgs e) {
            try {
                Button newButton = new Button();
                newButton.Content = "";
                AddButton a = new AddButton(newButton);
                a.ShowDialog();
                if (a.isAdded()) {
                    newButton.Height = 100;
                    newButton.FontSize = 30;
                    newButton.BorderThickness = new System.Windows.Thickness(0);
                    newButton.FontWeight = FontWeights.Bold;
                    newButton.Foreground = Brushes.White;
                    newButton.Visibility = Visibility.Visible;
                    newButton.Style = (Style)Application.Current.Resources["CustomButtonStyle"];
                    newButton.Click += onClickButtonMenu;


                    _botonesMenu.Add(newButton);
                    MenuClass mc = new MenuClass(newButton.Content.ToString(), _profile.id);
                    if (conexionMode) {
                        mc = Conexion.saveMenu(mc);
                        if (mc != null) {
                            Lista.addMenu(mc);
                            buttonStack.Children.Add(newButton);
                            string name = newButton.Content.ToString();
                            WrapPanelPrincipal wp = new WrapPanelPrincipal();
                            wp.name = name;
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
                    } else {
                        ConexionOffline.startConnection();
                        mc = ConexionOffline.addMenu(mc);
                        ConexionOffline.closeConnection();
                        if (mc != null) {
                            Lista.addMenu(mc);
                            buttonStack.Children.Add(newButton);
                            string name = newButton.Content.ToString();
                            WrapPanelPrincipal wp = new WrapPanelPrincipal();
                            wp.name = name;
                            gridPrincipal.Children.Add(wp);
                            wp.Visibility = Visibility.Visible;
                            _activatedButton = newButton;
                            _wrapsPrincipales.Add(wp);

                            Lista.addWrapPrincipal(wp);
                            wp.setButton(newButton);

                            onClickButtonMenu(newButton, e);
                        }
                    }
                }
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            } catch (SQLiteException exc2) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            }

        }

        /**
         * Borra un menu y todos los elementos que contiene
         */
        private void removeMenu(object sender, EventArgs e) {
            try {
                if (_activatedButton != null) {
                    if (conexionMode) {
                        long id = Lista.getMenuFromButton(_activatedButton).id;

                        WrapPanelPrincipal wp = Lista.getWrapFromMenu(Lista.getMenuFromButton(_activatedButton));
                        if (id != 0) {
                            Conexion.deleteMenu(id);
                            Lista.removeMenu(_activatedButton);

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
                                foreach (Button b in _botonesMenu) {
                                    onClickButtonMenu(b, e);
                                    break;
                                }
                            } else {
                                _activatedButton = null;
                            }
                            ReturnVisibility(false);
                        }

                    } else {
                        long id = Lista.getMenuFromButton(_activatedButton).id;
                        WrapPanelPrincipal wp = Lista.getWrapFromMenu(Lista.getMenuFromButton(_activatedButton));
                        if (id != 0) {
                            ConexionOffline.deleteMenu(id);
                            Lista.removeMenu(_activatedButton);
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
                                foreach (Button b in _botonesMenu) {
                                    onClickButtonMenu(b, e);
                                    break;
                                }
                            } else {
                                _activatedButton = null;
                            }
                            ReturnVisibility(false);
                        }
                    }

                }
                clearTextBox();
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            } catch (SQLiteException exc2) {
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
                if (conexionMode) {
                    Conexion.updateMode(mode, _profile);
                } else {
                    ConexionOffline.updateMode(mode, _profile);
                }
            } catch (MySqlException exc) {
                MessageBox.Show("No se ha podido conectar a la base de datos");
            } catch (SQLiteException exc2) {
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
                if (conexionMode) {
                    vi.loadDataConexion(_profile.id);
                } else {
                    vi.LoadProfileOffline(_profile);
                }
                
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
                    if (conexionMode) {
                        Conexion.deleteProfile(_newSelectedProfile.id);
                    } else {
                        ConexionOffline.deleteProfile(_newSelectedProfile.id);
                    }

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
            } catch (SQLiteException exc2) {
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
         * Si el menu online no se ha mostrado, se muestra y carga las peliculas y series que hay en el servidor, 
         * si por el contrario se pulsa en el otro boton, borra los datos del menu online y muestra el panel de carpetas del usuario
         */
        private void onChangeMenuClick(object sender, EventArgs e) {
            
                Button aux = (Button)sender;
            if (aux.Name.Equals("bOnlineMenu")) {
                if (menuOnline.Visibility == Visibility.Hidden) {

                    if (checkConnection()) {

                        menuOnline.Visibility = Visibility.Visible;
                        rectOnline.Visibility = Visibility.Visible;

                        buscadorOnline.Visibility = Visibility.Visible;

                        rectOffline.Visibility = Visibility.Hidden;
                        gridPrincipal.Visibility = Visibility.Hidden;
                        menuOffline.Visibility = Visibility.Hidden;
                        buscadorOffline.Visibility = Visibility.Hidden;
                        panelPrincSelected = true;

                        if (panelPrincSelected) {
                            buscadorOnline.Visibility = Visibility.Hidden;
                            rowBuscador.Height = new GridLength(0, GridUnitType.Star);
                            gridOnlinePanelPrinc.Visibility = Visibility.Visible;
                            gridOnlineShowAll.Visibility = Visibility.Hidden;
                            bPanelPrinc.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF595959"));
                            bShowAll.ClearValue(Button.BackgroundProperty);
                            ReturnVisibility(false);
                        }
                        try {
                            ListaOnline.loadData();
                            List<Capitulo> capitulosRecientes = ConexionServer.getCapitulosMasRecientes();
                            List<Pelicula> peliculasRecientes = ConexionServer.getPeliculasMasRecientes();
                            List<object> listaRecientes = new List<object>();
                            if (capitulosRecientes != null) {
                                listaRecientes.AddRange(capitulosRecientes);
                            }
                            if (peliculasRecientes != null) {
                                listaRecientes.AddRange(peliculasRecientes);
                            }
                            listaRecientes = OrderClass.orderDates(listaRecientes);
                            if (listaRecientes != null & listaRecientes.Count >= 8) {
                                listaRecientes = listaRecientes.GetRange(0, 8);
                            }

                            menuReciente.setList(listaRecientes);

                            List<Capitulo> capitulos2019 = ConexionServer.getTopEpisodios2019();
                            List<Pelicula> peliculas2019 = ConexionServer.getTopPeliculas2019();
                            List<object> listaTop2019 = new List<object>();
                            if (capitulos2019 != null) {
                                listaTop2019.AddRange(capitulos2019);
                            }
                            if (peliculas2019 != null) {
                                listaTop2019.AddRange(peliculas2019);
                            }
                            listaTop2019 = OrderClass.orderTops(listaTop2019);
                            if (listaTop2019 != null & listaTop2019.Count >= 10) {
                                listaTop2019 = listaTop2019.GetRange(0, 10);
                            }

                            top2019.setLista(ListaOnline.listToVideoElement(listaTop2019, this));

                            List<VideoElement> videoElements = ListaOnline.listCapituloToVideoElement(ConexionServer.getCapitulosMasVistos(), this);
                            if (videoElements != null) {
                                episodiosVistos.setLista(videoElements);
                            }

                            List<VideoElement> videoElements2 = ListaOnline.listPeliculaToVideoElement(ConexionServer.getPeliculasMasVistas(), this);
                            if (videoElements != null) {
                                peliculasVistas.setLista(videoElements2);
                            }
                        } catch (MySqlException exc) {
                            MessageBox.Show("No se ha podido conectar a la base de datos");
                        }

                        ListaOnline.createAllFolders(gridOnlineShowAll, wrapShowAll, this);

                        textOnline.Text = "";

                        rowAddMenu.Height = new GridLength(0, GridUnitType.Star);



                    } else {
                        MessageBox.Show("No se ha podido conectar con el servidor");
                    }
                }
            } else if (aux.Name.Equals("bOfflineMenu")) {
                if (menuOffline.Visibility == Visibility.Hidden) {
                    menuOnline.Visibility = Visibility.Hidden;
                    rectOnline.Visibility = Visibility.Hidden;
                    gridOnlinePanelPrinc.Visibility = Visibility.Hidden;
                    gridOnlineShowAll.Visibility = Visibility.Hidden;
                    buscadorOnline.Visibility = Visibility.Hidden;

                    menuOffline.Visibility = Visibility.Visible;
                    rectOffline.Visibility = Visibility.Visible;
                    gridPrincipal.Visibility = Visibility.Visible;
                    buscadorOffline.Visibility = Visibility.Visible;

                    menuReciente.clear();
                    episodiosVistos.clear();
                    peliculasVistas.clear();
                    top2019.clear();

                    ListaOnline.removeComponents(wrapShowAll);

                    rowBuscador.Height = new GridLength(30, GridUnitType.Auto);

                    rowAddMenu.Height = new GridLength(0.05, GridUnitType.Star);
                    if (Lista.getMenuVisible() != null) {
                        ReturnVisibility(true);
                    }
                }
            }
            

        }

        /**
         * Evento que permite cambiar entre el panel principal del menu online y el panel donde estan todas las series y peliculas
         */
        private void changeOnlinePanel(object sender, EventArgs e) {
            Button b = (Button)sender;
            if (b.Content.ToString().CompareTo("Panel Principal") == 0) {
                panelPrincSelected = true;
                buscadorOnline.Visibility = Visibility.Hidden;
                buscadorOffline.Visibility = Visibility.Visible;
                rowBuscador.Height = new GridLength(0, GridUnitType.Star);
                gridOnlinePanelPrinc.Visibility = Visibility.Visible;
                gridOnlineShowAll.Visibility = Visibility.Hidden;

                bPanelPrinc.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF595959"));
                bShowAll.ClearValue(Button.BackgroundProperty);

                ReturnVisibility(false);
            } else {
                panelPrincSelected = false;
                buscadorOnline.Visibility = Visibility.Visible;
                buscadorOffline.Visibility = Visibility.Hidden;
                rowBuscador.Height = new GridLength(30, GridUnitType.Auto);
                gridOnlinePanelPrinc.Visibility = Visibility.Hidden;
                gridOnlineShowAll.Visibility = Visibility.Visible;

                bShowAll.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF595959"));
                bPanelPrinc.ClearValue(Button.BackgroundProperty);
                MenuComponent mc = ListaOnline.getMenuVisible();
                if (mc != null) {
                    ReturnVisibility(true);
                }
            }
        }

        /**
         * Filtra por genero el panel seleccionado
         */
        private void bComboGenero_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBox cb = (ComboBox)sender;
            string content = ((ComboBoxItem)cb.SelectedItem).Content.ToString();
            if (content.Equals("Todos")) {
                WrapPanelPrincipal wp = Lista.getWrapVisible();
                if (wp != null) {
                    wp.showAll();
                }
                
            } else {
                WrapPanelPrincipal wp = Lista.getWrapVisible();
                if (wp != null) {
                    wp.showFoldersByGender(content);
                }
                
            }
        }

        /**
         * Filtra por genero el panel online
         */
        private void bComboGenero_SelectionChangedOnline(object sender, SelectionChangedEventArgs e) {
            ComboBox cb = (ComboBox)sender;
            string content = ((ComboBoxItem)cb.SelectedItem).Content.ToString();
            if (content.Equals("Todos")) {
                if (wrapShowAll != null) {
                    if (wrapShowAll.Visibility == Visibility.Visible) {
                        wrapShowAll.showAll();
                    }
                }
                
            } else {
                if (wrapShowAll != null & wrapShowAll.Visibility == Visibility.Visible) {
                    wrapShowAll.showFoldersByGender(content);
                }
            }
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
                }else if(buscadorOnline.Visibility == Visibility.Visible) {
                    if(wrapShowAll.Visibility == Visibility.Visible) {
                        wrapShowAll.showFoldersBySearch(textBox.Text);
                    } else {
                        MenuComponent mc = ListaOnline.getMenuVisible();
                        WrapPanelPrincipal wp = mc.getWrapVisible();
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
                } else if (buscadorOnline.Visibility == Visibility.Visible) {
                    if (wrapShowAll.Visibility == Visibility.Visible) {
                        wrapShowAll.showAll();
                    } else {
                        MenuComponent mc = ListaOnline.getMenuVisible();
                        WrapPanelPrincipal wp = mc.getWrapVisible();
                        wp.showAll();
                    }
                }
            }
        }

        /**
         * Vacia el buscador
         */
        public void clearTextBox() {
            //if (wp != null) {
            //    wp.showAll();
            //}
            
            textOfflineMain.Text = "";
            textOfflineSubFolder.Text = "";
            textOnline.Text = "";
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
    }
}