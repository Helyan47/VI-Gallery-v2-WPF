using System;
using MySql.Data.MySqlClient;

namespace Login {
    class Conexion {

        public static MySqlConnection getConnection() {
            MySqlConnection conexion = new MySqlConnection("datasource=127.0.0.1;port=3306;username=VI_Gallery;password=vi_gallery;database=vi_gallery");
            return conexion;
        }

        public static bool checkUserPass(string user, string pass) {
            MySqlConnection conexion = getConnection();
            conexion.Open();

            MySqlCommand comando = new MySqlCommand("SELECT nick,pass FROM Usuario WHERE nick=@val1 AND pass=@val2", conexion);
            comando.Parameters.AddWithValue("@val1", user);
            comando.Parameters.AddWithValue("@val2", pass);
            MySqlDataReader reader = comando.ExecuteReader();

            if (reader.HasRows) {
                conexion.Close();
                return true;
            } else {
                conexion.Close();
                return false;
            }
            
            
        }
    }
}
