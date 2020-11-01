using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data {
    public static class DataLogger {
        public static string FILE_NAME_ERROR = ".\\logs\\error.txt";
        public static string FILE_NAME_INFO = ".\\logs\\info.txt";
        public static string FILE_NAME_ALL = ".\\logs\\logs.txt";
        private static StreamWriter writeError = null;
        private static StreamWriter writeInfo = null;
        private static StreamWriter writeAll = null;

        public static void errorFolder(Exception e,string folderPath) {
            using (writeError = new StreamWriter(FILE_NAME_ERROR, true)) {
                writeError.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - Error creating folder with the next path: " + folderPath);
                writeError.WriteLine(e.Message);
            }
            allExceptionFolder(e, folderPath);
        }

        public static void errorFile(Exception e,string filePath) {
            using (writeError = new StreamWriter(FILE_NAME_ERROR, true)) {
                writeError.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - Error creating file with the next path: " + filePath);
                writeError.WriteLine(e.Message);
            }
            allExceptionFile(e, filePath);
        }

        public static void generalError(Exception e, string errorText) {
            using (writeError = new StreamWriter(FILE_NAME_ERROR, true)) {
                writeError.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + errorText);
                writeError.WriteLine(e.Message);
            }
            allGeneralInfo(e, errorText);
        }

        public static void infoFolder(string folderPath) {
            using (writeInfo = new StreamWriter(FILE_NAME_INFO, true)) {
                writeInfo.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - Created folder with the next path in the app: " + folderPath);
            }
            allInfoFolder(folderPath);
        }

        public static void infoFile(string filePath) {
            using (writeInfo = new StreamWriter(FILE_NAME_INFO, true)) {
                writeInfo.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - Created file with the next path in the app: " + filePath);
            }
            allInfoFile(filePath);
        }

        public static void generalInfo(string info) {
            using (writeInfo = new StreamWriter(FILE_NAME_INFO, true)) {
                writeInfo.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + info);
            }
            allGeneralInfo(null, info);
        }

        public static void allExceptionFolder(Exception e,string folderPath) {
            using (writeAll = new StreamWriter(FILE_NAME_ALL, true)) {
                writeAll.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - Error creating folder with the next path in app: " + folderPath);
                writeAll.WriteLine(e.Message);
            }
        }

        public static void allExceptionFile(Exception e, string filePath) {
            using (writeAll = new StreamWriter(FILE_NAME_ALL, true)) {
                writeAll.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - Error creating file with the next path in app: " + filePath);
                writeAll.WriteLine(e.Message);
            }
        }

        public static void allInfoFolder(string folderPath) {
            using (writeAll = new StreamWriter(FILE_NAME_ALL, true)) {
                writeInfo.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - Created folder with the next path in the app: " + folderPath);
            }
        }

        public static void allInfoFile(string filePath) {
            using (writeAll = new StreamWriter(FILE_NAME_ALL, true)) {
                writeInfo.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - Created file with the next path in the app: " + filePath);
            }
        }

        public static void allGeneralInfo(Exception e, string info) {
            using (writeAll = new StreamWriter(FILE_NAME_ALL, true)) {
                writeInfo.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + info);
                if(e != null) {
                    writeInfo.WriteLine(e);
                }
            }
        }

        public static void createAll() {
            DirectoryInfo di = new DirectoryInfo(".\\logs");
            if (!di.Exists) {
                di.Create();
            }
        }
    }
}
