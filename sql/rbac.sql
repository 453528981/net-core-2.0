/*
drop database if exists Rbac;
create database Rbac;
*/
use Rbac;
drop table if exists Role;
create table Role( 
Id int auto_increment not null comment 'Id',
Name nvarchar(50) not null default '' comment '名称',
Summary nvarchar(500) default '' comment'摘要',
Priority int not null default 0 comment '排序',
MangerId int not null default 0 comment '该角色所属管理者Id',
Status bool not null default 0 comment '状态 true可用 false不可用',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='角色';

/*
truncate table Role;
use Rbac;
insert into Role values(0,'超级系统管理员','超级系统管理员',0,0,1);
*/

drop table if exists Module;
create table Module( 
Id int auto_increment not null comment 'Id',
Pid int not null default 0 comment '父Id',
Name nvarchar(50) not null default '' comment '名称',
Path nvarchar(200) not null default '' comment '路径',
Icon varchar(50) default '' comment '图标',
Link varchar(500) default '' comment '链接',
Depth tinyint not null default 0 comment '深度',
Priority int not null default 0 comment '排序',
Status bool not null default 0 comment '状态 true可用 false不可用',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='模块';

/*
truncate table Module;
use Rbac;
insert into Module values(0,0,'系统模块','1','','',1,1,1);
insert into Module values(0,0,'卖家模块','2','','',1,1,1);
insert into Module values(0,0,'买家模块','3','','',1,1,1);
*/


drop table if exists Func;
create table Func(
Id char(36) not null comment 'Id',
Mid int not null default 0 comment '模块Id',
Name nvarchar(50) not null default '' comment '名称',
Summary nvarchar(500) default '' comment'摘要',
Priority int not null default 0 comment '排序',
Status bool not null default 0 comment '状态 true可用 false不可用',
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='模块功能';

/*
use Rbac;
insert into Func values('func-001',1,'功能1','',1,1);
insert into Func values('func-002',1,'功能2','',1,1);
insert into Func values('func-003',2,'功能3','',1,1);
*/


drop table if exists RoleFunc;
create table RoleFunc( 
RoleId int not null comment '角色Id',
FuncId char(36) not null comment '功能Id',
primary key(RoleId,FuncId)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='模块功能';


/*
use Rbac;
insert into RoleFunc values(1,'func-001');
insert into RoleFunc values(1,'func-002');
insert into RoleFunc values(1,'func-003');
*/
/*
drop table if exists UserRole;
create table UserRole( 
UserId int not null comment '用户Id',
RoleId char(36) not null comment '角色Id',
primary key(UserId,RoleId)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户角色';
*/
/*
use Rbac;
insert into UserRole values(1,'role-001');
*/

/*
select rf.*
,m.Id as ModuleId

,r.Name as RoleName
,f.Name as FuncName
,m.Name as ModuleName
,m.Icon as ModuleIcon
,m.Link as ModuleLink
,m.Priority as ModulePriority
,m.Pid as ModuleParentId
from UserRole ur
inner join RoleFunc rf on ur.RoleId=rf.RoleId
inner join Role r on rf.RoleId=r.Id
inner join Func f on rf.FuncId=f.Id
inner join Module m on f.Mid=m.Id
where ur.UserId=1
*/


select rf.*
,m.Id as ModuleId
,r.Name as RoleName
,f.Name as FuncName
,m.Name as ModuleName
,m.Icon as ModuleIcon
,m.Link as ModuleLink
,m.Priority as ModulePriority
,m.Pid as ModuleParentId
from RoleFunc rf
inner join Role r on rf.RoleId=r.Id
inner join Func f on rf.FuncId=f.Id
inner join Module m on f.Mid=m.Id