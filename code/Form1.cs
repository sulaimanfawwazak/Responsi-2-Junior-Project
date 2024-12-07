using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Responsi2Junpro
{
    public partial class Form1 : Form
    {
        private NpgsqlConnection conn;
        string connstring = "Host=localhost;port=5432;Username=postgres;Password=informatika;Database=Perusahaan";

        public DataTable dt;
        public static NpgsqlCommand cmd;
        private string sql = null;
        private DataGridViewRow r;

        public Form1()
        {
            InitializeComponent();

            cbDepKaryawan.DataSource = new DepartemenDropDown[]
            {
                new DepartemenDropDown{ ID = 1, Name = "HR"},
                new DepartemenDropDown{ ID = 2, Name = "Engineer"},
                new DepartemenDropDown{ ID = 3, Name = "Developer"},
                new DepartemenDropDown{ ID = 4, Name = "Product Manager"},
                new DepartemenDropDown{ ID = 5, Name = "Finance"}
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {

                conn = new NpgsqlConnection(connstring);
                conn.Open();

                // Initialize DataGridView
                dgvTableData.DataSource = null;
                sql = "SELECT k.id_karyawan, k.nama, k.id_dep, d.kode_dep, d.nama_dep FROM karyawan AS k LEFT JOIN departemen AS d ON k.id_dep = d.id_dep";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                NpgsqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dgvTableData.DataSource = dt;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Select Fail!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"INSERT INTO karyawan (id_karyawan, nama, id_dep) VALUES (_id_karyawan, _nama, _id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", tbNamaKaryawan.Text[0] + tbNamaKaryawan.Text[1] + tbNamaKaryawan.Text[2] + tbNamaKaryawan.Text[3] + tbNamaKaryawan.Text[4] + tbNamaKaryawan.Text[5]);
                cmd.Parameters.AddWithValue("_nama", tbNamaKaryawan.Text);
                cmd.Parameters.AddWithValue("_id_dep", tbNamaKaryawan.Text);
                dt = new DataTable();
                NpgsqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dgvTableData.DataSource = dt;

                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Select failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            conn.Open();
            sql = "UPDATE TABLE karyawan SET nama = _nama, id_dep = _id_dep";
            cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("_nama", tbNamaKaryawan.Text);
            cmd.Parameters.AddWithValue("_id_dep", (int)cbDepKaryawan.SelectedValue);

            if ((int)cmd.ExecuteScalar() == 1)
            {
                MessageBox.Show("Data Karyawan berhasil di-update", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                conn.Close();
                tbNamaKaryawan.Text = null;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"DELETE FROM karyawan WHERE nama = _nama AND dep_id = _dep_id";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_nama", tbNamaKaryawan.Text);
                cmd.Parameters.AddWithValue("_dep_id", (int)cbDepKaryawan.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Gagal menghapus data karyawan!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if ((int)cmd.ExecuteScalar() == 1)
            {
                MessageBox.Show("Data Karyawan berhasil dihapus", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                conn.Close();
                tbNamaKaryawan.Text = null;
            }
        }
    }
}
