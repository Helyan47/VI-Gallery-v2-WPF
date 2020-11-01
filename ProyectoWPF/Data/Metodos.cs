using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data {
    public static class Metodos {

        private static VIGallery vigallery;


        public static void setVIGallery(VIGallery main) {
            vigallery = main;
        }

        public static void notifyCanceled() {
            vigallery.notifyCanceled();
        }

        public static void notifyNewFolder() {
            vigallery.notifyNewFolder();
        }

        public static void notifyGenderFilter() {
            vigallery.notifyGenderFilter();
        }

        public static void notifyNewGendersSelected() {
            vigallery.notifyNewGendersSelected();
        }

        public static void notifyBeginFolderUpdate(Carpeta c) {
            vigallery.notifyBeginFolderUpdate(c);
        }

        public static void notifyEndFolderUpdate(bool updated, CarpetaClass c) {
            vigallery.notifyEndFolderUpdate(updated, c);
        }

        public static void clearTextBoxAndSelection() {
            vigallery.clearTextBoxAndSelection();
        }

        public static void ReturnVisibility(bool state) {
            vigallery.ReturnVisibility(state);
        }
    }
}
