using System;
using MySql.Data.MySqlClient;

namespace ProyectoWPF.Data {
    class Conexion {

        public static MySqlConnection getConnection() {
            MySqlConnection conexion = new MySqlConnection("server=localhost;database;Uid=VI_Gallery;pwd=vi_gallery;");
            return conexion;
        }

        public static void uploadFolder(object p) {
            if(p is Carpeta) {

            }else if(p is SubCarpeta) {

            }
        }

        public static void uploadFile(object p) {
            if (p is Carpeta) {

            } else if (p is SubCarpeta) {

            }
        }
    }
}
