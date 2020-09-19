using ProyectoWPF.Components;
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
using System.Windows.Shapes;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para Pruebas.xaml
    /// </summary>
    public partial class Pruebas : Window {
        public Pruebas() {
            InitializeComponent();
            GenderSelection gd = new GenderSelection();
            prueba.Children.Add(gd);
        }
    }
}
