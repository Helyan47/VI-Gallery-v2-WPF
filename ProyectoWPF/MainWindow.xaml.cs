using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private ICollection<WrapPanelPrincipal> wrapsPrincipales;
        private ICollection<Button> botonesMenu;
        private Lista lista;
        string[] folders;
        private int num1, num2, num3 = 0;
        private int num = 0;
        private Carpeta aux;
        private SubCarpeta aux2;
        List<string> rutas = new List<string>();
        SaveData sv = new SaveData("ArchivoData.txt");
        private Button activatedButton;
        public MainWindow() {
            InitializeComponent();
            UIElementCollection botones = buttonStack.Children;
            wrapsPrincipales = new List<WrapPanelPrincipal>();
            botonesMenu = new List<Button>();
            int cont = 0;
            foreach (Button b in botones) {
                botonesMenu.Add(b);
                string name = b.Content.ToString();
                aux = new Carpeta(this);
                WrapPanelPrincipal wp = new WrapPanelPrincipal();
                wp.Name = name;
                gridPrincipal.Children.Add(wp);
                if (cont == 0) {
                    wp.Visibility = Visibility.Visible;
                    cont++;
                    activatedButton = b;
                } else {
                    wp.Visibility = Visibility.Hidden;
                }
                
                wrapsPrincipales.Add(wp);
            }
            lista = new Lista(wrapsPrincipales, botonesMenu);
        }


        public void onClickButtonMenu(object sender,EventArgs e) {
            Button b = (Button)sender;

            if (lista.buttonInButtons(b)) {
                lista.hideAll();
                
                GridSecundario.SetValue(Grid.RowProperty, 1);
                GridPrincipal.SetValue(Grid.RowProperty, 0);
                lista.showWrapFromButton(b);

            }
            activatedButton = b;
        }


        private void NewTemp_Click(object sender, EventArgs e) {
            addSubCarpeta();
        }

        private void Button_MouseLeftButtonUp(object sender, RoutedEventArgs e) {
            string[] files = new string[0];
            using (var folderDialog = new CommonOpenFileDialog()) {

                folderDialog.IsFolderPicker = true;
                if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok && !string.IsNullOrWhiteSpace(folderDialog.FileName)) {
                    folders = Directory.GetDirectories(folderDialog.FileName);

                    for (int i = 0; i < folders.Length; i++) {
                        rutas.Add(folders[i]);

                        string[] aux = Directory.GetDirectories(folders[i]);
                        for (int j = 0; j < aux.Length; j++) {
                            rutas.Add(aux[j]);
                        }
                    }
                    if (folders != null) {
                        addText(folders);
                    }


                }
            }
            lista.hideAllExceptPrinc();
            
        }

        private void addSubCarpeta() {
            SubCarpeta c = new SubCarpeta();
            c.setTitle("");
            NewSubCarpeta n = new NewSubCarpeta();
           
            n.setSubCarpeta(c);
            n.ShowDialog();
            if (n.getSubCarpeta().getTitle() != "") {

                WrapPanelPrincipal p = lista.getSubWrapsVisibles();
                lista.addSubCarpeta(c);
                if (p != null) {
                    if (p.getCarpeta() == null) {
                        c.setDatos(p.getSubCarpeta().getSerie(), p,
                        p.getSubCarpeta().GetListaCarpetas(), p.getSubCarpeta().GetGridCarpeta());
                        p.addSubCarpeta(c);
                        p.getSubCarpeta().AddSubCarpetas();
                        //c.setIdHijo(p.getSubCarpeta().getNumSubCarp());
                        c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                        c.setRuta(p.getSubCarpeta().getRuta() + "/" + c.getTitle());

                        string name = activatedButton.Name;
                        c.getSerie().setTipo(name);
                        c.setRuta(p.getSubCarpeta().getRuta() + "/" + c.getTitle());
                        sv.saveData(c.getRuta(), name);

                    } else {

                        c.setDatos(p.getCarpeta().getSerie(), p,
                        p.getCarpeta().getListaCarpetas(), p.GetGridSubCarpetas());
                        p.addSubCarpeta(c);
                        p.getCarpeta().AddSubCarpetas();
                        //c.setIdHijo(p.getCarpeta().getNumSubCarp());
                        c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                        c.setRuta(p.getCarpeta().getRuta() + "/" + c.getTitle());

                        string name = activatedButton.Name;
                        c.getSerie().setTipo(name);
                        c.setRuta(p.getCarpeta().getRuta() + "/" + c.getTitle());
                        sv.saveData(c.getRuta(), name);

                    }

                }


                c.actualizar();

                c.Visibility = Visibility.Visible;
            } else {
                //c.Controls.Remove(c);
            }
        }

        private SubCarpeta addSubCarpetaCompleta(Carpeta p1, string nombre) {
            SubCarpeta c = new SubCarpeta();
            lista.addSubCarpeta(c);
            if (p1.getListaCarpetas() == null) {
            }
            p1.clickEspecial();
            //FlowCarpeta p = listaSeries.getFlowCarpVisible();
            WrapPanelPrincipal p = p1.GetWrapCarpPrincipal();
            c.setRuta(nombre);
            if (p != null) {
                if (p.getCarpeta() == null) {
                    c.setDatos(p.getSubCarpeta().getSerie(), p,
                    p.getSubCarpeta().GetListaCarpetas(), p.getSubCarpeta().GetGridCarpeta());
                    p.addSubCarpeta(c);
                    p.getSubCarpeta().AddSubCarpetas();
                    //c.setIdHijo(p.getSubCarpeta().getNumSubCarp());
                    c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));
                    
                    string name = activatedButton.Name;
                    c.getSerie().setTipo(name);
                    c.setRuta(nombre);
                    sv.saveData(c.getRuta(), name);

                } else {

                    c.setDatos(p.getCarpeta().getSerie(), p,
                    p.getCarpeta().getListaCarpetas(), p.GetGridSubCarpetas());
                    p.addSubCarpeta(c);
                    p.getCarpeta().AddSubCarpetas();
                    //c.setIdHijo(p.getCarpeta().getNumSubCarp());
                    c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));

                    string name = activatedButton.Name;
                    c.getSerie().setTipo(name);
                    c.setRuta(nombre);
                    sv.saveData(c.getRuta(), name);

                }

            }


            c.actualizar();


            c.Visibility = Visibility.Visible;
            return c;
        }

        private SubCarpeta addSubCarpetaCompleta(SubCarpeta sp1, string nombre) {
            SubCarpeta c = new SubCarpeta();
            lista.addSubCarpeta(c);
            if (sp1.GetListaCarpetas() == null) {
                System.Windows.MessageBox.Show("es null2");
            }
            sp1.click();
            //FlowCarpeta p = listaSeries.getFlowCarpVisible();
            WrapPanelPrincipal p = sp1.getWrapCarpPrincipal();
            c.setRuta(nombre);
            if (p != null) {
                if (p.getCarpeta() == null) {
                    c.setDatos(p.getSubCarpeta().getSerie(), p,
                    p.getSubCarpeta().GetListaCarpetas(), p.getSubCarpeta().GetGridCarpeta());
                    p.addSubCarpeta(c);
                    p.getSubCarpeta().AddSubCarpetas();
                    //c.setIdHijo(p.getSubCarpeta().getNumSubCarp());
                    c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));

                    string name = activatedButton.Name;
                    c.getSerie().setTipo(name);
                    c.setRuta(nombre);
                    sv.saveData(c.getRuta(), name);


                } else {

                    c.setDatos(p.getCarpeta().getSerie(), p,
                    p.getCarpeta().getListaCarpetas(), p.GetGridSubCarpetas());
                    p.addSubCarpeta(c);
                    p.getCarpeta().AddSubCarpetas();
                    //c.setIdHijo(p.getCarpeta().getNumSubCarp());
                    c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));

                    string name = activatedButton.Name;
                    c.getSerie().setTipo(name);
                    c.setRuta(p.getCarpeta().getRuta()+"/"+ c.getTitle());
                    sv.saveData(c.getRuta(), name);


                }

            }


            c.actualizar();


            c.Visibility = Visibility.Visible;
            return c;
        }


        private void addText(string[] files) {

            for (int i = 0; i < files.Length; i++) {
                /*textBox1.Text += files[i] + "\r\n";*/
                if (files == folders) {
                    aux = addCarpetaCompleta(files[i]);

                    if (aux.getListaCarpetas() == null) {
                        MessageBox.Show("es null");
                    }
                } else {
                    if (aux.getListaCarpetas() == null) {
                        MessageBox.Show("es null");
                    }

                    aux2 = lista.searchRuta(Directory.GetParent(files[i]).FullName);
                    if (!checkString(files[i])) {
                        aux2 = addSubCarpetaCompleta(aux2, files[i]);
                    } else {
                        aux2 = addSubCarpetaCompleta(aux, files[i]);


                    }


                }
                if (Directory.GetDirectories(files[i]) != null) {
                    addText(Directory.GetDirectories(files[i]));
                }
                //if (Directory.GetFiles(files[i]) != null) {
                //    string[] archivos = Directory.GetFiles(files[i]);
                //    for (int j = 0; j < archivos.Length; j++) {
                //        /*textBox1.Text += archivos[i] + "\r\n";*/

                //    }

                //}
            }
        }

        private void Button_AddCarpeta(object sender, EventArgs e) {
            addCarpeta();
        }

        private void addCarpeta() {
            Carpeta p1 = new Carpeta(this);
            p1.setListaCarpetas(this.lista);
            lista.addCarpeta(p1);
            AddCarpeta newSerie = new AddCarpeta(p1);
            newSerie.ShowDialog();
            if (!p1.getSerie().getTitle().Equals("")) {
                WrapPanelPrincipal aux = lista.getWrapVisible();
                


                p1.actualizar();

                string name = activatedButton.Name;
                p1.getSerie().setTipo(name);
                p1.setRuta("C/"+name+"/" + p1.getSerie().getTitle());
                sv.saveData(p1.getRuta(),name);

                p1.SetGridPadre(gridPrincipal);
                aux.addCarpeta(p1);
                p1.setPadreSerie(aux);
                p1.SetGridsOpciones(GridPrincipal, GridSecundario);

            } else {
                //p1.Controls.Remove(p1);
            }

        }

        private Carpeta addCarpetaCompleta(string filename) {
            Carpeta p1 = new Carpeta(this);
            p1.setListaCarpetas(this.lista);
            p1.setRuta(filename);
            lista.addCarpeta(p1);

            WrapPanelPrincipal aux = lista.getWrapVisible();



            SerieClass s = new SerieClass(System.IO.Path.GetFileNameWithoutExtension(filename), "");
            p1.setSerie(s);
            p1.actualizar();

            string name = activatedButton.Name;
            p1.getSerie().setTipo(name);
            p1.setRuta(filename);
            sv.saveData(p1.getRuta(), name);



            aux.addCarpeta(p1);
            
            p1.SetGridsOpciones(GridPrincipal, GridSecundario);
            p1.setPadreSerie(aux);
            p1.SetGridPadre(gridPrincipal);

            return p1;
        }

        private void Return_MouseLeftButtonUp(object sender, RoutedEventArgs e) {
            SubCarpeta p = lista.getSubWrapsVisibles().getSubCarpeta();
            Carpeta p1 = lista.getSubWrapsVisibles().getCarpeta();
            if (p != null) {
                p.clickInverso();
            } else if(p1!= null){
                p1.clickInverso();
            } else {
                MessageBox.Show("null");
            }
            
        }

        private void Form1_Resize(object sender, EventArgs e) {

            //for(int i = 0; i < fl.Length; i++){

            //    int anch = fl[i].Width;
            //    double capacidad = anch % panelAux.Width;
            //    if ((capacidad <= panelAux.Width) && (capacidad >= panelAux.Width - 1)){

            //        int padd = (int)(capacidad+1 / 2);
            //        Padding pd = new Padding(padd - 30, 50, padd - 30, 50);
            //        fl[i].Padding = pd;
            //    }else{

            //        int padd = (int)(capacidad + 50 / 2);
            //        Padding pd = new Padding(padd - 30, 50, padd - 30, 50);
            //        fl[i].Padding = pd;
            //    }

            //}
        }

        

        public bool checkString(string s) {
            foreach (string h in rutas) {
                if (s.Equals(h)) {
                    //MessageBox.Show(s);
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
    }
}