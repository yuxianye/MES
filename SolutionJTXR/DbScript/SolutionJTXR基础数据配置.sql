USE [SolutionJTXRDb]
GO
--��һ�����ȵ���Modules ,����֮����Ը��ڵ�id
--Data Source=(localdb)\MSSQLLocalDB; Integrated Security=True; Initial Catalog=SolutionDb; Pooling=True; MultipleActiveResultSets=True;
DELETE FROM [dbo].[Modules]
GO
dbcc checkident('[dbo].[Modules]',reseed,1)
GO
DECLARE @tmpId  int
SELECT @tmpId= IDENT_CURRENT('Modules')
print(@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��ҵ����',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��ҵ��Ϣ',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'���Ź���',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'Ա����Ϣ',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'���ڹ���',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��Ʒ����',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��Ʒ��Ϣ',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��Ʒ���',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�������',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��������',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'BOM����',null,@tmpId+5,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+5) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'BOR����',null,@tmpId+6,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+5) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�ƻ�����',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��������',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��������',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�ƻ�ִ��',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�豸����',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�豸���',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�豸����',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�豸��Ϣ',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�豸ά��',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'ά���ƻ�',null,@tmpId+5,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+5) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�豸����״̬',null,@tmpId+6,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+6) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��������',null,@tmpId+7,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+7) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�������',null,@tmpId+8,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+8) + '$',@tmpId)

INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�Ƴ̹���',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��Ʒ�Ƴ�',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��������',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�����ƻ�',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'����¼',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'���ݲɼ�',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'����׷��',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'����ͳ�Ʒ���',null,@tmpId+5,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+5) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�ĵ�����',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�����ĵ�',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��ҵָ��',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�ۺϿ���',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��������',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��������',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�豸����',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�豸����',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�ִ�����',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'������Ϣ',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'������',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�������',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'������',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��Դ����',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�յ�ϵͳ',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$' ,@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��ѹϵͳ',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'����ϵͳ',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'����ϵͳ',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��ˮϵͳ',null,@tmpId+5,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$' ,@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'���ݲɼ�',null,@tmpId+6,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'���۹���',null,@tmpId+7,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)


INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�ɱ�����',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'ԭ�ϳɱ�',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$' ,@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��Դ�ɱ�',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�˹��ɱ�',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��Ʒ�����ɱ�',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)

INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'AGV����',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��������',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$',@tmpId)

INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'ϵͳ����',null,@tmpId,null,null)
SELECT @tmpId= IDENT_CURRENT('Modules')
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'�û�����',null,@tmpId+1,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+1) + '$' ,@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'��ɫ����',null,@tmpId+2,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+2) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'ģ�����',null,@tmpId+3,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+3) + '$',@tmpId)
INSERT INTO [dbo].[Modules]([Icon],[Name],[Remark],[OrderCode],[TreePathString],[Parent_Id])VALUES('Images\\Folder_32x32.png',N'���ܹ���',null,@tmpId+4,'$' + Convert(varchar,@tmpId) + '$' + ',$' + Convert(varchar,@tmpId+4) + '$',@tmpId)



GO

--�ڶ���
--����ģ��Ȩ�޻������ݣ�role id��1
DELETE from [dbo].[ModuleRoles]
INSERT INTO [dbo].[ModuleRoles]([Module_Id],[Role_Id])  (select  id , '1'from Modules)
GO
--�ڶ���
--����ģ�鹦�ܻ�������,�������ơ�ģ��ƥ��
DELETE from [dbo].[ModuleFunctions]
INSERT INTO [dbo].[ModuleFunctions]([Module_Id],[Function_Id]) 
select * from  (select Modules.id  ,Functions.id as fid from  Modules  left join Functions on  Functions.Name  like  '%'+Modules.Name+'%')  as tmp  where tmp.fid is not null

GO

--���Ų�������
delete from [dbo].[EntDepartmentInfoes]
DECLARE @ct  int
set @ct=1
WHILE @ct < 500
BEGIN  

INSERT INTO [dbo].[EntDepartmentInfoes]
           ([Id],[DepartmentName],[DepartmentCode],[DepartmentFunction],[Description],[Remark],[CreatedTime],[CreatorUserId],[LastUpdatedTime],[LastUpdatorUserId])
     VALUES
           (NEWID(),N'����'+CAST(@ct as nvarchar),N'BM'+CAST(@ct as nvarchar),N'ͳ��',N'����',N'����'+CAST(@ct as nvarchar),GETDATE(),'admin', GETDATE(),'admin')
set @ct=@ct+1
END
GO
