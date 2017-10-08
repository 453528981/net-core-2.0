/*
drop database if exists Promotion;

create database Promotion;
*/

use Promotion;

drop table if exists Activity;

create table Activity(
Id int auto_increment not null comment 'Id',
Type tinyint not null default 0 comment '类型 0为满元减 1为满件折',
Name nvarchar(200) not null default '' comment '活动名称',
Title nvarchar(300) not null default '' comment '活动标题',
Global bool not null default 0 comment '适用于(全场店铺)所有商品',
WarmUp int not null default 1 comment '提前预热天数 0无预热', 
Infinite bool not null default 0 comment '上不封顶(当规则为满元减且只有一级时 该值可为true)', 
Picture varchar(500) comment '标准版 活动图片',
StartedOn datetime not null comment '限购开始时间',
StoppedOn datetime not null comment '限购结束时间',
Platform tinyint not null default 0 comment '活动适用平台 0为None 1为适用平于pc 2为适用平于wap 4为适用平于app',
MediaScope varchar(800) not null default '' comment '限定媒体Id 空为无限定 如需限定部分媒体 使用","分隔',
ItemScope varchar(4000) not null default '' comment 'Global==false时为包含的商品多个以,分隔 Global==true时为排除的商品多个以,分隔',
LimitType tinyint not null default 0 comment '用户参与活动限制类型 0无限制 1限制该活动总的参与次数 2限制该活动每个用户可参与次数', 
LimitValue int not null default 0 comment '用户参与活动限制值 LimitType不为0时有效', 

FreightFree bool not null default 0 comment '是否免运费', 
FreightFreeExclude varchar(200) comment ' 免运费排除在外的地区多个以,分隔', 

ExternalUrl varchar(500) comment '豪华版 专辑地址',
RuleData nvarchar(4000) not null default '' comment '活动规则',
SellerId int not null default 0 comment '卖家Id', 
SellerName nvarchar(50) not null default '' comment '卖家名称',
Status bool not null default 0 comment '状态 1为可用 0为不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='店铺活动';

/*
drop table if exists ActivityRule;
create table ActivityRule(
Id int auto_increment not null comment 'Id',
ParentId int not null default 0 comment '店铺活动Id', 
Threshold decimal(8,2) not null default 0 comment '满足满减/折最低门槛', 
Discount decimal(8,2) not null default 0 comment '满减/满折值', 
SendGift bool not null default 0 comment '送赠品', 
GiftData nvarchar(2000) not null default '' comment '赠品信息 Json格式', 
SendCoupon bool not null default 0 comment '送店铺优惠券', 
CouponData nvarchar(2000) not null default '' comment '优惠券信息 Json格式', 
SellerId int not null default 0 comment '卖家Id', 
Status bool not null default 0 comment '状态 true为可用 false为不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='店铺活动规则';
*/

drop table if exists Package;
create table Package(
Id int auto_increment not null comment 'Id',
Name nvarchar(40) not null default '' comment '套餐名称',
Fixed bool default 0 not null default 0 comment '固定组合套餐 商品打包成套餐销售 消费者打包购买 自选商品套餐 套餐中的附属商品 消费者可以通过复选框的方式有选择的购买',
Summary nvarchar(100) not null default '' comment '套餐简介 摘要 将出现在主商品详情描述页面 衣介绍套餐的卖点',
StartedOn datetime not null comment '限购开始时间',
StoppedOn datetime not null comment '限购结束时间',
Platform tinyint not null default 0 comment '活动适用平台 0为None 1为适用平于pc 2为适用平于wap 4为适用平于app',
MediaScope varchar(800) not null default '' comment '限定媒体Id 空为无限定 如需限定部分媒体 使用","分隔',
ItemId int not null default 0 comment '主商品Id',
ItemName nvarchar(200) not null default '' comment '主商品名称' ,
ItemPrice decimal(8,2) not null default 0 comment '主商品搭配价格 0为默认如果不设置搭配价 则执行在售价(适用于有多个不同Sku 如果没有sku则可设置一个搭配价格)',
ItemPictrue nvarchar(300) not null default '' comment '主商品搭配图' ,
LimitType tinyint not null default 0 comment '用户参与活动限制类型 0无限制 1限制该活动总的参与次数 2限制该活动每个用户可参与次数', 
LimitValue int not null default 0 comment '用户参与活动限制值 LimitType不为0时有效', 
SellerId int not null default 0 comment '卖家Id', 
SellerName nvarchar(50) not null default '' comment '卖家名称',
Status bool not null default 0 comment '状态 1为可用 0为不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='搭配组合套餐促销';


drop table if exists PackageItem;
create table PackageItem(
Id int auto_increment not null comment 'Id',
ParentId int not null default 0 comment '搭配组合套餐Id',
MainId int not null default 0 comment '主商品Id',
ItemId int not null default 0 comment '附属商品ItemId(无sku)',
SkuId int not null default 0 comment '附属商品SkuId',
Name nvarchar(200) not null default '' comment '附属商品名称',
Price decimal(8,2) not null default 0 comment '附属商品价格 0为默认如果不设置搭配价 则执行在售价',
Picture nvarchar(300) not null default '' comment '附属商品图片',
Selected bool not null default 0 comment '默认勾选',
Priority int not null default 0 comment '排序 从小到大',
SellerId int not null default 0 comment '卖家Id', 
Status bool not null default 0 comment '状态 1为可用 0为不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='套餐附属商品';


drop table if exists LimitBuy;
create table LimitBuy(
Id int auto_increment not null comment 'Id',
ItemId int not null default 0 comment '商品Id',
StartedOn datetime not null comment '限购开始时间',
StoppedOn datetime not null comment '限购结束时间',
Platform tinyint not null default 0 comment '活动适用平台 0为None 1为适用平于pc 2为适用平于wap 4为适用平于app',
MediaScope varchar(800) not null default '' comment '限定媒体Id 空为无限定 如需限定部分媒体 使用","分隔',
Value int not null default 0 comment '每个帐户限购数量',
SellerId int not null default 0 comment '卖家Id', 
SellerName nvarchar(50) not null default '' comment '卖家名称',
Status bool not null default 0 comment '状态 1为可用 0为不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='限购促销';

drop table if exists SpecialPrice;
create table SpecialPrice(
Id int auto_increment not null comment 'Id',
Type tinyint not null default 0 comment 'A打折  B减价  C促销价 活动创建后优惠方式将不能修改',
Name nvarchar(200) not null comment '活动名称',
Title nvarchar(300) not null comment '优惠标题',
StartedOn datetime not null comment '开始时间',
StoppedOn datetime not null comment '结束时间',
Platform tinyint not null default 0 comment '活动适用平台 0为None 1为适用平于pc 2为适用平于wap 4为适用平于app',
MediaScope varchar(800) not null default '' comment '限定媒体Id 空为无限定 如需限定部分媒体 使用","分隔',
FreightFree bool not null default 0 comment '免运费',
FreightFreeExclude varchar(200) default '' comment '免运费排除在外的地区(以,分隔)',
SellerId int not null default 0 comment '卖家Id', 
SellerName nvarchar(50) not null default '' comment '卖家名称',
Status bool not null default 0 comment '状态 1为可用 0为不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='特价促销';

drop table if exists SpecialPriceItem;
create table SpecialPriceItem(
Id int auto_increment not null comment 'Id',
ParentId int not null default 0 comment '特价Id',
ItemId int not null default 0 comment '商品Id',
Global bool not null default 0 comment '统一设置优惠(商品维度)',
Value decimal(8,2) not null default 0 comment '统一设置优惠值(商品维度)',
LimitType tinyint not null default 0 comment '用户参与活动限制类型 0无限制 1限制该活动总的参与次数 2限制该活动每个用户可参与次数', 
LimitValue int not null default 0 comment '用户参与活动限制值 LimitType不为0时有效', 
SkuData nvarchar(2000) not null default '' comment '对Sku设置的优惠信息 Json格式',
SellerId int not null default 0 comment '卖家Id', 
Status bool not null default 0 comment '状态 1为可用 0为不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品特价';

drop table if exists CartActivity;
create table CartActivity(
Id int auto_increment not null comment 'Id',
Type tinyint not null default 0 comment '类型 0为满元减 1为满件折',
Name nvarchar(200) not null default '' comment '活动名称',
Title nvarchar(300) not null default '' comment '活动标题',
StartedOn datetime not null comment '开始时间',
StoppedOn datetime not null comment '结束时间',
DiscountOn tinyint not null default 0 comment '促销折扣/减免金额作用于 订单总金额 商品总金额 运费 税费 商品价格',
DiscountValue decimal(8,2) not null default 0 comment '促销值 打x折 减x元',

Platform tinyint not null default 0 comment '活动适用平台 0为None 1为适用平于pc 2为适用平于wap 4为适用平于app',
UserGrade tinyint not null default 0 comment '限定最低用户级别 0为无限定',
/*PayMethod tinyint not null default 0 comment '限定支付方式 微信 支付宝等',*/
UserScope varchar(4000)not null default '' comment '限定用户Id 空为无限定 如需限定部分用户 使用","分隔',
ItemScope varchar(8000)not null default '' comment '限定商品ItemId 空为无限定 如需限定部分商品 使用","分隔',
CatgScope varchar(800)not null default '' comment '限定商品CageId 空为无限定 如需限定部分类目 使用","分隔',
BrandScope varchar(800)not null default '' comment '限定商品BrandId 空为无限定 如需限定部分品牌 使用","分隔',
MediaScope varchar(800) not null default '' comment '限定媒体Id 空为无限定 如需限定部分媒体 使用","分隔',
RegionScope varchar(800) not null default ''comment '限定区域Id 空为无限定 如需限定部分区域 使用","分隔',

LimitType tinyint not null default 0 comment '用户参与活动限制类型 0无限制 1限制该活动总的参与次数 2限制该活动每个用户可参与次数', 
LimitValue int not null default 0 comment '用户参与活动限制值 LimitType不为0时有效',
SellerId int not null default 0 comment '卖家Id', 
SellerName nvarchar(50) not null default '' comment '卖家名称',
Status bool not null default 0 comment '状态 1为可用 0为不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='购物车促销';

drop table if exists CartActivityRule;
create table CartActivityRule(
Id int auto_increment not null comment 'Id',
ParentId int not null default 0 comment '购物车促销Id',
StartedOn datetime not null comment '开始时间',
StoppedOn datetime not null comment '结束时间',
CalcType tinyint not null default 0 comment '计算方式',
CalcValue varchar(2000) not null default '' comment '计算使用到的参数值',
Priority int not null default 0 comment '优先顺序 从小到大',
SellerId int not null default 0 comment '卖家Id', 
Status bool not null default 0 comment '状态 1为可用 0为不可用',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='购物车促销规则';