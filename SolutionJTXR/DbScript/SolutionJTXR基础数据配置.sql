USE [SolutionJTXRDb]
GO
--第一步：先导入Modules ,导入之后除以父节点id
--Data Source=(localdb)\MSSQLLocalDB; Integrated Security=True; Initial Catalog=SolutionDb; Pooling=True; MultipleActiveResultSets=True;
DELETE FROM [dbo].[Modules]
GO
dbcc checkident('[dbo].[Modules]',reseed,1)
GO
DECLARE @tmpId  int
SELECT @tmpId= IDENT_CURRENT('Modules')
print(@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'企业管理',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'企业信息',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'部门管理',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'员工信息',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'考勤管理',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'产品管理',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'产品信息',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'产品类别',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'工序管理',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'工艺流程',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'BOM管理',null,@tmpId+5,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+5) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'BOR管理',null,@tmpId+6,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+5) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'计划管理',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'生产订单',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'工单管理',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'计划执行',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'设备管理',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'设备类别',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'设备厂家',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'设备信息',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'设备维护',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'维护计划',null,@tmpId+5,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+5) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'设备运行状态',null,@tmpId+6,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+6) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'备件管理',null,@tmpId+7,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+7) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'备件类别',null,@tmpId+8,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+8) + '$',@tmpId)

INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'制程管理',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'产品制程',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'质量管理',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'质量计划',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'检测记录',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'数据采集',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'质量追溯',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'质量统计分析',null,@tmpId+5,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+5) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'文档管理',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'工艺文档',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'作业指导',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'综合看板',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'工单看板',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'产量看板',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'设备看板',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'设备看板',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'仓储管理',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'基础信息',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'入库管理',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'出库管理',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'库存管理',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'能源管理',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'空调系统',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$' ,@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'空压系统',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'排气系统',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'电能系统',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'供水系统',null,@tmpId+5,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$' ,@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'数据采集',null,@tmpId+6,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'单价管理',null,@tmpId+7,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'成本管理',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'原料成本',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$' ,@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'能源成本',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'人工成本',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'备品备件成本',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)

INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'AGV调度',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'车辆管理',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)

INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'系统管理',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'用户管理',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$' ,@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'角色管理',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'模块管理',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'功能管理',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)



GO

--第二步
--插入模块权限基本数据，role id是1
DELETE from [dbo].[ModuleRoles]
INSERT INTO [dbo].[ModuleRoles]([Module_Id],[Role_Id])  (select  id , '1'from Modules)
GO
--第二步
--插入模块功能基本数据,根据名称。模糊匹配
DELETE from [dbo].[ModuleFunctions]
INSERT INTO [dbo].[ModuleFunctions]([Module_Id],[Function_Id]) 
select * from  (select Modules.id  ,Functions.id as fid from  Modules  left join Functions on  Functions.Name  like  '%'+Modules.Name+'%')  as tmp  where tmp.fid is not null

GO

--部门测试数据
delete from [dbo].[EntDepartmentInfoes]
DECLARE @ct  int
set @ct=1
WHILE @ct < 500
BEGIN  

INSERT INTO [dbo].[EntDepartmentInfoes]
           ([Id],[DepartmentName],[DepartmentCode],[DepartmentFunction],[Description],[Remark],[CreatedTime],[CreatorUserId],[LastUpdatedTime],[LastUpdatorUserId])
     VALUES
           (NEWID(),N'部门'+CAST(@ct as nvarchar),N'BM'+CAST(@ct as nvarchar),N'统计',N'测试',N'测试'+CAST(@ct as nvarchar),GETDATE(),'admin', GETDATE(),'admin')
set @ct=@ct+1
END
GO
