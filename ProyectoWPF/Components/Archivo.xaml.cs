using ProyectoWPF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoWPF.Components {
    /// <summary>
    /// Lógica de interacción para Archivo.xaml
    /// </summary>
    public partial class Archivo : UserControl {

        ArchivoClass archivoClass { get; set; }


        public Archivo(ArchivoClass archivo) {
            InitializeComponent();
            archivoClass = archivo;
            Title.SetSombraVisible(true);
            Title.ChangeForegroundColor(System.Windows.Media.Color.FromRgb(255, 255, 255));
            Title.SetText(archivoClass.nombre);
            Title.SetBorderCorners(new CornerRadius(0));
            Title.SetRadius(new CornerRadius(0));
        }
    }
}
