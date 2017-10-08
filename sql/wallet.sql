/*
drop database if exists Wallet;

create database Wallet;
*/
use Wallet;

drop table if exists Account;
create table Account(
Id int not null default 0 comment 'UserId',
Cash decimal(8,2) not null default 0 comment '余额',
FrozenCash decimal(8,2) not null default 0 comment '被冻结的余额 下单时使用了余额支付 但余额不足以支付整个订单 还需网银/现金支付 等情况下',
Point int not null default 0 comment '积分',
FrozenPoint int not null default 0 comment '被冻结的积分 下单使用了积分 但订单还未支付 等情况下',
Coin int not null default 0 comment '金币',
Password varchar(50) not null default '' comment '支付密码',
Status bool not null default 1 comment '帐号状态 1可用 0不可用',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='帐户信息';

drop table if exists CashFlow;
create table CashFlow(
Id int auto_increment not null comment 'Id',
Type int not null default 0 comment '收入/支出 分组 当Amount大于0时 1充值获得 2支付退还获得(订单退款/通知延迟导致多次支付成功等) 3别的用户转账获得 有限制 当Amount小于0时 1支付订单 2代别人支付订单 3转账给别的用户 4提现 5充值金币 6给别的用户充值金币 7充值游戏 8给别的用户充值游戏',
UserId int not null default 0 comment 'UserId',
Amount decimal(8,2) not null default 0 comment '发生金额 大于0为收入 小于0为支出',
Remain decimal(8,2) not null default 0 comment '发生后余额',
Subject nvarchar(50) not null default '' comment '主题',
Message nvarchar(500) not null default '' comment '消息',
RawData nvarchar(4000) not null default '' comment '相关数据 json格式',
RelatedId nvarchar(50) not null default '' comment '关联的Id 订单号 等',
OtherUserId int not null default 0 comment '别人转账 或 为别人代付 的UserId',
OtherUserName varchar(50) not null default '' comment '别人转账 或 为别人代付 的UserName',
Remark nvarchar(200) not null default '' comment '备注',
CreatedBy nvarchar(50) not null default '' comment '创建者 UserName',
CreatedOn datetime not null default current_timestamp comment '创建时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='帐户余额';


drop table if exists PointFlow;
create table PointFlow(
Id int auto_increment not null comment 'Id',
Type int not null default 0 comment '收入/支出 分组',
UserId int not null default 0 comment 'UserId',
Amount int not null default 0 comment '发生积分数 大于0为收入 小于0为支出',
Usable int not null default 0 comment '可用积分(获得的积分在有效期内可使用值)',
Remain int not null default 0 comment '发生后积分余额',
Subject nvarchar(50) not null default '' comment '主题',
Message nvarchar(500) not null default '' comment '消息',
RawData nvarchar(4000) not null default '' comment '相关数据 json格式',
RelatedId nvarchar(50) not null default '' comment '关联的Id 订单号 等',
ExpiredOn datetime not null default current_timestamp comment '获得的积分有效期',
Remark nvarchar(200) not null default '' comment '备注',
CreatedBy nvarchar(50) not null default '' comment '创建者 UserName',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp  comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='帐户积分';


/*网银/手机充值卡支付*/
drop table if exists Payment;
create table Payment(
Id varchar(32) not null comment '支付流水号',
No varchar(50) not null default '' comment '第三方支付流水号 支付宝 财富通等 支付成功后更新该字段',
Type int not null default 0 comment '支付类型 0为订单支付 1为帐户余额充值 2为金币充值',
UserId int not null default 0 comment 'UserId',
Method tinyint not null default 0 comment '支付方式 0 None 1使用网银 2手机充值卡等',
Amount decimal(8,2) not null default 0 comment '支付金额',
Subject nvarchar(100) not null default '' comment '主题',
Message nvarchar(500) not null default '' comment '消息',
RawData nvarchar(4000) not null default '' comment '相关数据 订单 充值等数据 json格式',

/*网银支付*/
BankId int not null default 0 comment '支付平台银行Id',
BankCode varchar(50) not null comment '支付平台银行编码 不同的支付平台 同一银行银行编码不同',
BankName nvarchar(50) not null comment '支付平台银行名称',
BankCard tinyint not null default 0 comment '0为None 1为储蓄卡 2为信用卡',
PlatformId int not null default 0 comment '支付平台Id',

/*手机充值卡*/
CardNo varchar(50) not null default '' comment '全国神州行充值卡 卡号17位 联通一卡充 卡号15位',
CardPwd varchar(50) not null default '' comment '充值卡卡密',
CardAmount decimal(8,2) not null default 0 comment '充值卡面值',

RelatedId nvarchar(100) not null default '' comment '关联的Id 订单号 充值单号 等',
IpAddress varchar(50) not null default '' comment 'Ip',
Extra nvarchar(2000) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 1成功 0失败',
CreatedBy nvarchar(50) not null default '' comment '创建者 UserName',
CreatedOn datetime not null default current_timestamp  comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp  comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='支付信息';


drop table if exists PaymentNote;
create table PaymentNote(
Id int auto_increment not null comment 'Id',
PayId varchar(32) not null comment '支付流水号',
PayNo varchar(50) not null default '' comment '第三方支付流水号 支付宝 财富通等',
UserId int not null default 0 comment 'UserId',
Subject nvarchar(50) not null default '' comment '主题',
Message nvarchar(500) not null default '' comment '消息',
RawData nvarchar(500) not null default '' comment '支付平台 支付信息',
Extra nvarchar(500) not null default '' comment '扩展',
CreatedBy nvarchar(50) not null default '' comment '创建者',
CreatedOn datetime default current_timestamp  not null comment '创建时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='支付记录';
