using Microsoft.Reporting.WinForms;
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

namespace Inventario.GUI.Almacen
{
    /// <summary>
    /// Lógica de interacción para Reporteador.xaml
    /// </summary>
    public partial class Reporteador : Window
    {
        string reporte;
        List<ReportDataSource> origenes;
        
        bool cargado;

        public Reporteador(string nombreReporte, List<ReportDataSource> datos)
        {
            InitializeComponent();
            reporte = nombreReporte;
            origenes = datos;
      
            Contenedor.Load += Contenedor_load;
        }

        private void Contenedor_load(object sender, EventArgs e)
        {
            if (!cargado)
            {
                Contenedor.LocalReport.ReportEmbeddedResource = reporte;
                foreach (ReportDataSource item in origenes)
                {
                    Contenedor.LocalReport.DataSources.Add(item);
                }
                Contenedor.RefreshReport();
                cargado = true;
            }
        }
    }
}
