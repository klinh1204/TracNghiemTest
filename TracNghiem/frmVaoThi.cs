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
    public partial class frmVaoThi : Form
    {
        public frmVaoThi()
        {
            InitializeComponent();
        }
        public void NapCT()
        {
            string query = "SELECT MaDeThi, TenDeThi, MaGV, SoCauNB, SoCauTH, SoCauVD, ThoiLuong, Chuong FROM tblDeThi WHERE MaMonHoc='" + cbMonHoc.SelectedValue + "'" + "AND MaDeThi='" + cbDeThi.SelectedValue + "'";
            DataTable dtnapct = DatabaseHelper.ExecuteQuery(query);
            txtMaDeThi.Text = dtnapct.Rows[0]["MaDeThi"].ToString();
            txtTenDeThi.Text = dtnapct.Rows[0]["TenDeThi"].ToString();
            txtMaGV.Text = dtnapct.Rows[0]["MaGV"].ToString();
            txtChuong.Text = dtnapct.Rows[0]["Chuong"].ToString();
            txtSCNB.Text = dtnapct.Rows[0]["SoCauNB"].ToString();
            txtSCTH.Text = dtnapct.Rows[0]["SoCauTH"].ToString();
            txtSCVD.Text = dtnapct.Rows[0]["SoCauVD"].ToString();
            txtThoiLuong.Text = dtnapct.Rows[0]["ThoiLuong"].ToString();
        }
        private void LoadCbMonHoc()
        {
            string query = "SELECT TenMon, MaMonHoc FROM tblMonHoc";
            DataTable cbdt = DatabaseHelper.ExecuteQuery(query);
            cbMonHoc.DataSource = cbdt;
            cbMonHoc.DisplayMember = "TenMon"; // Tên hiển thị
            cbMonHoc.ValueMember = "MaMonHoc"; // Giá trị ẩn (ID)
        }
        private void LoadCbDeThi()
        {
            string query = "SELECT TenDeThi, MaDeThi FROM tblDeThi WHERE MaMonHoc='"+cbMonHoc.SelectedValue+"'";
            DataTable cbdt = DatabaseHelper.ExecuteQuery(query);
            cbDeThi.DataSource = cbdt;
            cbDeThi.DisplayMember = "TenDeThi"; // Tên hiển thị
            cbDeThi.ValueMember = "MaDeThi"; // Giá trị ẩn (ID)
        }
        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCbDeThi();
        }

        private void lblMonHoc_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel9_Click(object sender, EventArgs e)
        {

        }

        private void frmVaoThi_Load(object sender, EventArgs e)
        {
            LoadCbMonHoc();
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbDeThi_SelectedIndexChanged(object sender, EventArgs e)
        {
            NapCT();
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMaSV_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click_1(object sender, EventArgs e)
        {

        }
        public static string MaLuotThi;
        private void btnVaoLam_Click(object sender, EventArgs e)
        {
            DateTime TGBD = DateTime.Now;
            string strSCNB = txtSCNB.Text;
            int SCNB = int.Parse(strSCNB);
            string strSCTH = txtSCNB.Text;
            int SCTH = int.Parse(strSCTH);
            string strSCVD = txtSCNB.Text;
            int SCVD = int.Parse(strSCVD);
            string strChuong = txtChuong.Text;
            int Chuong = int.Parse(strChuong);
            string VaoThi = "EXEC VaoThi '" + TGBD + "','" + Session.ID + "','" + txtMaDeThi.Text + "','" + cbMonHoc.SelectedValue + "','" + SCNB + "','" + SCTH + "','" + SCVD + "','" + Chuong + "'";
            object result = DatabaseHelper.ExecuteScalar(VaoThi);
            MaLuotThi = result.ToString();  // Chuyển đổi kết quả sang kiểu string
            this.Close();
            Form formnext = new frmLamBai();
            formnext.Show();
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
