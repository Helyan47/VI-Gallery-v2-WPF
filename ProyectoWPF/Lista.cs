using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProyectoWPF {
    public class Lista {

        private ICollection<WrapPanelPrincipal> _wrapsSecundarios;
        private ICollection<WrapPanelPrincipal> _wrapsPrincipales;
        private ICollection<Menu> _menus;
        private ICollection<Button> _buttons;
        private ICollection<Carpeta> _carpetas;
        private ICollection<SerieClass> _seriesClase;
        private ICollection<SubCarpeta> _subCarpetas;


        public Lista(ICollection<WrapPanelPrincipal> wraps, ICollection<Button> buttons) {
            this._wrapsPrincipales = wraps;
            this._buttons = buttons;
            this._menus = new List<Menu>();
            this._seriesClase = new List<SerieClass>();
            this._carpetas = new List<Carpeta>();
            this._subCarpetas = new List<SubCarpeta>();
            this._wrapsSecundarios = new List<WrapPanelPrincipal>();
        }

        public Lista() {
            this._wrapsPrincipales = new List<WrapPanelPrincipal>();
            this._buttons = new List<Button>();
            this._menus = new List<Menu>();
            this._seriesClase = new List<SerieClass>();
            this._carpetas = new List<Carpeta>();
            this._subCarpetas = new List<SubCarpeta>();
            this._wrapsSecundarios = new List<WrapPanelPrincipal>();
        }


        public bool buttonInButtons(Button b) {
            foreach(Button bt in _buttons) {
                if (bt == b) {
                    return true;
                }
            }
            return false;
        }

        public void addWrapPrincipal(WrapPanelPrincipal wp) {
            _wrapsPrincipales.Add(wp);
        }

        public void addSubWrap(WrapPanelPrincipal wp) {
            _wrapsSecundarios.Add(wp);
        }

        public WrapPanelPrincipal getSubWrapsVisibles() {
            foreach (WrapPanelPrincipal wp in _wrapsSecundarios) {
                if (wp.Visibility == System.Windows.Visibility.Visible) {
                    return wp;
                }
            }
            return null;
        }

        public void showWrapFromButton(Button b) {
            int comp = 0;
            int cont = 0;
            foreach (Button bt in _buttons) {
                if (bt == b) {
                    comp = cont;
                }
                cont++;
            }
            cont = 0;
            foreach (WrapPanelPrincipal wp in _wrapsPrincipales) {
                if (comp == cont) {
                    wp.Visibility = System.Windows.Visibility.Visible;
                } else {
                    wp.Visibility = System.Windows.Visibility.Hidden;
                }
                cont++;
            }
        }

        public void hideAllWraps() {
            foreach (WrapPanelPrincipal wp in _wrapsPrincipales) {
                wp.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public WrapPanelPrincipal getWrapVisible() {
            foreach(WrapPanelPrincipal wp in _wrapsPrincipales) {
                if (wp.Visibility == System.Windows.Visibility.Visible) {
                    return wp;
                }
            }
            return null;
        }
        public void addSeriesClase(SerieClass serie) {
            this._seriesClase.Add(serie);
            serie.setIdSerie(this._seriesClase.Count);
        }

        public void removeSeriesClase(SerieClass serie) {
            this._seriesClase.Remove(serie);
        }

        public void addMenu(Menu m) {
            this._menus.Add(m);
        }

        public void removeMenu(Menu m) {

            this._menus.Remove(m);
        }

        public void addCarpeta(Carpeta m) {
            _carpetas.Add(m);
        }

        public void removeCarpeta(Carpeta m) {
            _carpetas.Remove(m);
        }

        public void addWrapCarpeta(WrapPanelPrincipal p) {
            _wrapsPrincipales.Add(p);
        }

        public void removeWrapCarpeta(WrapPanelPrincipal p) {
            _wrapsPrincipales.Remove(p);
        }

        public void addSubCarpeta(SubCarpeta m) {
            _subCarpetas.Add(m);
        }

        public void removeSubCarpeta(SubCarpeta m) {
            _subCarpetas.Remove(m);
        }

        public Menu getMenuVisible() {
            foreach (Menu m in _menus) {
                if (m.Visibility == System.Windows.Visibility.Visible) {
                    return m;
                }
            }

            return null;
        }

        public Carpeta getCarpetaVisible() {
            foreach (Carpeta c in _carpetas) {
                if (c.Visibility == System.Windows.Visibility.Visible) {
                    return c;
                }
            }
            return null;
        }

        public WrapPanelPrincipal getWrapCarptVisible() {
            foreach (WrapPanelPrincipal c in _wrapsPrincipales) {
                if (c.Visibility == System.Windows.Visibility.Visible) {
                    return c;
                }
            }
            return null;
        }

        public SubCarpeta getSubCarpetaVisible() {
            foreach (SubCarpeta c in _subCarpetas) {
                if (c.Visibility == System.Windows.Visibility.Visible) {
                    return c;
                }
            }
            return null;
        }


        public Menu searchMenu(Carpeta p) {
            foreach (Menu m in _menus) {
                if (m.getCarpeta() == p) {
                    return m;
                }
            }

            return null;
        }

        public void ocultarWrapsPrincipales() {
            foreach (WrapPanelPrincipal f in _wrapsPrincipales) {
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
            foreach (SubCarpeta p in _subCarpetas) {
                if (p.getRutaDirectorio().Equals(s)) {
                    return p;
                }
            }
            return null;
        }

        public void hideAll() {
            foreach (Menu m in _menus) {
                m.Visibility = System.Windows.Visibility.Hidden;
            }
            foreach (WrapPanelPrincipal wp in _wrapsSecundarios) {
                wp.Visibility = System.Windows.Visibility.Hidden;
            }
            ocultarWrapsPrincipales();
        }

        public void hideAllExceptPrinc() {
            foreach (Menu m in _menus) {
                m.Visibility = System.Windows.Visibility.Hidden;
            }
            foreach (WrapPanelPrincipal wp in _wrapsSecundarios) {
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
            foreach(Carpeta p in _carpetas) {
                if (p.getSerie().getTipo().Equals(tipo)) {
                    p.changeMode(mode);
                }
                
            }

            foreach(SubCarpeta p in _subCarpetas) {
                if (p.getSerie().getTipo().Equals(tipo)) {
                    p.changeMode(mode);
                }
            }
        }

        public WrapPanelPrincipal getWrapFromButton(Button b) {
            int comp = 0;
            int cont = 0;
            foreach (Button bt in _buttons) {
                if (bt == b) {
                    comp = cont;
                }
                cont++;
            }
            cont = 0;
            foreach (WrapPanelPrincipal wp in _wrapsPrincipales) {
                if (comp == cont) {
                    return wp;
                }
                cont++;
            }
            return null;
        }

        public object getFolderRuta(string rutaPrograma) {
            object c = new object();
            string[] splits = rutaPrograma.Split('/');
            string namePadre = "";
            for (int i = 0; i < (splits.Length - 1); i++) {
                if (i == (splits.Length - 2)) {
                    namePadre += splits[i];
                } else {
                    namePadre += splits[i] + "/";
                }
            }
            Console.WriteLine("ruta padre"+namePadre);
            foreach(Carpeta p in _carpetas) {
                if (p.getRutaPrograma().Equals(namePadre)) {
                    c = p;
                }
            }
            foreach(SubCarpeta p in _subCarpetas) {
                if (p.getRutaPrograma().Equals(namePadre)) {
                    c = p;
                }
            }
            return c;
        }
    }
}
