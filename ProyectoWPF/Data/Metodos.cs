using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data {
    public static class Metodos {

        private static VIGallery vigallery;

        public static void notifyGenderFilter() {
            vigallery.notifyGenderFilter();
        }

        public static void setVIGallery(VIGallery main) {
            vigallery = main;
        }
    }
}
