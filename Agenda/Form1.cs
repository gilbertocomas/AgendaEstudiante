using System;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Agenda
{
    public partial class frmPrincipal : Form
    {
        private SqlConnection connection = new SqlConnection("Data Source=LENOVO-LEGION\\SQLEXPRESS; Initial Catalog=universidad ; integrated security = true");
        
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void consultarBD(string id)
        {
            connection.Open();
            string cadena = "";

            if(id == "")
            {
                cadena = "SELECT id, nombre, apellido, asignatura, movil, correo_electronico FROM estudiante;";
            } else if (id != "")
            {
                cadena = $"SELECT id, nombre, apellido, asignatura, movil, correo_electronico FROM estudiante WHERE id='{id}';";
            }
            SqlCommand command = new SqlCommand(cadena, connection);
            SqlDataReader reader = command.ExecuteReader();
            dgvPantalla.Rows.Clear();

            while (reader.Read())
            {
               
                dgvPantalla.Rows.Add(reader["id"].ToString(), reader["nombre"].ToString(), reader["apellido"].ToString(), reader["asignatura"].ToString(), reader["movil"].ToString(), reader["correo_electronico"].ToString());
            }
            
            connection.Close();
        }

        private void actualizarRegistro()
        {
            string id = txtId.Text;
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;       
            string asignatura = txtAsignatura.Text;
            string movil = txtMovil.Text;
            string correoElectronico = txtCorreoElectronico.Text;
            
            if (nombre == "" || apellido == "" || asignatura == "" ||  movil == "" ||  correoElectronico == "")
            {
                MessageBox.Show("Debe completar todos los campos para actualizar un contacto!", "Error campos en blanco", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                DialogResult respuesta;
                respuesta = MessageBox.Show("Estás seguro de querer actualizar este registro?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta == DialogResult.Yes)
                {
                    connection.Open();
                    string cadena = $"UPDATE estudiante SET nombre='{nombre}', apellido='{apellido}', asignatura='{asignatura}', movil='{movil}', correo_electronico='{correoElectronico}' WHERE id='{id}'";
                    SqlCommand command = new SqlCommand(cadena, connection);
                    command.ExecuteNonQuery();
                    limpiarCampos();
                    MessageBox.Show("Registro Actualizado!", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    connection.Close();
                }
                else
                {
                    limpiarCampos();
                }
            }   
        }

        private void insertarRegistro()
        {
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string asignatura = txtAsignatura.Text;
            string movil = txtMovil.Text;
            string correoElectronico = txtCorreoElectronico.Text;

            if (nombre == "" || apellido == "" || asignatura == ""  || movil == "" || correoElectronico == "")
            {
                MessageBox.Show("Por favor completar campo, excepto Id para agregar un contacto!", "Error campos en blanco", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult respuesta;
                respuesta = MessageBox.Show("Seguro que desea agregar un nuevo registro?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    connection.Open();
                    string cadena = $"INSERT INTO estudiante (nombre, apellido, asignatura, movil, correo_electronico) VALUES ('{nombre}', '{apellido}', '{asignatura}', '{movil}', '{correoElectronico}')";
                    SqlCommand command = new SqlCommand(cadena, connection);
                    command.ExecuteNonQuery();
                    limpiarCampos();
                    MessageBox.Show("Registro Agregado!", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    connection.Close();
                }
                else
                {
                    limpiarCampos();
                }
            }
        }

        private void eliminarRegistro()
        {
            string id = txtId.Text;
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string asignatura = txtAsignatura.Text;
            string movil = txtMovil.Text;
            string correoElectronico = txtCorreoElectronico.Text;

            if (nombre == "" || apellido == "" ||  asignatura == "" || movil == "" || correoElectronico == "")
            {
                MessageBox.Show("Debe completar todos los campos para agregar un contacto!", "Error campos en blanco", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                DialogResult respuesta;
                respuesta = MessageBox.Show("Seguro que desea eliminar este registro?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    connection.Open();
                    string cadena = $"DELETE FROM estudiante WHERE id={id};";
                    SqlCommand command = new SqlCommand(cadena, connection);
                    command.ExecuteNonQuery();
                    limpiarCampos();
                    MessageBox.Show("Registro Eliminado!", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    connection.Close();
                } else
                {
                    limpiarCampos();
                }

            }
        }

        private void seleccionarRegistro(DataGridViewCellEventArgs e)
        {
            int nRow = e.RowIndex;
            if (nRow != -1)
            {
                txtId.Text = dgvPantalla.CurrentRow.Cells["Id"].Value.ToString();
                txtNombre.Text = dgvPantalla.CurrentRow.Cells["Nombre"].Value.ToString();
                txtApellido.Text = dgvPantalla.CurrentRow.Cells["Apellido"].Value.ToString();
          
                txtAsignatura.Text = dgvPantalla.CurrentRow.Cells["asignatura"].Value.ToString();
                
                
                txtMovil.Text = dgvPantalla.CurrentRow.Cells["movil"].Value.ToString();
                
                txtCorreoElectronico.Text = dgvPantalla.CurrentRow.Cells["correoElectronico"].Value.ToString();
            }
        }

        private void limpiarCampos()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
           
            txtAsignatura.Text = "";
            
            
            txtMovil.Text = "";
            
            txtCorreoElectronico.Text = "";
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            consultarBD(txtId.Text);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            consultarBD(txtId.Text);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            insertarRegistro();
            consultarBD(txtId.Text);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            actualizarRegistro();
            consultarBD(txtId.Text);
        }

        private void dgvPantalla_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            seleccionarRegistro(e);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminarRegistro();
            consultarBD(txtId.Text);
        }

        private void bunifuPictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiarCampos();
        }
    }
}
