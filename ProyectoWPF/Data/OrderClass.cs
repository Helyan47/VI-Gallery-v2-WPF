using ProyectoWPF.Components;
using ProyectoWPF.Data.Online;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProyectoWPF.Data {
    public static class OrderClass {
        static NaturalStringComparer comparadorStrings = new NaturalStringComparer();
        static NaturalArchivoClassComparer comparadorArchivoClass = new NaturalArchivoClassComparer();
        static NaturalCarpetaClassComparer comparadorCarpetaClass = new NaturalCarpetaClassComparer();
        static NaturalWrapCarpetaNameComparer comparadorWrapCarpeta = new NaturalWrapCarpetaNameComparer();
        static DateComparer comparadorFechas = new DateComparer();
        static VisitsComparer comparadorVisitas = new VisitsComparer();
        public static List<string> orderListOfString(List<string> lista) {
            if (lista != null) {
                lista.Sort(comparadorStrings);
                return lista;
            } else {
                return null;
            }
        }

        public static string[] orderArrayOfString(string[] lista) {
            if (lista != null) {
                Array.Sort(lista, comparadorStrings);
                return lista;
            } else {
                return null;
            }
        }

        public static List<ArchivoClass> orderListOfArchivoClass(List<ArchivoClass> lista) {
            if (lista != null) {
                lista.Sort(comparadorArchivoClass);
                return lista;
            } else {
                return null;
            }
            
        }
        public static List<CarpetaClass> orderListOfCarpetaClass(List<CarpetaClass> lista) {
            if (lista != null) {
                lista.Sort(comparadorCarpetaClass);
                return lista;
            } else {
                return null;
            }
            
        }

        public static List<UIElement> orderChildOfWrap(List<UIElement> lista) {
            if (lista != null) {
                lista.Sort(comparadorWrapCarpeta);
                return lista;
            } else {
                return null;
            }
        }

        public static List<object> orderDates(List<object> lista) {
            if (lista != null) {
                lista.Sort(comparadorFechas);
                return lista;
            } else {
                return null;
            }
        }

        public static List<object> orderTops(List<object> lista) {
            if (lista != null) {
                lista.Sort(comparadorVisitas);
                return lista;
            } else {
                return null;
            }
        }
    }

    [SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);
    }

    public class NaturalStringComparer : IComparer<string> {
        public int Compare(string a, string b) {
            return SafeNativeMethods.StrCmpLogicalW(a, b);
        }
    }

    public class NaturalFileInfoNameComparer : IComparer<FileInfo> {
        public int Compare(FileInfo a, FileInfo b) {
            return SafeNativeMethods.StrCmpLogicalW(a.Name, b.Name);
        }
    }

    public class NaturalArchivoClassComparer : IComparer<ArchivoClass> {
        public int Compare(ArchivoClass a, ArchivoClass b) {
            return SafeNativeMethods.StrCmpLogicalW(a.nombre, b.nombre);
        }
    }

    public class NaturalCarpetaClassComparer : IComparer<CarpetaClass> {
        public int Compare(CarpetaClass a, CarpetaClass b) {
            return SafeNativeMethods.StrCmpLogicalW(a.nombre, b.nombre);
        }
    }

    public class DateComparer : IComparer<object> {
        public int Compare(object a, object b) {
            if((a is Capitulo) && (b is Capitulo)) {
                Capitulo aux1 = (Capitulo)a;
                Capitulo aux2 = (Capitulo)b;
                return aux2.fechaLanzamiento.CompareTo(aux1.fechaLanzamiento);
            }else if ((a is Capitulo) && (b is Pelicula)) {
                Capitulo aux1 = (Capitulo)a;
                Pelicula aux2 = (Pelicula)b;
                return aux2.fechaLanzamiento.CompareTo(aux1.fechaLanzamiento);
            } else if ((a is Pelicula) && (b is Capitulo)) {
                Pelicula aux1 = (Pelicula)a;
                Capitulo aux2 = (Capitulo)b;
                return aux2.fechaLanzamiento.CompareTo(aux1.fechaLanzamiento);
            } else if ((a is Pelicula) && (b is Pelicula)) {
                Pelicula aux1 = (Pelicula)a;
                Pelicula aux2 = (Pelicula)b;
                return aux2.fechaLanzamiento.CompareTo(aux1.fechaLanzamiento);
            } else {
                return 0;
            }
        }
    }

    public class VisitsComparer : IComparer<object> {
        public int Compare(object a, object b) {
            if ((a is Capitulo) && (b is Capitulo)) {
                Capitulo aux1 = (Capitulo)a;
                Capitulo aux2 = (Capitulo)b;
                if (aux1.numVisitas > aux2.numVisitas) {
                    return -1;
                }else if(aux1.numVisitas < aux2.numVisitas) {
                    return 1;
                } else {
                    return 0;
                }
            } else if ((a is Capitulo) && (b is Pelicula)) {
                Capitulo aux1 = (Capitulo)a;
                Pelicula aux2 = (Pelicula)b;
                if (aux1.numVisitas > aux2.numVisitas) {
                    return -1;
                } else if (aux1.numVisitas < aux2.numVisitas) {
                    return 1;
                } else {
                    return 0;
                }
            } else if ((a is Pelicula) && (b is Capitulo)) {
                Pelicula aux1 = (Pelicula)a;
                Capitulo aux2 = (Capitulo)b;
                if (aux1.numVisitas > aux2.numVisitas) {
                    return -1;
                } else if (aux1.numVisitas < aux2.numVisitas) {
                    return 1;
                } else {
                    return 0;
                }
            } else if ((a is Pelicula) && (b is Pelicula)) {
                Pelicula aux1 = (Pelicula)a;
                Pelicula aux2 = (Pelicula)b;
                if (aux1.numVisitas > aux2.numVisitas) {
                    return -1;
                } else if (aux1.numVisitas < aux2.numVisitas) {
                    return 1;
                } else {
                    return 0;
                }
            } else {
                return 0;
            }
        }
    }

    public sealed class NaturalWrapCarpetaNameComparer : IComparer<UIElement> {

        private bool isAscending = true;
        public int Compare(UIElement a, UIElement b) {
            if ((a is Carpeta) && (b is Archivo)) {
                if (isAscending) {
                    return -1;
                } else {
                    return 1;
                }
            } else if ((a is Archivo) && (b is Archivo)) {
                Archivo aux1 = (Archivo)a;
                Archivo aux2 = (Archivo)b;
                if (isAscending) {
                    return SafeNativeMethods.StrCmpLogicalW(aux1._archivoClass.nombre, aux2._archivoClass.nombre);
                } else {
                    return SafeNativeMethods.StrCmpLogicalW(aux2._archivoClass.nombre, aux1._archivoClass.nombre);
                }
            } else if ((a is Carpeta) && (b is Carpeta)) {
                Carpeta aux1 = (Carpeta)a;
                Carpeta aux2 = (Carpeta)b;
                if (isAscending) {
                    return SafeNativeMethods.StrCmpLogicalW(aux1.getClass().nombre, aux2.getClass().nombre);
                } else {
                    return SafeNativeMethods.StrCmpLogicalW(aux2.getClass().nombre, aux1.getClass().nombre);
                }
            } else { 
                if (isAscending) {
                    return 1;
                } else {
                    return -1;
                }
            }

        }

        public void setIsAscending(bool order) {
            isAscending = order;
        }
    }
}
