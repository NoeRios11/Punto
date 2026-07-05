using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Punto.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor, introduzca el usuario y la contraseña.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Conexion bd = new Conexion();


            using (MySqlConnection con = bd.ObtenerConexion())
            {
                try
                {
                    con.Open(); 

                    string query = "SELECT nombre_completo FROM usuarios WHERE username = @user AND password = @pass";
                    MySqlCommand cmd = new MySqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@user", txtUser.Text.Trim());
                    cmd.Parameters.AddWithValue("@pass", txtPassword.Text.Trim());

                    object resultado = cmd.ExecuteScalar();

                    if (resultado != null)
                    {
                        string nombreCompleto = resultado.ToString();
                        MessageBox.Show("¡Bienvenido, " + nombreCompleto + "!", "Acceso Autorizado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        frmPrincipal principal = new frmPrincipal();
                        principal.Show();

                        this.Hide(); 
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos. Intente de nuevo.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error crítico: El servidor MySQL se encuentra inaccesible.\nDetalle: " + ex.Message, "Error de Servidor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
