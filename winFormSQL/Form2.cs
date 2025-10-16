using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using winFormSQL.Models;

namespace winFormSQL
{
    public partial class Form2 : Form
    {

        private Model1 db = new Model1(); 
        public Form2()
        {
            InitializeComponent();
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            LoadFaculty();
        }
        private void LoadFaculty()
        {
            dgvFaculty.DataSource = db.Faculties.Select(f => new { f.FacultyID, f.FacultyName, f.TotalProfessor }).ToList();
        }


        private bool ChuaKyTuDacBiet(string input)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9\s]");
            return regex.IsMatch(input);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtID.Text) || (string.IsNullOrEmpty(txtGS.Text) || (string.IsNullOrEmpty(txtName.Text)))) throw new Exception("Nhap day du");
                if (ChuaKyTuDacBiet(txtID.Text)) throw new Exception("Ma khoa khong hop le");
                if (int.Parse(txtGS.Text) < 0 || int.Parse(txtGS.Text) > 15) throw new Exception("So luong gs ko hop le");

                Faculty f = new Faculty { FacultyID = int.Parse(txtID.Text), FacultyName = txtName.Text , TotalProfessor = int.Parse(txtGS.Text)};
                db.Faculties.Add(f);
                db.SaveChanges();
                LoadFaculty();


            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
       
        }

        private void dgvFaculty_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                txtID.Text = dgvFaculty.Rows[e.RowIndex].Cells["FacultyID"].Value.ToString();
                txtName.Text = dgvFaculty.Rows[e.RowIndex].Cells["FacultyName"].Value.ToString();
                txtGS.Text = dgvFaculty.Rows[e.RowIndex].Cells["TotalProfessor"].Value?.ToString();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtID.Text); 
            var fac = db.Faculties.FirstOrDefault(f => f.FacultyID == id);

            if (fac != null)
            {
                fac.FacultyName = txtName.Text;
                fac.TotalProfessor = int .Parse(txtGS.Text);
                db.SaveChanges();
                LoadFaculty() ;
            }


        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult result = MessageBox.Show(
                                        "Bạn có chắc chắn muốn xoá?",
                                        "Xác nhận",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    if (int.TryParse(txtID.Text.Trim(), out int id))
                    {
                        var s = db.Faculties.Find(id);
                        if (s != null)
                        {
                            db.Faculties.Remove(s);
                            db.SaveChanges();
                            MessageBox.Show("Xóa khoa thành công!");
                            LoadFaculty();
                        }
                    }
                    else
                    {
                        MessageBox.Show("ID không hợp lệ. Vui lòng nhập số nguyên.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khoa: " + ex.Message);
            }
        }

        private void dgvFaculty_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
