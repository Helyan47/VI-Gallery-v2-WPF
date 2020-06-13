using ProyectoWPF.Components;
using ProyectoWPF.Components.Online;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoWPF.Data.Online
{
    public static class ListaOnline{
        private static ICollection<Serie> _series = new List<Serie>();
        private static ICollection<Temporada> _temporadas = new List<Temporada>();
        private static ICollection<Capitulo> _capitulos = new List<Capitulo>();
        private static ICollection<Pelicula> _peliculas = new List<Pelicula>();
        private static ICollection<SerieComponent> _serieComponents = new List<SerieComponent>();
        private static ICollection<MenuComponent> _menuComponents = new List<MenuComponent>();
        private static ICollection<TemporadaComponent> _temporadaComponents = new List<TemporadaComponent>();

        public static void addSerie(Serie s) {
            if (!_series.Contains(s))
            {
                _series.Add(s);
            }
        }

        public static void addTemporada(Temporada t) {
            if (!_temporadas.Contains(t)){
                _temporadas.Add(t);
            }
        }

        public static void addCapitulo(Capitulo c) {
            if (!_capitulos.Contains(c)) {
                _capitulos.Add(c);
            }
        }

        public static void addPelicula(Pelicula p) {
            if (!_peliculas.Contains(p)) {
                _peliculas.Add(p);
            }
        }

        public static void addSerieComponent(SerieComponent sc) {
            if(sc != null & !_serieComponents.Contains(sc)) {
                _serieComponents.Add(sc);
            }
        }

        public static void addMenuComponent(MenuComponent mc) {
            if (mc != null & !_menuComponents.Contains(mc)) {
                _menuComponents.Add(mc);
            }
        }

        public static void addTempComponent(TemporadaComponent tc) {
            if(tc != null & !_temporadaComponents.Contains(tc)) {
                _temporadaComponents.Add(tc);
            }
        }

        public static Temporada getTempByCap(Capitulo p) {
            foreach(Temporada t in _temporadas) {
                if(t.id == p.fk_Temp) {
                    return t;
                }
            }
            return null;
        }

        public static Serie getSerieByTemp(Temporada t) {
            foreach (Serie s in _series) {
                if (s.id == t.fk_Serie) {
                    return s;
                }
            }
            return null;
        }

        public static void loadData() {
            _series = ConexionServer.loadSeries();
            _temporadas = ConexionServer.loadTemporadas();
            _capitulos = ConexionServer.loadCapitulos();
            _peliculas = ConexionServer.loadPeliculas();
            checkNulls();
        }

        public static void clearAll() {
            _series = new List<Serie>();
            _temporadas = new List<Temporada>();
            _capitulos = new List<Capitulo>();
            _peliculas = new List<Pelicula>();
        }

        public static void checkNulls() {
            if (_series == null) {
                _series = new List<Serie>();
            }
            if (_temporadas == null) {
                _temporadas = new List<Temporada>();
            }
            if (_capitulos == null) {
                _capitulos = new List<Capitulo>();
            }
            if (_peliculas == null) {
                _peliculas = new List<Pelicula>();
            }
        }

        public static List<Temporada> getTemporadasFromSerie(long id) {
            List<Temporada> lista = new List<Temporada>();
            foreach(Temporada t in _temporadas) {
                if (t.fk_Serie == id) {
                    lista.Add(t);
                }
            }
            if (lista.Count != 0) {
                return lista;
            } else {
                return null;
            }
            
        }

        public static List<Capitulo> getCapitulosFromTemporada(long id) {
            List<Capitulo> lista = new List<Capitulo>();
            foreach(Capitulo c in _capitulos) {
                if(c.fk_Temp == id) {
                    lista.Add(c);
                }
            }
            if (lista.Count != 0) {
                return lista;
            } else {
                return null;
            }
        }

        public static List<VideoElement> listCapituloToVideoElement(List<Capitulo> lista, VIGallery vi) {
            if (lista != null & lista.Count > 0) {
                List<VideoElement> videoElements = new List<VideoElement>();
                foreach (Capitulo c in lista) {
                    VideoElement ve = new VideoElement(vi);
                    ve.setCapitulo(c);
                    videoElements.Add(ve);
                }
                return videoElements;
            }
            return null;
            
        }

        public static List<VideoElement> listPeliculaToVideoElement(List<Pelicula> lista, VIGallery vi) {
            if (lista != null & lista.Count > 0) {
                List<VideoElement> videoElements = new List<VideoElement>();
                foreach (Pelicula p in lista) {
                    VideoElement ve = new VideoElement(vi);
                    ve.setPelicula(p);
                    videoElements.Add(ve);
                }
                return videoElements;
            }
            return null;

        }

        public static List<VideoElement> listToVideoElement(List<object> lista, VIGallery vi) {
            if (lista != null & lista.Count > 0) {
                List<VideoElement> videoElements = new List<VideoElement>();
                foreach (object o in lista) {
                    if(o is Capitulo) {
                        VideoElement ve = new VideoElement(vi);
                        ve.setCapitulo((Capitulo)o);
                        videoElements.Add(ve);
                    } else {
                        VideoElement ve = new VideoElement(vi);
                        ve.setPelicula((Pelicula)o);
                        videoElements.Add(ve);
                    }
                    
                }
                return videoElements;
            }
            return null;
        }

        public static void createAllFolders(Grid gridPrincipal, WrapPanelPrincipal padre, VIGallery vi) {
            foreach(Serie s in _series) {
                SerieComponent sc = new SerieComponent(s);
                padre.addSerie(sc);
                addSerieComponent(sc);
                MenuComponent mc = new MenuComponent(padre,sc,vi, gridPrincipal);
                gridPrincipal.Children.Add(mc);
                mc.Visibility = Visibility.Hidden;
                addMenuComponent(mc);
                sc.setMenu(mc);
                List<Temporada> temporadas = getTemporadasFromSerie(s.id);
                if (temporadas != null) {
                    sc.setTemporadas(temporadas);
                    List<TemporadaComponent> temporadaComponents = new List<TemporadaComponent>();
                    
                    foreach (Temporada t in temporadas) {
                        TemporadaComponent tc = new TemporadaComponent(mc, t);
                        temporadaComponents.Add(tc);
                        addTempComponent(tc);
                        mc.addTempComponent(tc);
                        List<Capitulo> capitulos = getCapitulosFromTemporada(t.id);
                        if (capitulos != null) {
                            List<ArchivoComponent> archivos = new List<ArchivoComponent>();
                            foreach(Capitulo c in capitulos) {
                                ArchivoComponent ac = new ArchivoComponent(c,vi);
                                ac.setTempComponent(tc);
                                archivos.Add(ac);
                            }

                            tc.setArchivos(archivos);
                        }
                    }

                }
                
            }
            foreach(Pelicula p in _peliculas) {
                ArchivoComponent ac = new ArchivoComponent(p,vi);
                padre.addEpisodio(ac);
            }
        }

        public static void removeComponents(WrapPanelPrincipal padre) {
            clearAll();
            padre.removeChildrens();
        }

        public static MenuComponent getMenuVisible() {
            foreach(MenuComponent mc in _menuComponents) {
                if (mc.isVisible) {
                    return mc;
                }
            }
            return null;
        }
    }
}
