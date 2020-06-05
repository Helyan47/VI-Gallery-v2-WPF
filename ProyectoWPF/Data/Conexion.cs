using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using SeleccionarProfile.Data;

namespace SeleccionarProfile.Data {
    public static class Conexion {

        public static MySqlConnection getConnection() {
            MySqlConnection conexion = new MySqlConnection("datasource=127.0.0.1;port=3306;username=VI_Gallery;password=vi_gallery;database=vi_gallery");
            return conexion;
        }

        public static string saveUser(string name, string pass) {
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id FROM Usuario WHERE nick=@nombre and pass=@pass", conexion);
                comando.Parameters.AddWithValue("@nombre", name);
                comando.Parameters.AddWithValue("@pass", pass);
                MySqlDataReader reader = comando.ExecuteReader();

                if (!reader.HasRows) {
                    reader.Close();

                    comando = new MySqlCommand("INSERT INTO Usuario VALUES (null, @nombre, @pass)", conexion);
                    comando.Transaction = myTrans;

                    comando.Parameters.AddWithValue("@nombre", name);
                    comando.Parameters.AddWithValue("@pass", pass);
                    comando.ExecuteNonQuery();
                    myTrans.Commit();

                    comando.Parameters.Clear();
                    reader.Close();
                    return "Usuario creado";
                } else {
                    return "Usuario existe";
                }

            } catch (MySqlException e) {
                Console.WriteLine("No se ha podido añadir el usuario:\n" + e);
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
                throw e;
            }
            return "Usuario no se ha podido crear";
        }

