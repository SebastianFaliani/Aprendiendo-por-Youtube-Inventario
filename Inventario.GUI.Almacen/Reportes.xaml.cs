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
using Microsoft.Reporting.WinForms;
using Inventario.COMMON.Modelos;

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
            manejadorEmpleados = new ManejadorEmpleados(new RepositoGenerico<Empleado>());
            manejadorVales = new ManejadorVales(new RepositoGenerico<Vale>());
            cmbPersona.ItemsSource = manejadorEmpleados.Listar.OrderBy(e=>e.Nombre);
        }

        private void btnImprimirPorPersona_Click(object sender, RoutedEventArgs e)
        {
            List<ReportDataSource> datos = new List<ReportDataSource>();
            ReportDataSource vales = new ReportDataSource();
            List<ModelValesParaReporte> valesporentregar = new List<ModelValesParaReporte>();

            foreach (Vale item in manejadorVales.ValesPorLiquidar())
            {
                valesporentregar.Add(new ModelValesParaReporte(item));
            }
            vales.Value = valesporentregar;
            vales.Name="Vales";
            datos.Add(vales);

            

            Reporteador ventana = new Reporteador("Inventario.GUI.Almacen.Reportes.ValesEntregar.rdlc", datos);
            ventana.ShowDialog();
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
