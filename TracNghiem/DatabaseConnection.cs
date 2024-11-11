using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class DatabaseHelper
{
    // Chuỗi kết nối từ file cấu hình (thay thế hard-code connection string)
    private static readonly string connectionString = "Data Source=MI-NOTEBOOK\\SQLEXPRESS;Initial Catalog=TracNghiem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    // Phương thức mở kết nối với using để tự động đóng
    public static SqlConnection GetConnection()
    {
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        return connection;
    }

    // Phương thức lấy dữ liệu (SELECT)
    public static DataTable ExecuteQuery(string query)
    {
        using (SqlConnection connection = GetConnection())
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
    }

    // Dùng cái này để lấy kết quả MaLuotThi từ procedure
    public static object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
    {
        using (SqlConnection connection = GetConnection())
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
            return command.ExecuteScalar();
        }
    }

    // Phương thức thêm, sửa, xóa dữ liệu (INSERT, UPDATE, DELETE)
    public static int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
    {
        using (SqlConnection connection = GetConnection())
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
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
        public bool isSelected { get; set; }
    }

    // Phương thức lấy danh sách câu hỏi và đáp án
    public static List<CauHoi> LayDSCH(string MaLuotThi)
    {
        List<CauHoi> DSCauHoi = new List<CauHoi>();
        string query = @"SELECT lc.MaCauHoi, ch.NoiDung AS NoiDungCauHoi, 
                         da.MaDapAn, da.NoiDung AS NoiDungDapAn, da.DungSai
                         FROM tblLuaChon lc
                         JOIN tblCauHoi ch ON lc.MaCauHoi = ch.MaCauHoi
                         JOIN tblDapAn da ON ch.MaCauHoi = da.MaCauHoi
                         WHERE lc.MaLuotThi = @MaLuotThi
                         ORDER BY lc.MaCauHoi, NEWID()";

        using (SqlConnection connection = GetConnection())
        using (SqlCommand sqlCommand = new SqlCommand(query, connection))
        {
            sqlCommand.Parameters.AddWithValue("@MaLuotThi", MaLuotThi);
            using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
            {
                string currentQuestionId = null;
                CauHoi currentQuestion = null;

                while (dataReader.Read())
                {
                    string MaCauHoi = dataReader["MaCauHoi"].ToString();
                    if (MaCauHoi != currentQuestionId)
                    {
                        currentQuestion = new CauHoi
                        {
                            MaCauHoi = MaCauHoi,
                            NoiDungCauHoi = dataReader["NoiDungCauHoi"].ToString(),
                            DSDapAn = new List<DapAn>()
                        };
                        DSCauHoi.Add(currentQuestion);
                        currentQuestionId = MaCauHoi;
                    }

                    currentQuestion.DSDapAn.Add(new DapAn
                    {
                        MaDapAn = dataReader["MaDapAn"].ToString(),
                        NoiDungDapAn = dataReader["NoiDungDapAn"].ToString(),
                        DungSai = Convert.ToInt32(dataReader["DungSai"])
                    });
                }
            }
        }
        return DSCauHoi;
    }

    public static void LuuLuaChon(string MaLuotThi, List<CauHoi> DSCH)
    {
        foreach (var cauHoi in DSCH)
        {
            foreach (var dapAn in cauHoi.DSDapAn)
            {
                if (dapAn.isSelected)
                {
                    string query = "UPDATE tblLuaChon SET MaDapAn=@MaDapAn WHERE MaLuotThi=@MaLuotThi AND MaCauHoi=@MaCauHoi";
                    var parameters = new Dictionary<string, object>
                    {
                        { "@MaDapAn", dapAn.MaDapAn },
                        { "@MaLuotThi", MaLuotThi },
                        { "@MaCauHoi", cauHoi.MaCauHoi }
                    };
                    ExecuteNonQuery(query, parameters);
                }
            }
        }
    }

    public static void TinhDiem(string MaLuotThi, DateTime TGKT)
    {
        string query = "EXEC HoanThanh @MaLuotThi, @TGKT";
        var parameters = new Dictionary<string, object>
        {
            { "@MaLuotThi", MaLuotThi },
            { "@TGKT", TGKT }
        };
        ExecuteNonQuery(query, parameters);
    }
}