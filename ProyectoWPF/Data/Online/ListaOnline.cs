using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data.Online
{
    public static class ListaOnline{
        private static ICollection<Serie> _series = new List<Serie>();
        private static ICollection<Temporada> _temporadas = new List<Temporada>();
        private static ICollection<Capitulo> _capitulos = new List<Capitulo>();
        private static ICollection<Pelicula> _peliculas = new List<Pelicula>();

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
    }
}
