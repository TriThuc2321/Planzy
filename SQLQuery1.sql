create table CHUYEN_BAY
(
	MA_CHUYEN_BAY VARCHAR(100) PRIMARY KEY NOT NULL,
	GIA_VE_CO_BAN INT,
	SAN_BAY_DI VARCHAR(100),
	SAN_BAY_DEN VARCHAR(100),
	NGAY_GIO_BAY SMALLDATETIME,
	THOI_GIAN_BAY SMALLINT,
	DA_BAY BIT,
)

creatE TABLE LOAI_HANG_GHE
(
	MA_LOAI_HANG_GHE VARCHAR(100) NOT NULL,
	MA_CHUYEN_BAY VARCHAR (100) NOT NULL,
	SO_LUONG_TONG INT,
	SO_LUONG_CON_LAI INT,
	TEN_LOAI_HANG_GHE VARCHAR(100),
	TY_LE SMALLINT
)
ALTER TABLE LOAI_HANG_GHE
ADD CONSTRAINT PK_LOAI_HANG_GHE
PRIMARY KEY (MA_LOAI_HANG_GHE,MA_CHUYEN_BAY)
CREATE TABLE SAN_BAY
(
	MA_SAN_BAY VARCHAR(100) PRIMARY KEY,
	TEN_SAN_BAY VARCHAR(100)
)
CREATE TABLE LICH_SU_CHUYEN_BAY
(
	MA_CHUYEN_BAY VARCHAR(100) NOT NULL,
	SO_VE INT,
	DOANH_THU INT,
	NGAY_BAY SMALLDATETIME NOT NULL
)
ALTER TABLE LICH_SU_CHUYEN_BAY
ADD CONSTRAINT PK_LICH_SU_CHUYEN_BAY
PRIMARY KEY (MA_CHUYEN_BAY,NGAY_BAY)
CREATE TABLE VE_CHUYEN_BAY
(
	MA_VE VARCHAR(100) PRIMARY KEY,
	MA_CHUYEN_BAY VARCHAR(100),
	MA_LOAI_HANG_GHE VARCHAR(100),
	TEN_HANH_KHACH VARCHAR(100),
	CMND VARCHAR(100),
	DIEN_THOAI VARCHAR(100),
	GIA_TIEN INT
)
CREATE TABLE PHIEU_DAT_CHO
(
	MA_PHIEU VARCHAR(100) PRIMARY KEY,
	MA_CHUYEN_BAY VARCHAR(100),
	MA_LOAI_HANG_GHE VARCHAR(100),
	TEN_HANH_KHACH VARCHAR(100),
	CMND VARCHAR(100),
	DIEN_THOAI VARCHAR(100),
	GIA_TIEN INT,
	NGAY_DAT SMALLDATETIME
)
CREATE TABLE SAN_BAY_TRUNG_GIAN
(
	MA_CHUYEN_BAY VARCHAR(100) not null,
	MA_SAN_BAY VARCHAR(100) not null,
	MA_SAN_BAY_TRUOC VARCHAR(100),
	MA_SAN_BAY_SAU VARCHAR(100),
	THOI_GIAN_DUNG SMALLINT
)
ALTER TABLE SAN_BAY_TRUNG_GIAN
ADD CONSTRAINT PK_SAN_BAY_TRUNG_GIAN
PRIMARY KEY (MA_CHUYEN_BAY,MA_SAN_BAY)

--//////

ALTER TABLE SAN_BAY_TRUNG_GIAN
ADD CONSTRAINT FK_SAN_BAY_TRUNG_GIAN1
FOREIGN KEY (MA_SAN_BAY)
REFERENCES SAN_BAY (MA_SAN_BAY);

ALTER TABLE SAN_BAY_TRUNG_GIAN
ADD CONSTRAINT FK_SAN_BAY_TRUNG_GIAN3
FOREIGN KEY (MA_SAN_BAY_TRUOC)
REFERENCES SAN_BAY (MA_SAN_BAY);

ALTER TABLE SAN_BAY_TRUNG_GIAN
ADD CONSTRAINT FK_SAN_BAY_TRUNG_GIAN4
FOREIGN KEY (MA_SAN_BAY_SAU)
REFERENCES SAN_BAY (MA_SAN_BAY);

