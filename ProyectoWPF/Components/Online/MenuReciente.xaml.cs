using ProyectoWPF.Data.Online;
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
    /// Lógica de interacción para MenuReciente.xaml
    /// </summary>
    public partial class MenuReciente : UserControl {
        private List<object> lista = new List<object>();
        private VIGallery main;
        public MenuReciente() {
            InitializeComponent();
            
        }

        public void setMain(VIGallery vi) {
            this.main = vi;
        }

        public void setList(List<object> lista) {
            if(lista.Count == 8) {
                this.lista = lista;
                int cont = 0;
                foreach(object o in lista) {
                    if(o is Capitulo) {
                        switch (cont) {
                            case 0:
                                img1.setMain(main);
                                img1.setCapitulo((Capitulo)o);
                                break;
                            case 1:
                                img2.setMain(main);
                                img2.setCapitulo((Capitulo)o);
                                break;
                            case 2:
                                img3.setMain(main);
                                img3.setCapitulo((Capitulo)o);
                                break;
                            case 3:
                                img4.setMain(main);
                                img4.setCapitulo((Capitulo)o);
                                break;
                            case 4:
                                img5.setMain(main);
                                img5.setCapitulo((Capitulo)o);
                                break;
                            case 5:
                                img6.setMain(main);
                                img6.setCapitulo((Capitulo)o);
                                break;
                            case 6:
                                img7.setMain(main);
                                img7.setCapitulo((Capitulo)o);
                                break;
                            case 7:
                                img8.setMain(main);
                                img8.setCapitulo((Capitulo)o);
                                break;
                        }
                        
                    }else if(o is Pelicula) {
                        switch (cont) {
                            case 0:
                                img1.setMain(main);
                                img1.setPelicula((Pelicula)o);
                                break;
                            case 1:
                                img2.setMain(main);
                                img2.setPelicula((Pelicula)o);
                                break;
                            case 2:
                                img3.setMain(main);
                                img3.setPelicula((Pelicula)o);
                                break;
                            case 3:
                                img4.setMain(main);
                                img4.setPelicula((Pelicula)o);
                                break;
                            case 4:
                                img5.setMain(main);
                                img5.setPelicula((Pelicula)o);
                                break;
                            case 5:
                                img6.setMain(main);
                                img6.setPelicula((Pelicula)o);
                                break;
                            case 6:
                                img7.setMain(main);
                                img7.setPelicula((Pelicula)o);
                                break;
                            case 7:
                                img8.setMain(main);
                                img8.setPelicula((Pelicula)o);
                                break;
                        }
                    }
                    cont++;
                }
            }
        }

        public void clear() {
            img1.clear();
            img2.clear();
            img3.clear();
            img4.clear();
            img5.clear();
            img6.clear();
            img7.clear();
            img8.clear();
            lista = new List<object>();
        }
    }
}
