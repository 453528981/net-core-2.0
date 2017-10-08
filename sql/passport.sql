/*
drop database if exists Passport;

create database Passport;
*/

use Passport;

drop table if exists User;
create table User(
Id int auto_increment not null comment 'Id',
Guid char(32) not null comment 'Guid',
Name nvarchar(50) not null comment '登录帐号 用户名/绑定的邮箱/绑定的手机号',
Email varchar(60) not null default '' comment '绑定的邮箱',
Mobile varchar(12) not null default '' comment '绑定的手机号',
Nickname nvarchar(50) not null default '' comment '用户昵称',
Password varchar(50) not null default '' comment '用户登录密码',
Role tinyint default 0 not null default 1 comment '用户角色 买家 卖家 平台管理员',
Grade  tinyint default 0  not null default 1 comment '用户级别',
/*RoleId int not null default 0 comment '角色Id 用户权限通过该值获取',*/
ParentId int not null default 0 comment '父Id',
Limitation int default 0  not null default 0 comment '用户限制',
Permission int default 0  not null default 0 comment '商家许可',
Avatar nvarchar(100) not null default '' comment 'Avatar',
Status tinyint  not null default 0 comment '0正常 1未通过手机 邮箱验证 2被系统隔离 无法下单 3被系统禁用 帐号异常或违规  255被系统删除 无法进行任何操作',
AuthedOn datetime comment '通过真实身份验证时间',
CreatedBy nvarchar(50) not null default '' comment '通过qq sina 等注册',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id),
constraint UNIQUE (Name)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户';

create index UserNameIndex on User(Name); 
create index UserEmailIndex on User(Email); 
create index UserMobileIndex on User(Mobile); 

drop table if exists UserProfile;
create table UserProfile(
Id int not null comment 'UserId',
Code varchar(18) not null default '' comment '身份证号',
Name nvarchar(50) not null default '' comment '真实姓名',
Gender tinyint not null default 0 comment '0为保密 1为男 2为女',
Marital tinyint not null default 0 comment '0为保密 1为单身 2为已婚',
Birthday date comment '出生日期',
Phone varchar(16) not null default '' comment '固定电话',
Mobile varchar(12) not null default '' comment '移动电话',
RegionId varchar(10) not null default '' comment '所属省市区Id',
Street nvarchar(200) not null default '' comment '街道门牌号',
SignUpIp varchar(16) not null default '' comment '注册Ip',
SignUpBy tinyint not null default 0 comment '0为通过用户名注册，1为通过邮箱注册，2为通过手机号码注册，3为通过手机短信注册',
TraceCode varchar(36) not null default '' comment '注册跟踪码',
LastSignInIp varchar(16) not null default '' comment '最后一次登录Ip',
LastSignInOn datetime not null default current_timestamp comment '最后一次登录时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户Profile';


/*预注册用户信息 通过邮箱注册的用户 首先会写入该表 通过邮箱里的链接认证通过后才会写入[User]表*/
drop table if exists UserPre;
create table UserPre(
Id char(32) not null default '' comment 'Id 验证成功后对应User表中的Guid',
Name nvarchar(50) not null default '' comment '待验证的Email',
Password varchar(255) not null default '' comment '密码',
Browser nvarchar(50) not null default '' comment 'Browser',
UserAgent nvarchar(50) not null default '' comment 'UserAgent',
IpAddress varchar(20) not null default '' comment 'IpAddress',
UrlReferrer nvarchar(300) not null default '' comment 'UrlReferrer',
MediaId int not null default 0 comment '媒体Id',
TraceCode nvarchar(50) not null default '' comment '媒体跟踪码',
CreatedOn datetime not null default current_timestamp comment '创建时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户预注册信息';


/*通过第三方平台登录的用户OAuth信息*/
drop table if exists UserOAuth;
create table UserOAuth(
Id int not null default 0 comment 'UserId',
OpenId varchar(36) not null default '' comment 'OpenId',
Provider nvarchar(20) not null default '' comment 'Provider (qq sina 等)',
OpenName nvarchar(50) not null default '' comment 'OpenName',
Scope varchar(500) not null default '' comment 'Scope',
AccessToken varchar(500) not null default '' comment 'AccessToken',
RefreshToken varchar(500) not null default '' comment 'RefreshToken',
ExpiredOn datetime not null comment 'AccessToken有效期',
Extra nvarchar(500) not null default '' comment '扩展信息',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id,OpenId,Provider)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户OAuth';


drop table if exists UserFavorite;
create table UserFavorite(
Id int auto_increment not null comment 'Id',
UserId int not null default 0 comment 'UserId',
GroupId tinyint not null default 0 comment '分组 0商品 2品牌 3店铺',
Name nvarchar(50) not null default '' comment '商品 品牌 店铺 名称',
Value nvarchar(200) not null default '' comment '商品Id 品牌Id 店铺Id',
Extra nvarchar(200) not null default '' comment '扩展信息 商品编号等',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id),
constraint UNIQUE (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户收藏';


drop table if exists UserAddress;
create table UserAddress(
Id int auto_increment not null comment 'Id',
UserId int not null default 0 comment 'UserId',
GroupId tinyint not null default 0 comment '分组 0为收货地址 2发货地址 3退货地址',
Consignee nvarchar(50) not null default '' comment '收货人',
CompanyName nvarchar(100) not null default '' comment '公司名称',
CountryId int not null default 0 comment '国家代码',
RegionId varchar(6) not null default '' comment '行政区编码',
Province nvarchar(20) not null default '' comment '省',
City nvarchar(20) not null default '' comment '市',
District nvarchar(50) not null default '' comment '区',
Street nvarchar(200) not null default '' comment '街道门牌号',
PostalCode varchar(20) not null default '' comment '邮政编码',
Phone varchar(50) not null default '' comment '固定电话号码',
Mobile varchar(50) not null default '' comment '移动电话号码',
IsDefault bool not null default 0 comment '是否为默认地址',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id),
constraint UNIQUE (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户地址';


drop table if exists UserInvoice;
create table UserInvoice(
Id int auto_increment not null comment 'Id',
Type tinyint not null default 0 comment '0为普通 1为增值',
Title nvarchar(50) not null default '' comment '发票抬头 个人或单位名称',
Content varchar(50) not null default '' comment '发票内容',
IsDefault bool not null default 0 comment '是否为默认',
UserId int not null default 0 comment 'UserId',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id),
constraint UNIQUE (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户发票';