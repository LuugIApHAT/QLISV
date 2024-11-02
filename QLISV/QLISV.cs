using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLISV
{
    public partial class QLISV : Form
    {
        // Chuỗi kết nối đến SQL Server
        private readonly string connectionString = "Server=localhost;Database=QLISV;Trusted_Connection=True;";

        public QLISV()
        {
            InitializeComponent();

            // Cấu hình DataGridView
            ConfigureDataGridView();
            LoadData();

            // Liên kết sự kiện CellClick với phương thức xử lý
            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        private void ConfigureDataGridView()
        {
            // Tạo các cột cho DataGridView và ngăn chỉnh sửa trực tiếp
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "Mã SV";
            dataGridView1.Columns[1].Name = "Tên SV";
            dataGridView1.Columns[2].Name = "Giới Tính";
            dataGridView1.Columns[3].Name = "Ngành";
            dataGridView1.Columns[4].Name = "Lớp";
            dataGridView1.ReadOnly = true;
        }

        private void ClearFields()
        {
            // Xóa nội dung các TextBox và reset các điều khiển khác
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear(); // Xóa nội dung của TextBox Lớp
            textBox4.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;

            // Mở khóa textBox1 để nhập Mã SV mới
            textBox1.Enabled = true;
        }

        private void LoadData()
        {
            // Tải dữ liệu từ cơ sở dữ liệu vào DataGridView
            string query = "SELECT MaSV, TenSV, GioiTinh, Nganh, MaLop FROM SinhVien";
            dataGridView1.Rows.Clear(); // Xóa các dòng cũ

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    dataGridView1.Rows.Add(row["MaSV"], row["TenSV"], row["GioiTinh"], row["Nganh"], row["MaLop"]);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Mở khóa textBox1 để có thể nhập Mã SV mới
            textBox1.Enabled = true;

            // Kiểm tra các trường bắt buộc trước khi thêm sinh viên
            if (string.IsNullOrEmpty(textBox1.Text) ||
                string.IsNullOrEmpty(textBox2.Text) ||
                string.IsNullOrEmpty(textBox3.Text) ||
                string.IsNullOrEmpty(textBox4.Text) ||
                (!radioButton1.Checked && !radioButton2.Checked))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng việc thêm nếu thông tin chưa đủ
            }

            // Kiểm tra xem Mã SV đã tồn tại trong cơ sở dữ liệu chưa
            string maSV = textBox1.Text.Trim();
            string checkQuery = "SELECT COUNT(*) FROM SinhVien WHERE MaSV = @MaSV";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@MaSV", maSV);
                connection.Open();

                int count = (int)checkCommand.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("Mã SV đã tồn tại. Vui lòng nhập mã khác.", "Trùng Mã SV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Dừng việc thêm nếu Mã SV đã tồn tại
                }
            }

            // Thêm sinh viên vào cơ sở dữ liệu và DataGridView
            string gender = radioButton1.Checked ? "Nam" : "Nữ";
            string insertQuery = "INSERT INTO SinhVien (MaSV, TenSV, GioiTinh, Nganh, MaLop) VALUES (@MaSV, @TenSV, @GioiTinh, @Nganh, @MaLop)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@MaSV", maSV);
                insertCommand.Parameters.AddWithValue("@TenSV", textBox2.Text);
                insertCommand.Parameters.AddWithValue("@GioiTinh", gender);
                insertCommand.Parameters.AddWithValue("@Nganh", textBox4.Text);
                insertCommand.Parameters.AddWithValue("@MaLop", textBox3.Text);

                connection.Open();
                insertCommand.ExecuteNonQuery();
            }

            // Tải lại dữ liệu và xóa trường nhập liệu
            LoadData();
            ClearFields();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Kiểm tra các trường bắt buộc trước khi sửa sinh viên
            if (string.IsNullOrEmpty(textBox1.Text) ||
                string.IsNullOrEmpty(textBox2.Text) ||
                string.IsNullOrEmpty(textBox3.Text) ||
                string.IsNullOrEmpty(textBox4.Text) ||
                (!radioButton1.Checked && !radioButton2.Checked))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng việc sửa nếu thông tin chưa đủ
            }

            // Sửa sinh viên trong cơ sở dữ liệu và DataGridView
            if (dataGridView1.CurrentRow != null)
            {
                string gender = radioButton1.Checked ? "Nam" : "Nữ";
                string maSV = textBox1.Text.Trim(); // Lấy Mã SV từ TextBox và loại bỏ khoảng trắng
                string query = "UPDATE SinhVien SET TenSV = @TenSV, GioiTinh = @GioiTinh, Nganh = @Nganh, MaLop = @MaLop WHERE MaSV = @MaSV";

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaSV", maSV); // Mã SV không thay đổi
                    command.Parameters.AddWithValue("@TenSV", textBox2.Text);
                    command.Parameters.AddWithValue("@GioiTinh", gender);
                    command.Parameters.AddWithValue("@Nganh", textBox4.Text);
                    command.Parameters.AddWithValue("@MaLop", textBox3.Text);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật sinh viên thành công.");
                    }
                    else
                    {
                        MessageBox.Show($"Không tìm thấy sinh viên với Mã SV {maSV}. Vui lòng kiểm tra lại.");
                    }
                }

                // Tải lại dữ liệu và xóa trường nhập liệu
                LoadData();
                ClearFields();
            }
            else
            {
                MessageBox.Show("Hãy chọn một dòng để sửa.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có dòng nào được chọn trong DataGridView không
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Cells[0].Value != null)
            {
                string maSV = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                string query = "DELETE FROM SinhVien WHERE MaSV = @MaSV";

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaSV", maSV);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                // Tải lại dữ liệu và xóa trường nhập liệu
                LoadData();
                ClearFields();
            }
            else
            {
                MessageBox.Show("Hãy chọn một dòng để xóa.");
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            // Mở lại form đăng nhập
            Form1 loginForm = new Form1();
            loginForm.Show();

            // Đóng form hiện tại (QLISV)
            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu hàng được chọn hợp lệ và có dữ liệu
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Kiểm tra từng ô trước khi truy cập
                textBox1.Text = row.Cells[0].Value?.ToString().Trim() ?? ""; // Mã SV
                textBox2.Text = row.Cells[1].Value?.ToString() ?? "";         // Tên SV

                string gender = row.Cells[2].Value?.ToString() ?? "";         // Giới Tính
                radioButton1.Checked = (gender == "Nam");
                radioButton2.Checked = (gender == "Nữ");

                textBox4.Text = row.Cells[3].Value?.ToString() ?? "";         // Ngành
                textBox3.Text = row.Cells[4].Value?.ToString() ?? "";         // Lớp

                // Khóa ô nhập Mã SV để tránh thay đổi khi sửa
                textBox1.Enabled = false;
            }
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi nội dung của textBox3 (Lớp) thay đổi (nếu cần)
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi nhấp vào label3 (nếu cần)
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                // Nếu textBox5 trống, tải lại tất cả dữ liệu
                LoadData();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Lấy từ khóa tìm kiếm từ textBox5
            string searchKeyword = textBox5.Text.Trim();

            // Kiểm tra nếu từ khóa không trống
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                // Câu lệnh SQL với điều kiện tìm kiếm theo Tên SV hoặc Mã SV
                string query = "SELECT MaSV, TenSV, GioiTinh, Nganh, MaLop FROM SinhVien " +
                               "WHERE MaSV LIKE @Keyword OR TenSV LIKE @Keyword";

                dataGridView1.Rows.Clear(); // Xóa các dòng cũ

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    // Sử dụng tham số cho từ khóa tìm kiếm, sử dụng dấu phần trăm (%) để tìm kiếm gần đúng
                    command.Parameters.AddWithValue("@Keyword", "%" + searchKeyword + "%");

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        dataGridView1.Rows.Add(row["MaSV"], row["TenSV"], row["GioiTinh"], row["Nganh"], row["MaLop"]);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.");
            }
        }
    }
}
