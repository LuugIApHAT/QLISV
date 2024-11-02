using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.VisualBasic.Logging;

namespace QLISV
{
    public partial class dky : Form
    {
        // Chuỗi kết nối đến SQL Server
        private readonly string connectionString = "Server=localhost;Database=QLISV;Trusted_Connection=True;";

        public dky()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*'; // Ẩn mật khẩu bằng ký tự '*'
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các TextBox
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            string email = textBox3.Text.Trim();
            string phone = textBox4.Text.Trim();

            // Kiểm tra dữ liệu đầu vào (ví dụ: kiểm tra trường rỗng)
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc.");
                return;
            }

            // Câu lệnh SQL để chèn dữ liệu vào bảng Users
            string query = "INSERT INTO Users (Username, Password, Email, Phone) VALUES (@Username, @Password, @Email, @Phone)";

            // Kết nối và chèn dữ liệu
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Phone", phone);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Đăng ký thành công.");
                        ClearFields(); // Xóa các trường nhập liệu sau khi đăng ký thành công

                        // Chuyển sang trang đăng nhập
                        Form loginForm = new Form1();
                        loginForm.Show();
                        this.Hide(); // Ẩn form đăng ký
                    }
                    else
                    {
                        MessageBox.Show("Đăng ký không thành công. Vui lòng thử lại.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void ClearFields()
        {
            // Xóa nội dung các TextBox sau khi đăng ký thành công
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi nội dung của textBox1 (Username) thay đổi (nếu cần)
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi nội dung của textBox2 (Password) thay đổi (nếu cần)
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi nội dung của textBox3 (Email) thay đổi (nếu cần)
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi nội dung của textBox4 (Phone) thay đổi (nếu cần)
        }

        private void label5_Click(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi nhấp vào label5 (nếu cần)
        }
    }
}
