using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data {
    public class ConexionOffline {
        public static List<PerfilClass> LoadProfiles() {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                var output = cnn.Query<PerfilClass>("select * from Perfil", new DynamicParameters());
                cnn.Close();
                return output.ToList();
            }
        }
        public static List<MenuClass> LoadMenus(PerfilClass profile) {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                MenuClass m = new MenuClass("", profile.id);
                var output = cnn.Query<MenuClass>("select * from Menu where idPerfil=@idPerfil", m);
                cnn.Close();
                return output.ToList();
            }
        }
        public static List<CarpetaClass> LoadCarpeta() {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                var output = cnn.Query<CarpetaClass>("select * from Carpeta", new DynamicParameters());
                cnn.Close();
                return output.ToList();
            }
        }

        public static List<CarpetaClass> LoadCarpetasFromMenu(MenuClass m) {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                CarpetaClass c = new CarpetaClass(m.id);
                var parameters = new { idMenu = m.id };
                var output = cnn.Query<CarpetaClass>("select * from Carpeta where idMenu=@idMenu and isFolder=0", parameters);
                cnn.Close();
                if (output.ToList().Count == 0) {
                    return null;
                } else {
                    return output.ToList();
                }
                
            }
        }

        public static List<CarpetaClass> loadSubCarpetasFromCarpeta(CarpetaClass c) {
            /*
            using (SQLiteConnection cnn = new SQLiteConnection(loadConnectionString())) {
                SQLiteCommand output = new SQLiteCommand("select * from Carpeta where idMenu=@idMenu and rutaPadre=@rutaPadre", cnn);
                output.Parameters.AddWithValue("@idMenu", c.menu);
                output.Parameters.AddWithValue("@rutaPadre", c.rutaPadre);
                SQLiteDataReader reader = output.ExecuteReader();
                List<CarpetaClass> carpetas = new List<CarpetaClass>();
                if (reader.HasRows) {
                    while (reader.Read()) {
                        carpetas.Add(new CarpetaClass((long)reader["id"],
                            (string)reader["nombre"],
                            (int)reader["numSubCarps"],
                            (int)reader["numArchivos"],
                            (string)reader["ruta"],
                            (string)reader["rutaPadre"],
                            (string)reader["desc"],
                            (string)reader["generos"],
                            (string)reader["img"],
                            (bool)reader["isFolder"],
                            (long)reader["idMenu"]));
                    }
                }

                cnn.Close();
                return carpetas;

            }*/
            
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                var parameters = new { idMenu = c.idMenu, rutaPadre = c.ruta };
                var output = cnn.Query<CarpetaClass>("select * from Carpeta where idMenu=@idMenu and rutaPadre=@rutaPadre and isFolder=1", parameters);
                cnn.Close();
                if (output.ToList().Count == 0) {
                    return null;
                } else {
                    return output.ToList();
                }

            }
        }

        public static List<ArchivoClass> LoadArchivo() {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                var output = cnn.Query<ArchivoClass>("select * from Archivo", new DynamicParameters());
                cnn.Close();
                return output.ToList();
            }
        }

        public static MenuClass getMenu(MenuClass mc) {
            /*
            using (SQLiteConnection cnn = new SQLiteConnection(loadConnectionString())) {
                SQLiteCommand output = new SQLiteCommand("select id from Menu where nombre=@nombre and idPerfil=@perfil",cnn);
                output.Parameters.AddWithValue("@idPerfil", mc.idPerfil);
                SQLiteDataReader reader = output.ExecuteReader();
                if (reader.HasRows) {
                    while (reader.Read()) {
                        mc.id = (long)reader["id"];
                        break;
                    }
                }
                
                cnn.Close();
                return mc;

            }
            */
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                var parameters = new { nombre = mc.nombre, idPerfil = mc.idPerfil};
                var output = cnn.Query<MenuClass>("select * from Menu where nombre=@nombre and idPerfil=@idPerfil", parameters);
                cnn.Close();
                if (output.ToList().Count == 0) {
                    return null;
                } else {
                    return output.ToList().First();
                }

            }

        }

        public static PerfilClass getPerfil(PerfilClass p) {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                var parameters = new { nombre = p.nombre};
                var output = cnn.Query<PerfilClass>("select * from Perfil where nombre=@nombre", parameters);
                cnn.Close();
                if (output.ToList().Count != 0) {
                    PerfilClass perfil = output.ToList().First<PerfilClass>();
                    return perfil;
                } else {
                    return null;
                }
            }
        }

        public static PerfilClass addProfile(PerfilClass profile) {
            try {
                using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                    cnn.Execute("insert into Perfil (nombre,numMenus,mode) values (@nombre,0,@mode)", profile);
                    Console.WriteLine("Añadido Perfil");
                    cnn.Close();
                    return getPerfil(profile);
                }
            } catch (Exception e) {
                Console.WriteLine("Clave Duplicada Profile");
            }
            return null;
        }
        public static MenuClass addMenu(MenuClass m) {
            try {
                using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                    cnn.Execute("insert into Menu (nombre,numCarps,idPerfil) values (@nombre,@numCarps,@idPerfil)", m);
                    cnn.Close();
                    Console.WriteLine("Añadido Menu");
                    return getMenu(m);
                    
                    
                }
            } catch (Exception e) {
                Console.WriteLine("Clave Duplicada Menu");
            }
            return null;
        }
        public static void addCarpeta(CarpetaClass carpeta) {
            try {
                using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                    Int64 esCarpeta = 0;
                    if (carpeta.isFolder) {
                        esCarpeta = 0;
                    } else {
                        esCarpeta = 1;
                    }
                    var parameters = new { nombre = carpeta.nombre, ruta = carpeta.ruta, rutaPadre = carpeta.rutaPadre, numSubCarps = carpeta.numSubCarps, numArchivos = carpeta.numArchivos, desc = carpeta.desc,
                        img = carpeta.img, generos = carpeta.getGeneros(), isFolder = esCarpeta, idMenu = carpeta.idMenu};
                    Console.WriteLine("--------------- " + carpeta.generos.ToString());
                    cnn.Execute("insert into Carpeta (nombre,ruta,rutaPadre,numSubCarps,numArchivos,desc,img,generos,isFolder,idMenu) values (@nombre,@ruta,@rutaPadre,@numSubCarps,@numArchivos,@desc,@img,@generos,@isFolder,@idMenu)", parameters);
                    Console.WriteLine("Añadido Carpeta");
                    
                    cnn.Close();
                    getCarpeta(carpeta);
                }

            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        public static void getCarpeta(CarpetaClass c) {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                var parameters = new { ruta = c.ruta , fkMenu= c.idMenu};
                var output = cnn.Query<CarpetaClass>("select * from Carpeta where ruta=@ruta and idMenu=@fkMenu", parameters);
                cnn.Close();
                if (output.ToList().Count != 0) {
                    CarpetaClass carpeta = output.ToList().First<CarpetaClass>();
                    c.id = carpeta.id;
                } else {
                    c.id = 0;
                }
            }
        }

        public static void addArchivo(ArchivoClass archivo) {
            try {
                using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                    cnn.Execute("insert into Archivo (nombre,rutaSistema,rutaPrograma,img,idCarpeta) values (@nombre,@rutaSistema,@rutaPrograma,@img,@idCarpeta)", archivo);
                    Console.WriteLine("Añadido Archivo");
                    cnn.Close();
                }
            } catch (Exception e) {
                Console.WriteLine("Clave Duplicada Archivo");
            }
        }

        private static string loadConnectionString(string id = "Default") {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static void removeProfile(int id) {
            try {
                using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                    cnn.Execute("delete from Profile where id=" + id);
                    Console.WriteLine("Borrado Perfil");
                    cnn.Close();
                }
            } catch (Exception e) {
                Console.WriteLine(e);

            }
        }

        public static void updateMode(long modeFolder, PerfilClass profile) {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                var parameters = new { mode = modeFolder, idPerfil = profile.id };
                var output = cnn.Query<CarpetaClass>("UPDATE perfil set mode=@mode where id=@idPerfil", parameters);
                cnn.Close();
            }
        }
    }
}
