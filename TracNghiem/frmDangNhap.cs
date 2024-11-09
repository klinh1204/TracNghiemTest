using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TracNghiem
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string ID = txtID.Text.Trim();
            string matkhau = txtMK.Text.Trim();
            if (string.IsNullOrEmpty(ID) || string.IsNullOrEmpty(matkhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.");
                return;
            }
            string query = "SELECT * FROM tblNguoiDung WHERE MaNguoiDung='" + ID + "' AND MatKhau='" + matkhau + "'";
            DataTable dtnd = DatabaseHelper.ExecuteQuery(query);
            if (dtnd.Rows.Count == 0)
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
                return;
            }    
            else
            {
                Session.ID = ID;
                Session.ThoiGianDangNhap = DateTime.Now;
                MessageBox.Show("Đăng nhập thành công!");
                this.Hide();
                Form FormNext = new frmVaoThi();
                FormNext.Show();
            }
        }
    }
}
