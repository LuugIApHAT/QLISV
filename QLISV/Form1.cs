using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLISV
{
    public partial class Form1 : Form
    {
        // Chuỗi kết nối đến SQL Server
        private readonly string connectionString = "Server=localhost;Database=QLISV;Trusted_Connection=True;";

        public Form1()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*'; // Ẩn mật khẩu bằng ký tự '*'
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các TextBox
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            // Kiểm tra dữ liệu đầu vào (ví dụ: kiểm tra trường rỗng)
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.");
                return;
            }

            // Câu lệnh SQL để kiểm tra tài khoản và mật khẩu
            string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND Password = @Password";

            // Kết nối và kiểm tra thông tin đăng nhập
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                try
                {
                    connection.Open();
                    int userExists = (int)command.ExecuteScalar();

                    if (userExists > 0)
                    {
                        MessageBox.Show("Đăng nhập thành công.");
                        // Chuyển sang trang QLISV
                        QLISV qlisvForm = new QLISV();
                        qlisvForm.Show();
                        this.Hide(); // Ẩn form đăng nhập
                    }
                    else
                    {
                        MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu. Vui lòng thử lại.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi nội dung của textBox1 (Username) thay đổi (nếu cần)
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi nội dung của textBox2 (Password) thay đổi (nếu cần)
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Mở form đăng ký
            dky dkyForm = new dky();
            dkyForm.Show();
        }
    }
}
