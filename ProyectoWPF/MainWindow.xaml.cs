using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private ICollection<WrapPanelPrincipal> wrapsPrincipales;
        private ICollection<Button> botonesMenu;
        private Lista lista;
        string[] folders;
        private int num1, num2, num3 = 0;
        private int num = 0;
        private Carpeta aux = new Carpeta();
        private SubCarpeta aux2;
        List<string> rutas = new List<string>();
        //SaveData sv = new SaveData("SeriesFile.txt", "FilmsFile.txt", "AnimesFile.txt");
        public MainWindow() {
            InitializeComponent();
            UIElementCollection botones = buttonStack.Children;
            wrapsPrincipales = new List<WrapPanelPrincipal>();
            botonesMenu = new List<Button>();
            foreach (Button b in botones) {
                botonesMenu.Add(b);
                string name = b.Content.ToString();
                
                WrapPanelPrincipal wp = new WrapPanelPrincipal();
                wp.Name = name;
                gridPrincipal.Children.Add(wp);
                if (name.Equals("Anime")) {
                    wp.Visibility = Visibility.Visible;
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
        }

        private void Button_MouseLeftButtonUp(object sender, RoutedEventArgs e) {
            Carpeta r = new Carpeta();
            WrapPanelPrincipal wp = lista.getWrapVisible();
            if (wp != null) {
                wp.addComponent(r);
            }
        }

        private void NewTemp_Click(object sender, EventArgs e) {
            addSubCarpeta();
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
                        c.setIdHijo(p.getSubCarpeta().getNumSubCarp());
                        c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                        c.setRuta(p.getSubCarpeta().getRuta() + "/" + c.getTitle());

                        //SerieClass sComp = p.getSubCarpeta().getSerie();
                        //if (sComp.getTipo().Equals("Anime")) {
                        //    sv.saveAnime(c.getRuta());
                        //} else if (sComp.getTipo().Equals("Serie")) {
                        //    sv.saveSerie(c.getRuta());
                        //} else if (sComp.getTipo().Equals("Pelicula")) {
                        //    sv.saveFilm(c.getRuta());
                        //}
                    } else {

                        c.setDatos(p.getCarpeta().getSerie(), p,
                        p.getCarpeta().getListaCarpetas(), p.GetGridSubCarpetas());
                        p.addSubCarpeta(c);
                        p.getCarpeta().AddSubCarpetas();
                        c.setIdHijo(p.getCarpeta().getNumSubCarp());
                        c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                        c.setRuta(p.getCarpeta().getRuta() + "/" + c.getTitle());

                        //SerieClass sComp = p.getCarpeta().getSerie();
                        //if (sComp.getTipo().Equals("Anime")) {
                        //    sv.saveAnime(c.getRuta());
                        //} else if (sComp.getTipo().Equals("Serie")) {
                        //    sv.saveSerie(c.getRuta());
                        //} else if (sComp.getTipo().Equals("Pelicula")) {
                        //    sv.saveFilm(c.getRuta());
                        //}
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
                MessageBox.Show("es null2");
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
                    c.setIdHijo(p.getSubCarpeta().getNumSubCarp());
                    c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));
                } else {

                    c.setDatos(p.getCarpeta().getSerie(), p,
                    p.getCarpeta().getListaCarpetas(), p.GetGridSubCarpetas());
                    p.addSubCarpeta(c);
                    p.getCarpeta().AddSubCarpetas();
                    c.setIdHijo(p.getCarpeta().getNumSubCarp());
                    c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));
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
                MessageBox.Show("es null2");
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
                    c.setIdHijo(p.getSubCarpeta().getNumSubCarp());
                    c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));
                } else {

                    c.setDatos(p.getCarpeta().getSerie(), p,
                    p.getCarpeta().getListaCarpetas(), p.GetGridSubCarpetas());
                    p.addSubCarpeta(c);
                    p.getCarpeta().AddSubCarpetas();
                    c.setIdHijo(p.getCarpeta().getNumSubCarp());
                    c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));
                }

            }


            c.actualizar();


            c.Visibility = Visibility.Visible;
            return c;
        }
        private void AddFolder_Click(object sender, EventArgs e) {
            string[] files = new string[0];


            OpenFileDialog folderDialog = new OpenFileDialog();
            if (folderDialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(folderDialog.FileName)) {
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
                if (Directory.GetFiles(files[i]) != null) {
                    string[] archivos = Directory.GetFiles(files[i]);
                    for (int j = 0; j < archivos.Length; j++) {
                        /*textBox1.Text += archivos[i] + "\r\n";*/

                    }

                }
            }
        }

        private void Button_AddCarpeta(object sender, EventArgs e) {
            addCarpeta();
        }

        private void addCarpeta() {
            Carpeta p1 = new Carpeta();
            p1.setListaCarpetas(this.lista);
            lista.addCarpeta(p1);
            AddCarpeta newSerie = new AddCarpeta(p1);
            newSerie.ShowDialog();
            if (!p1.getSerie().getTitle().Equals("")) {
                WrapPanelPrincipal aux = lista.getWrapVisible();
                
                p1.Width = 250;
                p1.Height = 400;


                p1.actualizar();
                //panelAux = p1;
                //if (fl[i] == flowAnime) {
                //    p1.getSerie().setTipo("Anime");
                //    p1.setRuta("Anime/" + p1.getSerie().getTitle());
                //    sv.saveAnime(p1.getRuta());

                //} else if (fl[i] == flowSeries) {
                //    p1.getSerie().setTipo("Serie");
                //    p1.setRuta("Serie/" + p1.getSerie().getTitle());
                //} else if (fl[i] == flowPelis) {
                //    p1.getSerie().setTipo("Pelicula");
                //    p1.setRuta("Pelicula/" + p1.getSerie().getTitle());
                //}
                p1.SetGridPadre(gridPrincipal);
                aux.addComponent(p1);
                p1.setPadreSerie(aux);
                p1.SetGridsOpciones(GridPrincipal, GridSecundario);

            } else {
                //p1.Controls.Remove(p1);
            }

        }

        private Carpeta addCarpetaCompleta(string filename) {
            Carpeta p1 = new Carpeta();
            p1.setListaCarpetas(this.lista);
            p1.setRuta(filename);
            lista.addCarpeta(p1);

            WrapPanelPrincipal aux = lista.getWrapVisible();

            p1.Width = 250;
            p1.Height = 400;

            SerieClass s = new SerieClass(System.IO.Path.GetFileNameWithoutExtension(filename), "");
            p1.setSerie(s);
            p1.actualizar();

            //panelAux = p1;
            //if (fl[i] == flowAnime) {
            //    p1.getSerie().setTipo("Anime");
            //} else if (fl[i] == flowSeries) {
            //    p1.getSerie().setTipo("Serie");
            //}

            
            aux.addComponent(p1);
            
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
    }
}