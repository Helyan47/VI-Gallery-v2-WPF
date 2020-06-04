using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data {
    public static class Filters {
        public static List<string> filterAlgorithm(string[] carpetas, string search) {
            List<string> resultados = new List<string>();

            /*
            Simplemente comprueba si alguno de los nombres del array carpetas contiene el parametro de busqueda
            */
            for (int i = 0; i < carpetas.Length; i++) {
                if (carpetas[i].ToLower().Contains(search.ToLower())) {
                    if (!resultados.Contains(carpetas[i])) {
                        //Si la carpeta contiene el parametro de busqueda, lo añade a la lista de resultados
                        resultados.Add(carpetas[i]);
                    }
                }
            }
            int numberOfWords = 1;

            //Si no ha encontrado ningun resultado, ejecuta el buscador por palabras

            if (resultados.Count == 0) {
                /*
                1-Primero se divide el string search en palabras.
                2-Después se dividen todos los nombres de las carpetas en palabras
                3-Se compara cada palabra del string search con cada palabra de los nombres de las carpetas
                */

                /*
                Guarda todas las palabras de los nombres de las carpetas con clave-valor siendo la clave el nombre completo
                y el valor las palabras que componen ese nombre
                */
                Dictionary<string, string[]> splittedFolders = splitWords(carpetas);

                if (search.Contains(" ")) {
                    //Divide el string search
                    string[] palabras = search.Split(' ');
                    //Asigna el numero de palabras. Este valor se utilizara al final del programa
                    numberOfWords = palabras.Length;

                    //Recorre el diccionario que contiene las palabras de las carpetas
                    foreach (KeyValuePair<string, string[]> s in splittedFolders) {
                        string[] aux = s.Value;
                        for (int i = 0; i < aux.Length; i++) {
                            for (int j = 0; j < palabras.Length; j++) {
                                //Comprueba si la palabra de una carpeta tiene la palabra del string search
                                if (aux[i].ToLower().Contains(palabras[j].ToLower())) {
                                    if (!resultados.Contains(s.Key)) {
                                        resultados.Add(s.Key);
                                    }
                                }
                            }
                        }
                    }
                }

                /*
                Si el buscador por palabrasno ha encontrado ningún resultado, se ejecuta el buscador por combinaciones del string search
                Este método descompone el string search en todas las combinaciones posibles
                Ejemplo: Palabra a buscar: Arrow
                Palabras Conseguidas:
                Arrow, Arro, Arr, Ar, A, rrow, rro, rr, r, row, ow, o

                Tras descomoponer el string search, se comprueba si algun nombre de la carpeta contiene alguno de los resultados 
                obtenidos al descomponer la busqueda.
                */
                if (resultados.Count == 0) {
                    List<string> caracteres = new List<string>();
                    //Metodo que descompone el string search
                    for (int i = 0; i < search.Length; i++) {
                        for (int j = search.Length; j >= i; j--) {
                            if (j - i > 0) {
                                string s = search.Substring(i, j - i);
                                //Console.WriteLine(s);
                                if (!caracteres.Contains(s)) {
                                    caracteres.Add(s);
                                }

                            }

                        }
                    }
                    /*
                    Recorre el array de nombres y comparar el nombre con la combinacion que tenga como minimo una longitud de 3, para evitar
                    letras comunes como la 'a', ya que sino el resultado serían todas las palabras con la letra 'a'
                    */
                    for (int i = 0; i < carpetas.Length; i++) {
                        foreach (string s in caracteres) {
                            if (s.Length > 2) {
                                //Tambien comprueba que la carpeta no haya sido añadida a la lista de resultados para evitar valores duplicados
                                if ((carpetas[i].ToLower().Contains(s.ToLower())) && !(resultados.Contains(carpetas[i]))) {
                                    resultados.Add(carpetas[i]);
                                }
                            }
                        }

                    }
                } else {
                    return resultados;
                }
            } else {
                return resultados;
            }

            //Instanciamos la clase comparador, que servirá para ordenar los resultados según más se parezcan al string search
            Comparador comparador = new Comparador();
            //Establecemos el numero de palabras que tiene el string search que lo asignamos anteriormente
            comparador.setNumberOfWords(numberOfWords);
            //Establecemos la longitud del string search
            comparador.setSearchLength(search.Length);
            //Ordenamos la lista segun el comparador (Revisa la clase comparador que es muy util para ordenar valores como a ti te parezca)
            resultados.Sort(comparador);

            //Sacamos por pantalla los resultados ordenados
            return resultados;
        }


        /*
        Divide los nombres de las carpetas en palabras
        */
        static Dictionary<string, string[]> splitWords(string[] carpetas) {
            Dictionary<string, string[]> splittedFolders = new Dictionary<string, string[]>();
            for (int i = 0; i < carpetas.Length; i++) {
                string[] split;
                if (carpetas[i].Contains(" ")) {
                    split = carpetas[i].Split(' ');
                } else {
                    split = new string[1];
                    split[0] = carpetas[i];
                }

                splittedFolders.Add(carpetas[i], split);
            }
            return splittedFolders;
        }
    }

    class Comparador : IComparer<string> {

        int numberOfWords = 0;
        int searchLength = 0;

        //Ordena la lista donde se utiliza esta clase
        public int Compare(string s1, string s2) {
            //Cogemos el numero de palabras que tiene el string s1
            int num1 = getNumberOfWords(s1);
            //Cogemos el numero de palabras que tiene el string s2
            int num2 = getNumberOfWords(s2);
            //Comprobamos el numero de palabras de cada string respecto al numero de palabras del string search
            if ((num1 > numberOfWords) && (num2 > numberOfWords)) {
                /*
                En los casos en los que tanto s1 como s2 tiene más palabras que el string search, se comprueba la longitud de ambos
                para ordenadorlos de mas corto a mas largo
                */
                if (s1.Length > s2.Length) {
                    return 1;
                } else if (s1.Length < s2.Length) {
                    return -1;
                } else {
                    return 0;
                }
            } else if ((num1 > numberOfWords) && (num2 < numberOfWords)) {
                return 1;
            } else if ((num1 < numberOfWords) && (num2 < numberOfWords)) {
                if (s1.Length > s2.Length) {
                    return 1;
                } else if (s1.Length < s2.Length) {
                    return -1;
                } else {
                    return 0;
                }
            } else if ((num1 < numberOfWords) && (num2 > numberOfWords)) {
                return 1;
            } else if ((num1 == numberOfWords) && (num2 > numberOfWords)) {
                return -1;
            } else if ((num1 == numberOfWords) && (num2 < numberOfWords)) {
                return -1;
            } else if ((num1 > numberOfWords) && (num2 == numberOfWords)) {
                return 1;
            } else if ((num1 < numberOfWords) && (num2 == numberOfWords)) {
                return 1;
            } else {
                /*
                En este caso tanto s1 como s2 tienen el mismo numero de palabras que search, por lo que se comprueba la longitud respecto
                a la longitud de search y tras esto respecto a la longitud ambos strings (s1,s2)
                */
                if ((s1.Length > searchLength) && (s2.Length > searchLength)) {
                    if (s1.Length > s2.Length) {
                        return 1;
                    } else if (s1.Length < s2.Length) {
                        return -1;
                    } else {
                        return 0;
                    }
                } else if ((s1.Length > searchLength) && (s2.Length < searchLength)) {
                    return 1;
                } else if ((s1.Length < searchLength) && (s2.Length < searchLength)) {
                    if (s1.Length > s2.Length) {
                        return 1;
                    } else if (s1.Length < s2.Length) {
                        return -1;
                    } else {
                        return 0;
                    }
                } else if ((s1.Length < searchLength) && (s2.Length > searchLength)) {
                    return -1;
                } else if ((s1.Length == searchLength) && (s2.Length > searchLength)) {
                    return -1;
                } else if ((s1.Length == searchLength) && (s2.Length < searchLength)) {
                    return -1;
                } else if ((s1.Length > searchLength) && (s2.Length == searchLength)) {
                    return 1;
                } else if ((s1.Length < searchLength) && (s2.Length == searchLength)) {
                    return 1;
                } else {
                    return 0;
                }
            }
        }

        public void setNumberOfWords(int num) {
            numberOfWords = num;
        }

        public void setSearchLength(int num) {
            searchLength = num;
        }

        public int getNumberOfWords(string s) {
            if (s.Contains(" ")) {
                return s.Split(' ').Length;
            } else {
                return 1;
            }
        }
    }
}
