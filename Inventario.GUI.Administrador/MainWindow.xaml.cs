using Inventario.BIZ;
using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using Inventario.DAL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Inventario.GUI.Administrador
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum accion
        {
            Nuevo,
            Editar
        }

        accion AccionEmpleados;
        accion AccionMateriales;

        IManejadorEmpleados manejadorEmpleados;
        IManejadorMateriales manejadorMateriales;

        public MainWindow()
        {
            InitializeComponent();
            manejadorEmpleados = new ManejadorEmpleados(new RepositorioDeEmpleados());
            manejadorMateriales = new ManejadorMateriales(new RepositorioDeMateriales());
            
            BotonesEmpleados(false);
            LimpiarEmpleados();
            ActualizarTablaEmpleados();

            BotonesMateriales(false);
            LimpiarMateriales();
            ActualizarTablaMateriales();

        }

        //************************************//
        //********** INICIO METODOS **********//
        //************************************//

        //Metodo
        private void ActualizarTablaEmpleados()
        {
            dtgEmpleados.ItemsSource = null; //Primero la vacio
            dtgEmpleados.ItemsSource = manejadorEmpleados.Listar;
        }
        
        //Metodo
        private void ActualizarTablaMateriales()
        {
            dtgMateriales.ItemsSource = null; //Primero la vacio
            dtgMateriales.ItemsSource = manejadorMateriales.Listar;
        }
        
        //Metodo
        private void LimpiarEmpleados()
        {
            txtEmpleadosApellidos.Clear();
            txtEmpleadosNombre.Clear();
            txtEmpleadosArea.Clear();
            txbEmpleadosId.Text = "";
        }
        
        //Metodo
        private void LimpiarMateriales()
        {
            txtMaterialesNombre.Clear();
            txtMaterialesCategoria.Clear();
            txtMaterialesDescripcion.Clear();
            txbMaterialesId.Text = "";
        }

        //Metodo
        private void BotonesEmpleados(bool value)
        {
            btnEmpleadosCancelar.IsEnabled = value;
            btnEmpleadosEditar.IsEnabled = !value;
            btnEmpleadosEliminar.IsEnabled = !value;
            btnEmpleadosGuardar.IsEnabled = value;
            btnEmpleadosNuevos.IsEnabled = !value;
        }

        //Metodo
        private void BotonesMateriales(bool value)
        {
            btnMaterialesCancelar.IsEnabled = value;
            btnMaterialesEditar.IsEnabled = !value;
            btnMaterialesEliminar.IsEnabled = !value;
            btnMaterialesGuardar.IsEnabled = value;
            btnMaterialesNuevos.IsEnabled = !value;
        }

        //Metodo - Convierto la imagen en un arreglo de byte
        private byte[] ImageToByte(ImageSource imagen)
        {
            if (imagen != null)
            {
                MemoryStream memoryStream = new MemoryStream();                     //Creo un espacio en memoria
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();                //Creo un codificador a jpg

                encoder.Frames.Add(BitmapFrame.Create(imagen as BitmapSource));      //Creo un mapa de bit de la imagen y la agrego al codificador
                encoder.Save(memoryStream);                                         //Lo guardo em el espacio en memoria
                return memoryStream.ToArray();                                      //Lo devuelvo en un array
            }
            else
            {
                return null;
            }
        }

        //Metodo - Convierto el arreglo de byte en una imagen
        private ImageSource ByteToImage(byte[] imageData)
        {
            if (imageData != null)
            {
                BitmapImage bitmap = new BitmapImage();                             //Creo una imagen de mapa de bit
                MemoryStream memoryStream = new MemoryStream(imageData);            //Creo un espacio en memoria con el array de imagen

                bitmap.BeginInit();                                                 //Inicializo el mapa de bit para la decodificacion
                bitmap.StreamSource = memoryStream;                                 //Al mapa de bit le paso el array de la imagen para decodificarlo
                bitmap.EndInit();                                                   //Finalizo el mapa de bit
                ImageSource imageSource = bitmap as ImageSource;                    //Lo convierto en Un origen de imagen
                return imageSource;                                                 //Devuelvo ese origen de imagen
            }
            else
            {
                return null;
            }
        }


        private void btnEmpleadosNuevos_Click(object sender, RoutedEventArgs e)
        {
            LimpiarEmpleados();
            BotonesEmpleados(true);
            AccionEmpleados = accion.Nuevo;
        }

        private void btnEmpleadosEditar_Click(object sender, RoutedEventArgs e)
        {
            Empleado emp = dtgEmpleados.SelectedItem as Empleado;
            if(emp != null)
            {
                txbEmpleadosId.Text = emp.Id;
                txtEmpleadosApellidos.Text = emp.Apellidos;
                txtEmpleadosNombre.Text = emp.Nombre;
                txtEmpleadosArea.Text = emp.Area;
                AccionEmpleados = accion.Editar;
                BotonesEmpleados(true);
            }
        }

        private void btnEmpleadosGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (AccionEmpleados == accion.Nuevo)
            {
                Empleado emp = new Empleado()
                {
                    Apellidos = txtEmpleadosApellidos.Text,
                    Nombre = txtEmpleadosNombre.Text,
                    Area = txtEmpleadosArea.Text
                };
                if (manejadorEmpleados.Agregar(emp))
                {
                    MessageBox.Show("Empleado Agregado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarEmpleados();
                    ActualizarTablaEmpleados();
                    BotonesEmpleados(false);
                }
                else
                {
                    MessageBox.Show("Empleado NO se pudo agregar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (AccionEmpleados == accion.Editar)
            {
                Empleado emp = dtgEmpleados.SelectedItem as Empleado;
                emp.Apellidos = txtEmpleadosApellidos.Text;
                emp.Nombre = txtEmpleadosNombre.Text;
                emp.Area = txtEmpleadosArea.Text;
                if (manejadorEmpleados.Modificar(emp))
                {
                    MessageBox.Show("Empleado Modificado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarEmpleados();
                    ActualizarTablaEmpleados();
                    BotonesEmpleados(false);
                }
                else
                {
                    MessageBox.Show("Empleado NO se pudo modificar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void btnEmpleadosCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarEmpleados();
            BotonesEmpleados(false);
        }

        private void btnEmpleadosEliminar_Click(object sender, RoutedEventArgs e)
        {
            Empleado emp = dtgEmpleados.SelectedItem as Empleado;
            if (emp != null)
            {
                if (MessageBox.Show("Realmente deseas eliminar este empleado", "Inventarios", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)
                {
                    if (manejadorEmpleados.Eliminar(emp.Id))
                    {
                        MessageBox.Show("Empleado Eliminado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                        LimpiarEmpleados();
                        ActualizarTablaEmpleados();
                        BotonesEmpleados(false);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo Eliminar el Empleado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
        }

        private void btnMaterialesNuevos_Click(object sender, RoutedEventArgs e)
        {
            LimpiarMateriales();
            BotonesMateriales(true);
            AccionMateriales = accion.Nuevo;
        }

        private void btnMaterialesEditar_Click(object sender, RoutedEventArgs e)
        {
            Material mat = dtgMateriales.SelectedItem as Material;
            if (mat != null)
            {
                LimpiarMateriales();
                AccionMateriales = accion.Editar;
                BotonesMateriales(true);
                txbMaterialesId.Text = mat.Id;
                txtMaterialesNombre.Text = mat.Nombre;
                txtMaterialesCategoria.Text = mat.Categoria;
                txtMaterialesDescripcion.Text = mat.Descripcion;
                imgFoto.Source = ByteToImage(mat.Fotografia);
                
            }
        }

         private void btnMaterialesGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (AccionMateriales == accion.Nuevo)
            {
                Material mat = new Material()
                {
                    Nombre = txtMaterialesNombre.Text,
                    Categoria = txtMaterialesCategoria.Text,
                    Descripcion = txtMaterialesDescripcion.Text,
                    Fotografia = ImageToByte(imgFoto.Source)
                };

                if (manejadorMateriales.Agregar(mat))
                {
                    MessageBox.Show("Material Agregado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarMateriales();
                    ActualizarTablaMateriales();
                    BotonesMateriales(false);
                }
                else
                {
                    MessageBox.Show("Material NO se pudo agregar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (AccionMateriales == accion.Editar)
            {
                Material mat = dtgMateriales.SelectedItem as Material;
                mat.Nombre = txtMaterialesNombre.Text;
                mat.Categoria = txtMaterialesCategoria.Text;
                mat.Descripcion = txtMaterialesDescripcion.Text;
                mat.Fotografia = ImageToByte(imgFoto.Source);
                if (manejadorMateriales.Modificar(mat))
                {
                    MessageBox.Show("Material Modificado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarMateriales();
                    ActualizarTablaMateriales();
                    BotonesMateriales(false);
                }
                else
                {
                    MessageBox.Show("Material NO se pudo modificar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void btnMaterialesCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarMateriales();
            BotonesMateriales(false);
        }

        private void btnMaterialesEliminar_Click(object sender, RoutedEventArgs e)
        {
            Material mat = dtgMateriales.SelectedItem as Material;
            if (mat != null)
            {
                if (MessageBox.Show("Realmente deseas eliminar este material", "Inventarios", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)
                {
                    if (manejadorMateriales.Eliminar(mat.Id))
                    {
                        MessageBox.Show("Material Eliminado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                        LimpiarMateriales();
                        ActualizarTablaMateriales();
                        BotonesMateriales(false);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo Eliminar el Material", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
        }

        private void btnCargarFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogo = new OpenFileDialog();
            dialogo.Title = "Seleccione una foto";
            dialogo.Filter = "Archivos de imagen|*.png;*.jpg;*.jpeg;*.gif|Todos los Archivos *.*|*.*";
            if (dialogo.ShowDialog().Value)
            {
                imgFoto.Source = new BitmapImage(new Uri(dialogo.FileName));
            }
        }

    }
}
