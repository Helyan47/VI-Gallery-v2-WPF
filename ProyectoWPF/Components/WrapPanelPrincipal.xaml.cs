using ProyectoWPF.Components;
using ProyectoWPF.Components.Online;
using ProyectoWPF.Data;
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

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para WrapPanelPrincipal.xaml
    /// </summary>
    public partial class WrapPanelPrincipal : UserControl
    {

        private System.Windows.Media.Color colorGridPadre;
        private Carpeta carpeta;
        private SerieComponent serie;
        private TemporadaComponent temporada;
        public string tipo = "";
        private Grid gridCarpeta;
        private ComboBoxItem buttonPrincipal;
        public List<UIElement> hijos { get; set; }
        public string name { get; set; }
        public long menu { get; set; }

        public WrapPanelPrincipal()
        {
            InitializeComponent();
            hijos = new List<UIElement>();
        }

        public void addCarpeta(Carpeta c) {
            c.Width = 250;
            c.Height = 400;
            c.Margin = new Thickness(40, 40, 40, 40);
            if (!c.getClass().img.Equals("")) {
                c.setImg();
            } else {
                c.setDefaultSource();
            }
            //c.changeColor(System.Drawing.Color.Red);
            wrapPanel.Children.Add(c);
            hijos.Add(c);
            
        }

        public void addSerie(SerieComponent s) {
            s.Width = 250;
            s.Height = 400;
            s.Margin = new Thickness(40, 40, 40, 40);
            //c.changeColor(System.Drawing.Color.Red);
            wrapPanel.Children.Add(s);
            hijos.Add(s);
        }

        public void addTemporada(TemporadaComponent t) {
            t.Width = 250;
            t.Height = 400;
            t.Margin = new Thickness(40, 40, 40, 40);
            //c.changeColor(System.Drawing.Color.Red);
            wrapPanel.Children.Add(t);
            hijos.Add(t);
        }

        public void setSerie(SerieComponent s) {
            serie = s;
        }

        public SerieComponent getSerie() {
            return serie;
        }

        public void setTemporada(TemporadaComponent t) {
            temporada = t;
        }

        public TemporadaComponent getTemporada() {
            return temporada;
        }

        public void addEpisodio(ArchivoComponent ac) {
            ac.Width = 250;
            ac.Height = 400;
            ac.Margin = new Thickness(40, 40, 40, 40);
            //c.changeColor(System.Drawing.Color.Red);
            wrapPanel.Children.Add(ac);
            hijos.Add(ac);
        }

        public void setColorGridPadre(Color grid) {
            this.colorGridPadre = grid;
        }

        public void setGridSubCarpetas(Grid p) {
            gridCarpeta = p;
        }

        public Grid GetGridSubCarpetas() {
            return gridCarpeta;
        }


        //public void setBackColor(Color c) {
        //    flowLayoutPanel1.BackColor = c;
        //}

        public void setCarpeta(Carpeta p) {
            carpeta = p;
        }

        public Carpeta getCarpeta() {
            return carpeta;
        }

        public void addFile(Archivo a) {
            a.Width = 250;
            a.Height = 400;
            a.Margin = new Thickness(20, 20, 20, 20);
            
            wrapPanel.Children.Add(a);
            hijos.Add(a);
        }

        public void removeChildrens() {
            wrapPanel.Children.Clear();
            hijos.Clear();
        }

        public void removeFolder(Carpeta c) {
            if (wrapPanel.Children.Contains(c)) {
                wrapPanel.Children.Remove(c);
                if (hijos.Contains(c)) {
                    hijos.Remove(c);
                }
                
            } else {
                Console.WriteLine("No se ha podido borrar la carpeta " + c.getClass().ruta);
            }
        }

        public void addUIElement(UIElement element) {
            if(element is Carpeta) {
                Carpeta aux = (Carpeta)element;
                aux.Width = 250;
                aux.Height = 400;
                aux.Margin = new Thickness(40, 40, 40, 40);
                wrapPanel.Children.Add(aux);
            } else if(element is Archivo) {
                Archivo aux = (Archivo)element;
                aux.Width = 250;
                aux.Height = 400;
                aux.Margin = new Thickness(40, 40, 40, 40);
                wrapPanel.Children.Add(aux);
            }
            
            
        }

        public void removeFile(Archivo a) {
            if (wrapPanel.Children.Contains(a)) {
                wrapPanel.Children.Remove(a);
                if (hijos.Contains(a)) {
                    hijos.Remove(a);
                }

            } else {
                Console.WriteLine("No se ha podido borrar el archivo " + a._archivoClass.nombre);
            }
        }

        public void setButton(ComboBoxItem b) {
            buttonPrincipal = b;
        }

        public WrapPanel getWrapPanel() {
            return wrapPanel;
        }

        public void showFoldersByGender(string gender) {
            foreach(UIElement ui in hijos) {
                if(ui is Carpeta) {
                    Carpeta aux = (Carpeta)ui;
                    if (aux.checkGender(gender)) {
                        ui.Visibility = Visibility.Visible;
                    } else {
                        ui.Visibility = Visibility.Collapsed;
                    }
                }else if(ui is SerieComponent) {
                    SerieComponent aux = (SerieComponent)ui;
                    if (aux.checkGender(gender)) {
                        ui.Visibility = Visibility.Visible;
                    } else {
                        ui.Visibility = Visibility.Collapsed;
                    }
                }else if(ui is ArchivoComponent) {
                    ArchivoComponent aux = (ArchivoComponent)ui;
                    if (aux.checkGender(gender)) {
                        ui.Visibility = Visibility.Visible;
                    } else {
                        ui.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        public void showFoldersBySearch(string search) {
            List<string> lista = new List<string>();
            
            foreach (UIElement ui in hijos) {
                if (ui is Carpeta) {
                    Carpeta aux = (Carpeta)ui;
                    lista.Add(aux.getClass().nombre);
                }else if(ui is Archivo) {
                    Archivo aux = (Archivo)ui;
                    lista.Add(aux._archivoClass.nombre);
                }else if(ui is ArchivoComponent) {
                    ArchivoComponent aux = (ArchivoComponent)ui;
                    string nombre = aux.getNombre();
                    if (nombre != null) {
                        lista.Add(nombre);
                    }
                }else if(ui is SerieComponent) {
                    SerieComponent aux = (SerieComponent)ui;
                    lista.Add(aux.getSerie().nombre);
                }else if(ui is TemporadaComponent) {
                    TemporadaComponent aux = (TemporadaComponent)ui;
                    lista.Add("Temporada "+ aux.getTemporada().numTemporada);
                }
            }

            if (lista.Count != 0) {
                List<string> resultados = Filters.filterAlgorithm(lista.ToArray(),search);
                foreach (UIElement ui in hijos) {
                    if (ui is Carpeta) {
                        Carpeta aux = (Carpeta)ui;
                        if (resultados.Contains(aux.getClass().nombre)) {
                            aux.Visibility = Visibility.Visible;
                        } else {
                            aux.Visibility = Visibility.Collapsed;
                        }
                    } else if (ui is Archivo) {
                        Archivo aux = (Archivo)ui;
                        if (resultados.Contains(aux._archivoClass.nombre)) {
                            aux.Visibility = Visibility.Visible;
                        } else {
                            aux.Visibility = Visibility.Collapsed;
                        }
                    } else if (ui is ArchivoComponent) {
                        ArchivoComponent aux = (ArchivoComponent)ui;
                        string nombre = aux.getNombre();
                        if (resultados.Contains(nombre)) {
                            aux.Visibility = Visibility.Visible;
                        } else {
                            aux.Visibility = Visibility.Collapsed;
                        }
                    } else if (ui is SerieComponent) {
                        SerieComponent aux = (SerieComponent)ui;
                        if (resultados.Contains(aux.getSerie().nombre)) {
                            aux.Visibility = Visibility.Visible;
                        } else {
                            aux.Visibility = Visibility.Collapsed;
                        }
                    } else if (ui is TemporadaComponent) {
                        TemporadaComponent aux = (TemporadaComponent)ui;
                        if (resultados.Contains("Temporada " + aux.getTemporada().numTemporada)) {
                            aux.Visibility = Visibility.Visible;
                        } else {
                            aux.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        public void showAll() {
            foreach (UIElement ui in hijos) {
                ui.Visibility = Visibility.Visible; 
            }
        }
    }
}
