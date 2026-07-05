using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Punto.Forms
{
    public partial class frmProductos : Form
    {
        public frmProductos()
        {
            InitializeComponent();
        }
        private void CargarProductos()
        {
            try
            {
                Conexion conexionDB = new Conexion();
                using (MySqlConnection conn = conexionDB.ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT producto_id, codigo, descripcion, precio, stock FROM productos";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvProductos.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message, "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmProductos_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }

        private void frmProductos_Load_1(object sender, EventArgs e)
        {
            CargarProductos();
           
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtNombre.Text) ||
                string.IsNullOrEmpty(txtPrecio.Text) || string.IsNullOrEmpty(txtStock.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("Por favor, ingresa un precio válido (Ejemplo: 18.50)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock))
            {
                MessageBox.Show("Por favor, ingresa un número entero para el stock.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Conexion conexionDB = new Conexion();
                using (MySqlConnection conn = conexionDB.ObtenerConexion())
                {
                    conn.Open();
                    string query = "INSERT INTO productos (codigo, descripcion, precio, stock) VALUES (@codigo, @descripcion, @precio, @stock)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codigo", txtCodigo.Text);
                    cmd.Parameters.AddWithValue("@descripcion", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@precio", precio); 
                    cmd.Parameters.AddWithValue("@stock", stock); 

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("¡Producto guardado con éxito!", "Excelente", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtCodigo.Clear();
                    txtNombre.Clear();
                    txtPrecio.Clear();
                    txtStock.Clear();

                    CargarProductos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProductos.Rows[e.RowIndex];
                lblId.Text = row.Cells["producto_id"].Value.ToString();
                txtCodigo.Text = row.Cells["codigo"].Value.ToString();
                txtNombre.Text = row.Cells["descripcion"].Value.ToString();
                txtPrecio.Text = row.Cells["precio"].Value.ToString();
                txtStock.Text = row.Cells["stock"].Value.ToString();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblId.Text) || lblId.Text == "0")
            {
                MessageBox.Show("Por favor, selecciona un producto de la tabla primero.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    Conexion conexionDB = new Conexion();
                    using (MySql.Data.MySqlClient.MySqlConnection conn = conexionDB.ObtenerConexion())
                    {
                        conn.Open();
                        string query = "DELETE FROM productos WHERE producto_id = @id";
                        MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn);

                        cmd.Parameters.AddWithValue("@id", lblId.Text);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("¡Producto eliminado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCodigo.Clear();
                        txtNombre.Clear();
                        txtPrecio.Clear();
                        txtStock.Clear();
                        lblId.Text = "0";
                        CargarProductos();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblId.Text) || lblId.Text == "0")
            {
                MessageBox.Show("Por favor, selecciona un producto de la tabla para poder editarlo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtNombre.Text) ||
                string.IsNullOrEmpty(txtPrecio.Text) || string.IsNullOrEmpty(txtStock.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios para actualizar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("Por favor, ingresa un precio válido.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock))
            {
                MessageBox.Show("Por favor, ingresa un stock válido.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Conexion conexionDB = new Conexion();
                using (MySql.Data.MySqlClient.MySqlConnection conn = conexionDB.ObtenerConexion())
                {
                    conn.Open();
                    string query = "UPDATE productos SET codigo = @codigo, descripcion = @descripcion, precio = @precio, stock = @stock WHERE producto_id = @id";

                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@codigo", txtCodigo.Text);
                    cmd.Parameters.AddWithValue("@descripcion", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@precio", precio);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@id", lblId.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("¡Producto actualizado con éxito!", "Excelente", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtCodigo.Clear();
                    txtNombre.Clear();
                    txtPrecio.Clear();
                    txtStock.Clear();
                    lblId.Text = "0";

                    CargarProductos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
