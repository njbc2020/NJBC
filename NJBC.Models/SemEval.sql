CREATE TABLE OrgQuestion(
    ORGQ_ID int IDENTITY(1,1) PRIMARY KEY,
    ORGQ_ID_Name nvarchar(50),
    OrgQSubject nvarchar(max),
    OrgQBody nvarchar(max),
);

CREATE TABLE RelQuestion(
    ORGQ_ID int,
    RELQ_ID int IDENTITY(1,1) PRIMARY KEY,
    RELQ_ID_Name nvarchar(50),
	RELQ_RANKING_ORDER nvarchar(50),
	RELQ_CATEGORY nvarchar(250),
	RELQ_DATE datetime,
    RELQ_USERID nvarchar(150),
	RELQ_USERNAME nvarchar(150),
	RELQ_RELEVANCE2ORGQ nvarchar(150),
	RelQSubject nvarchar(max),
	RelQBody nvarchar(max),

	FOREIGN KEY(ORGQ_ID) REFERENCES OrgQuestion(ORGQ_ID)
);

CREATE TABLE RelComment(
    RELQ_ID int,
    RELC_ID int IDENTITY(1,1) PRIMARY KEY,
    RELC_ID_Name nvarchar(50),
    RELC_DATE datetime,
    RELC_USERID nvarchar(50),
	RELC_USERNAME nvarchar(200),
	RELC_RELEVANCE2ORGQ nvarchar(50),
	RELC_RELEVANCE2RELQ nvarchar(50),
    RelCText nvarchar(max),

	FOREIGN KEY(RELQ_ID) REFERENCES RelQuestion(RELQ_ID)
);