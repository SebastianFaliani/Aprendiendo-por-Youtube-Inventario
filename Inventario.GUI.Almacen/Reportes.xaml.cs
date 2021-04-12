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
using Inventario.COMMON.Interfaces;
using Inventario.DAL;
using Inventario.BIZ;
using Inventario.COMMON.Entidades;

namespace Inventario.GUI.Almacen
{
    /// <summary>
    /// Lógica de interacción para Reportes.xaml
    /// </summary>
    public partial class Reportes : Window
    {
        IManejadorEmpleados manejadorEmpleados;
        IManejadorVales manejadorVales;
        public Reportes()
        {
            InitializeComponent();
            manejadorEmpleados = new ManejadorEmpleados(new RepositorioDeEmpleados());
            manejadorVales = new ManejadorVales(new RepositorioDeVales());
            cmbPersona.ItemsSource = manejadorEmpleados.Listar.OrderBy(e=>e.Nombre);
        }

        private void btnImprimirPorPersona_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbPersona_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPersona.SelectedItem != null)
            {
                lstTablaVales.ItemsSource = null;
                lstTablaVales.ItemsSource = manejadorVales.BuscarNoEntregadoPorEmpleado((cmbPersona.SelectedItem as Empleado));
            }
        }
    }
}
