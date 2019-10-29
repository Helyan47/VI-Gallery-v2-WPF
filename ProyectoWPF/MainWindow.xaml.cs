using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private ICollection<WrapPanelPrincipal> _wrapsPrincipales;
        private ICollection<Button> _botonesMenu;
        private Lista _lista;
        string[] _folders;
        private Carpeta _aux;
        private SubCarpeta _aux2;
        List<string> _rutas = new List<string>();
        SaveData _saveData = new SaveData("ArchivoData.txt");
        private UIElementCollection _botones;
        private Button _activatedButton;
        public MainWindow() {
            InitializeComponent();
            _botones = buttonStack.Children;
            _botonesMenu = new List<Button>();
            _wrapsPrincipales = new List<WrapPanelPrincipal>();
            _lista = new Lista();
            bool check=loadData();
            if (check == true) {

            } else {

                Button b = new Button();
                b.Height = 100;
                b.FontSize = 40;
                b.BorderThickness = new System.Windows.Thickness(0);
                b.FontWeight = FontWeights.Bold;
                b.Foreground = Brushes.White;
                b.Visibility = Visibility.Visible;
                b.Content = "Serie";
                b.Name = "Serie";
                b.Style = (Style)Application.Current.Resources["CustomButtonStyle"];
                b.Click += onClickButtonMenu;

                _botonesMenu.Add(b);
                buttonStack.Children.Add(b);
                int cont = 0;
                _botonesMenu.Add(b);
                string name = b.Content.ToString();
                _aux = new Carpeta(this);
                WrapPanelPrincipal wp = new WrapPanelPrincipal();
                wp.Name = name;
                gridPrincipal.Children.Add(wp);
                wp.Visibility = Visibility.Visible;
                _activatedButton = b;
                _wrapsPrincipales.Add(wp);
                _lista = new Lista(_wrapsPrincipales, _botonesMenu);
            }
        }


        public void onClickButtonMenu(object sender,EventArgs e) {
            Button b = (Button)sender;

            if (_lista.buttonInButtons(b)) {
                _lista.hideAll();
                
                GridSecundario.SetValue(Grid.RowProperty, 1);
                GridPrincipal.SetValue(Grid.RowProperty, 0);
                _lista.showWrapFromButton(b);

            }
            foreach(Button h in _botones) {
                h.ClearValue(Button.BackgroundProperty);
            }
            _activatedButton = b;
            b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF595959"));
        }


        private void NewTemp_Click(object sender, EventArgs e) {
            addSubCarpeta();
        }

        private void Button_MouseLeftButtonUp(object sender, RoutedEventArgs e) {
            string[] files = new string[0];
            using (var folderDialog = new CommonOpenFileDialog()) {

                folderDialog.IsFolderPicker = true;
                if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok && !string.IsNullOrWhiteSpace(folderDialog.FileName)) {
                    _folders = Directory.GetDirectories(folderDialog.FileName);

                    for (int i = 0; i < _folders.Length; i++) {
                        _rutas.Add(_folders[i]);

                        string[] aux = Directory.GetDirectories(_folders[i]);
                        for (int j = 0; j < aux.Length; j++) {
                            _rutas.Add(aux[j]);
                        }
                    }
                    if (_folders != null) {
                        addText(_folders);
                    }


                }
            }
            _lista.hideAllExceptPrinc();
            
        }

        private void addSubCarpeta() {
            SubCarpeta c = new SubCarpeta();
            c.setTitle("");
            NewSubCarpeta n = new NewSubCarpeta();
           
            n.setSubCarpeta(c);
            n.ShowDialog();
            if (n.getSubCarpeta().getTitle() != "") {

                WrapPanelPrincipal p = _lista.getSubWrapsVisibles();
                _lista.addSubCarpeta(c);
                if (p != null) {
                    if (p.getCarpeta() == null) {
                        c.setDatos(p.getSubCarpeta().getSerie(), p,
                        p.getSubCarpeta().GetListaCarpetas(), p.getSubCarpeta().GetGridCarpeta());
                        p.addSubCarpeta(c);
                        p.getSubCarpeta().AddSubCarpetas();
                        c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());

                        string name = _activatedButton.Name;
                        c.getSerie().setTipo(name);
                        c.setRutaPrograma(p.getSubCarpeta().getRutaPrograma() + "/" + c.getTitle());


                    } else {

                        c.setDatos(p.getCarpeta().getSerie(), p,
                        p.getCarpeta().getListaCarpetas(), p.GetGridSubCarpetas());
                        p.addSubCarpeta(c);
                        p.getCarpeta().AddSubCarpetas();
                        c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());

                        string name = _activatedButton.Name;
                        c.getSerie().setTipo(name);
                        c.setRutaPrograma(p.getCarpeta().getRutaPrograma() + "/" + c.getTitle());

                    }

                    _saveData.saveSubFolder(c);

                }



                c.actualizar();
                c.changeMode(_lista.actualiceMode(_activatedButton));
                c.Visibility = Visibility.Visible;
            } else {
                c = null;
            }
        }

        private SubCarpeta addSubCarpetaCompleta(Carpeta p1, string nombre) {
            SubCarpeta c = new SubCarpeta();
            _lista.addSubCarpeta(c);
            p1.clickEspecial();
            //FlowCarpeta p = listaSeries.getFlowCarpVisible();
            WrapPanelPrincipal p = p1.GetWrapCarpPrincipal();
            c.setRutaDirectorio(nombre);
            if (p != null) {
                if (p.getCarpeta() == null) {
                    c.setDatos(p.getSubCarpeta().getSerie(), p,
                    p.getSubCarpeta().GetListaCarpetas(), p.getSubCarpeta().GetGridCarpeta());
                    p.addSubCarpeta(c);
                    p.getSubCarpeta().AddSubCarpetas();
                    c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));
                    
                    string name = _activatedButton.Name;
                    c.getSerie().setTipo(name);
                    c.setRutaDirectorio(nombre);
                    c.setRutaPrograma(p.getSubCarpeta().getRutaPrograma() + "/" + c.getTitle());

                    _saveData.saveSubFolder(c);

                    c.changeMode(_lista.actualiceMode(_activatedButton));

                } else {

                    c.setDatos(p.getCarpeta().getSerie(), p,
                    p.getCarpeta().getListaCarpetas(), p.GetGridSubCarpetas());
                    p.addSubCarpeta(c);
                    p.getCarpeta().AddSubCarpetas();
                    c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));

                    string name = _activatedButton.Name;
                    c.getSerie().setTipo(name);
                    c.setRutaDirectorio(nombre);
                    c.setRutaPrograma(p.getCarpeta().getRutaPrograma() + "/" + c.getTitle());

                    _saveData.saveSubFolder(c);

                    c.changeMode(_lista.actualiceMode(_activatedButton));

                }

            }


            c.actualizar();


            c.Visibility = Visibility.Visible;
            return c;
        }

        private SubCarpeta addSubCarpetaCompleta(SubCarpeta sp1, string nombre) {
            SubCarpeta c = new SubCarpeta();
            _lista.addSubCarpeta(c);
            sp1.click();
            WrapPanelPrincipal p = sp1.getWrapCarpPrincipal();
            c.setRutaDirectorio(nombre);
            if (p != null) {
                if (p.getCarpeta() == null) {
                    c.setDatos(p.getSubCarpeta().getSerie(), p,
                    p.getSubCarpeta().GetListaCarpetas(), p.getSubCarpeta().GetGridCarpeta());
                    p.addSubCarpeta(c);
                    p.getSubCarpeta().AddSubCarpetas();
                    c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));

                    string name = _activatedButton.Name;
                    c.getSerie().setTipo(name);
                    c.setRutaDirectorio(nombre);
                    c.setRutaPrograma(p.getSubCarpeta().getRutaPrograma() + "/" + c.getTitle());

                    _saveData.saveSubFolder(c);

                    c.changeMode(_lista.actualiceMode(_activatedButton));

                } else {

                    c.setDatos(p.getCarpeta().getSerie(), p,
                    p.getCarpeta().getListaCarpetas(), p.GetGridSubCarpetas());
                    p.addSubCarpeta(c);
                    p.getCarpeta().AddSubCarpetas();
                    c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));

                    string name = _activatedButton.Name;
                    c.getSerie().setTipo(name);
                    c.setRutaDirectorio(p.getCarpeta().getRutaDirectorio()+"/"+ c.getTitle());
                    c.setRutaPrograma(p.getCarpeta().getRutaPrograma()+"/"+c.getTitle());

                    _saveData.saveSubFolder(c);

                    c.changeMode(_lista.actualiceMode(_activatedButton));

                }

            }


            c.actualizar();


            c.Visibility = Visibility.Visible;
            return c;
        }


        private void addText(string[] files) {

            for (int i = 0; i < files.Length; i++) {
                if (files == _folders) {
                    _aux = addCarpetaCompleta(files[i]);

                } else {

                    _aux2 = _lista.searchRuta(Directory.GetParent(files[i]).FullName);
                    if (!checkString(files[i])) {
                        _aux2 = addSubCarpetaCompleta(_aux2, files[i]);
                    } else {
                        _aux2 = addSubCarpetaCompleta(_aux, files[i]);


                    }


                }
                if (Directory.GetDirectories(files[i]) != null) {
                    addText(Directory.GetDirectories(files[i]));
                }
            }
        }

        private void Button_AddCarpeta(object sender, EventArgs e) {
            addCarpeta();
        }

        private void addCarpeta() {
            Carpeta p1 = new Carpeta(this);
            p1.setListaCarpetas(this._lista);
            _lista.addCarpeta(p1);
            AddCarpeta newSerie = new AddCarpeta(p1);
            newSerie.ShowDialog();
            if (!p1.getSerie().getTitle().Equals("")) {
                WrapPanelPrincipal aux = _lista.getWrapVisible();

                p1.actualizar();

                string name = _activatedButton.Name;
                p1.getSerie().setTipo(name);
                p1.setRutaPrograma("C/"+name+"/" + p1.getSerie().getTitle());

                _saveData.saveFolder(p1);

                p1.SetGridPadre(gridPrincipal);
                aux.addCarpeta(p1);
                p1.setPadreSerie(aux);
                p1.SetGridsOpciones(GridPrincipal, GridSecundario);

                p1.changeMode(_lista.actualiceMode(_activatedButton));

            }

        }

        private Carpeta addCarpetaCompleta(string filename) {
            Carpeta p1 = new Carpeta(this);
            p1.setListaCarpetas(this._lista);
            p1.setRutaDirectorio(filename);
            _lista.addCarpeta(p1);

            WrapPanelPrincipal aux = _lista.getWrapVisible();

            SerieClass s = new SerieClass(System.IO.Path.GetFileNameWithoutExtension(filename), "");
            p1.setSerie(s);
            p1.actualizar();

            string name = _activatedButton.Name;
            p1.getSerie().setTipo(name);
            p1.setRutaPrograma("C/" + name + "/" + p1.getSerie().getTitle());

            _saveData.saveFolder(p1);

            aux.addCarpeta(p1);
            
            p1.SetGridsOpciones(GridPrincipal, GridSecundario);
            p1.setPadreSerie(aux);
            p1.SetGridPadre(gridPrincipal);

            p1.changeMode(_lista.actualiceMode(_activatedButton));

            return p1;
        }

        private void loadCarpeta(SaveCarpeta sc) {
            Carpeta p1 = new Carpeta(this);
            p1.setListaCarpetas(this._lista);
            _lista.addCarpeta(p1);

            WrapPanelPrincipal aux = _lista.getWrapVisible();

            SerieClass s = new SerieClass(sc.getName(),sc.getDesc());
            p1.setSerie(s);
            p1.getSerie().setDirImge(sc.getDirImg());
            p1.actualizar();

            string name = _activatedButton.Name;
            p1.getSerie().setTipo(name);
            p1.setRutaPrograma(sc.getRutaPrograma());

            aux.addCarpeta(p1);

            p1.SetGridsOpciones(GridPrincipal, GridSecundario);
            p1.setPadreSerie(aux);
            p1.SetGridPadre(gridPrincipal);

            p1.changeMode(_lista.actualiceMode(_activatedButton));

            p1.clickEspecial();
        }

        private void loadSubCarpeta(SaveCarpeta sc) {
            SubCarpeta c = new SubCarpeta();
            _lista.addSubCarpeta(c);
            c.setRutaPrograma(sc.getRutaPrograma());
            object obj = _lista.getFolderRuta(sc.getRutaPrograma());
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

                    c.setDatos(p.getSubCarpeta().getSerie(), p,
                    p.getSubCarpeta().GetListaCarpetas(), p.getSubCarpeta().GetGridCarpeta());
                    p.addSubCarpeta(c);
                    p.getSubCarpeta().AddSubCarpetas();
                    c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                    c.setTitle(sc.getName());

                    string name = _activatedButton.Name;
                    c.getSerie().setTipo(name);

                    c.changeMode(_lista.actualiceMode(_activatedButton));

                } else {

                    c.setDatos(p.getCarpeta().getSerie(), p,
                    p.getCarpeta().getListaCarpetas(), p.GetGridSubCarpetas());
                    p.addSubCarpeta(c);
                    p.getCarpeta().AddSubCarpetas();
                    c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                    c.setTitle(sc.getName());

                    string name = _activatedButton.Name;
                    c.getSerie().setTipo(name);

                    c.changeMode(_lista.actualiceMode(_activatedButton));

                }

            }


            c.actualizar();


            c.Visibility = Visibility.Visible;
        }

        private void Return_MouseLeftButtonUp(object sender, RoutedEventArgs e) {
            SubCarpeta p = _lista.getSubWrapsVisibles().getSubCarpeta();
            Carpeta p1 = _lista.getSubWrapsVisibles().getCarpeta();
            if (p != null) {
                p.clickInverso();
            } else if(p1!= null){
                p1.clickInverso();
            } else {
                MessageBox.Show("null");
            }
            
        }

        public bool loadData() {
            bool check;
            if (File.Exists("ArchivoData.txt")) {
                check = true;
                ICollection<SaveCarpeta> folders = _saveData.loadFolders();
                ICollection<Button> ib = _saveData.loadButtons(folders);
                ICollection<SaveCarpeta> subFolders = _saveData.loadSubFolders();
                int cont = 0;
                foreach (Button b in ib) {
                    b.Click += onClickButtonMenu;
                    
                    _botonesMenu.Add(b);
                    buttonStack.Children.Add(b);
                    
                    string name = b.Content.ToString();
                    _aux = new Carpeta(this);
                    WrapPanelPrincipal wp = new WrapPanelPrincipal();
                    wp.Name = name;
                    gridPrincipal.Children.Add(wp);
                    
                    if (cont == 0) {
                        wp.Visibility = Visibility.Visible;
                        _activatedButton = b;
                        b.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    } else {
                        wp.Visibility = Visibility.Hidden;
                    }
                    cont++;

                    _wrapsPrincipales.Add(wp);
                }
                _lista = new Lista(_wrapsPrincipales, _botonesMenu);

                foreach (SaveCarpeta sc in folders){
                    loadCarpeta(sc);
                }

                foreach(SaveCarpeta sc in subFolders) {
                    loadSubCarpeta(sc);
                }

                
                return check;
            } else {
                check = false;
                return check;
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
            _lista.changeMode(_activatedButton);
        }
    }
}