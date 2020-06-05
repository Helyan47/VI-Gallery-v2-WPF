using System;
using System.Collections.Generic;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using SeleccionarProfile.Data;

namespace SeleccionarProfile.Data.Online {
    public static class ConexionServer {
        public static MySqlConnection getConnection() {
            MySqlConnection conexion = new MySqlConnection("datasource=192.168.0.147;port=3306;username=vigallery;password=vigallery;database=vi_gallery_online");
            return conexion;
        }

        public static List<Serie> loadSeries() {
            List<Serie> series = new List<Serie>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,numTemps,descripcion,generos,fechaLanzamiento,img FROM Serie", conexion);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idSerie = (Int32)reader["id"];
                        Int32 numTemps = (Int32)reader["numTemps"];
                        Int32 fechaLanzamiento = (Int32)reader["fechaLanzamiento"];
                        string[] generos = reader["generos"].ToString().Split('|');
                        Serie serie = new Serie((long)idSerie, reader["nombre"].ToString(), numTemps, reader["descripcion"].ToString(), generos, fechaLanzamiento, reader["img"].ToString());
                        series.Add(serie);
                    }
                    reader.Close();
                    conexion.Close();
                    return series;
                }

            } catch (MySqlException e) {
                Console.WriteLine("Boton  \n" + e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static List<Temporada> loadTemporadas() {
            List<Temporada> temporadas = new List<Temporada>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,numTemporada,numCaps,year,img,fk_Serie FROM Temporada", conexion);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idTemporada = (Int32)reader["id"];
                        Int32 numTemporada = (Int32)reader["numTemporada"];
                        Int32 numCaps = (Int32)reader["numCaps"];
                        Int32 year = (Int32)reader["year"];
                        Int32 fk_Serie = (Int32)reader["fk_Serie"];
                        Temporada temporada = new Temporada((long)idTemporada, numTemporada, numCaps, year, reader["img"].ToString(), fk_Serie);
                        temporadas.Add(temporada);
                    }
                    reader.Close();
                    conexion.Close();
                    return temporadas;
                }

            } catch (MySqlException e) {
                Console.WriteLine("Temporada  \n" + e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static List<Capitulo> loadCapitulos() {
            List<Capitulo> capitulos = new List<Capitulo>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,rutaWeb,numEpisodio,tiempoActual,fechaLanzamiento,numVisitas,fk_Temp FROM Capitulo", conexion);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idCapitulo = (Int32)reader["id"];
                        Int32 numEpisodio = (Int32)reader["numEpisodio"];
                        Int64 tiempoActual = (Int64)reader["tiempoActual"];
                        Int32 numVisitas = (Int32)reader["numVisitas"];
                        DateTime date = reader.GetDateTime("fechaLanzamiento");
                        Int32 fk_Temp = (Int32)reader["fk_Temp"];
                        Capitulo capitulo = new Capitulo((long)idCapitulo, reader["nombre"].ToString(), reader["rutaWeb"].ToString(), numEpisodio, tiempoActual, date, numVisitas, fk_Temp); ;
                        capitulos.Add(capitulo);
                    }
                    reader.Close();
                    conexion.Close();
                    return capitulos;
                }

            } catch (MySqlException e) {
                Console.WriteLine("Capitulo  \n" + e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static List<Pelicula> loadPeliculas() {
            List<Pelicula> peliculas = new List<Pelicula>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,rutaWeb,descripcion,generos,tiempoActual,fechaLanzamiento,numVisitas,img FROM Pelicula", conexion);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idCapitulo = (Int32)reader["id"];
                        Int64 tiempoActual = (Int64)reader["tiempoActual"];
                        Int32 numVisitas = (Int32)reader["numVisitas"];
                        DateTime date = reader.GetDateTime("fechaLanzamiento");
                        string[] generos = reader["generos"].ToString().Split('|');
                        Pelicula pelicula = new Pelicula((long)idCapitulo, reader["nombre"].ToString(), reader["rutaWeb"].ToString(), reader["descripcion"].ToString(), generos, tiempoActual, date, numVisitas, reader["img"].ToString()); ;
                        peliculas.Add(pelicula);
                    }
                    reader.Close();
                    conexion.Close();
                    return peliculas;
                }

            } catch (MySqlException e) {
                Console.WriteLine("Capitulo  \n" + e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static List<Capitulo> getCapitulosMasRecientes() {
            List<Capitulo> capitulos = new List<Capitulo>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,rutaWeb,numEpisodio,tiempoActual,fechaLanzamiento,numVisitas,fk_Temp FROM Capitulo ORDER BY fechaLanzamiento DESC LIMIT 8", conexion);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idCapitulo = (Int32)reader["id"];
                        Int32 numEpisodio = (Int32)reader["numEpisodio"];
                        Int64 tiempoActual = (Int64)reader["tiempoActual"];
                        Int32 numVisitas = (Int32)reader["numVisitas"];
                        DateTime date = reader.GetDateTime("fechaLanzamiento");
                        Int32 fk_Temp = (Int32)reader["fk_Temp"];
                        Capitulo capitulo = new Capitulo((long)idCapitulo, reader["nombre"].ToString(), reader["rutaWeb"].ToString(), numEpisodio, tiempoActual, date, numVisitas, fk_Temp); ;
                        capitulos.Add(capitulo);
                    }
                    reader.Close();
                    conexion.Close();
                    return capitulos;
                }

            } catch (MySqlException e) {
                Console.WriteLine("Capitulo  \n" + e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static List<Pelicula> getPeliculasMasRecientes() {
            List<Pelicula> peliculas = new List<Pelicula>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,rutaWeb,descripcion,generos,tiempoActual,fechaLanzamiento,numVisitas,img FROM Pelicula ORDER BY fechaLanzamiento DESC LIMIT 8", conexion);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idCapitulo = (Int32)reader["id"];
                        Int64 tiempoActual = (Int64)reader["tiempoActual"];
                        Int32 numVisitas = (Int32)reader["numVisitas"];
                        DateTime date = reader.GetDateTime("fechaLanzamiento");
                        string[] generos = reader["generos"].ToString().Split('|');
                        Pelicula pelicula = new Pelicula((long)idCapitulo, reader["nombre"].ToString(), reader["rutaWeb"].ToString(), reader["descripcion"].ToString(), generos, tiempoActual, date, numVisitas, reader["img"].ToString()); ;
                        peliculas.Add(pelicula);
                    }
                    reader.Close();
                    conexion.Close();
                    return peliculas;
                }

            } catch (MySqlException e) {
                Console.WriteLine("Capitulo  \n" + e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static List<Capitulo> getCapitulosMasVistos() {
            List<Capitulo> capitulos = new List<Capitulo>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,rutaWeb,numEpisodio,tiempoActual,fechaLanzamiento,numVisitas,fk_Temp FROM Capitulo ORDER BY numVisitas DESC LIMIT 10", conexion);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idCapitulo = (Int32)reader["id"];
                        Int32 numEpisodio = (Int32)reader["numEpisodio"];
                        Int64 tiempoActual = (Int64)reader["tiempoActual"];
                        Int32 numVisitas = (Int32)reader["numVisitas"];
                        DateTime date = reader.GetDateTime("fechaLanzamiento");
                        Int32 fk_Temp = (Int32)reader["fk_Temp"];
                        Capitulo capitulo = new Capitulo((long)idCapitulo, reader["nombre"].ToString(), reader["rutaWeb"].ToString(), numEpisodio, tiempoActual, date, numVisitas, fk_Temp); ;
                        capitulos.Add(capitulo);
                    }
                    reader.Close();
                    conexion.Close();
                    return capitulos;
                }

            } catch (MySqlException e) {
                Console.WriteLine("Capitulo  \n" + e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static List<Pelicula> getPeliculasMasVistas() {
            List<Pelicula> peliculas = new List<Pelicula>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,rutaWeb,descripcion,generos,tiempoActual,fechaLanzamiento,numVisitas,img FROM Pelicula ORDER BY numVisitas DESC LIMIT 10", conexion);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idCapitulo = (Int32)reader["id"];
                        Int64 tiempoActual = (Int64)reader["tiempoActual"];
                        Int32 numVisitas = (Int32)reader["numVisitas"];
                        DateTime date = reader.GetDateTime("fechaLanzamiento");
                        string[] generos = reader["generos"].ToString().Split('|');
                        Pelicula pelicula = new Pelicula((long)idCapitulo, reader["nombre"].ToString(), reader["rutaWeb"].ToString(), reader["descripcion"].ToString(), generos, tiempoActual, date, numVisitas, reader["img"].ToString()); ;
                        peliculas.Add(pelicula);
                    }
                    reader.Close();
                    conexion.Close();
                    return peliculas;
                }

            } catch (MySqlException e) {
                Console.WriteLine("Capitulo  \n" + e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static List<Pelicula> getTopPeliculas2019() {
            List<Pelicula> peliculas = new List<Pelicula>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,rutaWeb,descripcion,generos,tiempoActual,fechaLanzamiento,numVisitas,img FROM Pelicula WHERE YEAR(fechaLanzamiento) in (2019) ORDER BY numVisitas DESC LIMIT 10", conexion);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idCapitulo = (Int32)reader["id"];
                        Int64 tiempoActual = (Int64)reader["tiempoActual"];
                        Int32 numVisitas = (Int32)reader["numVisitas"];
                        DateTime date = reader.GetDateTime("fechaLanzamiento");
                        string[] generos = reader["generos"].ToString().Split('|');
                        Pelicula pelicula = new Pelicula((long)idCapitulo, reader["nombre"].ToString(), reader["rutaWeb"].ToString(), reader["descripcion"].ToString(), generos, tiempoActual, date, numVisitas, reader["img"].ToString()); ;
                        peliculas.Add(pelicula);
                    }
                    reader.Close();
                    conexion.Close();
                    return peliculas;
                }

            } catch (MySqlException e) {
                Console.WriteLine("Capitulo  \n" + e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static List<Capitulo> getTopEpisodios2019() {
            List<Capitulo> capitulos = new List<Capitulo>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,rutaWeb,numEpisodio,tiempoActual,fechaLanzamiento,numVisitas,fk_Temp FROM Capitulo WHERE YEAR(fechaLanzamiento) in (2019) ORDER BY numVisitas DESC LIMIT 10", conexion);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idCapitulo = (Int32)reader["id"];
                        Int32 numEpisodio = (Int32)reader["numEpisodio"];
                        Int64 tiempoActual = (Int64)reader["tiempoActual"];
                        Int32 numVisitas = (Int32)reader["numVisitas"];
                        DateTime date = reader.GetDateTime("fechaLanzamiento");
                        Int32 fk_Temp = (Int32)reader["fk_Temp"];
                        Capitulo capitulo = new Capitulo((long)idCapitulo, reader["nombre"].ToString(), reader["rutaWeb"].ToString(), numEpisodio, tiempoActual, date, numVisitas, fk_Temp); ;
                        capitulos.Add(capitulo);
                    }
                    reader.Close();
                    conexion.Close();
                    return capitulos;
                }

            } catch (MySqlException e) {
                Console.WriteLine("Capitulo  \n" + e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static void increaseNumVisitasCap(Capitulo c) {
            MySqlConnection conexion = null;
            MySqlTransaction myTrans = null;
            try {
                conexion = getConnection();
                conexion.Open();

                myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("UPDATE Capitulo set numVisitas=numVisitas+1 where id=@idCapitulo", conexion);
                comando.Parameters.AddWithValue("@idCapitulo", c.id);
                comando.ExecuteNonQuery();

                myTrans.Commit();
            } catch (MySqlException e) {
                if (myTrans != null) {
                    myTrans.Rollback();
                }
                Console.WriteLine(e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void increaseNumVisitasPelicula(Pelicula p) {
            MySqlConnection conexion = null;
            MySqlTransaction myTrans = null;
            try {
                conexion = getConnection();
                conexion.Open();

                myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("UPDATE Pelicula set numVisitas=numVisitas+1 where id=@idPelicula", conexion);
                comando.Parameters.AddWithValue("@idPelicula", p.id);
                comando.ExecuteNonQuery();

                myTrans.Commit();
            } catch (MySqlException e) {
                if (myTrans != null) {
                    myTrans.Rollback();
                }
                Console.WriteLine(e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void updateTiempoActualCap(Capitulo c) {
            MySqlConnection conexion = null;
            MySqlTransaction myTrans = null;
            try {
                conexion = getConnection();
                conexion.Open();

                myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("UPDATE Capitulo set tiempoActual=@tiempoActual where id=@idCapitulo", conexion);
                comando.Parameters.AddWithValue("@tiempoActual", c.tiempoactual);
                comando.Parameters.AddWithValue("@idCapitulo", c.id);
                comando.ExecuteNonQuery();

                myTrans.Commit();
            } catch (MySqlException e) {
                if (myTrans != null) {
                    myTrans.Rollback();
                }
                Console.WriteLine(e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void updateTiempoActualPel(Pelicula p) {
            MySqlConnection conexion = null;
            MySqlTransaction myTrans = null;
            try {
                conexion = getConnection();
                conexion.Open();

                myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("UPDATE Pelicula set tiempoActual=@tiempoActual where id=@idPelicula", conexion);
                comando.Parameters.AddWithValue("@tiempoActual", p.tiempoactual);
                comando.Parameters.AddWithValue("@idPelicula", p.id);
                comando.ExecuteNonQuery();

                myTrans.Commit();
            } catch (MySqlException e) {
                if (myTrans != null) {
                    myTrans.Rollback();
                }
                Console.WriteLine(e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }
    }
}
