using ProyectoWPF.Components;
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

    public sealed class NaturalWrapCarpetaNameComparer : IComparer<UIElement> {

        private bool isAscending = true;
        public int Compare(UIElement a, UIElement b) {
            if ((a is SubCarpeta) && (b is SubCarpeta)) {
                SubCarpeta aux1 = (SubCarpeta)a;
                SubCarpeta aux2 = (SubCarpeta)b;
                if (isAscending) {
                    
                    return SafeNativeMethods.StrCmpLogicalW(aux1.getClass().nombre, aux2.getClass().nombre);
                } else {
                    return SafeNativeMethods.StrCmpLogicalW(aux2.getClass().nombre, aux1.getClass().nombre);
                }
            } else if ((a is SubCarpeta) && (b is Archivo)) {
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
