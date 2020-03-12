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

        public static PerfilClassOnline addPerfil(PerfilClassOnline p) {
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id FROM Perfil WHERE nombre=@nombre and fk_Usuario=@usuario", conexion);
                comando.Parameters.AddWithValue("@nombre", p.nombre);
                comando.Parameters.AddWithValue("@usuario", VIGallery.getUser().id);
                MySqlDataReader reader = comando.ExecuteReader();

                if (!reader.HasRows) {
                    reader.Close();

                    comando = new MySqlCommand("INSERT INTO Perfil VALUES (null, @nombrePerfil, @mode, @numMenus, @fkUsuario)", conexion);
                    comando.Transaction = myTrans;

                    comando.Parameters.AddWithValue("@nombreMenu", p.nombre);
                    comando.Parameters.AddWithValue("@fkPerfil", VIGallery.getUser().id);
                    comando.ExecuteNonQuery();
                    myTrans.Commit();

                    comando.Parameters.Clear();
                    reader.Close();
                    comando = new MySqlCommand("SELECT id FROM Menu WHERE nombre=@nombre and fk_Usuario=@fkUsuario", conexion);
                    comando.Parameters.AddWithValue("@nombre", p.nombre);
                    comando.Parameters.AddWithValue("@fkUsuario", p.idUsuario);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows) {
                        reader.Read();
                        Int32 id = (Int32)reader["id"];
                        p.id = id;
                        return p;
                    }

                    return p;
                }

            } catch (MySqlException e) {
                Console.WriteLine("Perfil  \n" + e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static MenuClass addMenu(MenuClass m) {
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
                    comando.Parameters.AddWithValue("@fkPerfil", m.idPerfil);
                    comando.ExecuteNonQuery();
                    myTrans.Commit();

                    comando.Parameters.Clear();
                    reader.Close();
                    comando = new MySqlCommand("SELECT id FROM Menu WHERE nombre=@nombre and fk_Perfil=@perfil", conexion);
                    comando.Parameters.AddWithValue("@nombre", m.nombre);
                    comando.Parameters.AddWithValue("@perfil", m.idPerfil);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows) {
                        reader.Read();
                        Int32 id = (Int32)reader["id"];
                        m.id = id;
                        return m;
                    }

                    return m;
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

        public static void saveFolder(Carpeta p) {
            MySqlConnection conexion = null;
            MySqlTransaction myTrans = null;
            try {
                conexion = getConnection();
                conexion.Open();

                myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id FROM Carpeta WHERE ruta=@ruta and fk_Menu=@idMenu", conexion);
                comando.Parameters.AddWithValue("@ruta", p.getClass().rutaPrograma);
                comando.Parameters.AddWithValue("@idMenu", p.getClass().menu);
                MySqlDataReader reader = comando.ExecuteReader();
                if (!reader.HasRows) {
                    comando.Parameters.Clear();
                    reader.Close();

                    comando = new MySqlCommand("CALL insertCarpeta(@nombre,0,0,@ruta,@rutaPadre,@descripcion,@generos,@img,@isFolder,@idMenu)", conexion);
                    comando.Transaction = myTrans;
                    comando.Parameters.AddWithValue("@nombre", p.getTitle());
                    comando.Parameters.AddWithValue("@ruta", p.getClass().rutaPrograma);
                    comando.Parameters.AddWithValue("@rutaPadre", p.getClass().rutaPadre);
                    comando.Parameters.AddWithValue("@descripcion", p.getClass().desc);
                    comando.Parameters.AddWithValue("@generos", p.getClass().generos.ToString());
                    comando.Parameters.AddWithValue("@img", p.getClass().img);
                    comando.Parameters.AddWithValue("@isFolder", true);
                    comando.Parameters.AddWithValue("@idMenu", p.getClass().menu);
                    comando.ExecuteNonQuery();
                    myTrans.Commit();

                    comando.Parameters.Clear();
                    comando = new MySqlCommand("SELECT id FROM carpeta where ruta=@ruta and fk_Menu=@idMenu", conexion);
                    comando.Parameters.AddWithValue("@ruta", p.getClass().rutaPrograma);
                    comando.Parameters.AddWithValue("@idMenu", p.getClass().menu);
                    reader = comando.ExecuteReader();
                    if (!reader.HasRows) {
                        reader.Read();
                        Int32 id = (Int32)reader["id"];
                        p.getClass().id = id;
                        reader.Close();
                        comando.Parameters.Clear();
                    }
                }
            }catch(MySqlException e) {
                if (myTrans != null) {
                    myTrans.Rollback();
                }
                Console.WriteLine("Carpeta  \n"+e);
            } finally {
                if(conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void saveSubFolder(SubCarpeta p) {
            MySqlConnection conexion = null;
            MySqlTransaction myTrans = null;
            try {
                conexion = getConnection();
                conexion.Open();

                myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT ruta FROM Carpeta WHERE ruta=@rutaPadre and fk_Menu=@idMenu", conexion);
                comando.Parameters.AddWithValue("@rutaPadre", p.getClass().rutaPadre);
                comando.Parameters.AddWithValue("@idMenu", p.getClass().menu);
                MySqlDataReader reader = comando.ExecuteReader();

                

                if (reader.HasRows) {
                    comando.Parameters.Clear();
                    reader.Close();

                    comando = new MySqlCommand("SELECT ruta FROM Carpeta WHERE ruta=@ruta and fk_Menu=@idMenu", conexion);
                    comando.Parameters.AddWithValue("@ruta", p.getClass().rutaPrograma);
                    comando.Parameters.AddWithValue("@idMenu", p.getClass().menu);
                    reader = comando.ExecuteReader();
                    if (!reader.HasRows) {
                        reader.Close();
                        comando.Parameters.Clear();

                        comando = new MySqlCommand("CALL insertCarpeta(@nombre,0,0,@ruta,@rutaPadre,@descripcion,@generos,@img,@isFolder,@idMenu)", conexion);
                        comando.Transaction = myTrans;
                        comando.Parameters.AddWithValue("@nombre", p.getClass().nombre);
                        comando.Parameters.AddWithValue("@ruta", p.getClass().rutaPrograma);
                        comando.Parameters.AddWithValue("@rutaPadre", p.getClass().rutaPadre);
                        comando.Parameters.AddWithValue("@descripcion", p.getClass().desc);
                        comando.Parameters.AddWithValue("@generos", p.getClass().generos.ToString());
                        comando.Parameters.AddWithValue("@img", p.getClass().img);
                        comando.Parameters.AddWithValue("@isFolder", false);
                        comando.Parameters.AddWithValue("@idMenu", p.getClass().menu);
                        comando.ExecuteNonQuery();
                        myTrans.Commit();

                        comando.Parameters.Clear();
                        comando = new MySqlCommand("SELECT id FROM carpeta where ruta=@ruta and fk_Menu=@idMenu", conexion);
                        comando.Parameters.AddWithValue("@ruta", p.getClass().rutaPrograma);
                        comando.Parameters.AddWithValue("@idMenu", p.getClass().menu);
                        reader = comando.ExecuteReader();
                        if (!reader.HasRows) {
                            reader.Read();
                            Int32 id = (Int32)reader["id"];
                            p.getClass().id = id;
                            reader.Close();
                            comando.Parameters.Clear();
                        }
                    }
                }
            } catch (MySqlException e) {
                if (myTrans != null) {
                    myTrans.Rollback();
                }
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void saveFile(object p) {
            
        }

        public static List<PerfilClass> loadPerfiles(long id) {
            List<PerfilClass> perfiles = new List<PerfilClass>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,numMenus,mode FROM Perfil WHERE fk_Usuario=@idUsuario", conexion);
                comando.Parameters.AddWithValue("@idUsuario", id);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idPerfil = (Int32)reader["id"];
                        Int32 numMenus = (Int32)reader["numMenus"];
                        Int32 mode = (Int32)reader["mode"];
                        PerfilOnline perfil = new PerfilOnline((long)idPerfil, reader["nombre"].ToString(), (long)numMenus, mode, id);
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

        public static List<MenuClass> loadMenus(long id) {
            List<MenuClass> menus = new List<MenuClass>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,numCarps FROM Menu WHERE fk_Perfil=@fkPerfil", conexion);
                comando.Parameters.AddWithValue("@fkPerfil", id);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idMenu = (Int32)reader["id"];
                        Int32 numCarp = (Int32)reader["numCarps"];
                        MenuClass menu = new MenuClass((long)idMenu, reader["nombre"].ToString(), (long)numCarp, id);
                        menus.Add(menu);
                    }
                    reader.Close();
                    conexion.Close();
                    return menus;
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

        public static List<CarpetaClass> loadCarpetasFromMenu(long id) {
            List<CarpetaClass> carpetas = new List<CarpetaClass>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,numSubCarps,numArchivos,ruta,rutaPadre,descripcion,generos,img FROM Carpeta WHERE fk_Menu=@fkMenu and esCarpeta=true", conexion);
                comando.Parameters.AddWithValue("@fkMenu", id);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idCarpeta = (Int32)reader["id"];
                        Int32 numSubCarp = (Int32)reader["numSubCarps"];
                        Int32 numArchivos = (Int32)reader["numArchivos"];
                        ICollection<string> generos = new List<string>();
                        CarpetaClass carpeta = new CarpetaClass((long)idCarpeta, reader["nombre"].ToString(),numSubCarp,numArchivos, reader["ruta"].ToString(), reader["rutaPadre"].ToString(),
                            reader["descripcion"].ToString(),generos, reader["img"].ToString(), true, id);
                        carpetas.Add(carpeta);
                    }
                    reader.Close();
                    conexion.Close();
                    return carpetas;
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

        public static List<CarpetaClass> loadSubCarpetasFromCarpeta(string ruta,long id) {
            List<CarpetaClass> carpetas = new List<CarpetaClass>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,numSubCarps,numArchivos,ruta,rutaPadre,descripcion,generos,img FROM Carpeta WHERE fk_Menu=@fkMenu and rutaPadre=@rutaPadre", conexion);
                comando.Parameters.AddWithValue("@fkMenu", id);
                comando.Parameters.AddWithValue("@rutaPadre", ruta);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idCarpeta = (Int32)reader["id"];
                        Int32 numSubCarp = (Int32)reader["numSubCarps"];
                        Int32 numArchivos = (Int32)reader["numArchivos"];
                        ICollection<string> generos = new List<string>();
                        CarpetaClass carpeta = new CarpetaClass((long)idCarpeta, reader["nombre"].ToString(), numSubCarp, numArchivos, reader["ruta"].ToString(), reader["rutaPadre"].ToString(),
                            reader["descripcion"].ToString(), generos, reader["img"].ToString(), true, id);
                        carpetas.Add(carpeta);
                    }
                    reader.Close();
                    conexion.Close();
                    return carpetas;
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

                if (reader.HasRows) {
                    reader.Read();
                    Int32 id = (Int32)reader["id"];
                    user = new UsuarioClass((long)id, reader["nombre"].ToString(), reader["apellidos"].ToString(), reader["nick"].ToString(), reader["email"].ToString());
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

        public static void updateMode(int mode,PerfilClass p) {
            MySqlConnection conexion = null;
            MySqlTransaction myTrans = null;
            try {
                conexion = getConnection();
                conexion.Open();

                myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("UPDATE perfil set mode=@mode where id=@idPerfil", conexion);
                comando.Parameters.AddWithValue("@mode", mode);
                comando.Parameters.AddWithValue("@idPerfil", p.id);
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
