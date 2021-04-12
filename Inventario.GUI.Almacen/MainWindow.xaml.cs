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

using Inventario.COMMON.Entidades;
using Inventario.BIZ;
using Inventario.DAL;
using Inventario.COMMON.Interfaces;

namespace Inventario.GUI.Almacen
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IManejadorVales manejadorVales;
        ManejadorEmpleados manejadorEmpleados;
        ManejadorMateriales manejadorMateriales;
        Vale vVale;
        enum Accion
        {
            Nuevo,
            Editar
        }

        Accion accionVale;

        public MainWindow()
        {
            InitializeComponent();
            manejadorVales = new ManejadorVales(new RepositorioDeVales());
            manejadorEmpleados = new ManejadorEmpleados(new RepositorioDeEmpleados());
            manejadorMateriales = new ManejadorMateriales(new RepositorioDeMateriales());
            ActualizarTabladeVales();
            gridDetalle.IsEnabled = false;
        }

        private void ActualizarTabladeVales()
        {
            dtgVales.ItemsSource = null;
            dtgVales.ItemsSource = manejadorVales.Listar;
        }

        private void btnNuevoVale_Click(object sender, RoutedEventArgs e)
        {
            accionVale = Accion.Nuevo;

            gridDetalle.IsEnabled = true;
            ActualizarCombosDetalle();
            vVale = new Vale();
            vVale.MaterialesPrestados = new List<Material>();
            ActualizarListaDeMaterialesEnVale();
        }

        private void ActualizarListaDeMaterialesEnVale()
        {
            dtgMaterialesVales.ItemsSource = null;
            dtgMaterialesVales.ItemsSource = vVale.MaterialesPrestados;
        }

        private void ActualizarCombosDetalle()
        {
            cmbAlmacenista.ItemsSource = null;
            cmbAlmacenista.ItemsSource = manejadorEmpleados.EmpleadosPorArea("Almacen");
            
            cmbMateriales.ItemsSource = null;
            cmbMateriales.ItemsSource = manejadorMateriales.Listar;

            cmbSolicitante.ItemsSource = null;
            cmbSolicitante.ItemsSource = manejadorEmpleados.Listar;
        }

        private void btnEliminarVale_Click(object sender, RoutedEventArgs e)
        {
            Vale v = dtgVales.SelectedItem as Vale;
            if (v != null)
            {
                if(MessageBox.Show("Realmente deseas eliminar el vale","Almacen",MessageBoxButton.YesNo,MessageBoxImage.Question)== MessageBoxResult.Yes)
                {
                    if (manejadorVales.Eliminar(v.Id))
                    {
                        MessageBox.Show("Eliminado con exito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Information);
                        LimpiarCamposDeVales();
                        ActualizarTabladeVales();
                        gridDetalle.IsEnabled = false;
                    }
                    else
                    {
                        MessageBox.Show("El vale no se elimino", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnGuardarVale_Click(object sender, RoutedEventArgs e)
        {
            if (accionVale == Accion.Nuevo)
            {
                vVale.EncargadoDeAlmacen = cmbAlmacenista.SelectedItem as Empleado;
                vVale.FechaEntrega = dtpFechaEntrega.SelectedDate.Value;
                vVale.FechaHoraSolicitud = DateTime.Now;
                vVale.Solicitante=cmbSolicitante.SelectedItem as Empleado;
                if (manejadorVales.Agregar(vVale))
                {
                    MessageBox.Show("Vale Guardado con exito","Almacén",MessageBoxButton.OK,MessageBoxImage.Information);
                    LimpiarCamposDeVales();
                    ActualizarTabladeVales();
                    gridDetalle.IsEnabled = false;
                }
                else
                {
                    MessageBox.Show("Error al guardar el vale", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                vVale.EncargadoDeAlmacen = cmbAlmacenista.SelectedItem as Empleado;
                vVale.FechaEntrega = dtpFechaEntrega.SelectedDate.Value;
                vVale.FechaHoraSolicitud = DateTime.Now;
                vVale.Solicitante = cmbSolicitante.SelectedItem as Empleado;
                if (lblFechaHoraEntregaReal.Content.ToString() != "") vVale.FechaEntegaReal = Convert.ToDateTime(lblFechaHoraEntregaReal.Content.ToString());
                if (manejadorVales.Modificar(vVale))
                {
                    MessageBox.Show("Vale Guardado con exito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCamposDeVales();
                    ActualizarTabladeVales();
                    gridDetalle.IsEnabled = false;
                }
                else
                {
                    MessageBox.Show("Error al guardar el vale", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LimpiarCamposDeVales()
        {
            dtpFechaEntrega.SelectedDate = DateTime.Now;
            lblFechaHoraEntregaReal.Content = "";
            lblFechaHoraPrestamo.Content="";
            dtgMaterialesVales.ItemsSource = null;
            cmbAlmacenista.ItemsSource = null;
            cmbMateriales.ItemsSource = null;
            cmbSolicitante.ItemsSource = null;

        }

        private void btnCancelarVale_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeVales();
            gridDetalle.IsEnabled = false;
        }

        private void btnEntregarVale_Click(object sender, RoutedEventArgs e)
        {
            lblFechaHoraEntregaReal.Content = DateTime.Now;
        }

        private void btnAgregarMaterial_Click(object sender, RoutedEventArgs e)
        {
            Material mat = cmbMateriales.SelectedItem as Material;
            if (mat != null)
            {
                vVale.MaterialesPrestados.Add(mat);
                ActualizarListaDeMaterialesEnVale();
            }

        }

        private void btnEliminarMateria_Click(object sender, RoutedEventArgs e)
        {
            Material mat = dtgMaterialesVales.SelectedItem as Material;
            if (mat != null)
            {
                vVale.MaterialesPrestados.Remove(mat);
                ActualizarListaDeMaterialesEnVale();
            }
        }

        private void dtgVales_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Vale v = dtgVales.SelectedItem as Vale;
            if (v != null)
            {
                gridDetalle.IsEnabled = true;
                vVale = v;
                ActualizarListaDeMaterialesEnVale();
                accionVale = Accion.Editar;
                lblFechaHoraPrestamo.Content = vVale.FechaHoraSolicitud.ToString();
                lblFechaHoraEntregaReal.Content = vVale.FechaEntegaReal.ToString();
                ActualizarCombosDetalle();
                cmbAlmacenista.Text = vVale.EncargadoDeAlmacen.ToString();
                cmbSolicitante.Text = vVale.Solicitante.ToString();
                dtpFechaEntrega.SelectedDate = vVale.FechaEntrega;
            }
        }

        private void btnReportes_Click(object sender, RoutedEventArgs e)
        {
            Reportes reportes = new Reportes();
            reportes.Show();
        }
    }
}
