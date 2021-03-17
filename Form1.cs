using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4demoWinForm
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source= DESKTOP-G2GES0F; Initial Catalog= DemoCRUD; Integrated Security= True");
        int StudentID=0;
        public Form1()
        {
            InitializeComponent();
            GetStudentsRecord();
        }

        private void GetStudentsRecord()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM STUDENTsTB", conn);
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            studentRecordData.DataSource = dt;
        }

        private bool isValidData()
        {
            if (familyNameTextBox.Text == string.Empty
                || givenNameTextBox.Text == string.Empty
                || addressTextBox.Text == string.Empty
                || string.IsNullOrEmpty(phoneNumberTextBox.Text)
                || string.IsNullOrEmpty(noTextBox.Text))
            {
                MessageBox.Show("Có chỗ chưa nhập dữ liệu!!!", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if(isValidData())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO STUDENTsTB VALUES " +
                                                "(@Name, @FatherName, @RollNumber, @Address, @Mobile)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", familyNameTextBox.Text);
                cmd.Parameters.AddWithValue("@FatherName", givenNameTextBox.Text);
                cmd.Parameters.AddWithValue("@RollNumber", noTextBox.Text);
                cmd.Parameters.AddWithValue("@Address", addressTextBox.Text);
                cmd.Parameters.AddWithValue("@Mobile", phoneNumberTextBox.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                GetStudentsRecord();
            }
        }

        private void studentRecordData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(studentRecordData.CurrentRow.Index!=-1)
            {
                if (studentRecordData.CurrentCell != null && studentRecordData.CurrentCell.Value != null)
                {
                    
                    //set SelectionMode to FullRowSelected to use "SelectedRows[0]"
                    StudentID = Convert.ToInt32(studentRecordData.SelectedRows[0].Cells[0].Value);
                    //MessageBox.Show(StudentID.ToString());
                    familyNameTextBox.Text = studentRecordData.SelectedRows[0].Cells[1].Value.ToString();
                    givenNameTextBox.Text = studentRecordData.SelectedRows[0].Cells[2].Value.ToString();
                    noTextBox.Text = studentRecordData.SelectedRows[0].Cells[3].Value.ToString();
                    addressTextBox.Text = studentRecordData.SelectedRows[0].Cells[4].Value.ToString();
                    phoneNumberTextBox.Text = studentRecordData.SelectedRows[0].Cells[5].Value.ToString();
                }
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (StudentID > 0)
            {
                SqlCommand cmd = new SqlCommand("UPDATE STUDENTsTB  SET " +
                                                "Name=@Name, FatherName=@FatherName, RollNumber=@RollNumber, Address=@Address, Mobile=@Mobile " +
                                                "WHERE StudentID=@StudentID", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", familyNameTextBox.Text);
                cmd.Parameters.AddWithValue("@FatherName", givenNameTextBox.Text);
                cmd.Parameters.AddWithValue("@RollNumber", noTextBox.Text);
                cmd.Parameters.AddWithValue("@Address", addressTextBox.Text);
                cmd.Parameters.AddWithValue("@Mobile", phoneNumberTextBox.Text);
                cmd.Parameters.AddWithValue("@StudentID", this.StudentID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                GetStudentsRecord();
                ResetData();
            }
            else
                MessageBox.Show("Cập nhật bị lỗi!!!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ResetData()
        {
            StudentID = 0;
            familyNameTextBox.Text = string.Empty;
            givenNameTextBox.Text = string.Empty;
            noTextBox.Text = string.Empty;
            addressTextBox.Text = string.Empty;
            phoneNumberTextBox.Text = string.Empty;
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (StudentID > 0)
            {
                SqlCommand cmd = new SqlCommand("DELETE STUDENTsTB " +
                                                "WHERE StudentID=@StudentID", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@StudentID", this.StudentID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                GetStudentsRecord();
                ResetData();
            }
            else
                MessageBox.Show("Xóa bị lỗi!!!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
