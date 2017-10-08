/*
drop database if exists Store;

create database Store;
*/

use store;

drop table if exists Item;
create table Item(
Id int auto_increment not null comment 'Id',
SpuId int not null default 0 comment '产品Id',
CatgId int not null default 0 comment '最小类目Id',
CatgRId int not null default 0 comment '根类目Id',
CatgFId varchar(50) not null default '' comment '全类目Id',
CatgFName varchar(50) not null default '' comment '全类目名称',
BrandId int not null default 0 comment '品牌Id',
BrandName nvarchar(100) not null default '' comment '品牌名',
BrandInitial char(1) not null default '' comment '品牌首字母',
Code varchar(100) not null default '' comment '商家设置的外部Id',
Name nvarchar(160) not null default '' comment '商品名称,不能超过60字节',
Title nvarchar(160) not null default '' comment '标题 活动 促销信息',
Stock int not null default 0 comment '商品库存数量',
Price decimal(10,2) not null default 0 comment '商品价格',
AppPrice decimal(10,2) not null default 0 comment 'app商品价格',
RetailPrice decimal(10,2) not null default 0  comment '商品建议零售价格',
Barcode varchar(50) not null default '' comment '条形码',
Keyword nvarchar(2000) not null default '' comment '关键字',
Summary nvarchar(1000) not null default '' comment '商品概要',
Picture nvarchar(500) not null default '' comment '商品主图片地址',
ItemImgStr nvarchar(1000) not null default '' comment '商品图片列表(包括主图)',
PropImgStr nvarchar(1000) not null default '' comment '商品属性图片列表',
Width decimal(10,2) not null default 0 comment '宽度',
Depth decimal(10,2) not null default 0 comment '深度',
Height decimal(10,2) not null default 0 comment '高度',
Weight decimal(10,2) not null default 0 comment '重量',
Country varchar(12) not null default ''  comment '商品所在国家三位字母代码',
Location varchar(12) not null default ''  comment '商品所在国内城市Id',
IsBonded bool not null default 0 comment '是否为保税仓发货',
IsOversea bool not null default 0 comment '是否为海外直邮',
IsTiming bool not null default 0 comment '是否定时上架商品',
IsVirtual bool not null default 0 comment '是否为虚拟物品',
IsAutoFill bool not null default 0 comment '代充商品类型 可选类型： timecard(点卡软件代充) feecard(话费软件代充)',
SupportCod bool not null default 0 comment '是否支持货到付款',
FreightFree bool not null default 0 comment '是否免运费',
FreightTplId int not null default 0 comment '运费模板Id',
SubStock tinyint not null default 0 comment '0为拍下减库存 1为付款减库存',
Showcase int not null default 0 comment '橱窗推荐',
OnlineOn datetime not null comment '上架时间',
OfflineOn datetime not null comment '下架时间',
RewardRate decimal(2,2) not null default 0.3  comment '积分奖励',
HasInvoice bool not null default 0 comment '是否有发票',
HasWarranty bool not null default 0 comment '是否有保修',
HasGuarantee bool not null default 0 comment '是否承诺退换货服务',
SaleCount int not null default 0 comment '销售数量',
CollectCount int not null default 0 comment '收藏数量',
ConsultCount int not null default 0 comment '咨询数量',
CommentCount int not null default 0 comment '评论数量',
PropStr nvarchar(2000) not null default '' comment '商品属性值 格式 pid:vid:pname:vname;pid:vid:pname:vname',
PropAlias nvarchar(500) not null default '' comment '属性值别名,比如颜色的自定义名称 1627207:28335:草绿;1627207:3232479:深紫',
InputId nvarchar(500) not null default '' comment '商品输入属性Id',
InputStr nvarchar(600) not null default '' comment '商品输入属性值',
SearchPropId nvarchar(800) not null default '' comment '用于搜索的商品属性Id 格式：pid:vid;pid:vid',
SearchPropStr nvarchar(2000) not null default '' comment '用于搜索的商品属性项/值',
Meta varchar(200) not null default '' comment 'Meta',
Label varchar(200) not null default '' comment 'Label',
Related varchar(200) not null default '' comment '相关关联的',
Prority int not null default 0 comment '优先级',
SellerId int not null default 0 comment '卖家Id',
SellerName nvarchar(32) not null default '' comment '卖家用户名',
Status tinyint not null default 0 comment '状态 0为可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品';

drop table if exists ItemDesc;
create table ItemDesc(
Id int not null default 0 comment '商品Id',
Detail nvarchar(4000) default '' comment '商品详情',
Manual nvarchar(2000) default '' comment '使用指南 usage为mysql关键字无法使用',
Photo nvarchar(2000) default '' comment '产品实拍',
Story nvarchar(2000) default '' comment '品牌故事',
Notice nvarchar(2000) default '' comment '使用须知',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品描述';


drop table if exists Sku;
create table Sku(
Id int auto_increment not null comment 'Id',
SpuId int not null default 0 comment '产品Id',
ItemId int not null default 0 comment '商品Id',
CatgId int not null default 0 comment '最小类目Id',
CatgRId int not null default 0 comment '根类目Id',
BrandId int not null default 0 comment '品牌Id',
Code varchar(100) default '' comment '商家设置的外部id',
Stock int not null default 0 comment '库存数量',
Price decimal(10,2) not null default 0 comment '价格',
AppPrice decimal(10,2) not null default 0 comment 'app价格',
RetailPrice decimal(10,2) not null default 0  comment '建议零售价格',
Barcode varchar(50) not null default '' comment '条形码',
PropId nvarchar(800) not null default '' comment '商品属性Id 格式：pid:vid;pid:vid',
PropStr nvarchar(2000) not null default '' comment '商品属性值 格式 pid:vid:pname:vname;pid:vid:pname:vname',
SaleCount int not null default 0 comment '销售数量',
SellerId int not null default 0 comment '商家Id',
Status tinyint not null default 0 comment '状态 0为可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品Sku';

drop table if exists ItemComment;
create table ItemComment(
Id int not null default 0 comment '商品Id',
ImgCount int not null default 0 comment '有晒图的评价总数',
SumCount int not null default 0 comment '所有评价总数',
CountA int not null default 0 comment '1分评价数',
CountB int not null default 0 comment '2分评价数',
CountC int not null default 0 comment '3分评价数',
CountD int not null default 0 comment '4分评价数',
CountE int not null default 0 comment '5分评价数',
TagData nvarchar(2000) not null default '' comment '买家印象标签',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品评价汇总摘要';


drop table if exists Comment;
create table Comment(
Id int auto_increment not null comment 'Id',
Score tinyint not null default 3 comment '评分 1-5',
Content varchar(500) not null default '' comment '内容',
OrderId nvarchar(32) not null default '' comment '订单Id',
ItemId int not null default 0 comment '商品Id',
ItemImg nvarchar(160) not null default '' comment '商品图片',
ItemName nvarchar(160) not null default '' comment '商品名称',
SkuId int not null default 0 comment '商品SkuId',
SkuProp nvarchar(200) not null default '' comment '商品销售属性',
TagData nvarchar(500) not null default '' comment '买家印象标签 多个以","分隔',
ImageData varchar(1000) not null default '' comment '晒图 多个以","分隔',
Priority int not null default 0 comment '排序优先级',
LikeCount int not null default 0 comment '该评价被赞成总数',
ReplyCount int not null default 0 comment '该评价被回复总数',
RewardPoint int not null default 0 comment '奖励积分',
Reply varchar(500) not null default '' comment '卖家回复',
ReplyTime datetime comment '卖家回复时间',
Append varchar(500) not null default '' comment '买家追加评价',
AppendTime datetime comment '买家追加评价时间',
UserId int not null default 0 comment '买家Id',
UserName nvarchar(32) not null default '' comment '买家用户名',
SellerId int not null default 0 comment '卖家Id',
SellerName nvarchar(32) not null default '' comment '卖家用户名',
Status tinyint not null default 0 comment '0待审核 1审核未通过 2通过 3积分已返还',
CreatedBy nvarchar(50) not null default '' comment '来源 pc wap iphone android',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品评价';

drop table if exists CommentReply;
create table CommentReply(
Id int auto_increment not null comment 'Id',
Pid int not null default 0 comment '父级Id',
Path varchar(50) not null default '' comment '全路径',
Depth int not null default 0 comment '深度',
ItemId int not null default 0 comment '商品Id',
CommentId int not null default 0 comment '评价详情Id',
Content varchar(500) not null default '' comment '内容',
UserId int not null default 0 comment '买家Id',
UserName nvarchar(32) not null default '' comment '买家用户名',
Status bool not null default 0 comment '状态 true显示 false不显示',
CreatedBy nvarchar(50) not null default '' comment '来源 pc wap iphone android',
CreatedOn datetime not null default current_timestamp comment '创建时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品评价回复';

/*
drop table if exists Comment;
create table Comment(
Id int auto_increment not null comment 'Id',
Score tinyint not null default 3 comment '评分 1-5',
Content varchar(500) not null default '' comment '内容',
ItemId int not null default 0 comment '商品Id',
ItemImg nvarchar(160) not null default '' comment '商品图片',
ItemName nvarchar(160) not null default '' comment '商品名称',
SkuId int not null default 0 comment '商品SkuId',
SkuProp nvarchar(160) not null default '' comment '商品销售属性',
TagData nvarchar(200) not null default '' comment '买家印象标签',
ImageData varchar(3000) not null default '' comment '晒图 多个以","分隔',
Recommend bool not null default 0 comment '是否推荐',
LikeCount int not null default 0 comment '该评价被赞成总数',
ReplyCount int not null default 0 comment '该评价被回复总数',
RewardPoint int not null default 0 comment '奖励积分',
UserId int not null default 0 comment '用户Id',
UserName nvarchar(32) not null default '' comment '用户名',
SellerId int not null default 0 comment '商家Id',
OrderId nvarchar(32) not null default '' comment '订单Id',
Status tinyint not null default 1 comment '0待审核 1审核未通过 2通过 3积分已返还',
CreatedBy nvarchar(50) not null default '' comment '来源 pc wap iphone android',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品评价';


drop table if exists Consultation;
create table Consultation(
Id int auto_increment not null comment 'Id',
SkuId int not null default 0 comment '商品SkuId',
ItemId int not null default 0 comment '商品Id',
GroupId tinyint not null default 0 comment '分组 0商品咨询 1库存配送 2支付问题 3发票保修',
UserId int not null default 0 comment '用户Id',
UserName nvarchar(32) not null default '' comment '用户',
Question varchar(500) not null default '' comment '咨询内容',
Reply varchar(500) not null default '' comment '回复',
ReplyFlag tinyint not null default 0 comment '回复处理标识',
Replier nvarchar(50) not null default '' comment '回复者',
RepliedOn datetime comment '回复时间',
SellerId int not null default 0 comment '卖家Id',
SellerName nvarchar(32) not null default '' comment '卖家',
Useful int not null default 0 comment '有用数',
Status bool not null default 0 comment '状态 false未处理 true已回复',
CreatedBy nvarchar(50) not null default '' comment '来源 pc wap iphone android',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品咨询';


drop table if exists CustomsRecord;
create table CustomsRecord(
Id int auto_increment not null comment 'Id',
Code varchar(100) not null default '' comment '商品编号',
Name varchar(100) not null default '' comment '商品名称',
Brand varchar(100) not null default '' comment '品牌名称',
HSCode varchar(100) not null default '' comment 'HSCode',
Origin varchar(100) not null default '' comment '原产国',
Manufacturer varchar(100) not null default '' comment '生产商',
TariffRate decimal(4,3) not null default 0.17 comment '关税税率',
SellerId int default 0 not null comment '商家Id',
Status bool default 1 not null comment '状态 1为可用',
CreatedBy nvarchar(50) not null default '' comment 'pc wap iphone android',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品海关备案';
*/


drop table if exists Article;
create table Article( 
Id int auto_increment not null comment 'Id',
Type int not null default 0 comment '类型',
Name nvarchar(100) not null default '' comment '名称 方便后台使用',
Title nvarchar(150) not null default ''comment '标题 前台使用',
Label nvarchar(200) not null default '' comment '标签',
Link nvarchar(300) not null default '' comment '链接',
Icon varchar(300) not null default '' comment '图标',
Picture nvarchar(300) not null default '' comment '图片',
Keyword nvarchar(300) not null default '' comment '关键词',
Summary nvarchar(300) not null default '' comment '内容摘要',
Content text comment '详细内容',
Source nvarchar(50) not null default '' comment '来源',
Author nvarchar(50) not null default '' comment '作者',
StartedOn datetime not null default now() comment '开始时间',
StoppedOn datetime not null default now() comment '结束时间',
ViewCount int not null default 0 comment '浏览数量',
LikeCount int not null default 0 comment '点赞数量',
CollectCount int not null default 0 comment '收藏数量',
CommentCount int not null default 0 comment '评论数量',
Priority int not null default 0 comment '排序优先级 从小到大',
Badge nvarchar(100) not null default '' comment '徽章 标记',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status tinyint not null default 1 comment '状态 0可用 1不可用 2违禁',
UserId int not null default 0 comment '用户id',
CreatedBy nvarchar(50) not null default '' comment '创建者',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='文章';


/*
drop table if exists TrialPlan;
      
create table TrialPlan( 
Id varchar(20)  not null comment 'Id 系统生成',
Type int not null default 0 comment '类型 1免费试用 2超值试用',
Name nvarchar(100) not null default '' comment '活动名称 后台使用',
Title nvarchar(100) not null default '' comment '活动标题 前台使用',
Picture nvarchar(200) not null default '' comment '活动封面图',
Summary nvarchar(800) not null default '' comment '活动描述摘要',
StartedOn datetime not null default now() comment '开始时间',
StoppedOn datetime not null default now() comment '结束时间',
AnnouncedOn datetime not null default now() comment '审核结果公布时间',
ExpiredOn datetime not null default now() comment '报告提交截止时间',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status tinyint not null default 1 comment '状态 0审核通过 1待审核 2审核未通过',
SellerId int not null default 0 comment '卖家Id',
CreatedBy nvarchar(50) not null default '' comment '创建者',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='试用计划';


drop table if exists TrialProduct;
      
create table TrialProduct( 
Id varchar(20)  not null comment 'Id 试用计划Id', 
SkuId int not null default 0 comment 'SkuId',
ItemId int not null default 0 comment 'ItemId',
Name nvarchar(50) not null default '' comment '名称',
Picture nvarchar(500) not null default '' comment '图片',
Summary nvarchar(800) not null default '' comment '描述摘要',
Price decimal(8,2) not null default 0 comment '试用价格',
MarketPrice decimal(8,2) not null default 0 comment '市场价格',
Stock int not null default 0 comment '限量',
Applied int not null default 0 comment '已申请人数',
Accepted int not null default 0 comment '已通过审核人数',
Priority int not null default 0 comment '排序号 即商品展示的排序号。后端、前端均按排序号从大到小排列商品。如果有重复，按添加时间早晚排序。',
Extra nvarchar(200) not null default '' comment '扩展信息',
SellerId int not null default 0 comment '卖家Id',
CreatedBy nvarchar(50) not null default '' comment '创建者',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id,SkuId,ItemId)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='试用商品';



drop table if exists TrialRecord;      

create table TrialRecord( 
Id varchar(20)  not null comment 'Id 试用计划Id', 
SkuId int not null default 0 comment 'SkuId',
ItemId int not null default 0 comment 'ItemId',
Name nvarchar(50) not null default '' comment '名称',
Picture nvarchar(500) not null default '' comment '图片',
BuyerId int not null default 0 comment '买家Id',
BuyerName nvarchar(20) not null default '' comment '买家用户名',
SellerId int not null default 0 comment '卖家Id',
SellerName nvarchar(20) not null default '' comment '卖家用户名',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status int not null default 0 comment '0申请中 2申请失败',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id,SkuId,ItemId)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='试用申请';
*/

drop table if exists ActPlan;
create table ActPlan( 
Id int auto_increment not null comment 'Id',
Name nvarchar(50) not null default '' comment '名称',
Title nvarchar(50) not null default '' comment '标题',
Icon nvarchar(300) not null default '' comment '图标',
Picture nvarchar(300) not null default '' comment '图片',
WarmUp int not null default 1 comment '提前预热天数',
Summary nvarchar(300) not null default '' comment '描述',
OpendOn datetime not null default now() comment '报名开始时间',
ClosedOn datetime not null default now() comment '报名结束时间',
StartedOn datetime not null default now() comment '活动开始时间 必需晚于报名结束时间',
StoppedOn datetime not null default now() comment '活动结束时间',
Priority int not null default 0 comment '排序优先级',
Badge nvarchar(200) not null default '' comment '徽章 标记',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 true可用 false不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='官方活动计划';


drop table if exists ActItem;
create table ActItem( 
Id int auto_increment not null comment 'Id',
Type tinyint not null default 0 comment '分类店铺 商品 活动等',
PlanId int not null default 0 comment '计划Id',
GroupId int not null default 0 comment '分组Id',
Name nvarchar(50) not null default '' comment '名称',
Title nvarchar(50) not null default '' comment '标题',
Link nvarchar(300) not null default '' comment '链接',
Icon nvarchar(300) not null default '' comment '图标',
Picture nvarchar(300) not null default '' comment '图片',
Summary nvarchar(300) not null default '' comment '描述',
DataKey nvarchar(200) not null default '' comment '数据键',
DataVal nvarchar(4000) not null default '' comment '数据值',
StartedOn datetime not null default now() comment '开始时间',
StoppedOn datetime not null default now() comment '结束时间',
Priority int not null default 0 comment '排序优先级 从小到大',
Badge nvarchar(200) not null default '' comment '徽章 标记',
Extra nvarchar(200) not null default '' comment '扩展信息',
SellerId int not null default 0 comment '卖家Id',
SellerName nvarchar(32) not null default '' comment '卖家用户名',
Status bool not null default 0 comment '状态 true可用 false不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='官方活动条目';
