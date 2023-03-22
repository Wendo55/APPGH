using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace APPGH
{
    public partial class Form1 : Form
    {
        private List<Datos> datosList = new List<Datos>();
        private List<int> data = new List<int>();

        public Form1()
        {
            InitializeComponent();

            this.Controls.Add(chart1);

            label1.AutoSize = true;

            label1.Text = "Estadísticas por Ciudad";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {

            string criterioBusqueda = comboBox1.SelectedItem.ToString();
            string textoBusqueda = txtBuscar.Text;

            // Filtrar los datos según el criterio de búsqueda y el texto ingresado
            List<Datos> datosFiltrados = new List<Datos>();
            if (criterioBusqueda == "Codigo Postal")
            {
                datosFiltrados = datosFiltrados = datosList.Where(d => d.CodigoPostal.ToLower().Contains(textoBusqueda.ToLower())).ToList();
            }
            else if (criterioBusqueda == "Asentamiento")
            {
                datosFiltrados = datosFiltrados = datosList.Where(d => d.Asentamiento.ToLower().Contains(textoBusqueda.ToLower())).ToList();
            }
            else if (criterioBusqueda == "Colonia ")
            {
                datosFiltrados = datosFiltrados = datosList.Where(d => d.Colonia.ToLower().Contains(textoBusqueda.ToLower())).ToList();
            }
            else if (criterioBusqueda == "Ciudad")
            {
                datosFiltrados = datosFiltrados = datosList.Where(d => d.Ciudad.ToLower().Contains(textoBusqueda.ToLower())).ToList();
            }

            dataGridView1.DataSource = datosFiltrados;
            chart1.Series[0].Points.Clear();
            UpdateSelectedDataLabel();

            chart1.Series.Clear();

            Series series1 = new Series();
            series1.ChartType = SeriesChartType.Bar;
            series1.IsValueShownAsLabel = true;
            series1.LabelForeColor = Color.Black;
            series1.Font = new Font("Arial", 10);
            series1.Name = criterioBusqueda;
            chart1.Series.Add(series1);
            foreach (var ciudad in datosFiltrados.GroupBy(d => d.Ciudad).Select(g => new { Pais = g.Key, Cantidad = g.Count() }).ToList())
            {
                series1.Points.AddXY(ciudad.Pais, ciudad.Cantidad);
            }

            // Actualizar el título del eje X del gráfico
            chart1.ChartAreas[0].AxisX.Title = criterioBusqueda;

            // Actualizar el título de la leyenda del gráfico
            chart1.Series[0].LegendText = criterioBusqueda;

            // Mostrar los datos filtrados en el DataGridView y actualizar el gráfico de estadísticas
            dataGridView1.DataSource = datosFiltrados;
            chart1.Series[0].Points.Clear();


            foreach (var ciudad in datosFiltrados.GroupBy(d => d.Ciudad).Select(g => new { Pais = g.Key, Cantidad = g.Count() }).ToList())
            {
                chart1.Series[0].Points.AddXY(ciudad.Pais, ciudad.Cantidad);
            }
        }

        private void UpdateSelectedDataLabel()
        {
            // Obtener el valor de la columna Ciudad de la fila seleccionada en el DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string ciudad = selectedRow.Cells["Ciudad"].Value.ToString();
                label1.Text = "Estadísticas por Ciudad: " + ciudad;
            }
            else
            {
                // Si no hay filas seleccionadas, utilizar un valor predeterminado
                label1.Text = "Estadísticas por Ciudad";
            }
        }

        private List<Datos> LeerDatosDesdeArchivo(string rutaArchivo)
        {
            List<Datos> datosList = new List<Datos>();

            StreamReader sr = new StreamReader(rutaArchivo, Encoding.UTF8);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split('|');
                Datos datos = new Datos(parts[0], parts[1], parts[3], parts[4]);
                datosList.Add(datos);
            }
            sr.Close();

            return datosList;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                datosList = LeerDatosDesdeArchivo(openFileDialog.FileName);

                // Configurar el DataGridView para mostrar los resultados de búsqueda
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = datosList;

                // Configurar el ComboBox de criterios de búsqueda
                comboBox1.Items.AddRange(new object[] { "Codigo Postal", "Asentamiento", "Colonia", "Ciudad" });
                comboBox1.SelectedIndex = 0;

                this.WindowState = FormWindowState.Maximized;
            }

        }
    }
}
