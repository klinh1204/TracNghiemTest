alter table tblLuotThi
alter column DiemSo decimal(5,2)

EXEC HoanThanh 'sinhvien12412',''
drop procedure HoanThanh
CREATE PROCEDURE HoanThanh
@MaLuotThi varchar(50),
@TGKT datetime
AS
BEGIN
SET NOCOUNT ON; -- Tắt thông báo dòng bị ảnh hưởng
DECLARE
	@DiemSo decimal(5,2),
	@SoCauDung int,	
	@TongSoCau int;
--Tính số câu đúng
SELECT @SoCauDung = COUNT(*)
FROM tblLuaChon lc INNER JOIN tblDapAn da ON lc.MaDapAn = da.MaDapAn
WHERE MaLuotThi = @MaLuotThi AND DungSai=1;
--Tính tổng số câu
SELECT @TongSoCau = SUM (SoCauNB + SoCauTH + SoCauVD)
FROM tblLuotThi lt JOIN tblDeThi dt ON lt.MaDeThi = dt.MaDeThi
WHERE lt.MaLuotThi = @MaLuotThi;
--Tính điểm
SET @DiemSo = ROUND(@SoCauDung * 10.0 / @TongSoCau, 2);
--INSERT DL vừa tính vào bảng lượt thi
UPDATE tblLuotThi 
SET TGKT=@TGKT, DiemSo=@DiemSo, SoCauDung = @SoCauDung
WHERE MaLuotThi = @MaLuotThi
END;
