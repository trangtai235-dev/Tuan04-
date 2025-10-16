using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using winFormSQL.Models;

namespace winFormSQL
{
    public partial class Form1 : Form
    {

        private Model1 db = new Model1 ();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadFacultyList();
            LoadStudentList();
            loadTongSv();
        }

        public void LoadFacultyList()
        {
            cmbFaculty.DataSource = db.Faculties.ToList();
            cmbFaculty.DisplayMember = "FacultyName";
            cmbFaculty.ValueMember = "FacultyID";
        }

        public void LoadStudentList()
        {
       
            dgvStudents.DataSource = db.Students
                .Select(s => new
                {
                    s.StudentID,
                    s.FullName,
                    FacultyName = s.Faculty.FacultyName,
                    s.AverageScore
                })
                .ToList();
        }
        public void loadTongSv()
        {
            txtTongSinhVien.Text = db.Students.Count(s => s.StudentID != null).ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtStudentID.Text) || string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtStudentID.Text.Length < 10 || txtStudentID.Text.Length > 10) throw new Exception("Vui long nhap mssv dung 10 ky tu");
                if (double.Parse(txtAverage.Text) < 0 || double.Parse(txtAverage.Text) > 10) throw new Exception("Vui long nhap diem hop le");


                Student s = new Student
                {
                    StudentID = txtStudentID.Text.Trim(),
                    FullName = txtFullName.Text.Trim(),
                    AverageScore = double.Parse(txtAverage.Text),
                    FacultyID = int.Parse(cmbFaculty.SelectedValue.ToString())
                };
                db.Students.Add(s);
                db.SaveChanges();
                LoadStudentList();
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo");
                loadTongSv();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm sinh viên: " + ex.Message);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var s = db.Students.Find(txtStudentID.Text.Trim());
                if (s != null)
                {
                    s.FullName = txtFullName.Text.Trim();
                    s.AverageScore = double.Parse(txtAverage.Text);
                    s.FacultyID = int.Parse(cmbFaculty.SelectedValue.ToString());
                    db.SaveChanges();
                    LoadStudentList();
                    MessageBox.Show("Cập nhật sinh viên thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtStudentID.Text = dgvStudents.Rows[e.RowIndex].Cells["StudentID"].Value.ToString();
                txtFullName.Text = dgvStudents.Rows[e.RowIndex].Cells["FullName"].Value.ToString();
                txtAverage.Text = dgvStudents.Rows[e.RowIndex].Cells["AverageScore"].Value.ToString();
                cmbFaculty.Text = dgvStudents.Rows[e.RowIndex].Cells["FacultyName"].Value.ToString();
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
                    var s = db.Students.Find(txtStudentID.Text.Trim());
                    if (s != null)
                    {
                        db.Students.Remove(s);
                        db.SaveChanges();
                        LoadStudentList();
                        MessageBox.Show("Xóa sinh viên thành công!");
                        loadTongSv();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa sinh viên: " + ex.Message);
            }
        }


        private void callForm2()
        {
            this.Hide(); 

            Form2 frm = new Form2();
            frm.FormClosed += (s, args) => this.Show(); 
            frm.Show(); 
        }

        private void callForm3()
        {
            this.Hide();

            Form3 frm = new Form3(this);
            frm.FormClosed += (s, args) => this.Show();
            frm.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            callForm2();

        }

        private void mnFaculty_Click(object sender, EventArgs e)
        {
            callForm2();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            callForm3();
        }

        private void mnSearch_Click(object sender, EventArgs e)
        {
            callForm3();
        }
    }
}
