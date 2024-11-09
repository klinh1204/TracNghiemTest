CREATE PROCEDURE TaoND
	@MaLuotThi varchar(50),
	@MaMonHoc varchar(50),
    @SoCauNB INT,
    @SoCauTH INT,
    @SoCauVD INT,
    @Chuong INT
AS
BEGIN
DECLARE @TotalSoCauNB INT,
		@TotalSoCauTH INT,
		@TotalSoCauVD INT;

    IF (@Chuong = 0) -- Nếu chọn tất cả các chương
    BEGIN
		-- Lưu kết quả đếm vào biến
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
		END;
	--Set số câu NB
        INSERT INTO tblLuaChon(MaCauHoi, MaLuotThi)
        SELECT TOP(@SoCauNB) MaCauHoi, @MaLuotThi
		FROM tblCauHoi
        WHERE DoKho = 'NB' AND MaMonHoc=@MaMonHoc
        ORDER BY NEWID();
	--Set số câu TH
        INSERT INTO tblLuaChon(MaCauHoi, MaLuotThi)
        SELECT TOP(@SoCauTH) MaCauHoi, @MaLuotThi
		FROM tblCauHoi
        WHERE DoKho = 'TH' AND MaMonHoc=@MaMonHoc
        ORDER BY NEWID();
	--Set số câu VD
        INSERT INTO tblLuaChon(MaCauHoi, MaLuotThi)
        SELECT TOP(@SoCauVD) MaCauHoi, @MaLuotThi
		FROM tblCauHoi
        WHERE DoKho = 'VD' AND MaMonHoc=@MaMonHoc
        ORDER BY NEWID();
    END
	
	ELSE --nếu chọn số chương nhất định
	BEGIN
		-- Lưu kết quả đếm vào biến
		SELECT @TotalSoCauNB = COUNT(*) 
		FROM tblCauHoi 
		WHERE DoKho = 'NB' AND Chuong = @Chuong AND MaMonHoc=@MaMonHoc;

		SELECT @TotalSoCauTH = COUNT(*) 
		FROM tblCauHoi 
		WHERE DoKho = 'TH' AND Chuong = @Chuong AND MaMonHoc=@MaMonHoc;

		SELECT @TotalSoCauVD = COUNT(*) 
		FROM tblCauHoi 
		WHERE DoKho = 'VD' AND Chuong = @Chuong AND MaMonHoc=@MaMonHoc;

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
		END;
	--Set số câu NB
        SET ROWCOUNT @SoCauNB;
        INSERT INTO tblLuaChon(MaCauHoi, MaLuotThi)
        SELECT TOP(@SoCauNB) MaCauHoi, @MaLuotThi
		FROM tblCauHoi
        WHERE DoKho = 'NB'AND Chuong=@Chuong AND MaMonHoc=@MaMonHoc
        ORDER BY NEWID();
	--Set số câu TH
		SET ROWCOUNT @SoCauTH;
        INSERT INTO tblLuaChon(MaCauHoi, MaLuotThi)
        SELECT TOP(@SoCauTH) MaCauHoi, @MaLuotThi
		FROM tblCauHoi
        WHERE DoKho = 'TH'AND Chuong=@Chuong AND MaMonHoc=@MaMonHoc
        ORDER BY NEWID();
	--Set số câu VD
		SET ROWCOUNT @SoCauVD;
        INSERT INTO tblLuaChon(MaCauHoi, MaLuotThi)
        SELECT TOP(@SoCauVD) MaCauHoi, @MaLuotThi
		FROM tblCauHoi
        WHERE DoKho = 'VD' AND Chuong=@Chuong AND MaMonHoc=@MaMonHoc
        ORDER BY NEWID();
	END
END;

EXEC TaoND 1236,CSDL,3,3,3,0

Alter table tblDeThi
add Chuong int;