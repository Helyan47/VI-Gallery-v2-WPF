using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using ProyectoWPF.Data;

namespace ProyectoWPF {
    public static class Lista {

        private static ICollection<WrapPanelPrincipal> _wrapsSecundarios = new List<WrapPanelPrincipal>();
        private static ICollection<WrapPanelPrincipal> _wrapsPrincipales = new List<WrapPanelPrincipal>();
        private static ICollection<Menu> _menus = new List<Menu>();
        private static ICollection<Button> _buttons = new List<Button>();
        private static ICollection<Carpeta> _carpetas = new List<Carpeta>();
        private static ICollection<SerieClass> _seriesClase = new List<SerieClass>();
        private static ICollection<SubCarpeta> _subCarpetas = new List<SubCarpeta>();

        /*
        public static Lista(ICollection<WrapPanelPrincipal> wraps,ICollection<Button> buttons) {
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
        }*/


        public static bool buttonInButtons(Button b) {
            foreach(Button bt in _buttons) {
                if (bt == b) {
                    return true;
                }
            }
            return false;
        }

        public static void addButton(Button b) {
            _buttons.Add(b);
        }

        public static void addWrapPrincipal(WrapPanelPrincipal wp) {
            _wrapsPrincipales.Add(wp);
        }

        public static void addSubWrap(WrapPanelPrincipal wp) {
            _wrapsSecundarios.Add(wp);
        }

        public static WrapPanelPrincipal getSubWrapsVisibles() {
            foreach (WrapPanelPrincipal wp in _wrapsSecundarios) {
                if (wp.Visibility == System.Windows.Visibility.Visible) {
                    return wp;
                }
            }
            return null;
        }

        public static void showWrapFromButton(Button b) {
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

        public static void hideAllWraps() {
            foreach (WrapPanelPrincipal wp in _wrapsPrincipales) {
                wp.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public static WrapPanelPrincipal getWrapVisible() {
            foreach(WrapPanelPrincipal wp in _wrapsPrincipales) {
                if (wp.Visibility == System.Windows.Visibility.Visible) {
                    return wp;
                }
            }
            return null;
        }
        public static void addSeriesClase(SerieClass serie) {
            _seriesClase.Add(serie);
            serie.setIdSerie(_seriesClase.Count);
        }

        public static void removeSeriesClase(SerieClass serie) {
            _seriesClase.Remove(serie);
        }

        public static void addMenu(Menu m) {
            _menus.Add(m);
        }

        public static void removeMenu(Menu m) {

            _menus.Remove(m);
        }

        public static void addCarpeta(Carpeta m) {
            _carpetas.Add(m);
        }

        public static void removeCarpeta(Carpeta m) {
            _carpetas.Remove(m);
        }

        public static void addWrapCarpeta(WrapPanelPrincipal p) {
            _wrapsPrincipales.Add(p);
        }

        public static void removeWrapCarpeta(WrapPanelPrincipal p) {
            _wrapsPrincipales.Remove(p);
        }

        public static void addSubCarpeta(SubCarpeta m) {
            _subCarpetas.Add(m);
        }

        public static void removeSubCarpeta(SubCarpeta m) {
            _subCarpetas.Remove(m);
        }

        public static Menu getMenuVisible() {
            foreach (Menu m in _menus) {
                if (m.Visibility == System.Windows.Visibility.Visible) {
                    return m;
                }
            }

            return null;
        }

        public static Carpeta getCarpetaVisible() {
            foreach (Carpeta c in _carpetas) {
                if (c.Visibility == System.Windows.Visibility.Visible) {
                    return c;
                }
            }
            return null;
        }

        public static WrapPanelPrincipal getWrapCarptVisible() {
            foreach (WrapPanelPrincipal c in _wrapsPrincipales) {
                if (c.Visibility == System.Windows.Visibility.Visible) {
                    return c;
                }
            }
            return null;
        }

        public static SubCarpeta getSubCarpetaVisible() {
            foreach (SubCarpeta c in _subCarpetas) {
                if (c.Visibility == System.Windows.Visibility.Visible) {
                    return c;
                }
            }
            return null;
        }


        public static Menu searchMenu(Carpeta p) {
            foreach (Menu m in _menus) {
                if (m.getCarpeta() == p) {
                    return m;
                }
            }

            return null;
        }

        public static void ocultarWrapsPrincipales() {
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


        public static SubCarpeta searchRuta(string s) {
            foreach (SubCarpeta p in _subCarpetas) {
                if (p.getRutaDirectorio() != null) {
                    if (p.getRutaDirectorio().Equals(s)) {
                        return p;
                    }
                }
                
            }
            return null;
        }

        public static void hideAll() {
            foreach (Menu m in _menus) {
                m.Visibility = System.Windows.Visibility.Hidden;
            }
            foreach (WrapPanelPrincipal wp in _wrapsSecundarios) {
                wp.Visibility = System.Windows.Visibility.Hidden;
            }
            ocultarWrapsPrincipales();
        }

        public static void hideAllExceptPrinc() {
            foreach (Menu m in _menus) {
                m.Visibility = System.Windows.Visibility.Hidden;
            }
            foreach (WrapPanelPrincipal wp in _wrapsSecundarios) {
                wp.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public static void changeMode(Button b) {

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
        public static int actualiceMode(Button b) {

            WrapPanelPrincipal visible = getWrapFromButton(b);
            if (visible != null) {
                int actualMode = visible.getMode();
                return actualMode;
            }
            return 0;
        }

        public static void modifyMode(string tipo,int mode) {
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

        public static WrapPanelPrincipal getWrapFromButton(Button b) {
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

        public static object getFolderRuta(string rutaPrograma) {
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

        public static string getRutaPadre(string rutaPrograma) {
            string[] splits = rutaPrograma.Split('/');
            string namePadre = "";
            for (int i = 0; i < (splits.Length - 1); i++) {
                if (i == (splits.Length - 2)) {
                    namePadre += splits[i];
                } else {
                    namePadre += splits[i] + "/";
                }
            }
            return namePadre;
        }

        public static Button getButtonFromFolder(string s) {
            string[] splits = s.Split('/');
            foreach(Button b in _buttons) {
                if (b.Name.CompareTo(splits[1]) == 0) {
                    return b;
                }
            }
            return null;
        }

        public static bool Contains(string ruta) {
            foreach(Carpeta carpeta in _carpetas) {
                Console.WriteLine(carpeta.getRutaPrograma());
                if (carpeta.getRutaPrograma().CompareTo(ruta) == 0) {
                    return true;
                }
            }
            return false;
        }
    }
}
