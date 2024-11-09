EXEC VaoThi '2024-11-06 14:30:00','sinhvien','123','CSDL',3,3,3,0

create table tblLuaChon(
MaCauHoi varchar(50) foreign key references tblCauHoi(MaCauHoi),
MaLuotThi varchar(50) foreign key references tblLuotThi(MaLuotThi),
MaDapAn varchar(50) foreign key references tblDapAn(MaDapAn),
primary key (MaLuotThi, MaCauHoi)
)

CREATE PROCEDURE VaoThi
    @TGBD datetime,
    @MaSV varchar(50),
    @MaDeThi varchar(50),
	@MaMonHoc varchar(50),
    @SoCauNB INT,
    @SoCauTH INT,
    @SoCauVD INT,
    @Chuong INT
AS
BEGIN
    DECLARE @DemSoLuot int,
            @LuotSo int,
            --@GioiHan int,
			@MaLuotThi varchar(50);
			--@TotalSoCauNB INT,
			--@TotalSoCauTH INT,
			--@TotalSoCauVD INT;
	
    -- Đếm số lượt thi
    SELECT @DemSoLuot = COUNT(*)
    FROM tblLuotThi
    WHERE MaDeThi = @MaDeThi AND MaSV = @MaSV;

    -- Tính lượt thi
    SET @LuotSo = @DemSoLuot + 1;
	SET @MaLuotThi = @MaSV + @MaDeThi + CAST(@LuotSo AS VARCHAR);

    /* Lấy giới hạn từ bảng tblDeThi
    SELECT @GioiHan = GioiHan FROM tblDeThi WHERE MaDeThi = @MaDeThi;

    -- Kiểm tra điều kiện
    IF (@GioiHan IS NOT NULL AND @LuotSo > @GioiHan) 
    BEGIN
        PRINT 'Đã đạt giới hạn số lượt thi!';
    END

    ELSE */
    BEGIN
        INSERT INTO tblLuotThi (MaLuotThi, MaSV, MaDeThi, LuotSo, TGBD) 
        VALUES (@MaLuotThi, @MaSV, @MaDeThi, @LuotSo, @TGBD)
    END
	SELECT @MaLuotThi AS MaLuotThi

	IF (@Chuong = 0) -- Nếu chọn tất cả các chương
    BEGIN
		/* Lưu kết quả đếm vào biến
		SELECT @TotalSoCauNB = COUNT(*) 
		FROM tblCauHoi 
		WHERE DoKho = 'NB' AND MaMonHoc=@MaMonHoc;

		SELECT @TotalSoCauTH = COUNT(*) 
		FROM tblCauHoi 
		WHERE DoKho = 'TH' AND MaMonHoc=@MaMonHoc;

		SELECT @TotalSoCauVD = COUNT(*) 
		FROM tblCauHoi 
		WHERE DoKho = 'VD' AND MaMonHoc=@MaMonHoc;

		-- Kiểm tra số câu được nhập có hợp lý không
		IF @SoCauNB > @TotalSoCauNB OR @SoCauNB<0
		BEGIN
			PRINT 'Số câu hỏi nhận biết yêu cầu không hợp lý!';
			 RETURN; -- Dừng thực hiện
		END;
		IF @SoCauTH > @TotalSoCauTH OR @SoCauTH<0
		BEGIN
			PRINT 'Số câu hỏi thông hiểu yêu cầu không hợp lý!';
			 RETURN; -- Dừng thực hiện
		END;
		IF @SoCauVD > @TotalSoCauVD OR @SoCauVD<0
		BEGIN
			PRINT 'Số câu hỏi vận dụng yêu cầu không hợp lý!';
			 RETURN; -- Dừng thực hiện
		END;*/

	--Set số câu NB
        INSERT INTO tblLuaChon(MaCauHoi, MaLuotThi)
        SELECT TOP(@SoCauNB) MaCauHoi, @MaLuotThi
		FROM tblCauHoi
        WHERE DoKho = 'NB' AND MaMonHoc=@MaMonHoc
		ORDER BY NEWID()
	--Set số câu TH
        INSERT INTO tblLuaChon(MaCauHoi, MaLuotThi)
        SELECT TOP(@SoCauTH) MaCauHoi, @MaLuotThi
		FROM tblCauHoi
        WHERE DoKho = 'TH' AND MaMonHoc=@MaMonHoc
		ORDER BY NEWID()
	--Set số câu VD
        INSERT INTO tblLuaChon(MaCauHoi, MaLuotThi)
        SELECT TOP(@SoCauVD) MaCauHoi, @MaLuotThi
		FROM tblCauHoi
        WHERE DoKho = 'VD' AND MaMonHoc=@MaMonHoc
		ORDER BY NEWID()
    END
	
	ELSE --nếu chọn số chương nhất định
	BEGIN
		/*Lưu kết quả đếm vào biến
		SELECT @TotalSoCauNB = COUNT(*) 
		FROM tblCauHoi 
		WHERE DoKho = 'NB' AND Chuong = @Chuong AND MaMonHoc=@MaMonHoc;

		SELECT @TotalSoCauTH = COUNT(*) 
		FROM tblCauHoi 
		WHERE DoKho = 'TH' AND Chuong = @Chuong AND MaMonHoc=@MaMonHoc;

		SELECT @TotalSoCauVD = COUNT(*) 
		FROM tblCauHoi 
		WHERE DoKho = 'VD' AND Chuong = @Chuong AND MaMonHoc=@MaMonHoc;

		Kiểm tra số câu được nhập có hợp lý không
		IF @SoCauNB > @TotalSoCauNB OR @SoCauNB<0
		BEGIN
			 PRINT 'Số câu hỏi nhận biết yêu cầu không hợp lý!';
			 RETURN; -- Dừng thực hiện
		END;
		IF @SoCauTH > @TotalSoCauTH OR @SoCauTH<0
		BEGIN
			PRINT 'Số câu hỏi thông hiểu yêu cầu không hợp lý!';
			 RETURN; -- Dừng thực hiện
		END;
		IF @SoCauVD > @TotalSoCauVD OR @SoCauVD<0
		BEGIN
			PRINT 'Số câu hỏi vận dụng yêu cầu không hợp lý!';
			 RETURN; -- Dừng thực hiện
		END; */

	--Set số câu NB
        INSERT INTO tblLuaChon(MaCauHoi, MaLuotThi)
        SELECT TOP(@SoCauNB) MaCauHoi, @MaLuotThi
		FROM tblCauHoi
        WHERE DoKho = 'NB'AND Chuong=@Chuong AND MaMonHoc=@MaMonHoc
		ORDER BY NEWID()
	--Set số câu TH
        INSERT INTO tblLuaChon(MaCauHoi, MaLuotThi)
        SELECT TOP(@SoCauTH) MaCauHoi, @MaLuotThi
		FROM tblCauHoi
        WHERE DoKho = 'TH'AND Chuong=@Chuong AND MaMonHoc=@MaMonHoc
		ORDER BY NEWID()
	--Set số câu VD
        INSERT INTO tblLuaChon(MaCauHoi, MaLuotThi)
        SELECT TOP(@SoCauVD) MaCauHoi, @MaLuotThi
		FROM tblCauHoi
        WHERE DoKho = 'VD' AND Chuong=@Chuong AND MaMonHoc=@MaMonHoc
		ORDER BY NEWID()
	END
END;

