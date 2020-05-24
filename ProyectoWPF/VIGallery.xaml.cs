using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using ProyectoWPF.Data;
using System;
using System.Collections.Generic;
using ProyectoWPF.NewFolders;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ProyectoWPF.Components;
using Reproductor;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para VIGallery.xaml
    /// </summary>
    public partial class VIGallery : Window {

        private ICollection<WrapPanelPrincipal> _wrapsPrincipales;
        private ICollection<Button> _botonesMenu;
        string[] _folders;
        private Carpeta _aux;
        private SubCarpeta _aux2;
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
        public VIGallery(PerfilClass profile) {
            InitializeComponent();
            _profile = profile;
            Lista.clearListas();
            _botones = buttonStack.Children;
            _newSelectedProfile = _profile;
            _botonesMenu = new List<Button>();
            changedProfile = false;
            _wrapsPrincipales = new List<WrapPanelPrincipal>();
        }


        public void onClickButtonMenu(object sender,EventArgs e) {
            Button b = (Button)sender;
            MenuClass mc = Lista.getMenuFromButton(b);
            if (Lista.buttonInButtons(mc)) {
                Lista.hideAll();
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
        }


        private void NewTemp_Click(object sender, EventArgs e) {
            addSubCarpeta();
        }

        private void Button_MouseLeftButtonUp(object sender, RoutedEventArgs e) {
            if (_activatedButton != null) {
                string[] files = new string[0];
                using (var folderDialog = new CommonOpenFileDialog()) {

                    folderDialog.IsFolderPicker = true;
                    firstFolder = 0;
                    if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok && !string.IsNullOrWhiteSpace(folderDialog.FileName)) {
                        _folders = OrderClass.orderArrayOfString(Directory.GetDirectories(folderDialog.FileName));
                        for (int i = 0; i < _folders.Length; i++) {
                            _rutas.Add(_folders[i]);

                            string[] aux = Directory.GetDirectories(_folders[i]);
                            for (int j = 0; j < aux.Length; j++) {
                                _rutas.Add(aux[j]);
                            }
                        }
                        
                        if (_folders != null) {
                            if (!conexionMode) {
                                ConexionOffline.startConnection();
                            }
                            addText(_folders);
                            if (!conexionMode) {
                                ConexionOffline.closeConnection();
                            }
                        }


                    }
                }
                Lista.modifyMode(_profile.mode);
                Lista.orderWrapsSecundarios();
                Lista.hideAllExceptPrinc();

            } else {
                menuButtons.BorderThickness = new Thickness(5);
                MessageBox.Show("No has creado ningún menú");
                menuButtons.BorderThickness = new Thickness(0);
            }

}

        private void addSubCarpeta() {
            SubCarpeta c = new SubCarpeta();
            WrapPanelPrincipal p = Lista.getSubWrapsVisibles();
            NewSubCarpeta n = null;
            if (p.getCarpeta() == null) {
                n = new NewSubCarpeta(p.getSubCarpeta().getClass().ruta);
            } else {
                n = new NewSubCarpeta(p.getCarpeta().getClass().ruta);
            }
            
           
            n.setSubCarpeta(c);
            n.ShowDialog();
            if (n.getSubCarpeta().getClass().nombre != "") {

                
                Lista.addSubCarpeta(c);
                c.getClass().idMenu = Lista.getMenuFromButton(_activatedButton).id;
                if (p != null) {
                    if (p.getCarpeta() == null) {
                        c.setDatos(c.getClass(), p, p.getSubCarpeta().GetGridCarpeta());
                        p.addSubCarpeta(c);
                        p.getSubCarpeta().AddSubCarpetas();
                        c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                        c.setImg(p.getSubCarpeta().getClass().img);

                        string name = _activatedButton.Content.ToString();
                        c.getClass().ruta = p.getSubCarpeta().getClass().ruta + "/" + c.getClass().nombre;
                        c.getClass().rutaPadre = p.getSubCarpeta().getClass().ruta;

                    } else {

                        c.setDatos(c.getClass(), p, p.GetGridSubCarpetas());
                        p.addSubCarpeta(c);
                        p.getCarpeta().AddSubCarpetas();
                        c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());

                        string name = _activatedButton.Content.ToString();
                        c.getClass().ruta = p.getCarpeta().getClass().ruta + "/" + c.getClass().nombre;
                        c.getClass().rutaPadre = p.getCarpeta().getClass().ruta;
                        c.setImg(p.getCarpeta().getClass().img);

                    }

                    if (conexionMode) {
                        Conexion.saveSubFolder(c);
                    } else {
                        ConexionOffline.startConnection();
                        ConexionOffline.addCarpeta(c.getClass());
                        ConexionOffline.closeConnection();
                    }

                    c.actualizar();
                    c.Visibility = Visibility.Visible;

                }



                
            } else {
                c = null;
            }
        }

        private SubCarpeta addSubCarpetaCompleta(Carpeta p1, string nombre) {
            SubCarpeta c = new SubCarpeta();
            
            p1.clickEspecial();
            //FlowCarpeta p = listaSeries.getFlowCarpVisible();
            WrapPanelPrincipal p = p1.GetWrapCarpPrincipal();
            CarpetaClass s = new CarpetaClass(System.IO.Path.GetFileName(nombre), "", false);
            c.setClass(s);
            s.idMenu = Lista.getMenuFromButton(_activatedButton).id;
            c.setRutaDirectorio(nombre);
            if (p != null) {

                
                s.ruta = p1.getClass().ruta + "/" + s.nombre;
                bool checkIfExists = Lista.Contains(c.getClass().ruta);

                if (!checkIfExists) {
                    Lista.addSubCarpeta(c);
                    c.setImg(p1.getClass().img);
                    c.setDatos(s, p, p.GetGridSubCarpetas());
                    p.addSubCarpeta(c);
                    p.getCarpeta().AddSubCarpetas();
                    c.setMenuCarpeta(p1.GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileName(nombre));
                    c.getClass().rutaPadre = p1.getClass().ruta;

                    c.setRutaDirectorio(nombre);

                    if (conexionMode) {
                        Conexion.saveSubFolder(c);
                    } else {
                        ConexionOffline.addCarpeta(c.getClass());
                    }

                    c.actualizar();
                    c.Visibility = Visibility.Visible;
                } else {
                    c = null;
                }

            }


            
            return c;
        }

        private SubCarpeta addSubCarpetaCompleta(SubCarpeta sp1, string nombre) {
            SubCarpeta c = new SubCarpeta();
            
            sp1.click();
            WrapPanelPrincipal p = sp1.getWrapCarpPrincipal();
            CarpetaClass s = new CarpetaClass(System.IO.Path.GetFileName(nombre), "", false);
            c.setClass(s);
            s.idMenu = Lista.getMenuFromButton(_activatedButton).id;
            c.setRutaDirectorio(nombre);
            if (p != null) {
                
                s.ruta = sp1.getClass().ruta + "/" + s.nombre;
                bool checkIfExists = Lista.Contains(c.getClass().ruta);
                if (!checkIfExists) {
                    Lista.addSubCarpeta(c);
                    c.setImg(sp1.getClass().img);
                    c.setDatos(s, p, sp1.GetGridCarpeta());
                    p.addSubCarpeta(c);
                    p.getSubCarpeta().AddSubCarpetas();
                    c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileName(nombre));
                    
                    c.getClass().rutaPadre = sp1.getClass().ruta;

                    c.setRutaDirectorio(nombre);

                    if (conexionMode) {
                        Conexion.saveSubFolder(c);
                    } else {
                        ConexionOffline.addCarpeta(c.getClass());
                    }

                    c.actualizar();
                    c.Visibility = Visibility.Visible;
                } else {
                    c = null;
                }

            }
            
            return c;
        }

        private void addFileCarpeta(string fileName, Carpeta c) {
            string ruta = "F" + c.getClass().ruta.Substring(1) + "/" + System.IO.Path.GetFileName(fileName);
            ArchivoClass ac = new ArchivoClass(System.IO.Path.GetFileNameWithoutExtension(fileName), fileName, ruta, c.getClass().img, c.getClass().id);
            Archivo a = new Archivo(ac, this);

            a.setCarpetaPadre(c);

            if (conexionMode) {
                Conexion.saveFile(ac);
            } else {
                ConexionOffline.addArchivo(ac);
            }
            
            c.GetWrapCarpPrincipal().addFile(a);
            c.addFile(a);
        }

        private void addFileSubCarpeta(string fileName, SubCarpeta c) {
            string ruta = "F" + c.getClass().ruta.Substring(1) + "/" + System.IO.Path.GetFileName(fileName);
            ArchivoClass ac = new ArchivoClass(System.IO.Path.GetFileNameWithoutExtension(fileName), fileName, ruta, c.getClass().img, c.getClass().id);
            Archivo a = new Archivo(ac, this);

            a.setSubCarpetaPadre(c);


            if (conexionMode) {
                Conexion.saveFile(ac);
            } else {
                ConexionOffline.addArchivo(ac);
            }

            c.getWrapCarpPrincipal().addFile(a);
            c.addFile(a);
        }


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
                        _aux2 = addSubCarpetaCompleta(_aux2, files[i]);
                    } else {
                        if (_aux != null) {
                            _aux2 = addSubCarpetaCompleta(_aux, files[i]);
                        }
                    }
                    string[] archivos = OrderClass.orderArrayOfString(Directory.GetFiles(files[i]));
                    for (int j = 0; j < archivos.Length; j++) {
                        foreach (string s in Lista._extensiones) {
                            if (s.ToLower().CompareTo(System.IO.Path.GetExtension(archivos[j]).ToLower()) == 0) {
                                addFileSubCarpeta(archivos[j], _aux2);
                                Console.WriteLine("Added: " + archivos[j]);
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
            if (_activatedButton != null) {
                ConexionOffline.startConnection();
                addCarpeta();
                ConexionOffline.closeConnection();
                
            } else {
                menuButtons.BorderThickness = new Thickness(5);
                MessageBox.Show("No has creado ninguno menú");
                menuButtons.BorderThickness = new Thickness(0);
            }

        }

        private void addCarpeta() {
            Carpeta p1 = new Carpeta(this);
            
            AddCarpeta newSerie = new AddCarpeta(p1, _activatedButton);
            newSerie.ShowDialog();

            if (newSerie.createdSerie()) {
                Lista.addCarpeta(p1);
                WrapPanelPrincipal aux = Lista.getWrapVisible();

                p1.actualizar();

                string name = _activatedButton.Content.ToString();
                p1.setRutaPrograma("C/" + name + "/" + p1.getClass().nombre);
                p1.getClass().rutaPadre = "";

                if (conexionMode) {
                    Conexion.saveFolder(p1);
                } else {
                    ConexionOffline.addCarpeta(p1.getClass());
                }

                p1.SetGridPadre(gridPrincipal);
                aux.addCarpeta(p1);
                p1.setPadreSerie(aux);
                p1.SetGridsOpciones(GridPrincipal, GridSecundario);

            } else {
                p1 = null;
            }
            

        }

        private Carpeta addCarpetaCompleta(string filename) {
            Carpeta p1 = new Carpeta(this);
            p1.setRutaDirectorio(filename);
            

            WrapPanelPrincipal aux = Lista.getWrapVisible();
            CarpetaClass s = new CarpetaClass(System.IO.Path.GetFileName(filename), "", true);
            p1.setClass(s);
            s.idMenu = Lista.getMenuFromButton(_activatedButton).id;
            s.rutaPadre = "";
            p1.actualizar();

            string name = _activatedButton.Content.ToString();
            p1.setRutaPrograma("C/" + name + "/" + p1.getClass().nombre);
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

                aux.addCarpeta(p1);

                p1.SetGridsOpciones(GridPrincipal, GridSecundario);
                p1.setPadreSerie(aux);
                p1.SetGridPadre(gridPrincipal);

            } else {
                p1 = null;
                s = null;
            }
            return p1;
        }

        private void addCarpetaFromLoad(CarpetaClass cc) {
            Carpeta p1 = new Carpeta(this);
            Lista.addCarpeta(p1);

            WrapPanelPrincipal aux = Lista.getWrapVisible();
            p1.setClass(cc);
            p1.actualizar();

            string name = _activatedButton.Content.ToString();

            aux.addCarpeta(p1);

            p1.SetGridsOpciones(GridPrincipal, GridSecundario);
            p1.setPadreSerie(aux);
            p1.SetGridPadre(gridPrincipal);

            p1.clickEspecial();
        }

        private void addSubCarpetaFromLoad(CarpetaClass cc) {
            SubCarpeta c = new SubCarpeta();
            Lista.addSubCarpeta(c);
            c.setClass(cc);
            object obj = Lista.getFolderRuta(cc.ruta);
            WrapPanelPrincipal p=null;
            if (obj is Carpeta) {
                Carpeta aux = (Carpeta)obj;
                p = aux.GetWrapCarpPrincipal();
            } else if (obj is SubCarpeta) {
                SubCarpeta aux = (SubCarpeta)obj;
                p = aux.getWrapCarpPrincipal();
            }
             
            if (p != null) {
                if (p.getCarpeta() == null) {

                    c.setDatos(cc, p, p.getSubCarpeta().GetGridCarpeta());
                    p.addSubCarpeta(c);
                    c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());

                    string name = _activatedButton.Content.ToString();

                } else {

                    c.setDatos(cc, p, p.GetGridSubCarpetas());
                    p.addSubCarpeta(c);
                    c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());

                    string name = _activatedButton.Content.ToString();

                }

            }


            c.actualizar();


            c.Visibility = Visibility.Visible;
        }

        private void Return_MouseLeftButtonUp(object sender, RoutedEventArgs e) {
            SubCarpeta p = Lista.getSubWrapsVisibles().getSubCarpeta();
            Carpeta p1 = Lista.getSubWrapsVisibles().getCarpeta();
            if (p != null) {
                p.clickInverso();
            } else if(p1!= null){
                p1.clickInverso();
            } else {
                MessageBox.Show("null");
            }
            
        }

        public void LoadProfileOffline(PerfilClass perfil) {
            List<MenuClass> menus = ConexionOffline.LoadMenus(perfil);

            foreach(MenuClass m in menus) {
                addMenuFromClass(m);
                List<CarpetaClass> carpetas = ConexionOffline.LoadCarpetasFromMenu(m);
                if (carpetas != null) {
                    foreach(CarpetaClass c in carpetas) {
                        addCarpetaFromLoad(c);
                        loadFilesOffline(c);
                        loadSubCarpetasOffline(c);
                    }
                }
            }
            Lista.modifyMode(_profile.mode);
            Lista.orderWrapsSecundarios();
        }

        private void loadFiles(CarpetaClass c) {
            if (c.isFolder) {
                Carpeta carpeta = Lista.getCarpetaById(c.id);
                List<ArchivoClass> archivos = OrderClass.orderListOfArchivoClass(Conexion.loadFiles(c.id));
                if (archivos != null) {
                    foreach (ArchivoClass ac in archivos) {
                        Archivo a = new Archivo(ac, this);
                        carpeta.GetWrapCarpPrincipal().addFile(a);
                        carpeta.addFile(a);
                        a.setCarpetaPadre(carpeta);
                    }
                }
            } else {
                SubCarpeta subcarpeta = Lista.getSubCarpetaById(c.id);
                List<ArchivoClass> archivos = OrderClass.orderListOfArchivoClass(Conexion.loadFiles(c.id));
                if (archivos != null) {
                    foreach (ArchivoClass ac in archivos) {
                        Archivo a = new Archivo(ac, this);
                        subcarpeta.getWrapCarpPrincipal().addFile(a);
                        subcarpeta.addFile(a);
                        a.setSubCarpetaPadre(subcarpeta);
                    }
                }
            }
            
        }

        private void loadFilesOffline(CarpetaClass c) {
            if (!c.isFolder) {
                Carpeta carpeta = Lista.getCarpetaById(c.id);
                List<ArchivoClass> archivos = OrderClass.orderListOfArchivoClass(ConexionOffline.loadFiles(c));
                if (archivos != null) {
                    foreach (ArchivoClass ac in archivos) {
                        Archivo a = new Archivo(ac, this);
                        carpeta.GetWrapCarpPrincipal().addFile(a);
                        carpeta.addFile(a);
                        a.setCarpetaPadre(carpeta);
                    }
                }
            } else {
                SubCarpeta subcarpeta = Lista.getSubCarpetaById(c.id);
                List<ArchivoClass> archivos = OrderClass.orderListOfArchivoClass(ConexionOffline.loadFiles(c));
                if (archivos != null) {
                    foreach (ArchivoClass ac in archivos) {
                        Archivo a = new Archivo(ac, this);
                        subcarpeta.getWrapCarpPrincipal().addFile(a);
                        subcarpeta.addFile(a);
                        a.setSubCarpetaPadre(subcarpeta);
                    }
                }
            }

        }

        public void loadDataConexion(long id) {
            List<MenuClass> menus = Conexion.loadMenus(_profile.id);
            if (menus != null) {
                foreach(MenuClass m in menus) {
                    addMenuFromClass(m);
                    List<CarpetaClass> carpetas = OrderClass.orderListOfCarpetaClass(Conexion.loadFoldersFromMenu(m.id));
                    if (carpetas != null) {
                        foreach(CarpetaClass c in carpetas) {
                            addCarpetaFromLoad(c);
                            loadFiles(c);
                            //Console.WriteLine(c.rutaPrograma);
                            loadSubCarpetas(c, m.id);
                        }
                    }
                }
            }
            Lista.modifyMode(_profile.mode);
            Lista.orderWrapsSecundarios();
        }

        public void loadSubCarpetas(CarpetaClass c, long idMenu) {
            List<CarpetaClass> carpetas = OrderClass.orderListOfCarpetaClass(Conexion.loadSubFoldersFromCarpeta(c, idMenu));
            if (carpetas != null) {
                foreach(CarpetaClass cc in carpetas) {
                    addSubCarpetaFromLoad(cc);
                    loadFiles(cc);
                    loadSubCarpetas(cc, idMenu);
                }
            }
        }

        public void loadSubCarpetasOffline(CarpetaClass c) {
            List<CarpetaClass> carpetas = OrderClass.orderListOfCarpetaClass(ConexionOffline.loadSubCarpetasFromCarpeta(c));
            if (carpetas != null) {
                foreach (CarpetaClass cc in carpetas) {
                    addSubCarpetaFromLoad(cc);
                    loadFilesOffline(cc);
                    loadSubCarpetasOffline(cc);
                }
            }
        }

        public void addMenuFromClass(MenuClass m) {
            Button newButton = new Button();
            newButton.Content = m.nombre;
            newButton.Height = 100;
            newButton.FontSize = 40;
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
            int cont = 0;
            string name = newButton.Content.ToString();
            _aux = new Carpeta(this);
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

        private void addMenuClick(object sender, EventArgs e) {
            Button newButton = new Button();
            newButton.Content = "";
            AddButton a = new AddButton(newButton);
            a.ShowDialog();
            if (a.isAdded()) {
                newButton.Height = 100;
                newButton.FontSize = 40;
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
                        int cont = 0;
                        string name = newButton.Content.ToString();
                        _aux = new Carpeta(this);
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
                        int cont = 0;
                        string name = newButton.Content.ToString();
                        _aux = new Carpeta(this);
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

        }

        private void removeMenu(object sender, EventArgs e) {
            if (_activatedButton != null) {
                if (conexionMode) {
                    long id = Lista.getMenuFromButton(_activatedButton).id;
                    if (id != 0) {
                        Conexion.deleteMenu(id);
                        Lista.removeMenu(_activatedButton);
                        if (_botonesMenu.Contains(_activatedButton)) {
                            _botonesMenu.Remove(_activatedButton);
                        }
                        if (_botones.Contains(_activatedButton)) {
                            _botones.Remove(_activatedButton);
                        }
                        if (_botonesMenu.Count != 0) {
                            foreach (Button b in _botonesMenu) {
                                onClickButtonMenu(b, e);
                                break;
                            }
                        } else {
                            _activatedButton = null;
                        }
                    }

                } else {
                    long id = Lista.getMenuFromButton(_activatedButton).id;
                    if (id != 0) {
                        ConexionOffline.deleteMenu(id);
                        Lista.removeMenu(_activatedButton);
                        if (_botonesMenu.Contains(_activatedButton)) {
                            _botonesMenu.Remove(_activatedButton);
                        }
                        if (_botones.Contains(_activatedButton)) {
                            _botones.Remove(_activatedButton);
                        }
                        if (_botonesMenu.Count != 0) {
                            foreach(Button b in _botonesMenu) {
                                onClickButtonMenu(_activatedButton, e);
                                break;
                            }
                        } else {
                            _activatedButton = null;
                        }
                    }
                }
                
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

        public void ReturnVisibility(bool flag) {
            if (flag) {
                Return.Visibility = Visibility.Visible;
            } else {
                Return.Visibility = Visibility.Hidden;
            }
        }

        public void CerrarApp(object sender, RoutedEventArgs e) {
            this.Close();
        }

        public void MaximizeApp(object sender, RoutedEventArgs e) {
            if (this.WindowState == WindowState.Normal) {
                this.WindowState = WindowState.Maximized;
            }else if (this.WindowState == WindowState.Maximized) {
                this.WindowState = WindowState.Normal;
                this.Width = 1000;
                this.Height = 700;
            }
            
        }

        public void MinimizeApp(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }

        public void ChangeMode(object sender,RoutedEventArgs e) {
            Lista.changeMode();
        }

        public static void updateMode(long mode) {
            _profile.mode = mode;
            if (conexionMode) {
                Conexion.updateMode(mode, _profile);
            } else {
                ConexionOffline.updateMode(mode, _profile);
            }
            
        }

        private void showOptions(object sender, RoutedEventArgs e) {
            optionPanel.Visibility = Visibility.Visible;
            addProfilesOptions();
        }

        private void showMain(object sender, RoutedEventArgs e) {
            optionPanel.Visibility = Visibility.Hidden;
        }

        private void onOptionClick(object sender, RoutedEventArgs e) {
            Button b = (Button) sender;
            switch (b.Content.ToString()) {
                case "Mi cuenta":
                    panelUSuario.Visibility = Visibility.Visible;
                    panelPerfiles.Visibility = Visibility.Hidden;
                    break;
                case "Perfiles":
                    panelUSuario.Visibility = Visibility.Hidden;
                    panelPerfiles.Visibility = Visibility.Visible;
                    break;
                case "Temas":
                    break;
                case "Reproductor":
                    break;
            }
        }

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
                    Rectangle rect = new Rectangle();
                    rect.HorizontalAlignment = HorizontalAlignment.Stretch;
                    rect.Height = 2;
                    rect.Fill = new SolidColorBrush(Color.FromRgb(60, 60, 60));
                    perfiles.Children.Add(rect);
                } else {
                    b = null;
                }
                if (b.Content.ToString().CompareTo(_profile.nombre) == 0) {
                    selectProfile(b);
                }
            }
            
        }

        private void selectProfile(object sender,RoutedEventArgs e) {
            Button aux = (Button)sender;
            PerfilClass perfilSelected = Lista.getProfile(aux.Content.ToString());
            if (perfilSelected != null) {
                if (_newSelectedProfile.nombre.CompareTo(aux.Content.ToString()) != 0) {
                    _newSelectedProfile = perfilSelected;
                    Lista.clearBackProfile();
                    aux.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF595959"));
                }
            }
        }

        private void selectProfile(Button b) {
            PerfilClass perfilSelected = Lista.getProfile(b.Content.ToString());
            if (perfilSelected != null) {
                Lista.clearBackProfile();
                b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF595959"));
            }
        }

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
                Rectangle rect = new Rectangle();
                rect.HorizontalAlignment = HorizontalAlignment.Stretch;
                rect.Height = 2;
                rect.Fill = new SolidColorBrush(Color.FromRgb(60, 60, 60));
                perfiles.Children.Add(rect);
                
                MessageBox.Show("El perfil ha sido creado. Cambia al perfil desde el panel de opciones pulsando \"Cambiar Perfil\"");
                
            } else {
                MessageBox.Show("No se ha podido crear el perfil");
            }
            
        }

        public static UsuarioClass getUser() {
            return _user;
        }

        private void removeProfile(object sender,RoutedEventArgs e) {

        }

        public Grid getFirstGrid() {
            return firstPanel;
        }

    }
}