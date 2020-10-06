

INSERT INTO [dbo].[LeaveTypes]([CreatedOn]  ,[Name]  ,[Description]) VALUES  ('2020-09-10','Anarwtikh','Anarwtikh adeia xarti ap giatro')


INSERT INTO [dbo].[Specializations]([CreatedOn]  ,[Name] ,[Description]   ,[PayPerHour])  VALUES ('2020-09-10','Security','Perigrafi',0)
INSERT INTO [dbo].[Specializations]([CreatedOn]  ,[Name] ,[Description]   ,[PayPerHour])  VALUES ('2020-09-10','Service','Perigrafi',0)
INSERT INTO [dbo].[Specializations]([CreatedOn]  ,[Name] ,[Description]   ,[PayPerHour])  VALUES ('2020-09-10','Cleaning','Perigrafi',0)

INSERT INTO [dbo].[Companies]  ([CreatedOn],[Afm] ,[Description],[Title]) VALUES('2020-09-10','1111111111','H etairia 1','Company 1')
INSERT INTO [dbo].[Companies]  ([CreatedOn],[Afm] ,[Description],[Title]) VALUES('2020-09-10','2222222222','H etairia 2','Company 2')

INSERT INTO [dbo].[Customers]  ([CreatedOn] ,[Afm]  ,[Profession]  ,[Address] ,[PostalCode] ,[DOY],[Description],[CompanyId] ,[ΙdentifyingΝame])VALUES('2020-09-10','1111111111','Doctor','L.Eleytherias 39','16345','Athinas','Eimai h perigrafi toy pelath',1,'Customer 1')
INSERT INTO [dbo].[Customers]  ([CreatedOn] ,[Afm]  ,[Profession]  ,[Address] ,[PostalCode] ,[DOY],[Description],[CompanyId] ,[ΙdentifyingΝame])VALUES('2020-09-10','1111111111','Doctor','L.Eleytherias 39','16345','Athinas','Eimai h perigrafi toy pelath',2,'Customer 2')



