using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using winFormSQL.Models;

namespace winFormSQL
{
    public partial class Form3 : Form
    {

        private Form1 parentForm;
        private Model1 db = new Model1();
        public Form3(Form1 parent)
        {
            InitializeComponent();
            this.parentForm = parent;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            loadFac();
            dgvSearch.DataSource = db.Students
                                    .Select(s => new
                                    {
                                        s.StudentID,
                                        s.FullName,
                                        FacultyName = s.Faculty.FacultyName,
                                        s.AverageScore
                                    })
                                    .ToList();
        }

        private void loadFac()
        {
            cmbFaculty.DataSource = db.Faculties.ToList();
            cmbFaculty.DisplayMember = "FacultyName";
            cmbFaculty.ValueMember = "FacultyID";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            int facultyId = -1;
            if (cmbFaculty.SelectedValue != null)
            {
                int.TryParse(cmbFaculty.SelectedValue.ToString(), out facultyId);
            }

            string studentId = txtStudentID.Text.Trim();
            string fullName = txtFullName.Text.Trim();

            var student = db.Students
                .Where(s =>
                    (string.IsNullOrEmpty(studentId) || s.StudentID == studentId) &&
                    (string.IsNullOrEmpty(fullName) || s.FullName == fullName) &&
                    (facultyId != -1 && s.FacultyID == facultyId))
                .Select(s => new
                {
                    s.StudentID,
                    s.FullName,
                    FacultyName = s.Faculty.FacultyName,
                    s.AverageScore
                })
                .ToList();


            if (student.Count > 0)
            {
                dgvSearch.DataSource = student;
                lblResult.Text = $"Kết quả tìm kiếm: {student.Count}";
            }
            else
            {
                MessageBox.Show("Không tìm thấy sinh viên phù hợp.");
                dgvSearch.DataSource = null;
                lblResult.Text = "Kết quả tìm kiếm: 0";
            }
        }


        private void dgvSearch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0)
            {
                txtStudentID.Text = dgvSearch.Rows[e.RowIndex].Cells["StudentID"].Value.ToString();
                txtFullName.Text = dgvSearch.Rows[e.RowIndex].Cells["FullName"].Value.ToString();
                cmbFaculty.Text = dgvSearch.Rows[e.RowIndex].Cells["FacultyName"].Value.ToString();

            }
        }

        private void dgvSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtStudentID.Text = dgvSearch.Rows[e.RowIndex].Cells["StudentID"].Value.ToString();
            txtFullName.Text = dgvSearch.Rows[e.RowIndex].Cells["FullName"].Value.ToString();
            cmbFaculty.Text = dgvSearch.Rows[e.RowIndex].Cells["FacultyName"].Value.ToString();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            parentForm.Show();           
            parentForm.LoadStudentList();
            parentForm.loadTongSv();      
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
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
                        MessageBox.Show("Xóa sinh viên thành công!");
                        txtFullName.Text = "";
                        txtFullName.Text = "";
                        cmbFaculty.SelectedIndex = 0;
                        dgvSearch.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa sinh viên: " + ex.Message);
            }
        }
    }
}
