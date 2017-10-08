/*
drop database if exists Trade;

create database Trade;

select date_add(now(),interval 1 week) as d
*/
use Trade;
drop table if exists OrderNote;
drop table if exists OrderItem;
drop table if exists OrderInfo;
create table OrderInfo(
Id char(18) not null comment '订单Id',
Type tinyint not null default 0 comment '订单类型:0网购订单 1竞购订单 2竞购补差订单 3积分兑换订单',
Quantity int not null default 0 comment '商品总数量',
SubTotal decimal(8,2) not null default 0 comment '订单小计(商品总金额)',
Freight decimal(8,2) not null default 0 comment '运费',
Tax decimal(8,2) not null default 0 comment '关税税费',
Discount decimal(8,2) not null default 0 comment '订单优惠总金额(使用积分抵扣 优惠券 店铺优惠等)',
Total decimal(8,2) not null default 0 comment '订单总金额(需要支付的总金额[SubTotal+Postage-Discount])',
Paid decimal(8,2) not null default 0 comment '已支付金额(一个订单可能分多次支付)',
PayId varchar(150) not null default '' comment '支付流水号(一个订单可能分多次支付)',
PaidOn datetime comment '支付日期(最后一次支付完成整个订单的时间)',
PointUse int not null default 0 comment '使用积分',
PointRealUse int not null default 0 comment '实际使用积分',
PointReward int not null default 0 comment '奖励积分',
Coupon varchar(36) not null default '' comment '优惠券',
CouponUse decimal(8,2) not null default 0 comment'优惠券抵消金额',
GiftCard varchar(36) not null default '' comment '礼品卡',
GiftCardUse decimal(8,2) not null default 0 comment '礼品卡抵消金额',
PromotionData nvarchar(4000) not null default '' comment'优惠详情json格式',
Weight decimal(8,2) not null default 0 comment '重量',
ETicket varchar(200) not null default '' comment'电子凭证',
IsVirtual bool not null default 0 comment '是否为虚拟物品 无需发货订单',
IsBonded bool not null default 0 comment '是否为保税仓发货',
IsOversea bool not null default 0 comment '是否为海外直邮',

PaymentType tinyint not null default 0 comment '支付方式',
PaymentData varchar(200) not null default '' comment'支付方式信息',
ShipmentType tinyint not null default 0 comment '配送方式',
ShipmentData varchar(200) not null default '' comment'配送方式信息',


ExpiredOn datetime not null comment'超时时间',
ConsignedOn datetime comment '发货日期',
FinishedOn datetime comment '结束日期 交易成功时间(更新交易状态为成功的同时更新)/确认收货时间或者交易关闭时间',

InvoiceType tinyint not null default 0 comment '0为不需要 1纸质发票 2为电子发票', 
InvoiceTitle nvarchar(100) not null default '' comment '发票抬头 个人或单位名称',
InvoiceContent nvarchar(100) not null default '' comment '发票内容',
InvoiceStatus tinyint not null default 0 comment '发票状态 0未开 1已开纸质发票/已下载电子发票',

LogisticsNo varchar(100) not null default '' comment '物流公司运单号',
LogisticsType tinyint not null default 0 comment '0默认 1分拆成多个包裹发货 2多个订单合成一个包裹发货',
LogisticsCode varchar(100) not null default '' comment '物流公司编号',
LogisticsName nvarchar(100) not null default '' comment '物流公司名称',

Receiver nvarchar(50) not null default '' comment '收货人',
ReceiverPhone varchar(25) not null default '' comment '固定电话',
ReceiverMobile varchar(11) not null default '' comment '移动电话',
ReceiverRegionId varchar(6) not null default '' comment '行政区号',
ReceiverProvince varchar(50) not null default '' comment '省',
ReceiverCity varchar(50) not null default '' comment '市',
ReceiverDistrict varchar(50) not null default '' comment '区',
ReceiverStreet nvarchar(200) not null default '' comment '街道门牌号',
ReceiverPostalCode varchar(20) not null default '' comment '邮编',

BuyerId int not null default 0 comment '买家Id',
BuyerName nvarchar(20) not null default '' comment '买家用户名',
BuyerFlag tinyint not null default 0 comment '买家备注旗帜 只有买家才能查看该字段',
BuyerMemo nvarchar(100) not null default '' comment '买家备注',
BuyerRated bool not null default 0 comment '买家是否已评价 可选值:true(已评价),false(未评价) 如买家只评价未打分，此字段仍返回false',
BuyerMessage nvarchar(500) not null default '' comment '买家留言',
SellerId int not null default 0 comment '卖家Id',
SellerName nvarchar(20) not null default '' comment '卖家用户名',
SellerFlag tinyint not null default 0 comment '卖家备注Flag',
SellerMemo nvarchar(100) not null default '' comment '卖家备注',
HasReturn bool not null default 0 comment '是否有退/换货',
HasRefund bool not null default 0 comment '是否有退款',
CancelId tinyint not null default 0 comment '订单取消类型 0为none 1为系统取消 2为买家取消 3为卖家取消',
CancelReason nvarchar(200) not null default '' comment '订单取消原因',
RelatedId varchar(50) not null default '' comment '关联Id',
MediaId int not null default 0 comment '媒体Id',
TraceCode varchar(50) not null default '' comment '媒体跟踪码',
IpAddress varchar(20) not null default '' comment 'ip',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status int not null default 1 comment '订单状态',
CreatedBy nvarchar(50) not null default '' comment '订单来源 pc wap app 等',
CreatedOn datetime not null default current_timestamp  comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp  comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单';

drop table if exists OrderItem;
create table OrderItem(
Id char(20) not null comment '订单明细Id',
OrderId char(18) not null default '' comment '订单Id',
SpuId int not null default 0 comment 'SpuId',
ItemId int not null default 0 comment 'ItemId',
SkuId int not null default 0 comment 'SkuId', 
CatgRId int not null default 0 comment '根类目id',
CatgMId varchar(100) not null default '' comment '中间类目id',
CatgId int default 0 not null default 0  comment '最小类目id',
PackageId int default 0 not null default 0 comment '套餐Id',
PackageName nvarchar(100) not null default '' comment '套餐名',
Code varchar(100) not null default '' comment '商家设置的外部id',
Name nvarchar(80) not null default '' comment '商品名称',
Price decimal(8,2) not null default 0 comment '成交时真实单价 精确到2位小数 单位 元',
PriceShow decimal(8,2) not null default 0 comment '交易页展示单价 精确到2位小数 单位 元',
Quantity int not null default 0 comment '数量',
Tax decimal(8,2) not null default 0 comment '关税税费',
Adjust decimal(8,2) not null default 0 comment '卖家手工调整金额',
Discount decimal(8,2) not null default 0 comment '订单优惠金额 精确到2位小数 单位元',
Total decimal(8,2) not null default 0 comment '商品金额小计 精确到2位小数 单位元',
TaxRate decimal(8,2) not null default 0 comment '关税税率',
Picture varchar(500) not null default '' comment '图片',
PropText nvarchar(500) not null default '' comment '商品销售属性',
IsGift bool not null default 0 comment '是否为赠品',
IsVirtual bool not null default 0 comment '是否为虚拟物品',
IsService bool not null default 0 comment '是否是服务项目',
PromotionData nvarchar(4000) not null default '' comment'优惠详情json格式',

ExpiredOn datetime not null comment '超时时间',
ConsignedOn datetime comment '子订单发货时间，当卖家对订单进行了多次发货，子订单的发货时间和主订单的发货时间可能不一样了，那么就需要以子订单的时间为准。没有进行多次发货的订单，主订单的发货时间和子订单的发货时间都一样',
FinishedOn datetime comment '子订单的交易结束时间 说明：子订单有单独的结束时间，与主订单的结束时间可能有所不同，在有退款发起的时候或者是主订单分阶段付款的时候，子订单的结束时间会早于主订单的结束时间',

LogisticsNo varchar(50) not null default '' comment '子订单所在包裹的运单号',
LogisticsName nvarchar(50) not null default '' comment '子订单发货的物流公司名称',

ReturnId char(24) not null default '' comment '退/换货Id',
ReturnStatus tinyint not null default 0 comment '退/换货状态',
RefundId char(24) not null default '' comment '退款Id',
RefundStatus tinyint not null default 0 comment '退款状态',
BuyerId int not null default 0 comment '买家Id',
BuyerName nvarchar(20) not null default '' comment '买家用户名',
BuyerRated bool not null default 0 comment '买家是否已评价 可选值:true(已评价),false(未评价) 如买家只评价未打分，此字段仍返回false',
SellerId int not null default 0 comment '卖家Id',
SellerName nvarchar(20) not null default '' comment '卖家用户名',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status int not null default 1 comment '子订单状态',
CreatedOn datetime not null default current_timestamp  comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp  comment '最后一次编辑时间',
primary key(Id),
foreign key(OrderId) references OrderInfo(Id) on delete cascade
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单明细';


/*
drop table if exists OrderExtend;
create table OrderExtend(
Id char(18) not null comment '订单Id',


primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单扩展信息';
*/

drop table if exists OrderNote;
create table OrderNote(
Id int auto_increment not null comment 'Id',
Type int not null default 0 comment '类型',
UserId int not null default 0 comment 'UserId',
OrderId char(18) not null default '' comment '订单Id',
Subject nvarchar(50) not null default '' comment '主题',
Message nvarchar(500) not null default '' comment '消息',
Extra nvarchar(500) not null default '' comment '扩展',
CreatedBy nvarchar(50) not null default '' comment '创建者 UserName',
CreatedOn datetime default current_timestamp  not null comment '创建时间',
primary key(Id),
foreign key(OrderId) references OrderInfo(Id) on delete cascade
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单Note';

drop table if exists Shipment;
create table Shipment(
Id char(18) not null comment '物流单Id(默认/拆单时该值为OrderId,合单时该值为系统生成)',
Type tinyint not null default 0 comment '0默认 1为拆单 2为合单',
IsCOD bool not null default 0 comment '是否货到付款',
OrderId char(18) not null comment '订单Id(默认/拆单时该值为订单Id,合单时该值为多个订单Id)',
Payment decimal(8,2) not null comment '货到付款 待支付金额',
ItemData nvarchar(4000) not null comment '货物明细json格式',

LogisticsNo varchar(100) not null default '' comment '物流公司运单号',
LogisticsCode varchar(100) not null default '' comment '物流公司编号',
LogisticsName nvarchar(100) not null default '' comment '物流公司名称',

Receiver nvarchar(50) not null default '' comment '收货人',
ReceiverPhone varchar(25) not null default '' comment '固定电话',
ReceiverMobile varchar(11) not null default '' comment '移动电话',
ReceiverRegionId varchar(6) not null default '' comment '行政区号',
ReceiverProvince varchar(50) not null default '' comment '省',
ReceiverCity varchar(50) not null default '' comment '市',
ReceiverDistrict varchar(50) not null default '' comment '区',
ReceiverStreet nvarchar(200) not null default '' comment '街道门牌号',
ReceiverPostalCode varchar(20) not null default '' comment '邮编',

BuyerId int not null default 0 comment '买家Id',
BuyerName nvarchar(20) not null default '' comment '买家用户名',
SellerId int not null default 0 comment '卖家Id',
SellerName nvarchar(20) not null default '' comment '卖家用户名',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status int not null default 1 comment '状态',
CreatedBy nvarchar(50) not null default '' comment '',
CreatedOn datetime not null default current_timestamp  comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp  comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='物流单';

/*
物流订单状态,可选值:CREATED(订单已创建) 
RECREATED(订单重新创建) 
CANCELLED(订单已取消) 
CLOSED(订单关闭) 
SENDING(等候发送给物流公司) 
ACCEPTING(已发送给物流公司,等待接单) 
ACCEPTED(物流公司已接单) 
REJECTED(物流公司不接单) 
PICK_UP(物流公司揽收成功) 
PICK_UP_FAILED(物流公司揽收失败) 
LOST(物流公司丢单) 
REJECTED_BY_RECEIVER(对方拒签) 
ACCEPTED_BY_RECEIVER(发货方式在线下单：对方已签收；自己联系：卖家已发货)
*/