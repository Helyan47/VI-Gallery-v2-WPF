using System;
using System.Collections.Generic;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace SeleccionarProfile.Data.Online {
    public static class ConexionServer {
        public static MySqlConnection getConnection() {
            MySqlConnection conexion = new MySqlConnection("datasource=192.168.0.147;port=3306;username=vigallery;password=vigallery;database=vi_gallery_online");
            return conexion;
        }

        public static void increaseNumVisitasCap(long id) {
            MySqlConnection conexion = null;
            MySqlTransaction myTrans = null;
            try {
                conexion = getConnection();
                conexion.Open();

                myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("UPDATE Capitulo set numVisitas=numVisitas+1 where id=@idCapitulo", conexion);
                comando.Parameters.AddWithValue("@idCapitulo", id);
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

        public static void increaseNumVisitasPelicula(long id) {
            MySqlConnection conexion = null;
            MySqlTransaction myTrans = null;
            try {
                conexion = getConnection();
                conexion.Open();

                myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("UPDATE Pelicula set numVisitas=numVisitas+1 where id=@idPelicula", conexion);
                comando.Parameters.AddWithValue("@idPelicula", id);
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

        public static void updateTiempoActualCap(long id, long time) {
            MySqlConnection conexion = null;
            MySqlTransaction myTrans = null;
            try {
                conexion = getConnection();
                conexion.Open();

                myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("UPDATE Capitulo set tiempoActual=@tiempoActual where id=@idCapitulo", conexion);
                comando.Parameters.AddWithValue("@tiempoActual", time);
                comando.Parameters.AddWithValue("@idCapitulo", id);
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

        public static void updateTiempoActualPel(long id, long time) {
            MySqlConnection conexion = null;
            MySqlTransaction myTrans = null;
            try {
                conexion = getConnection();
                conexion.Open();

                myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("UPDATE Pelicula set tiempoActual=@tiempoActual where id=@idPelicula", conexion);
                comando.Parameters.AddWithValue("@tiempoActual", time);
                comando.Parameters.AddWithValue("@idPelicula", id);
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
        public static long getTimePelicula(long id) {
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlCommand comando = new MySqlCommand("SELECT tiempoActual from Pelicula where id=@idPelicula", conexion);
                comando.Parameters.AddWithValue("@idPelicula", id);

                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    long time = 0;
                    while (reader.Read()) {
                        time = reader.GetInt64("tiempoActual");
                    }
                    reader.Close();
                    conexion.Close();
                    return time;
                }
            } catch (MySqlException e) {
                Console.WriteLine(e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return 0;
        }

        public static long getTimeCapitulo(long id) {
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlCommand comando = new MySqlCommand("SELECT tiempoActual from Capitulo where id=@idCapitulo", conexion);
                comando.Parameters.AddWithValue("@idCapitulo", id);

                MySqlDataReader reader = comando.ExecuteReader();
                if(reader.HasRows) {
                    long time = 0;
                    while (reader.Read()) {
                       time  = reader.GetInt64("tiempoActual");
                    }
                    reader.Close();
                    conexion.Close();
                    return time;
                }
            } catch (MySqlException e) {
                Console.WriteLine(e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return 0;
        }
    }
}
