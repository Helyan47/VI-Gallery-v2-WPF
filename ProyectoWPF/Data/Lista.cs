using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SeleccionarProfile.Data;

namespace SeleccionarProfile {
    public static class Lista {

        private static ICollection<WrapPanelPrincipal> _wrapsSecundarios = new List<WrapPanelPrincipal>();
        private static ICollection<WrapPanelPrincipal> _wrapsPrincipales = new List<WrapPanelPrincipal>();
        private static ICollection<Menu> _menus = new List<Menu>();
        private static ICollection<MenuClass> _menusClass = new List<MenuClass>();
        private static ICollection<Button> _buttonsMenu = new List<Button>();
        private static ICollection<Carpeta> _carpetas = new List<Carpeta>();
        private static ICollection<CarpetaClass> _carpetasClase = new List<CarpetaClass>();
        private static ICollection<SubCarpeta> _subCarpetas = new List<SubCarpeta>();
        private static List<PerfilClass> _perfiles = new List<PerfilClass>();
        private static List<Button> _bPerfiles = new List<Button>();
        public static string[] _extensiones = { ".mp4", ".avi", ".mkv", ".mpeg", ".wmv", ".flv", ".mov", ".wav" };

        public static void clearListas() {
            _wrapsSecundarios = new List<WrapPanelPrincipal>();
            _wrapsPrincipales = new List<WrapPanelPrincipal>();
            _menus = new List<Menu>();
            _menusClass = new List<MenuClass>();
            _carpetas = new List<Carpeta>();
            _carpetasClase = new List<CarpetaClass>();
            _subCarpetas = new List<SubCarpeta>();
            _perfiles = new List<PerfilClass>();
            _bPerfiles = new List<Button>();
        }

        public static void addButtonMenu(Button b) {
            _buttonsMenu.Add(b);
        }

        public static ICollection<Carpeta> getCarpetas() {
            return _carpetas;
        }

        public static void addProfile(PerfilClass p) {
            if (!_perfiles.Contains(p)) {
                _perfiles.Add(p);
            }
        }

        public static bool addButtonProfile(Button b) {
            if (!_bPerfiles.Contains(b)) {
                _bPerfiles.Add(b);
                return true;
            } else {
                return false;
            }
        }

        public static List<PerfilClass> getProfiles() {
            return _perfiles;
        }

        public static void reloadProfiles() {
            if (VIGallery.conexionMode) {
                _perfiles = Conexion.loadProfiles(VIGallery.getUser().id);
            } else {
                _perfiles = ConexionOffline.LoadProfiles();
            }
        }

        public static bool checkProfile(string s) {
            foreach(PerfilClass p in _perfiles) {
                if (p.nombre.CompareTo(s) == 0) {
                    return true;
                }
            }
            return false;
        }

        public static PerfilClass getProfile(string s) {
            foreach(PerfilClass p in _perfiles) {
                if (p.nombre.CompareTo(s) == 0) {
                    return p;
                }
            }
            return null;
        }

        public static bool checkMenu(string nombre) {
            foreach(MenuClass m in _menusClass) {
                if (m.nombre.Equals(nombre)) {
                    return true;
                }
            }
            return false;
        }

        public static bool buttonInButtons(MenuClass m) {
            foreach(MenuClass mc in _menusClass) {
                if (mc == m) {
                    return true;
                }
            }
            return false;
        }

        public static MenuClass getMenuFromButton(Button b) {
            string s = b.Content.ToString();
            foreach(MenuClass m in _menusClass) {
                if (m.nombre.CompareTo(s) == 0) {
                    return m;
                }
            }
            return null;
        }

        public static MenuClass getFirstMenu() {
            int cont = 0;
            foreach(MenuClass m in _menusClass) {
                if (cont == 0) {
                    return m;
                }
            }
            return null;
        }