ALTER TABLE SAN_BAY_TRUNG_GIAN
ADD CONSTRAINT FK_SAN_BAY_TRUNG_GIAN2
FOREIGN KEY (MA_CHUYEN_BAY)
REFERENCES CHUYEN_BAY (MA_CHUYEN_BAY);
---
ALTER TABLE CHUYEN_BAY
ADD CONSTRAINT FK_CHUYEN_BAY1
FOREIGN KEY (SAN_BAY_DI)
REFERENCES SAN_BAY (MA_SAN_BAY);

ALTER TABLE CHUYEN_BAY
ADD CONSTRAINT FK_CHUYEN_BAY2
FOREIGN KEY (SAN_BAY_DEN)
REFERENCES SAN_BAY (MA_SAN_BAY);
---
ALTER TABLE LOAI_HANG_GHE
ADD CONSTRAINT FK_LOAI_HANG_GHE1
FOREIGN KEY (MA_CHUYEN_BAY)
REFERENCES CHUYEN_BAY (MA_CHUYEN_BAY);
---
ALTER TABLE PHIEU_DAT_CHO
ADD CONSTRAINT FK_PHIEU_DAT_CHO
FOREIGN KEY (MA_LOAI_HANG_GHE, MA_CHUYEN_BAY)
REFERENCES LOAI_HANG_GHE (MA_LOAI_HANG_GHE ,MA_CHUYEN_BAY);

---
ALTER TABLE VE_CHUYEN_BAY
ADD CONSTRAINT FK_VE_CHUYEN_BAY
FOREIGN KEY (MA_LOAI_HANG_GHE, MA_CHUYEN_BAY)
REFERENCES LOAI_HANG_GHE (MA_LOAI_HANG_GHE ,MA_CHUYEN_BAY);

create table THAM_SO_QUY_DINH
(
	SO_SAN_BAY_TOI_DA INT,
	THOI_GIAN_BAY_TOI_THIEU INT,
	SO_SAN_BAY_TRUNG_GIAN_TOI_DA INT,
	THOI_GIAN_DUNG_TOI_DA INT,
	THOI_GIAN_DUNG_TOI_THIEU INT,
	SO_LUONG_CAC_HANG_VE INT,
	THOI_GIAN_CHAM_NHAT_HUY_VE INT,
	THOI_GIAN_CHAM_NHAT_DAT_VE INT,
)
create table LOAI_HANG_GHE
(
	MA_LOAI_HANG_GHE VARCHAR(100) PRIMARY KEY NOT NULL,
	TEN_LOAI_HANG_GHE NVARCHAR(100),
	TY_LE INT
)

ALTER TABLE CHI_TIET_HANG_GHE
ADD CONSTRAINT FK_CHI_TIET_HANG_GHE
FOREIGN KEY (MA_LOAI_HANG_GHE)
REFERENCES LOAI_HANG_GHE (MA_LOAI_HANG_GHE);

CREATE TABLE HANH_KHACH(
	MA_TAI_KHOAN VARCHAR(100) PRIMARY KEY,
	GMAIL VARCHAR(100),
	MAT_KHAU VARCHAR(100),
	TEN_HANH_KHACH NVARCHAR(225),
	CMND VARCHAR(15),
	SDT VARCHAR(15),
)

CREATE TABLE CHI_TIET_PHIEU_DAT_CHO(
	MA_PHIEU VARCHAR(100),
	MA_TAI_KHOAN VARCHAR(100)
	CONSTRAINT PK_CHITIET PRIMARY KEY (MA_PHIEU,MA_TAI_KHOAN)
)
ALTER TABLE CHI_TIET_PHIEU_DAT_CHO
ADD FOREIGN KEY (MA_TAI_KHOAN) REFERENCES HANH_KHACH(MA_TAI_KHOAN);

ALTER TABLE CHI_TIET_PHIEU_DAT_CHO
ADD FOREIGN KEY (MA_PHIEU) REFERENCES PHIEU_DAT_CHO(MA_PHIEU);