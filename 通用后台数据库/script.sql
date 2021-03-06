--创建数据库
CREATE DATABASE imow_admin  
ON
(
    NAME = Epiphany,
    FILENAME = 'd:\imow_admin_data.mdf',
    SIZE = 5MB,
    MAXSIZE = 20,
    FILEGROWTH = 20
)
LOG ON 
(
    NAME = Epiphany,
    FILENAME = 'd:\imow_admin_log.ldf',
    SIZE = 2MB,
    MAXSIZE = 10MB,
    FILEGROWTH = 1MB
);



GO
/****** Object:  Table [dbo].[ecAdminModule]    Script Date: 2017/10/11 16:12:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ecAdminModule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](40) NOT NULL,
	[Pid] [int] NOT NULL,
	[Url] [varchar](200) NULL,
	[Depth] [int] NOT NULL,
	[IsDel] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsShow] [bit] NOT NULL,
	[Code] [varchar](max) NULL,
	[IsLast] [bit] NOT NULL,
 CONSTRAINT [PK_ECPOPMODULE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ecAdminRole]    Script Date: 2017/10/11 16:12:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ecAdminRole](
	[Id] [bigint] NOT NULL,
	[Name] [varchar](40) NOT NULL,
	[IsDel] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[ModuleIds] [varchar](2000) NULL,
	[Remark] [varchar](200) NULL,
 CONSTRAINT [PK_ecAdminRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ecAdminRoleModule]    Script Date: 2017/10/11 16:12:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecAdminRoleModule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [bigint] NOT NULL,
	[ModuleId] [int] NOT NULL,
 CONSTRAINT [PK_ecAdminRoleModule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ecAdminUser]    Script Date: 2017/10/11 16:12:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ecAdminUser](
	[Id] [bigint] NOT NULL,
	[RoleId] [varchar](max) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Pwd] [nvarchar](64) NOT NULL,
	[LastLoginTime] [datetime] NULL,
	[RealName] [nvarchar](32) NULL,
	[Sex] [char](1) NULL,
	[Mobile] [varchar](11) NULL,
	[Email] [varchar](64) NULL,
	[Remark] [varchar](256) NULL,
	[IsStop] [bit] NOT NULL,
	[IsDel] [bit] NOT NULL,
	[UpdateTime] [datetime] NULL,
	[CreateTime] [datetime] NOT NULL,
	[QQ] [varchar](20) NULL,
 CONSTRAINT [PK__ecAdminU__3214EC278DD60119] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ecAdminUserRole]    Script Date: 2017/10/11 16:12:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecAdminUserRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AdminId] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
 CONSTRAINT [PK_ecAdminUserRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]




--触发器
GO
/****** Object:  Trigger [dbo].[trig_insert_code]    Script Date: 2017/10/11 16:12:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create TRIGGER [dbo].[trig_insert_code] ON [dbo].[ecAdminModule]
    AFTER INSERT
AS
    BEGIN
        DECLARE @mid INT;--刚插入的数据的id
		DECLARE @pid INT;--刚插入的数据的pid
		DECLARE @depth INT; --深度
        DECLARE @code VARCHAR(max)= ''; --最终排序数据
        SELECT  @mid = Id,@pid=Pid
        FROM    Inserted; --查询插入的数据ID
		--查询所有的父级
		IF @pid=0
		BEGIN
			SET @code=@mid;
			SET @depth=0;
        END
        ELSE
        BEGIN
			SELECT @code=code,@depth=Depth+1 FROM dbo.ecAdminModule WHERE id=@pid;
			SET @code=@code+CAST(@mid AS VARCHAR(MAX));
			PRINT @code
        END
        UPDATE ecAdminModule SET isLast=0 WHERE id=@pid;
			--更新sort
        UPDATE  ecAdminModule  SET  code = @code,depth=@depth,isLast=1  WHERE   Id = @mid;
      

    END;







GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminModule', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminModule', @level2type=N'COLUMN',@level2name=N'Pid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'访问地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminModule', @level2type=N'COLUMN',@level2name=N'Url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'级别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminModule', @level2type=N'COLUMN',@level2name=N'Depth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminModule', @level2type=N'COLUMN',@level2name=N'IsDel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminModule', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否显示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminModule', @level2type=N'COLUMN',@level2name=N'IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminRole', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminRole', @level2type=N'COLUMN',@level2name=N'IsDel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminRole', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'冗余对应的模块' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminRole', @level2type=N'COLUMN',@level2name=N'ModuleIds'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminRoleModule', @level2type=N'COLUMN',@level2name=N'RoleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模块ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminRoleModule', @level2type=N'COLUMN',@level2name=N'ModuleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUser', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUser', @level2type=N'COLUMN',@level2name=N'RoleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录账号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUser', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUser', @level2type=N'COLUMN',@level2name=N'Pwd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后登录时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUser', @level2type=N'COLUMN',@level2name=N'LastLoginTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'真实姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUser', @level2type=N'COLUMN',@level2name=N'RealName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUser', @level2type=N'COLUMN',@level2name=N'Sex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUser', @level2type=N'COLUMN',@level2name=N'Mobile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUser', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUser', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'停用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUser', @level2type=N'COLUMN',@level2name=N'IsStop'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUser', @level2type=N'COLUMN',@level2name=N'IsDel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUser', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUser', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUserRole', @level2type=N'COLUMN',@level2name=N'AdminId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ecAdminUserRole', @level2type=N'COLUMN',@level2name=N'RoleId'
GO
