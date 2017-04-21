

--==========cập nhật: 15/04/2017 -- trục liên thông của tỉnh ================================

insert into Config values('TrucLienthongTinh', '', N'http://123.30.75.134:8079/gw-portlet/api/secure/axis/Plugin_NSSGateway_NSSGatewayService?wsdl', 1,4,1)
insert into Config values('UsernameTrucTinh', 'qlvb.vpubnd', N'Tài khoản Trục liên thông', 2,4,1)
insert into Config values('PasswordTrucTinh', '', N'12345qwe
+		', 3,4,1)
insert into Config values('MaDonviTrucTinh', '1:30:0:0:0:0:1', N'Mã đơn vị trên Trục liên thông', 4,4,1)



--==========cập nhật: 07/03/2017-- lưu nhật ký gửi số liệu văn bản ==========================

CREATE TABLE [dbo].[TonghopVanban](
	[intid] [int] IDENTITY(1,1) NOT NULL,
	[Ngaygui] [smalldatetime] NULL,
	[Ngaybatdau] [smalldatetime] NULL,
	[Ngayketthuc] [smalldatetime] NULL,
	[VBGiayDen] [int] NULL,
	[VBGiayDi] [int] NULL,
	[VBDientuDen] [int] NULL,
	[VBDientuDi] [int] NULL,
 CONSTRAINT [PK_TonghopVanban] PRIMARY KEY CLUSTERED 
(
	[intid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


--==========cập nhật: 01/03/2017 - module báo cáo số liệu gửi nhận văn bản về UBT ===============

insert into Config values('IsSendTonghopVB', 'true', N'Báo cáo số liệu gửi nhận văn bản', 1,3,1)
insert into Config values('IPAddressUBT', '123.28.82.234/thvb', N'Địa chỉ IP Web Services', 2,3,1)
insert into Config values('TimeSend', '8', N'Thời gian bắt đầu gửi báo cáo (0-24 giờ)', 3,3,1)
insert into Config values('TimeAutoSendVB', '15', N'Thời gian tự động gửi (phút)', 4,3,1)

CREATE TABLE [dbo].[LogGuiTonghopVB](
	[intid] [int] IDENTITY(1,1) NOT NULL,
	[Ngaygui] [smalldatetime] NOT NULL,
	[intTrangthai] [int] NOT NULL,
 CONSTRAINT [PK_LogGuiTonghopVB] PRIMARY KEY CLUSTERED 
(
	[intid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


-- ========== cập nhật: 17/9/2016 -- bổ sung theo dõi hồi báo văn bản đi =======================
insert into CategoryVanban values(5,N'Theo dõi hồi báo',2,1,5,1)

-- ========  cap nhat: 30/10/2015 -- bo sung module luu tru  ============================

insert into quyen values(39,N'Cập nhật Lưu trữ Văn bản đi', 'LuutruTracuuVanbandi',3,1)
insert into quyen values(38,N'Cập nhật Lưu trữ Văn bản đến', 'LuutruTracuuVanbanden',3,1)

insert into quyen values(27,N'Cập nhật Lưu trữ Văn bản đến', 'LuutruVanbanden',12,1)
insert into quyen values(28,N'Cập nhật Lưu trữ Văn bản đi', 'LuutruVanbandi',11,1)
-----------  table Luutruvanban co trong 2 database qlvb va qlvbStore -----------------
CREATE TABLE [dbo].[LuutruVanban](
	[intid] [int] IDENTITY(1,1) NOT NULL,
	[intidvanban] [int] NOT NULL,
	[intloaivanban] [int] NULL,
	[inthopso] [int] NULL,
	[intdonvibaoquan] [int] NULL,
	[strthoihanbaoquan] [nvarchar](100) NULL,
	[strnoidung] [nvarchar](1000) NULL,
	[intidnguoicapnhat] [int] NULL,
	[strngaycapnhat] [smalldatetime] NULL,
 CONSTRAINT [PK_LuutruVanban] PRIMARY KEY CLUSTERED 
(
	[intid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


-- cap nhat: 20/09/2015 -- update tinh nang khong cho phep nguoi dung dong ho so

alter table hosocongviec add intdonghoso int null

-- =========cap nhat : 12/09/2015 =========================
insert into menu values(38,5,1,N'Tra cứu văn bản đến','Index','TracuuVanbanden',null,0,5,'TracuuVanbanden',1)
insert into menu values(39,5,1,N'Tra cứu văn bản đi','Index','TracuuVanbandi',null,0,6,'TracuuVanbandi',1)


insert into quyen values(38,N'Truy cập Tra cứu văn bản đến', 'TracuuVanbanden',1,1)
insert into quyen values(38,N'Hiển thị tất cả văn bản đến', 'AllTracuuVanbanden',2,1)
insert into quyen values(39,N'Truy cập Tra cứu văn bản đi', 'TracuuVanbandi',1,1)
insert into quyen values(39,N'Hiển thị tất cả văn bản đi', 'AllTracuuVanbandi',2,1)


-- =========cap nhat : 30/08/2015 =========================

alter table vanbandenmail add strmadinhdanh nvarchar(50) null
alter table vanbandenmail add strhantraloi smalldatetime null


--============== cap nhat 20/08/2015 ==============
CREATE TABLE [dbo].[Tuychon](
	[intid] [int] IDENTITY(1,1) NOT NULL,
	[strthamso] [nvarchar](255) NULL,
	[strgiatri] [nvarchar](255) NULL,
	[strmota] [nvarchar](500) NULL,
	[intorder] [int] NULL,
	[intgroup] [int] NULL,
	[inttrangthai] [int] NULL,
 CONSTRAINT [PK_Tuychon] PRIMARY KEY CLUSTERED 
(
	[intid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

SET IDENTITY_INSERT [dbo].[Tuychon] ON;

BEGIN TRANSACTION;
INSERT INTO [dbo].[Tuychon]([intid], [strthamso], [strgiatri], [strmota], [intorder], [intgroup], [inttrangthai])
SELECT 1, N'MenuType', N'sidebar-left-mini', N'Loại Menu', 1, 1, 1 UNION ALL
SELECT 2, N'IsMenuClickable', N'false', N'Click Menu', 2, 1, 1 UNION ALL
SELECT 3, N'HomePage', N'27', N'Trang chủ', 3, 1, 1
COMMIT;
RAISERROR (N'[dbo].[Tuychon]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT [dbo].[Tuychon] OFF;

--========================================================

CREATE TABLE [dbo].[TuychonCanbo](
	[intid] [int] IDENTITY(1,1) NOT NULL,
	[intidcanbo] [int] NOT NULL,
	[intidoption] [int] NOT NULL,
	[strgiatri] [nvarchar](255) NULL,
 CONSTRAINT [PK_TuychonCanbo] PRIMARY KEY CLUSTERED 
(
	[intid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

--=============cap nhat : 15/06/2015=====================================

insert into menu values(36,2,1,N'Đơn vị thực hiện YKCĐ', N'Index', N'YKCDDonvi', NULL, 0, 13, N'YKCDDonvi', 1)
insert into menu values(37, 5, 1, N'Ý kiến chỉ đạo', N'Index', N'YKCD', NULL, 0, 5, N'YKCDXuly', 1)

insert into quyen values(36, N'Truy cập Ý kiến chỉ đạo', N'YKCDDonvi', 1, 1)
insert into quyen values(36, N'Cập nhật các đơn vị thực hiện YKCĐ', N'UpdateYKCDDonvi', 2, 1)
insert into quyen values(27, N'Theo dõi ý kiến chỉ đạo', N'YKCDVanbanden', 11, 1)
insert into quyen values(28, N'Theo dõi ý kiến chỉ đạo', N'YKCDVanbandi', 11, 1)
insert into quyen values(37, N'Truy cập Ý kiến chỉ đạo', N'YKCDXuly', 1, 1)

