using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracNghiem
{
    public static class Session
    {
        // Thuộc tính để lưu trữ thông tin mã sinh viên đăng nhập
        public static string ID { get; set; }

        // Thuộc tính lưu thời gian đăng nhập
        public static DateTime ThoiGianDangNhap { get; set; }

        // Phương thức để kiểm tra xem phiên có hợp lệ không
        public static bool IsSessionActive()
        {
            return !string.IsNullOrEmpty(ID);
        }

        // Phương thức để đăng xuất hoặc xóa thông tin phiên, gọi ra lúc tắt
        public static void ClearSession()
        {
            ID = null;
            ThoiGianDangNhap = default;
        }
    }

}
