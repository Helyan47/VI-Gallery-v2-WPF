using System;
using MySql.Data.MySqlClient;

namespace ProyectoWPF.Data {
    class Conexion {

        public static MySqlConnection getConnection() {
            MySqlConnection conexion = new MySqlConnection("datasource=127.0.0.1;port=3306;username=VI_Gallery;password=vi_gallery;database=vi_gallery");
            return conexion;
        }

        public static void uploadFolder(Carpeta p) {
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlCommand comando = new MySqlCommand("SELECT id FROM Serie WHERE rutaSerie=@rutaSerie", conexion);
                comando.Parameters.AddWithValue("@rutaSerie", p.getSerie().getRuta());
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    int idSerie=-1;
                    if (reader.Read()) {
                        idSerie = reader.GetInt32("id");
                    }

                    Console.WriteLine(idSerie + "");
                    reader.Close();

                    comando = new MySqlCommand("SELECT ruta FROM Carpeta WHERE id=@id",conexion);
                    comando.Parameters.AddWithValue("@id", idSerie);
                    reader = comando.ExecuteReader();
                    Console.WriteLine(reader.HasRows&& idSerie!=-1);
                    if (!reader.HasRows && idSerie!=-1) {
                        comando.Parameters.Clear();
                        reader.Close();

                        comando = new MySqlCommand("INSERT INTO Carpeta VALUES (null,@nombre,@idSerie,@numSubCarps,@ruta,@img,@isFolder,@idUsuario)",conexion);
                        comando.Parameters.AddWithValue("@nombre", p.getTitle());
                        comando.Parameters.AddWithValue("@idSerie", idSerie);
                        comando.Parameters.AddWithValue("@numSubCarps", p.getNumSubCarp());
                        comando.Parameters.AddWithValue("@ruta", p.getRutaPrograma());
                        comando.Parameters.AddWithValue("@img", p.getSerie().getDirImg());
                        comando.Parameters.AddWithValue("@isFolder", true);
                        comando.Parameters.AddWithValue("@idUsuario", MainWindow.idUsuario);
                        comando.ExecuteNonQuery();
                    }
                } else {
                    reader.Close();
                    comando = new MySqlCommand("INSERT INTO Serie VALUES (null, @nombreSerie, @rutaSerie, @desc, @tipo, @generos, @idUsuario)",conexion);

                    Console.WriteLine(p.getSerie().getRuta());
                    comando.Parameters.AddWithValue("@nombreSerie", p.getSerie().getTitle());
                    comando.Parameters.AddWithValue("@rutaSerie", p.getSerie().getRuta());
                    comando.Parameters.AddWithValue("@desc", p.getDescripcion());
                    comando.Parameters.AddWithValue("@tipo", p.getSerie().getTipo());
                    comando.Parameters.AddWithValue("@generos", "");
                    comando.Parameters.AddWithValue("@idUsuario", MainWindow.idUsuario);
                    comando.ExecuteNonQuery();

                    comando.Parameters.Clear();
                    reader.Close();

                    comando = new MySqlCommand("SELECT id FROM Serie WHERE rutaSerie=@rutaSerie",conexion);
                    comando.Parameters.AddWithValue("@rutaSerie", p.getSerie().getRuta());
                    reader = comando.ExecuteReader();

                    
                    if (reader.HasRows) {
                        int idSerie = -1;
                        if (reader.Read()) {
                            idSerie = reader.GetInt32("id");
                        }

                        reader.Close();
                        comando.Parameters.Clear();

                        comando = new MySqlCommand("SELECT ruta FROM Carpeta WHERE ruta=@ruta",conexion);
                        comando.Parameters.AddWithValue("@ruta", p.getRutaPrograma());
                        reader = comando.ExecuteReader();
                        if (!reader.HasRows) {
                            comando.Parameters.Clear();
                            reader.Close();

                            comando = new MySqlCommand("INSERT INTO Carpeta VALUES (null,@nombre,@idSerie,@numSubCarps,@ruta,@img,@isFolder,@idUsuario)",conexion);
                            comando.Parameters.AddWithValue("@nombre", p.getTitle());
                            comando.Parameters.AddWithValue("@idSerie", idSerie);
                            comando.Parameters.AddWithValue("@numSubCarps", p.getNumSubCarp());
                            comando.Parameters.AddWithValue("@ruta", p.getRutaPrograma());
                            comando.Parameters.AddWithValue("@img", p.getSerie().getDirImg());
                            comando.Parameters.AddWithValue("@isFolder", 1);
                            comando.Parameters.AddWithValue("@idUsuario", MainWindow.idUsuario);
                            comando.Prepare();
                        }
                    }
                }
            }catch(MySqlException e) {
                //Console.WriteLine(e);
            } finally {
                if(conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void uploadSubFolder(SubCarpeta p) {
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlCommand comando = new MySqlCommand("SELECT ruta FROM Carpeta WHERE ruta=@val1", conexion);
                comando.Parameters.AddWithValue("@val1", Lista.getRutaPadre(p.getRutaPrograma()));
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    comando.Parameters.Clear();
                    reader.Close();

                    comando = new MySqlCommand("SELECT ruta FROM Carpeta WHERE ruta=@val1",conexion);
                    comando.Parameters.AddWithValue("@val1", p.getRutaPrograma());
                    reader = comando.ExecuteReader();
                    if (!reader.HasRows) {
                        reader.Close();
                        comando.Parameters.Clear();

                        comando = new MySqlCommand("SELECT id FROM Serie WHERE rutaSerie=@rutaSerie", conexion);
                        comando.Parameters.AddWithValue("@rutaSerie", p.getSerie().getRuta());
                        reader = comando.ExecuteReader();

                        if (reader.HasRows) {
                            int idSerie = -1;
                            if (reader.Read()) {
                                idSerie = reader.GetInt32("id");
                                comando.Parameters.Clear();
                                reader.Close();
                                comando = new MySqlCommand("INSERT INTO Carpeta VALUES (null,@nombre,@idSerie,@numSubCarps,@ruta,@img,@isFolder,@idUsuario)", conexion);
                                comando.Parameters.AddWithValue("@nombre", p.getTitle());
                                comando.Parameters.AddWithValue("@idSerie", idSerie);
                                comando.Parameters.AddWithValue("@numSubCarps", p.getNumSubCarp());
                                comando.Parameters.AddWithValue("@ruta", p.getRutaPrograma());
                                comando.Parameters.AddWithValue("@img", p.getSerie().getDirImg());
                                comando.Parameters.AddWithValue("@isFolder", 0);
                                comando.Parameters.AddWithValue("@idUsuario", MainWindow.idUsuario);
                                comando.ExecuteNonQuery();
                                Console.WriteLine("Ejecutado guardado de subcarpeta");
                            }
                        }
                    }
                }
            } catch (MySqlException e) {
                Console.WriteLine(e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void uploadFile(object p) {
            
        }

        
    }
}
