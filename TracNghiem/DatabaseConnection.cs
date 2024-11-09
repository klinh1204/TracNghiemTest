using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

public class DatabaseHelper
{
    // Chuỗi kết nối (Connection String)
    private static string connectionString = "Data Source=MI-NOTEBOOK\\SQLEXPRESS;Initial Catalog=TracNghiem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    private static SqlConnection connection;

    // Phương thức mở kết nối
    public static SqlConnection GetConnection()
    {
        if (connection == null)
        {
            connection = new SqlConnection(connectionString);
        }

        if (connection.State == ConnectionState.Closed)
        {
            connection.Open();
        }

        return connection;
    }

    // Phương thức lấy dữ liệu (SELECT)
    public static DataTable ExecuteQuery(string query)
    {
        DataTable dt = new DataTable();
        using (SqlCommand command = new SqlCommand(query, GetConnection()))
        {
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
        }
        return dt;
    }

    //Dùng cái này để lấy kết quả MaLuotThi từ procedure
    public static object ExecuteScalar(string query)
    {
        object result;
        using (SqlConnection connection = new SqlConnection("Data Source=MI-NOTEBOOK\\SQLEXPRESS;Initial Catalog=TracNghiem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            result = command.ExecuteScalar();
            connection.Close();
        }
        return result;
    }

    // Phương thức thêm, sửa, xóa dữ liệu (INSERT, UPDATE, DELETE)
    public static int ExecuteNonQuery(string query)
    {
        using (SqlCommand command = new SqlCommand(query, GetConnection()))
        {
            return command.ExecuteNonQuery();
        }
    }

    public class CauHoi
    {
        public string MaCauHoi { get; set; }
        public string NoiDungCauHoi { get; set; }
        public List<DapAn> DSDapAn { get; set; }
    }

    public class DapAn
    {
        public string MaDapAn { get; set; }
        public string NoiDungDapAn { get; set; }
        public int DungSai { get; set; }
        public Boolean isSelected;
    }

    public static List<CauHoi> LayDSCH(string MaLuotThi)
    {
        List<CauHoi> DSCauHoi = new List<CauHoi>();
        string queryDS = @"SELECT lc.MaCauHoi, ch.NoiDung AS NoiDungCauHoi, 
                   da.MaDapAn, da.NoiDung AS NoiDungDapAn, da.DungSai
            FROM tblLuaChon lc
            JOIN tblCauHoi ch ON lc.MaCauHoi = ch.MaCauHoi
            JOIN tblDapAn da ON ch.MaCauHoi = da.MaCauHoi
            WHERE lc.MaLuotThi = @MaLuotThi
            ORDER BY lc.MaCauHoi, NEWID()";
        // Mở kết nối và thực thi truy vấn
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand sqlCommand = new SqlCommand(queryDS, connection);
            sqlCommand.Parameters.AddWithValue("@MaLuotThi", MaLuotThi); // Truyền tham số vào câu lệnh SQL

            connection.Open();
            SqlDataReader dataReader = sqlCommand.ExecuteReader();

            string currentQuestionId = ""; // Lưu trữ mã câu hỏi hiện tại
            CauHoi currentQuestion = null;
            // Đọc từng dòng dữ liệu trong kết quả trả về
            while (dataReader.Read())
            {
                string MaCauHoi = dataReader["MaCauHoi"].ToString();

                // Nếu gặp câu hỏi mới, tạo mới đối tượng Question
                if (MaCauHoi != currentQuestionId)
                {
                    currentQuestion = new CauHoi
                    {
                        MaCauHoi = MaCauHoi,
                        NoiDungCauHoi = dataReader["NoiDungCauHoi"].ToString(),
                        DSDapAn = new List<DapAn>()
                    };

                    DSCauHoi.Add(currentQuestion); // Thêm câu hỏi vào danh sách
                    currentQuestionId = MaCauHoi; // Cập nhật mã câu hỏi hiện tại
                }

                // Thêm đáp án cho câu hỏi hiện tại
                currentQuestion.DSDapAn.Add(new DapAn
                {
                    MaDapAn = dataReader["MaDapAn"].ToString(),
                    NoiDungDapAn = dataReader["NoiDungDapAn"].ToString(),
                    DungSai = Convert.ToInt32(dataReader["DungSai"])
                });
            }
            return DSCauHoi; // Trả về danh sách câu hỏi và đáp án
        }
    }

    public static void LuuLuaChon(string MaLuotThi, List<CauHoi> DSCH)
    {
        //gán biến để duyệt các câu hỏi trong danh sách từ đầu đến cuối, xem có đáp án chưa, và có thì là đáp án nào. thì insert vào dtb
        int i = 0;
        for (i=0; i<DSCH.Count; i++)
        {
            var CauHoi = DSCH[i];
            if (CauHoi.DSDapAn.Count>2)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (CauHoi.DSDapAn[j].isSelected == true)
                    {
                        string query = "UPDATE tblLuaChon SET MaDapAn='" + CauHoi.DSDapAn[j].MaDapAn + "' WHERE MaLuotThi='" + MaLuotThi + "' AND MaCauHoi='" + CauHoi.MaCauHoi + "'";
                        ExecuteNonQuery(query);
                    }
                }
            }
            else
            {
                for (int j = 0; j < 2; j++)
                {
                    if (CauHoi.DSDapAn[j].isSelected == true)
                    {
                        string query = "UPDATE tblLuaChon SET MaDapAn='" + CauHoi.DSDapAn[j].MaDapAn + "' WHERE MaLuotThi='" + MaLuotThi + "' AND MaCauHoi='" + CauHoi.MaCauHoi + "'";
                        ExecuteNonQuery(query);
                    }
                }
            } 
                
        }    
    }

    public static void TinhDiem(string MaLuotThi, DateTime TGKT)
    {
        string query = "EXEC HoanThanh '" + MaLuotThi + "','" + TGKT +"'";
        ExecuteNonQuery(query);
    }
}

