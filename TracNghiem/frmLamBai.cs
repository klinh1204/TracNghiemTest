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
    public partial class frmLamBai : Form
    {
        public List<DatabaseHelper.CauHoi> DSCH;
        int CurrentCauHoiIndex = 0;

        public frmLamBai()
        {
            InitializeComponent();
        }
        public void LoadData()
        {
            string MaLuotThi = frmVaoThi.MaLuotThi; // Lấy mã lượt thi từ form khác
            MessageBox.Show("Đã thực hiện, mã lượt thi: " + MaLuotThi);
            DSCH = DatabaseHelper.LayDSCH(MaLuotThi); // Truyền MaLuotThi vào LayDSCH

            // Kiểm tra nếu DSCH đã được lấy thành công và có dữ liệu
            if (DSCH == null || DSCH.Count == 0)
            {
                MessageBox.Show("Không có câu hỏi nào được tải.");
                return;
            }

            DisplayCauHoi();
        }

        public void DisplayCauHoi()
        {
            lblindex.Text = CurrentCauHoiIndex.ToString();
            if (CurrentCauHoiIndex < DSCH.Count)
            {
                var cauHoi = DSCH[CurrentCauHoiIndex];
                // Hiển thị nội dung câu hỏi
                lblCH.Text = cauHoi.NoiDungCauHoi;
                //Kiểm tra số lượng câu
                if (cauHoi.DSDapAn.Count <3)
                {
                    rdDA3.Visible = false;
                    rdDA4.Visible = false;
                }
                else
                {
                    rdDA3.Visible = true;
                    rdDA4.Visible = true;
                } 
                    
                // Kiểm tra từng đáp án trong trường hợp ít hơn 4 đáp án hoặc không có đáp án nào
                    rdDA1.Text = cauHoi.DSDapAn != null && cauHoi.DSDapAn.Count > 0 ? cauHoi.DSDapAn[0].NoiDungDapAn : "";
                    rdDA2.Text = cauHoi.DSDapAn != null && cauHoi.DSDapAn.Count > 1 ? cauHoi.DSDapAn[1].NoiDungDapAn : "";
                    rdDA3.Text = cauHoi.DSDapAn != null && cauHoi.DSDapAn.Count > 2 ? cauHoi.DSDapAn[2].NoiDungDapAn : "";
                    rdDA4.Text = cauHoi.DSDapAn != null && cauHoi.DSDapAn.Count > 3 ? cauHoi.DSDapAn[3].NoiDungDapAn : "";
                //Kiểm tra xem câu đó người dùng chọn đáp án chưa, rồi thì check vào đúng câu đã chọn, chưa thì không chọn gì
                    rdDA1.Checked = false;
                    rdDA2.Checked = false;
                    rdDA3.Checked = false;
                    rdDA4.Checked = false;
                    if (cauHoi.DSDapAn[0].isSelected == true) rdDA1.Checked = true;
                    if (cauHoi.DSDapAn[1].isSelected == true) rdDA2.Checked = true;
                    if (cauHoi.DSDapAn.Count > 2)
                    {
                        if (cauHoi.DSDapAn[2].isSelected == true) rdDA3.Checked = true;
                        if (cauHoi.DSDapAn[3].isSelected == true) rdDA4.Checked = true;
                    }
                }
            
        }

        public void LuaChon(int index)
        {
            var cauHoi = DSCH[CurrentCauHoiIndex];
            if (cauHoi.DSDapAn.Count >2)
            {
                for (int i = 0; i < 4; i++) cauHoi.DSDapAn[i].isSelected = false;
                cauHoi.DSDapAn[index - 1].isSelected = true;
            }
            else
            {
                for(int i = 0; i < 2; i++) cauHoi.DSDapAn[i].isSelected = false;
                cauHoi.DSDapAn[index - 1].isSelected = true;
            }  
        }

        //Khai báo timer
        private System.Windows.Forms.Timer aTimer;
        private int counter, secondsLeft;
        public int LayThoiLuong(string MaLuotThi)
        {
            string strThoiLuong;
            string query = "SELECT ThoiLuong FROM tblDeThi dt JOIN tblLuotThi lt ON dt.MaDeThi=lt.MaDeThi WHERE MaLuotThi='" + MaLuotThi + "'";
            object result = DatabaseHelper.ExecuteScalar(query);
            strThoiLuong = result.ToString();
            int ThoiLuong = int.Parse(strThoiLuong);
            return ThoiLuong;
        }

        //Tạo 1 navigation panel để map câu hỏi

        public void frmLamBai_Load(object sender, EventArgs e)
        {
            LoadData();
            TaoMapCauHoi(DSCH);
            string MaLuotThi = frmVaoThi.MaLuotThi;
            counter = LayThoiLuong(MaLuotThi);
            secondsLeft = 0;

            aTimer = new System.Windows.Forms.Timer();

            aTimer.Tick += new EventHandler(aTimer_Tick);

            aTimer.Interval = 1000; // 1 sec

            aTimer.Start();
        }
        private void aTimer_Tick(object sender, EventArgs e)

        {
            // Giảm dần giây còn lại mỗi lần Tick
            if (secondsLeft > 0)
            {
                secondsLeft--;
            }
            else if (counter > 0)
            {
                // Nếu hết giây, giảm phút và đặt lại giây
                counter--;
                secondsLeft = 59;
            }
            else
            {
                aTimer.Stop();
                MessageBox.Show("Đã hết giờ làm bài thi");
                string MaLuotThi = frmVaoThi.MaLuotThi;
                DateTime TGKT = DateTime.Now;
                DatabaseHelper.LuuLuaChon(MaLuotThi, DSCH);
                DatabaseHelper.TinhDiem(MaLuotThi, TGKT);
            }

            // Tính số giờ từ số phút
            int hours = counter / 60;
            int minutes = counter % 60;

            // Hiển thị thời gian theo định dạng giờ:phút:giây (hh:mm:ss)
            lblTimer.Text = $"{hours:D2}:{minutes:D2}:{secondsLeft:D2}";  // Định dạng hai chữ số cho giờ, phút, giây
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnKetThuc_Click(object sender, EventArgs e)
        {
            aTimer.Stop();
            MessageBox.Show("Đã hoàn thành bài thi");
            string MaLuotThi = frmVaoThi.MaLuotThi;
            DatabaseHelper.LuuLuaChon(MaLuotThi, DSCH);
            DateTime TGKT = DateTime.Now;
            DatabaseHelper.TinhDiem(MaLuotThi, TGKT);
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentCauHoiIndex < DSCH.Count -1)
            {
                CurrentCauHoiIndex++;
            }
            DisplayCauHoi();
        }

        private void lblCauHoi_Click(object sender, EventArgs e)
        {

        }

        private void lblCau_Click(object sender, EventArgs e)
        {

        }
        
        public void TaoMapCauHoi (List<DatabaseHelper.CauHoi> DSCH)
        {
            int SoCau = DSCH.Count;
            int i;
            for (i=0; i<SoCau; i++)
            {
                Button cau = new Button();
                cau.Text = (i+1).ToString();
                cau.Click += btnCau_click;
                pnMap.Controls.Add(cau);
                cau.Tag = i;
            }
            pnMap.AutoScroll = true;
        }

        public void btnCau_changecolor(int i)
        {
            var btn = pnMap.Controls.OfType<Button>()
                .FirstOrDefault(b => (int)b.Tag == i);
            bool DaChonDapAn = false;
            var CauHoi = DSCH[i];
            if (CauHoi.DSDapAn.Count > 2)
            {
                for (int m = 1; m < 5; m++)
                {
                    if (CauHoi.DSDapAn[m - 1].isSelected == true) DaChonDapAn = true;
                }
            }
            else
            {
                for (int m = 1; m < 3; m++)
                {
                    if (CauHoi.DSDapAn[m - 1].isSelected == true) DaChonDapAn = true;
                }
            }
            if (DaChonDapAn == true)
                btn.BackColor = Color.Gray;
        }    

        public void btnCau_click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            CurrentCauHoiIndex = (int)btn.Tag;
            DisplayCauHoi();
        }
        private void rdDA2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            if (radio.Checked) LuaChon(2);
            btnCau_changecolor(CurrentCauHoiIndex);
        }

        private void rdDA1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            if (radio.Checked) LuaChon(1);
            btnCau_changecolor(CurrentCauHoiIndex);
        }

        private void rdDA3_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            if (radio.Checked) LuaChon(3);
            btnCau_changecolor(CurrentCauHoiIndex);
        }

        private void rdDA4_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            if (radio.Checked) LuaChon(4);
            btnCau_changecolor(CurrentCauHoiIndex);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentCauHoiIndex>0)
            {
                CurrentCauHoiIndex--;
            }    
            
            DisplayCauHoi();
        }
    }
}