        public static PerfilClassOnline saveProfile(PerfilClassOnline p) {
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

                    comando.Parameters.AddWithValue("@nombrePerfil", p.nombre);
                    comando.Parameters.AddWithValue("@mode", 0);
                    comando.Parameters.AddWithValue("@numMenus", 0);
                    comando.Parameters.AddWithValue("@fkUsuario", VIGallery.getUser().id);
                    comando.ExecuteNonQuery();
                    myTrans.Commit();

                    comando.Parameters.Clear();
                    reader.Close();
                    comando = new MySqlCommand("SELECT id FROM Perfil WHERE nombre=@nombre and fk_Usuario=@fkUsuario", conexion);
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
                Console.WriteLine("No se ha podido añadir el perfil:\n" + e);
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static MenuClass saveMenu(MenuClass m) {
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
                Console.WriteLine("No se ha podido añadir el menu:\n" + e);
                throw e;
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
                comando.Parameters.AddWithValue("@ruta", p.getClass().ruta);
                comando.Parameters.AddWithValue("@idMenu", p.getClass().idMenu);
                MySqlDataReader reader = comando.ExecuteReader();
                if (!reader.HasRows) {
                    comando.Parameters.Clear();
                    reader.Close();

                    comando = new MySqlCommand("CALL insertCarpeta(@nombre,0,0,@ruta,@rutaPadre,@descripcion,@generos,@img,@isFolder,@idMenu)", conexion);
                    comando.Transaction = myTrans;
                    comando.Parameters.AddWithValue("@nombre", p.getTitle());
                    comando.Parameters.AddWithValue("@ruta", p.getClass().ruta);
                    comando.Parameters.AddWithValue("@rutaPadre", p.getClass().rutaPadre);
                    comando.Parameters.AddWithValue("@descripcion", p.getClass().desc);
                    comando.Parameters.AddWithValue("@generos", p.getClass().getGeneros());
                    comando.Parameters.AddWithValue("@img", p.getClass().img);
                    comando.Parameters.AddWithValue("@isFolder", true);
                    comando.Parameters.AddWithValue("@idMenu", p.getClass().idMenu);
                    comando.ExecuteNonQuery();
                    myTrans.Commit();

                    comando.Parameters.Clear();
                    comando = new MySqlCommand("UPDATE Menu set numCarps=numCarps+1 where id=@id", conexion);
                    comando.Parameters.AddWithValue("@id", p.getClass().idMenu);
                    comando.ExecuteNonQuery();

                    comando.Parameters.Clear();
                    comando = new MySqlCommand("SELECT id FROM carpeta where ruta=@ruta and fk_Menu=@idMenu", conexion);
                    comando.Parameters.AddWithValue("@ruta", p.getClass().ruta);
                    comando.Parameters.AddWithValue("@idMenu", p.getClass().idMenu);
                    reader = comando.ExecuteReader();
                    if (reader.HasRows) {
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
                Console.WriteLine("No se ha podido añadir la carpeta:\n"+e);
                throw e;
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
                comando.Parameters.AddWithValue("@idMenu", p.getClass().idMenu);
                MySqlDataReader reader = comando.ExecuteReader();

                

                if (reader.HasRows) {
                    comando.Parameters.Clear();
                    reader.Close();

                    comando = new MySqlCommand("SELECT ruta FROM Carpeta WHERE ruta=@ruta and fk_Menu=@idMenu", conexion);
                    comando.Parameters.AddWithValue("@ruta", p.getClass().ruta);
                    comando.Parameters.AddWithValue("@idMenu", p.getClass().idMenu);
                    reader = comando.ExecuteReader();
                    if (!reader.HasRows) {
                        reader.Close();
                        comando.Parameters.Clear();

                        comando = new MySqlCommand("CALL insertCarpeta(@nombre,0,0,@ruta,@rutaPadre,@descripcion,@generos,@img,@isFolder,@idMenu)", conexion);
                        comando.Transaction = myTrans;
                        comando.Parameters.AddWithValue("@nombre", p.getClass().nombre);
                        comando.Parameters.AddWithValue("@ruta", p.getClass().ruta);
                        comando.Parameters.AddWithValue("@rutaPadre", p.getClass().rutaPadre);
                        comando.Parameters.AddWithValue("@descripcion", p.getClass().desc);
                        comando.Parameters.AddWithValue("@generos", p.getClass().getGeneros());
                        comando.Parameters.AddWithValue("@img", p.getClass().img);
                        comando.Parameters.AddWithValue("@isFolder", false);
                        comando.Parameters.AddWithValue("@idMenu", p.getClass().idMenu);
                        comando.ExecuteNonQuery();
                        myTrans.Commit();

                        comando.Parameters.Clear();
                        comando = new MySqlCommand("UPDATE Carpeta set numSubCarps=numSubCarps+1 where ruta=@rutaPadre", conexion);
                        comando.Parameters.AddWithValue("@rutaPadre", p.getClass().rutaPadre);
                        comando.ExecuteNonQuery();

                        comando.Parameters.Clear();
                        comando = new MySqlCommand("SELECT id FROM carpeta where ruta=@ruta and fk_Menu=@idMenu", conexion);
                        comando.Parameters.AddWithValue("@ruta", p.getClass().ruta);
                        comando.Parameters.AddWithValue("@idMenu", p.getClass().idMenu);
                        reader = comando.ExecuteReader();
                        if (reader.HasRows) {
                            reader.Read();
                            Int32 id = (Int32)reader["id"];
                            p.getClass().id = id;
                            reader.Close();
                            comando.Parameters.Clear();
                        }
                    }
                }
            } catch (MySqlException e) {
                Console.WriteLine("No se ha podido añadir la subcarpeta:\n" + e);
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void saveFile(ArchivoClass ac) {
            MySqlConnection conexion = null;
            MySqlTransaction myTrans = null;
            try {
                conexion = getConnection();
                conexion.Open();

                myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id FROM Archivo WHERE rutaPrograma=@ruta and fk_Carpeta=@idCarpeta", conexion);
                comando.Parameters.AddWithValue("@ruta", ac.rutaPrograma);
                comando.Parameters.AddWithValue("@idCarpeta", ac.idCarpeta);
                MySqlDataReader reader = comando.ExecuteReader();
                if (!reader.HasRows) {
                    comando.Parameters.Clear();
                    reader.Close();

                    comando = new MySqlCommand("INSERT INTO Archivo VALUES (null, @nombre, @ruta, @rutaPrograma, @img, @idCarpeta)", conexion);
                    comando.Transaction = myTrans;
                    comando.Parameters.AddWithValue("@nombre", ac.nombre);
                    comando.Parameters.AddWithValue("@ruta", ac.rutaSistema);
                    comando.Parameters.AddWithValue("@rutaPrograma", ac.rutaPrograma);
                    comando.Parameters.AddWithValue("@img", ac.img);
                    comando.Parameters.AddWithValue("@idCarpeta", ac.idCarpeta);
                    comando.ExecuteNonQuery();
                    myTrans.Commit();

                    comando.Parameters.Clear();
                    comando = new MySqlCommand("UPDATE Carpeta set numArchivos=numArchivos+1 where id=@idCarpeta", conexion);
                    comando.Parameters.AddWithValue("@idCarpeta", ac.idCarpeta);
                    comando.ExecuteNonQuery();

                    comando.Parameters.Clear();
                    comando = new MySqlCommand("SELECT id FROM Archivo where rutaPrograma=@ruta and fk_Carpeta=@idCarpeta", conexion);
                    comando.Parameters.AddWithValue("@ruta", ac.rutaPrograma);
                    comando.Parameters.AddWithValue("@idCarpeta", ac.idCarpeta);
                    reader = comando.ExecuteReader();
                    if (!reader.HasRows) {
                        reader.Read();
                        Int32 id = (Int32)reader["id"];
                        ac.id = id;
                        reader.Close();
                        comando.Parameters.Clear();
                    }
                }
            } catch (MySqlException e) {
                Console.WriteLine("No se ha podido añadir " + ac.nombre);
                Console.WriteLine("Archivo " + e);
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static List<PerfilClass> loadProfiles(long id) {
            List<PerfilClass> perfiles = null;
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,numMenus,mode FROM Perfil WHERE fk_Usuario=@idUsuario", conexion);
                comando.Parameters.AddWithValue("@idUsuario", id);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    perfiles = new List<PerfilClass>();
                    while (reader.Read()) {
                        Int32 idPerfil = (Int32)reader["id"];
                        Int32 numMenus = (Int32)reader["numMenus"];
                        Int32 mode = (Int32)reader["mode"];
                        PerfilClassOnline perfil = new PerfilClassOnline((long)idPerfil, reader["nombre"].ToString(), mode, (long)numMenus, id);
                        perfiles.Add(perfil);
                    }
                    reader.Close();
                }

            } catch (MySqlException e) {
                Console.WriteLine("Error al cargar los perfiles:\n" + e);
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return perfiles;
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
                Console.WriteLine("Error al cargar los menus:+\n" + e);
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static List<CarpetaClass> loadFoldersFromMenu(long id) {
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
                        CarpetaClass carpeta = new CarpetaClass((long)idCarpeta, reader["nombre"].ToString(), reader["ruta"].ToString(), reader["rutaPadre"].ToString(), numSubCarp, numArchivos,
                            reader["descripcion"].ToString(), reader["img"].ToString(), reader["generos"].ToString(), true, id);
                        carpetas.Add(carpeta);
                    }
                    reader.Close();
                    conexion.Close();
                    return carpetas;
                }

            } catch (MySqlException e) {
                Console.WriteLine("Error al cargar Carpetas de un Menu:\n" + e);
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static List<CarpetaClass> loadSubFoldersFromCarpeta(CarpetaClass c,long id) {
            List<CarpetaClass> carpetas = new List<CarpetaClass>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nombre,numSubCarps,numArchivos,ruta,rutaPadre,descripcion,generos,img FROM Carpeta WHERE fk_Menu=@fkMenu and rutaPadre=@rutaPadre", conexion);
                comando.Parameters.AddWithValue("@fkMenu", id);
                comando.Parameters.AddWithValue("@rutaPadre", c.ruta);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idCarpeta = (Int32)reader["id"];
                        Int32 numSubCarp = (Int32)reader["numSubCarps"];
                        Int32 numArchivos = (Int32)reader["numArchivos"];
                        CarpetaClass carpeta = new CarpetaClass((long)idCarpeta, reader["nombre"].ToString(),  reader["ruta"].ToString(), reader["rutaPadre"].ToString(), numSubCarp, numArchivos,
                            reader["descripcion"].ToString(),  reader["img"].ToString(), reader["generos"].ToString(), false, id);
                        carpetas.Add(carpeta);
                    }
                    reader.Close();
                    conexion.Close();
                    return carpetas;
                }

            } catch (MySqlException e) {
                Console.WriteLine("Error al cargar Subcarpeta: \n" + e);
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static List<ArchivoClass> loadFiles(long idCarpeta) {
            List<ArchivoClass> archivos = new List<ArchivoClass>();
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT a.id as id,a.nombre as nombre,a.rutaSistema as rutaSistema,a.rutaPrograma as rutaPrograma, a.img as img " +
                    "FROM Archivo a, Carpeta c WHERE c.id=a.fk_Carpeta and c.id=@fkCarpeta", conexion);
                comando.Parameters.AddWithValue("@fkCarpeta", idCarpeta);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    while (reader.Read()) {
                        Int32 idArchivo = (Int32)reader["id"];
                        ICollection<string> generos = new List<string>();
                        ArchivoClass archivo = new ArchivoClass((long)idArchivo, reader["nombre"].ToString(), reader["rutaSistema"].ToString(), reader["rutaPrograma"].ToString(), reader["img"].ToString(), idCarpeta);
                        archivos.Add(archivo);
                    }
                    reader.Close();
                    conexion.Close();
                    return archivos;
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

        public static void deleteProfile(long id) {
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlCommand comando = new MySqlCommand("DELETE FROM Perfil WHERE id=@id", conexion);
                comando.Parameters.AddWithValue("@id", id);
                comando.ExecuteNonQuery();
                



            } catch (MySqlException e) {
                Console.WriteLine("No se ha podido borrar el perfil");
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void deleteMenu(long id) {
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlCommand comando = new MySqlCommand("DELETE FROM Menu WHERE id=@id", conexion);
                comando.Parameters.AddWithValue("@id", id);
                comando.ExecuteNonQuery();


            } catch (MySqlException e) {
                Console.WriteLine("No se ha podido borrar el menu");
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void deleteFolder(CarpetaClass c) {
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlCommand comando = new MySqlCommand("DELETE FROM Carpeta where id=@id", conexion);
                comando.Parameters.AddWithValue("@id", c.id);
                comando.ExecuteNonQuery();

                comando.Parameters.Clear();
                if (c.isFolder) {
                    comando = new MySqlCommand("UPDATE Menu set numCarps=numCarps-1 where id=@id", conexion);
                    comando.Parameters.AddWithValue("@id", c.idMenu);
                    comando.ExecuteNonQuery();
                } else {
                    comando = new MySqlCommand("UPDATE Carpeta set numSubCarps=numSubCarps-1 where ruta=@rutaPadre", conexion);
                    comando.Parameters.AddWithValue("@rutaPadre", c.rutaPadre);
                    comando.ExecuteNonQuery();
                }
                




            } catch (MySqlException e) {
                Console.WriteLine("No se ha podido borrar la carpeta \n"+e);
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void deleteFile(ArchivoClass a) {
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlCommand comando = new MySqlCommand("DELETE FROM Archivo where id=@id", conexion);
                comando.Parameters.AddWithValue("@id", a.id);
                comando.ExecuteNonQuery();

                comando.Parameters.Clear();
                comando = new MySqlCommand("UPDATE Carpeta set numArchivos=numArchivos-1 where id=@idCarpeta", conexion);
                comando.Parameters.AddWithValue("@idCarpeta", a.idCarpeta);
                comando.ExecuteNonQuery();


            } catch (MySqlException e) {
                Console.WriteLine("No se ha podido borrar la carpeta \n" + e);
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static UsuarioClass checkUser(string nombre, string pass) {
            UsuarioClass user = null;
            MySqlConnection conexion = null;
            try {
                conexion = getConnection();
                conexion.Open();

                MySqlTransaction myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("SELECT id,nick FROM Usuario WHERE nick=@nick and pass=@pass", conexion);
                comando.Parameters.AddWithValue("@nick", nombre);
                comando.Parameters.AddWithValue("@pass", pass);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows) {
                    reader.Read();
                    Int32 id = (Int32)reader["id"];
                    user = new UsuarioClass((long)id, reader["nick"].ToString());
                    reader.Close();
                }
                conexion.Close();
                return user;
            } catch (MySqlException e) {
                Console.WriteLine("Error al comprobar el usuario  \n" + e);
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
            return null;
        }

        public static void updateMode(long mode,PerfilClass p) {
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
                Console.WriteLine("Error al actualizar el modo: \n"+e);
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void updateFolderName(CarpetaClass c) {
            MySqlConnection conexion = null;
            MySqlTransaction myTrans = null;
            try {
                conexion = getConnection();
                conexion.Open();

                myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("UPDATE Carpeta set nombre = @nombre, ruta = @ruta, rutaPadre = @rutaPadre where id = @idCarpeta", conexion);
                comando.Parameters.AddWithValue("@nombre", c.nombre);
                comando.Parameters.AddWithValue("@ruta", c.ruta);
                comando.Parameters.AddWithValue("@rutaPadre", c.rutaPadre);
                comando.Parameters.AddWithValue("@idCarpeta", c.id);
                comando.ExecuteNonQuery();

                myTrans.Commit();
            } catch (MySqlException e) {
                if (myTrans != null) {
                    myTrans.Rollback();
                }
                Console.WriteLine("No se ha podido actualiza el nombre de la carpeta:\n"+e);
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

        public static void updateFile(ArchivoClass c) {
            MySqlConnection conexion = null;
            MySqlTransaction myTrans = null;
            try {
                conexion = getConnection();
                conexion.Open();

                myTrans = conexion.BeginTransaction();

                MySqlCommand comando = new MySqlCommand("UPDATE Archivo set nombre=@nombre, rutaPrograma=@ruta where id=@idArchivo", conexion);
                comando.Parameters.AddWithValue("@nombre", c.nombre);
                comando.Parameters.AddWithValue("@ruta", c.rutaPrograma);
                comando.Parameters.AddWithValue("@idArchivo", c.id);
                comando.ExecuteNonQuery();

                myTrans.Commit();
            } catch (MySqlException e) {
                if (myTrans != null) {
                    myTrans.Rollback();
                }
                Console.WriteLine("No se ha podido cambiar el nombre del archivo:\n"+e);
                throw e;
            } finally {
                if (conexion != null) {
                    conexion.Close();
                }
            }
        }

    }
}