INSERT INTO [dbo].[WorkPlaces]([CreatedOn] ,[Title] ,[Description]   ,[CustomerId])  VALUES('2020-09-10','Posto 1','Eimai to posto 1',1)
INSERT INTO [dbo].[WorkPlaces]([CreatedOn] ,[Title] ,[Description]   ,[CustomerId])  VALUES('2020-09-10','Posto 2','Eimai to posto 2',1)
INSERT INTO [dbo].[WorkPlaces]([CreatedOn] ,[Title] ,[Description]   ,[CustomerId])  VALUES('2020-09-10','Posto 3','Eimai to posto 3',2)
INSERT INTO [dbo].[WorkPlaces]([CreatedOn] ,[Title] ,[Description]   ,[CustomerId])  VALUES('2020-09-10','Posto 4','Eimai to posto 4',2)


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1001','Employee1','employee1')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1002','Employee2','employee2')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1003','Employee3','employee3')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1004','Employee4','employee4')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1005','Employee5','employee5')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1006','Employee6','employee6')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1007','Employee7','employee7')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1008','Employee8','employee8')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1009','Employee9','employee9')


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1010','Employee10','employee10')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1011','Employee11','employee11')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1012','Employee12','employee12')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1013','Employee13','employee13')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1014','Employee14','employee14')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1015','Employee15','employee15')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1016','Employee16','employee16')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1017','Employee17','employee17')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1018','Employee18','employee18')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1019','Employee19','employee19')


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1020','Employee20','employee20')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1021','Employee21','employee21')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1022','Employee22','employee22')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1023','Employee23','employee23')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1024','Employee24','employee24')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1025','Employee25','employee25')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1026','Employee26','employee26')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1027','Employee27','employee27')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1028','Employee28','employee28')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1029','Employee29','employee29')


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1020','Employee20','employee20')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1021','Employee21','employee21')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1022','Employee22','employee22')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1023','Employee23','employee23')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1024','Employee24','employee24')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1025','Employee25','employee25')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1026','Employee26','employee26')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1027','Employee27','employee27')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1028','Employee28','employee28')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1029','Employee29','employee29')


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1030','Employee30','employee30')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1031','Employee31','employee31')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1032','Employee32','employee32')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1033','Employee33','employee33')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1034','Employee34','employee34')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1035','Employee35','employee35')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1036','Employee36','employee36')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1037','Employee37','employee37')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1038','Employee38','employee38')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1039','Employee39','employee39')


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1040','Employee40','employee40')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1041','Employee41','employee41')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1042','Employee42','employee42')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1043','Employee43','employee43')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1044','Employee44','employee44')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1045','Employee45','employee45')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1046','Employee46','employee46')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1047','Employee47','employee47')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1048','Employee48','employee48')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1049','Employee49','employee49')


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1050','Employee50','employee50')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1051','Employee51','employee51')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1052','Employee52','employee52')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1053','Employee53','employee53')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1054','Employee54','employee54')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1055','Employee55','employee55')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1056','Employee56','employee56')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1057','Employee57','employee57')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1058','Employee58','employee58')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1059','Employee59','employee59')


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1060','Employee60','employee60')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1061','Employee61','employee61')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1062','Employee62','employee62')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1063','Employee63','employee63')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1064','Employee64','employee64')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1065','Employee65','employee65')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1066','Employee66','employee66')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1067','Employee67','employee67')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1068','Employee68','employee68')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1069','Employee69','employee69')


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1070','Employee70','employee70')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1071','Employee71','employee71')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1072','Employee72','employee72')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1073','Employee73','employee73')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1074','Employee74','employee74')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1075','Employee75','employee75')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1076','Employee76','employee76')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1077','Employee77','employee77')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1078','Employee78','employee78')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1079','Employee79','employee79')


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1080','Employee80','employee80')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1081','Employee81','employee81')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1082','Employee82','employee82')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1083','Employee83','employee83')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1084','Employee84','employee84')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1085','Employee85','employee85')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1086','Employee86','employee86')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1087','Employee87','employee87')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1088','Employee88','employee88')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1089','Employee89','employee89')



INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1090','Employee90','employee90')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1091','Employee91','employee91')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1092','Employee92','employee92')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1093','Employee93','employee93')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1094','Employee94','employee94')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1095','Employee95','employee95')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1096','Employee96','employee96')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1097','Employee97','employee97')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1098','Employee98','employee98')
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName])VALUES
('2020-09-10','2020-09-10','1111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1099','Employee99','employee99')




INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,1)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,2)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,3)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,4)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,5)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,6)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,7)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,8)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,9)

INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,10)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,11)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,12)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,13)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,14)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,15)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,16)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,17)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,18)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,19)

INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,20)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,21)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,22)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,23)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',1,24)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,25)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,26)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,27)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,28)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,29)

INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,30)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,31)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,32)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,33)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,34)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,35)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,36)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,37)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,38)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,39)

INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,40)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,41)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,42)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,43)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,44)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,45)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,46)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,47)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,48)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',2,49)

INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,50)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,51)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,52)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,53)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,54)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,55)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,56)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,57)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,58)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,59)

INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,60)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,61)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,62)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,63)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,64)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,65)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,66)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,67)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,68)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,69)

INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,70)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,71)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,72)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,73)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',3,74)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,75)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,76)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,77)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,78)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,79)

INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,80)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,81)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,82)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,83)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,84)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,85)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,86)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,87)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,88)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,89)

INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,90)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,91)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,92)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,93)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,94)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,95)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,96)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,97)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,98)
INSERT INTO [dbo].[EmployeeWorkPlaces] ( [CreatedOn]  ,[WorkPlaceId],[EmployeeId] )VALUES('2020-09-10',4,99)