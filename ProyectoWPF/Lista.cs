using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProyectoWPF {
    public class Lista {

        private ICollection<WrapPanelPrincipal> wrapsSecundarios;
        private ICollection<WrapPanelPrincipal> wrapsPrincipales;
        private ICollection<Menu> menus;
        private ICollection<Button> buttons;
        private ICollection<Carpeta> carpetas;
        private ICollection<SerieClass> seriesClase;
        private ICollection<SubCarpeta> subCarpetas;


        public Lista(ICollection<WrapPanelPrincipal> wraps, ICollection<Button> buttons) {
            this.wrapsPrincipales = wraps;
            this.buttons = buttons;
            this.menus = new List<Menu>();
            this.seriesClase = new List<SerieClass>();
            this.carpetas = new List<Carpeta>();
            this.subCarpetas = new List<SubCarpeta>();
            this.wrapsSecundarios = new List<WrapPanelPrincipal>();
        }

        public Lista() {
            this.wrapsPrincipales = new List<WrapPanelPrincipal>();
            this.buttons = new List<Button>();
            this.menus = new List<Menu>();
            this.seriesClase = new List<SerieClass>();
            this.carpetas = new List<Carpeta>();
            this.subCarpetas = new List<SubCarpeta>();
            this.wrapsSecundarios = new List<WrapPanelPrincipal>();
        }


        public bool buttonInButtons(Button b) {
            foreach(Button bt in buttons) {
                if (bt == b) {
                    return true;
                }
            }
            return false;
        }

        public void addSubWrap(WrapPanelPrincipal wp) {
            wrapsSecundarios.Add(wp);
        }

        public WrapPanelPrincipal getSubWrapsVisibles() {
            foreach (WrapPanelPrincipal wp in wrapsSecundarios) {
                if (wp.Visibility == System.Windows.Visibility.Visible) {
                    return wp;
                }
            }
            return null;
        }

        public void showWrapFromButton(Button b) {
            int comp = 0;
            int cont = 0;
            foreach (Button bt in buttons) {
                if (bt == b) {
                    comp = cont;
                }
                cont++;
            }
            cont = 0;
            foreach (WrapPanelPrincipal wp in wrapsPrincipales) {
                if (comp == cont) {
                    wp.Visibility = System.Windows.Visibility.Visible;
                } else {
                    wp.Visibility = System.Windows.Visibility.Hidden;
                }
                cont++;
            }
        }

        public void hideAllWraps() {
            foreach (WrapPanelPrincipal wp in wrapsPrincipales) {
                wp.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public WrapPanelPrincipal getWrapVisible() {
            foreach(WrapPanelPrincipal wp in wrapsPrincipales) {
                if (wp.Visibility == System.Windows.Visibility.Visible) {
                    return wp;
                }
            }
            return null;
        }
        public void addSeriesClase(SerieClass serie) {
            this.seriesClase.Add(serie);
            serie.setIdSerie(this.seriesClase.Count);
        }

        public void removeSeriesClase(SerieClass serie) {
            this.seriesClase.Remove(serie);
        }

        public void addMenu(Menu m) {
            this.menus.Add(m);
        }

        public void removeMenu(Menu m) {

            this.menus.Remove(m);
        }

        public void addCarpeta(Carpeta m) {
            carpetas.Add(m);
        }

        public void removeCarpeta(Carpeta m) {
            carpetas.Remove(m);
        }

        public void addWrapCarpeta(WrapPanelPrincipal p) {
            wrapsPrincipales.Add(p);
        }

        public void removeWrapCarpeta(WrapPanelPrincipal p) {
            wrapsPrincipales.Remove(p);
        }

        public void addSubCarpeta(SubCarpeta m) {
            subCarpetas.Add(m);
        }

        public void removeSubCarpeta(SubCarpeta m) {
            subCarpetas.Remove(m);
        }

        public Menu getMenuVisible() {
            foreach (Menu m in menus) {
                if (m.Visibility == System.Windows.Visibility.Visible) {
                    return m;
                }
            }

            return null;
        }

        public Carpeta getCarpetaVisible() {
            foreach (Carpeta c in carpetas) {
                if (c.Visibility == System.Windows.Visibility.Visible) {
                    return c;
                }
            }
            return null;
        }

        public WrapPanelPrincipal getWrapCarptVisible() {
            foreach (WrapPanelPrincipal c in wrapsPrincipales) {
                if (c.Visibility == System.Windows.Visibility.Visible) {
                    return c;
                }
            }
            return null;
        }

        public SubCarpeta getSubCarpetaVisible() {
            foreach (SubCarpeta c in subCarpetas) {
                if (c.Visibility == System.Windows.Visibility.Visible) {
                    return c;
                }
            }
            return null;
        }


        public Menu searchMenu(Carpeta p) {
            foreach (Menu m in menus) {
                if (m.getCarpeta() == p) {
                    return m;
                }
            }

            return null;
        }

        public void ocultarWrapsPrincipales() {
            foreach (WrapPanelPrincipal f in wrapsPrincipales) {
                f.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        //public void mostrar(SubCarpeta s, Carpeta c) {
        //    foreach (WrapPanelPrincipal f in wrapsPrincipales) {
        //        if ((f.getCarpeta() == c) && (f.getSubCarpetaPadre() == s)) {
        //            f.Visibility = System.Windows.Visibility.Visible;
        //        } else {
        //            f.Visibility = System.Windows.Visibility.Hidden;
        //        }
        //    }
        //}


        public SubCarpeta searchRuta(string s) {
            foreach (SubCarpeta p in subCarpetas) {
                if (p.getRuta().Equals(s)) {
                    return p;
                }
            }
            return null;
        }

        public void hideAll() {
            foreach (Menu m in menus) {
                m.Visibility = System.Windows.Visibility.Hidden;
            }
            foreach (WrapPanelPrincipal wp in wrapsSecundarios) {
                wp.Visibility = System.Windows.Visibility.Hidden;
            }
            ocultarWrapsPrincipales();
        }

        public void hideAllExceptPrinc() {
            foreach (Menu m in menus) {
                m.Visibility = System.Windows.Visibility.Hidden;
            }
            foreach (WrapPanelPrincipal wp in wrapsSecundarios) {
                wp.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public void changeMode(Button b) {

            WrapPanelPrincipal visible= getWrapFromButton(b);
            if (visible != null) {
                int actualMode = visible.getMode();
                if (actualMode == 0) {
                    int newMode = 1;
                    visible.setMode(newMode);
                    modifyMode(visible.Name, newMode);

                } else if (actualMode == 1) {
                    int newMode = 2;
                    visible.setMode(newMode);
                    modifyMode(visible.Name, newMode);
                }else if(actualMode == 2) {
                    int newMode = 3;
                    visible.setMode(newMode);
                    modifyMode(visible.Name, newMode);
                }else if (actualMode == 3) {
                    int newMode = 0;
                    visible.setMode(newMode);
                    modifyMode(visible.Name, newMode);
                }
            }
        }
        public int actualiceMode(Button b) {

            WrapPanelPrincipal visible = getWrapFromButton(b);
            if (visible != null) {
                int actualMode = visible.getMode();
                return actualMode;
            }
            return 0;
        }

        public void modifyMode(string tipo,int mode) {
            foreach(Carpeta p in carpetas) {
                if (p.getSerie().getTipo().Equals(tipo)) {
                    p.changeMode(mode);
                }
                
            }

            foreach(SubCarpeta p in subCarpetas) {
                if (p.getSerie().getTipo().Equals(tipo)) {
                    p.changeMode(mode);
                }
            }
        }

        public WrapPanelPrincipal getWrapFromButton(Button b) {
            int comp = 0;
            int cont = 0;
            foreach (Button bt in buttons) {
                if (bt == b) {
                    comp = cont;
                }
                cont++;
            }
            cont = 0;
            foreach (WrapPanelPrincipal wp in wrapsPrincipales) {
                if (comp == cont) {
                    return wp;
                }
                cont++;
            }
            return null;
        }
    }
}
