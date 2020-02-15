using System;
using System.Collections.Generic;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using ProyectoWPF.Data;

namespace ProyectoWPF.Data {
    public static class Conexion {

        public static MySqlConnection getConnection() {
            MySqlConnection conexion = new MySqlConnection("datasource=127.0.0.1;port=3306;username=VI_Gallery;password=vi_gallery;database=vi_gallery");
            return conexion;
        }

        public static void addMenu(MenuClass m) {
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id FROM Menu WHERE nombre=@nombre and fk_Perfil=@perfil", conexion);
                comando.Parameters.AddWithValue("@nombre", m.nombre);
                comando.Parameters.AddWithValue("@perfil", m.idPerfil);
                MySqlDataReader reader = comando.ExecuteReader();

                if (!reader.HasRows) {
                    reader.Close();

                    comando = new MySqlCommand("INSERT INTO Menu VALUES (null, @nombreMenu, 0, @fkPerfil)", conexion);
                    comando.Transaction = myTrans;

                    comando.Parameters.AddWithValue("@nombreMenu", m.nombre);
                    comando.Parameters.AddWithValue("@numCarp", 0);
                    comando.Parameters.AddWithValue("@fkPerfil", m.idPerfil);
                    comando.ExecuteNonQuery();
                    myTrans.Commit();

                    comando.Parameters.Clear();
                    reader.Close();
                }

            } catch (MySqlException e) {
                Console.WriteLine("Boton  \n" + e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void uploadFolder(Carpeta p) {
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans=conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id FROM Carpeta WHERE ruta=@ruta",conexion);
                comando.Parameters.AddWithValue("@ruta", p.getClass().rutaPrograma);
                MySqlDataReader reader = comando.ExecuteReader();
                if (!reader.HasRows) {
                    comando.Parameters.Clear();
                    reader.Close();

                    comando = new MySqlCommand("INSERT INTO Carpeta VALUES (null,@nombre,0,0,@ruta,@rutaPadre,@descripcion,@img,@isFolder,@idMenu)", conexion);
                    comando.Transaction = myTrans;
                    comando.Parameters.AddWithValue("@nombre", p.getTitle());
                    comando.Parameters.AddWithValue("@ruta", p.getClass().rutaPrograma);
                    comando.Parameters.AddWithValue("@ruta", p.getClass().rutaPadre);
                    comando.Parameters.AddWithValue("@descripcion", p.getClass().desc);
                    comando.Parameters.AddWithValue("@img", p.getClass().img);
                    comando.Parameters.AddWithValue("@isFolder", true);
                    comando.Parameters.AddWithValue("@idMenu", p.getClass().menu);
                    comando.ExecuteNonQuery();

                    myTrans.Commit();
                }
            }catch(MySqlException e) {
                Console.WriteLine("Carpeta  \n"+e);
            } finally {
                if(conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void saveSubFolder(SubCarpeta p) {
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT ruta FROM Carpeta WHERE ruta=@rutaPadre", conexion);
                comando.Parameters.AddWithValue("@rutaPadre", p.getClass().rutaPadre);
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

                        comando = new MySqlCommand("INSERT INTO Carpeta VALUES (null,@nombre,0,0,@ruta,@rutaPadre,@descripcion,@img,@isFolder,@idMenu)", conexion);
                        comando.Transaction = myTrans;
                        comando.Parameters.AddWithValue("@nombre", p.getTitle());
                        comando.Parameters.AddWithValue("@ruta", p.getClass().rutaPrograma);
                        comando.Parameters.AddWithValue("@ruta", p.getClass().rutaPadre);
                        comando.Parameters.AddWithValue("@descripcion", p.getClass().desc);
                        comando.Parameters.AddWithValue("@img", p.getClass().img);
                        comando.Parameters.AddWithValue("@isFolder", true);
                        comando.Parameters.AddWithValue("@idMenu", p.getClass().menu);
                        comando.ExecuteNonQuery();
                        myTrans.Commit();
                        Console.WriteLine("Ejecutado guardado de subcarpeta");
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

        public static List<PerfilOnline> loadPerfiles(long id) {
            List<PerfilOnline> perfiles = new List<PerfilOnline>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,numMenus,fk_Usuario FROM Perfil WHERE fk_Usuario=@idUsuario", conexion);
                comando.Parameters.AddWithValue("@idUsuario", id);
                MySqlDataReader reader = comando.ExecuteReader();

                if (!reader.HasRows) {
                    while (reader.Read()) {
                        PerfilOnline perfil = new PerfilOnline((long)reader["id"], reader["nombre"].ToString(), (long)reader["numMenus"],(long)reader["fk_Usuario"]);
                        perfiles.Add(perfil);
                    }
                    reader.Close();
                    conexion.Close();
                    return perfiles;
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

        public static UsuarioClass checkUser(string nombre, string pass) {
            UsuarioClass user = null;
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,apellidos,nick,email FROM Usuario WHERE nick=@nick and pass=@pass", conexion);
                comando.Parameters.AddWithValue("@nick", nombre);
                comando.Parameters.AddWithValue("@pass", pass);
                MySqlDataReader reader = comando.ExecuteReader();

                if (!reader.HasRows) {
                    user = new UsuarioClass((long)reader["id"], reader["nombre"].ToString(), reader["apellidos"].ToString(), reader["nick"].ToString(), reader["email"].ToString());
                    reader.Close();
                }
                conexion.Close();
                return user;
            } catch (MySqlException e) {
                Console.WriteLine("Boton  \n" + e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

    }
}