        public static void addMenu(MenuClass m) {
            _menusClass.Add(m);
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

        public static void showWrapFromMenu(MenuClass m) {
            int comp = 0;
            int cont = 0;
            foreach (MenuClass mc in _menusClass) {
                if (mc == m) {
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
        public static void addCarpetaClass(CarpetaClass carpeta) {
            _carpetasClase.Add(carpeta);
        }

        public static void removeSeriesClase(CarpetaClass serie) {
            _carpetasClase.Remove(serie);
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
            if (_subCarpetas.Contains(m)) {
                _subCarpetas.Remove(m);
            }
            
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

        public static void changeMode() {
            long actualMode = VIGallery._profile.mode;
            if (actualMode == 0) {
                int newMode = 1;
                modifyMode(newMode);
            } else if (actualMode == 1) {
                int newMode = 2;
                modifyMode(newMode);
            } else if (actualMode == 2) {
                int newMode = 3;
                modifyMode(newMode);
            } else if (actualMode == 3) {
                int newMode = 0;
                modifyMode(newMode);
            }
        }

        public static void modifyMode(long mode) {
            VIGallery.updateMode(mode);
            foreach (Carpeta p in _carpetas) {
                p.changeMode(mode);               
            }

            foreach(SubCarpeta p in _subCarpetas) {
                p.changeMode(mode);
            }
        }

        public static WrapPanelPrincipal getWrapFromMenu(MenuClass m) {
            foreach (WrapPanelPrincipal wp in _wrapsPrincipales) {
                if (wp.name.Equals(m.nombre)) {
                    return wp;
                }
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
                if (p.getClass().ruta.Equals(namePadre)) {
                    c = p;
                }
            }
            foreach(SubCarpeta p in _subCarpetas) {
                if (p.getClass().ruta.Equals(namePadre)) {
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

        public static MenuClass getMenuFromFolder(string s) {
            string[] splits = s.Split('/');
            foreach(MenuClass m in _menusClass) {
                if (m.nombre.CompareTo(splits[1]) == 0) {
                    return m;
                }
            }
            return null;
        }

        public static bool Contains(string ruta) {
            foreach(Carpeta carpeta in _carpetas) {
                if (carpeta.getClass().ruta.CompareTo(ruta) == 0) {
                    return true;
                }
            }
            return false;
        }

        public static bool profileExists(string s) {
            foreach(PerfilClass p in _perfiles) {
                if (p.nombre.CompareTo(s) != 0) {
                    return true;
                }
            }
            return false;
        }

        public static void clearBackProfile() {
            foreach(Button b in _bPerfiles) {
                b.ClearValue(Button.BackgroundProperty);
            }
        }

        public static Carpeta getCarpetaById(long id) {
            foreach(Carpeta c in _carpetas) {
                if(c.getClass().id == id) {
                    return c;
                }
            }
            return null;
        }

        public static SubCarpeta getSubCarpetaById(long id) {
            foreach (SubCarpeta c in _subCarpetas) {
                if (c.getClass().id == id) {
                    return c;
                }
            }
            return null;
        }

        public static Button getProfileButton(string name) {
            foreach (Button b in _bPerfiles) {
                if (b.Content.ToString().Equals(name)) {
                    return b;
                }
            }
            return null;
        }

        public static void removeProfile(string name) {
            foreach(Button b in _bPerfiles) {
                if (b.Content.ToString().Equals(name)) {
                    _bPerfiles.Remove(b);
                    break;
                }
            }

            foreach (PerfilClass p in _perfiles) {
                if (p.nombre.ToString().Equals(name)) {
                    _perfiles.Remove(p);
                    break;
                }
            }
        }

        public static void removeMenu(Button b) {
            string name = b.Content.ToString();
            long id = 0;
            MenuClass aux = null;
            foreach (MenuClass m in _menusClass) {
                if (m.nombre.Equals(name)) {
                    id=m.id;
                    aux = m;
                    break;
                }
            }
            if (_buttonsMenu.Contains(b)) {
                _buttonsMenu.Remove(b);
            }
            if (id != 0) {
                removeFolderByMenu(id);
                removeWrapPanelPrincipal(getWrapFromMenu(aux));
                //removeMenuFromPrinc(menu);
                _menusClass.Remove(aux);
                Console.WriteLine("Borrado menu " + name);
                Lista.hideAllExceptPrinc();
            }
            
        }

        public static void removeFolder(Carpeta c) {
            string name = c.getClass().ruta;
            c.remove();
            if (_carpetas.Contains(c)) {
                _carpetas.Remove(c);
                Console.WriteLine("Borrada carpeta "+name);
            }
            
        }
        public static void removeFolderByMenu(long id) {
            try {
                foreach (Carpeta c in _carpetas) {
                    if (c.getClass().idMenu == id) {
                        removeFolder(c);
                    }
                }
            }catch(InvalidOperationException e) {
                removeFolderByMenu(id);
            }

        }

        public static void removeSubFolders(Carpeta c) {
            try {
                foreach (SubCarpeta sc in _subCarpetas) {
                    if (sc.getClass().rutaPadre.Equals(c.getClass().ruta)) {
                        sc.remove();
                    }
                }
            } catch(InvalidOperationException e) {
                removeSubFolders(c);
            }
            
        }

        public static void removeSubFolders(SubCarpeta c) {
            try {
                foreach (SubCarpeta sc in _subCarpetas) {
                    if (sc.getClass().rutaPadre.Equals(c.getClass().ruta)) {
                        sc.remove();
                    }
                }
            } catch (InvalidOperationException e) {
                removeSubFolders(c);
            }
        }

        public static void removeWrapPanelSecundario(WrapPanelPrincipal wp) {
            if (_wrapsSecundarios.Contains(wp)) {
                _wrapsSecundarios.Remove(wp);
                Console.WriteLine("Borrado WrapPanel Secundario");
            }
            
        }

        public static void removeWrapPanelPrincipal(WrapPanelPrincipal wp) {
            if (_wrapsPrincipales.Contains(wp)) {
                _wrapsPrincipales.Remove(wp);
                Console.WriteLine("Borrado WrapPanel Principal");
            }

        }

        public static void removeCarpetaClass(long id) {
            foreach(CarpetaClass cc in _carpetasClase) {
                if(cc.id == id) {
                    _carpetasClase.Remove(cc);
                    break;
                }
            }
        }

        public static void orderWrapsSecundarios() {
            foreach(WrapPanelPrincipal wp in _wrapsSecundarios) {
                List<UIElement> hijos= OrderClass.orderChildOfWrap(wp.hijos);
                wp.getWrapPanel().Children.Clear();
                foreach(UIElement o in hijos) {
                    wp.getWrapPanel().Children.Add(o);
                }
            }
        }
        public static void orderWrap(WrapPanelPrincipal wp) {
            List<UIElement> hijos = OrderClass.orderChildOfWrap(wp.hijos);
            wp.getWrapPanel().Children.Clear();
            foreach (UIElement o in hijos) {
                wp.getWrapPanel().Children.Add(o);
            }
        }

        public static void orderWrapsPrincipales() {
            foreach (WrapPanelPrincipal wp in _wrapsPrincipales) {
                List<UIElement> hijos = OrderClass.orderChildOfWrap(wp.hijos);
                wp.getWrapPanel().Children.Clear();
                foreach (UIElement o in hijos) {
                    wp.getWrapPanel().Children.Add(o);
                }
            }
        }

        public static void removeMenuFromPrinc(Menu m) {
            try {
                foreach (WrapPanelPrincipal wp in _wrapsPrincipales) {
                    if (wp.getWrapPanel().Children.Contains(m)) {
                        wp.getWrapPanel().Children.Remove(m);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine("No se ha podido borrar el menu");
            }
        }

        public static void changeSubFoldersName(string rutaAntigua, string rutaNueva) {
            foreach(SubCarpeta sc in _subCarpetas) {
                if (sc.getClass().rutaPadre.Equals(rutaAntigua)) {
                    sc.updateRuta(rutaAntigua, rutaNueva); //Asignaremos la nueva rutaPadre y haremos un replace a la ruta de la rutaAntigua por la rutaNueva. Dentro de updateRuta se llamara a este metodo de nuevo con los nuevos valores
                }
            }
        }

    }
}
