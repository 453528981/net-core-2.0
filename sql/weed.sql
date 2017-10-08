/*
drop database if exists Weed;
create database Weed;
*/
use Weed;

drop table if exists WeedDir;
create table WeedDir( 
Id int auto_increment not null comment 'Id',
Pid int not null default 0 comment '父Id',
Name nvarchar(50) not null default '' comment '名称',
Depth tinyint not null default 0 comment '深度',
Badge nvarchar(200) not null default '' comment '标记',
Extra nvarchar(200) not null default '' comment '扩展信息',
UserId int not null default 0 comment '用户id',
Status bool not null default 0 comment '状态 true可用 false不可用',
CreatedBy nvarchar(50) not null default '' comment '创建者',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='目录';

drop table if exists WeedFile;
create table WeedFile( 
Id varchar(20) not null comment 'Id',
Uid int not null default 0 comment '用户Id',
Did int not null default 0 comment '目录Id',
Ext varchar(5) not null default '' comment '扩展名',
Url varchar(300) not null default '' comment 'Url',
Size int not null default 0 comment '大小',
Name nvarchar(50) not null default '' comment '名称',
Badge nvarchar(200) not null default '' comment '标记',
Extra nvarchar(200) not null default '' comment '扩展信息',
Status bool not null default 0 comment '状态 true可用 false不可用',
CreatedBy nvarchar(50) not null default '' comment '创建者',
CreatedOn datetime not null default current_timestamp comment '创建时间',
ModifiedBy nvarchar(50) not null default '' comment '最后一次编辑者',
ModifiedOn timestamp not null default current_timestamp on update current_timestamp comment '最后一次编辑时间',
primary key(Id,Uid)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='文件';