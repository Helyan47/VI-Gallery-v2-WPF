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
        public static List<PerfilClass> LoadProfile() {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                var output = cnn.Query<PerfilClass>("select * from Perfil", new DynamicParameters());
                return output.ToList();
            }
        }
        public static List<MenuClass> LoadMenu() {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                var output = cnn.Query<MenuClass>("select * from Menu", new DynamicParameters());
                return output.ToList();
            }
        }
        public static List<CarpetaClass> LoadCarpeta() {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                var output = cnn.Query<CarpetaClass>("select * from Carpeta", new DynamicParameters());
                return output.ToList();
            }
        }

        public static List<ArchivoClass> LoadArchivo() {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                var output = cnn.Query<ArchivoClass>("select * from Archivo", new DynamicParameters());
                return output.ToList();
            }
        }

        public static MenuClass getMenu(MenuClass mc) {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                var output = cnn.Query<MenuClass>("select * from Menu where nombre=@nombre and idPerfil=@perfil", mc);
                if (output.ToList().Count == 0) {
                    MenuClass menu = output.ToList().First<MenuClass>();
                    return menu;
                } else {
                    return null;
                }
            }
            return null;
        }

        public static PerfilClass getPerfil(PerfilClass p) {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                var output = cnn.Query<PerfilClass>("select * from Perfil where nombre=@nombre", p);
                if (output.ToList().Count == 0) {
                    PerfilClass perfil = output.ToList().First<PerfilClass>();
                    return perfil;
                } else {
                    return null;
                }
            }
            return null;
        }

        public static PerfilClass addProfile(PerfilClass profile) {
            try {
                using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                    cnn.Execute("insert into Perfil (nombre,numMenus,mode) values (@nombre,0,@mode)", profile);
                    Console.WriteLine("Añadido Perfil");
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
                    cnn.Execute("insert into Menu (nombre,tipo,idProfile) values (@nombre,@tipo,@idProfile)", m);
                    return getMenu(m);
                    Console.WriteLine("Añadido Menu");
                }
            } catch (Exception e) {
                Console.WriteLine("Clave Duplicada Menu");
            }
            return null;
        }
        public static void addCarpeta(CarpetaClass carpeta) {
            try {
                using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                    cnn.Execute("insert into Carpeta (nombre,ruta,rutaPadre,img,isFolder,idMenu) values (@nombre,@ruta,@rutaPadre,@img,@isFolder,@idMenu)", carpeta);
                    Console.WriteLine("Añadido Carpeta");
                }
            } catch (Exception e) {
                Console.WriteLine("Clave Duplicada Carpeta");
            }
        }
        public static void addArchivo(ArchivoClass archivo) {
            try {
                using (IDbConnection cnn = new SQLiteConnection(loadConnectionString())) {
                    cnn.Execute("insert into User (nombre,rutaSistema,rutaPrograma,tipoArchivo,idCarpeta) values (@nombre,@rutaSistema,@rutaPrograma,@tipoArchivo,@idCarpeta)", archivo);
                    Console.WriteLine("Añadido Archivo");
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
                }
            } catch (Exception e) {
                Console.WriteLine(e);

            }
        }
    }
}
