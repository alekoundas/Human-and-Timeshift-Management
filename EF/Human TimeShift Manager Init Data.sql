

INSERT INTO [dbo].[LeaveTypes]([CreatedOn]  ,[Name]  ,[Description],[IsActive]) VALUES  ('2020-09-10','Anarwtikh','Anarwtikh adeia xarti ap giatro',1)


INSERT INTO [dbo].[Specializations]([CreatedOn]  ,[Name] ,[Description]   ,[PayPerHour],[IsActive])  VALUES ('2020-09-10','Security','Perigrafi',0,1)
INSERT INTO [dbo].[Specializations]([CreatedOn]  ,[Name] ,[Description]   ,[PayPerHour],[IsActive])  VALUES ('2020-09-10','Service','Perigrafi',0,1)
INSERT INTO [dbo].[Specializations]([CreatedOn]  ,[Name] ,[Description]   ,[PayPerHour],[IsActive])  VALUES ('2020-09-10','Cleaning','Perigrafi',0,1)

INSERT INTO [dbo].[Companies]  ([CreatedOn],[Afm] ,[Description],[Title],[IsActive]) VALUES('2020-09-10','1111111111','H etairia 1','Company 1',1)
INSERT INTO [dbo].[Companies]  ([CreatedOn],[Afm] ,[Description],[Title],[IsActive]) VALUES('2020-09-10','2222222222','H etairia 2','Company 2',1)

INSERT INTO [dbo].[Customers]  ([CreatedOn] ,[Afm]  ,[Profession]  ,[Address] ,[PostalCode] ,[DOY],[Description],[CompanyId] ,[ΙdentifyingΝame],[IsActive])VALUES('2020-09-10','1111111111','Doctor','L.Eleytherias 39','16345','Athinas','Eimai h perigrafi toy pelath',1,'Customer 1',1)
INSERT INTO [dbo].[Customers]  ([CreatedOn] ,[Afm]  ,[Profession]  ,[Address] ,[PostalCode] ,[DOY],[Description],[CompanyId] ,[ΙdentifyingΝame],[IsActive])VALUES('2020-09-10','2222222222','Doctor','L.Eleytherias 39','16345','Athinas','Eimai h perigrafi toy pelath',2,'Customer 2',1)



INSERT INTO [dbo].[WorkPlaces]([CreatedOn] ,[Title] ,[Description]   ,[CustomerId],[IsActive])  VALUES('2020-09-10','Posto 1','Eimai to posto 1',1,1)
INSERT INTO [dbo].[WorkPlaces]([CreatedOn] ,[Title] ,[Description]   ,[CustomerId],[IsActive])  VALUES('2020-09-10','Posto 2','Eimai to posto 2',1,1)
INSERT INTO [dbo].[WorkPlaces]([CreatedOn] ,[Title] ,[Description]   ,[CustomerId],[IsActive])  VALUES('2020-09-10','Posto 3','Eimai to posto 3',2,1)
INSERT INTO [dbo].[WorkPlaces]([CreatedOn] ,[Title] ,[Description]   ,[CustomerId],[IsActive])  VALUES('2020-09-10','Posto 4','Eimai to posto 4',2,1)


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1001','Employee1','employee1',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111112','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1002','Employee2','employee2',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','1111111113','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1003','Employee3','employee3',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111114','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1004','Employee4','employee4',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111115','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1005','Employee5','employee5',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111116','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1006','Employee6','employee6',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111117','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1007','Employee7','employee7',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111118','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1008','Employee8','employee8',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111119','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1009','Employee9','employee9',1)


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1010','Employee10','employee10',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','21111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1011','Employee11','employee11',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','31111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1012','Employee12','employee12',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','41111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1013','Employee13','employee13',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','51111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1014','Employee14','employee14',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','61111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1015','Employee15','employee15',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','71111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1016','Employee16','employee16',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','81111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1017','Employee17','employee17',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','91111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1018','Employee18','employee18',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','01111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1019','Employee19','employee19',1)


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','12111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1020','Employee20','employee20',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','221111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1021','Employee21','employee21',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','331111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1022','Employee22','employee22',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','441111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1023','Employee23','employee23',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','551111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1024','Employee24','employee24',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','661111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1025','Employee25','employee25',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','771111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1026','Employee26','employee26',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','881111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1027','Employee27','employee27',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','991111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1028','Employee28','employee28',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','001111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,1,'1029','Employee29','employee29',1)


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','13111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1020','Employee20','employee20',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','2221111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1021','Employee21','employee21',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','3331111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1022','Employee22','employee22',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','4441111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1023','Employee23','employee23',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','5551111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1024','Employee24','employee24',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','6661111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1025','Employee25','employee25',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','7771111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1026','Employee26','employee26',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','8881111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1027','Employee27','employee27',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','9991111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1028','Employee28','employee28',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','0001111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1029','Employee29','employee29',1)


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','14111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1030','Employee30','employee30',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','22221111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1031','Employee31','employee31',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','33331111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1032','Employee32','employee32',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','44441111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1033','Employee33','employee33',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','55551111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1034','Employee34','employee34',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','66661111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1035','Employee35','employee35',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','77771111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1036','Employee36','employee36',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','88881111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1037','Employee37','employee37',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','99991111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1038','Employee38','employee38',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','00001111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1039','Employee39','employee39',1)


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','15111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1040','Employee40','employee40',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111122','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1041','Employee41','employee41',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111133','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1042','Employee42','employee42',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111144','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1043','Employee43','employee43',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111155','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1044','Employee44','employee44',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111166','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1045','Employee45','employee45',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111177','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1046','Employee46','employee46',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111188','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1047','Employee47','employee47',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111199','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1048','Employee48','employee48',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111100','1111111111','Employee@gmail.gr','L.Eleytherias 39',2,1,'1049','Employee49','employee49',1)


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','16111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1050','Employee50','employee50',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','1111111111222','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1051','Employee51','employee51',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','1111111111333','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1052','Employee52','employee52',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','1111111111444','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1053','Employee53','employee53',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','1111111111555','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1054','Employee54','employee54',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','1111111111666','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1055','Employee55','employee55',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','1111111111777','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1056','Employee56','employee56',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','1111111111888','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1057','Employee57','employee57',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','1111111111999','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1058','Employee58','employee58',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','1111111111000','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1059','Employee59','employee59',1)


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','17111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1060','Employee60','employee60',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111112222','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1061','Employee61','employee61',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111113333','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1062','Employee62','employee62',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111114444','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1063','Employee63','employee63',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111115555','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1064','Employee64','employee64',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111116666','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1065','Employee65','employee65',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111117777','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1066','Employee66','employee66',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111118888','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1067','Employee67','employee67',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111119999','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1068','Employee68','employee68',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111111110000','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1069','Employee69','employee69',1)


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','18111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1070','Employee70','employee70',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111122222','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1071','Employee71','employee71',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111133333','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1072','Employee72','employee72',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111144444','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1073','Employee73','employee73',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111155555','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1074','Employee74','employee74',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111166666','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1075','Employee75','employee75',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111177777','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1076','Employee76','employee76',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111188888','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1077','Employee77','employee77',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111199999','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1078','Employee78','employee78',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111111100000','1111111111','Employee@gmail.gr','L.Eleytherias 39',3,2,'1079','Employee79','employee79',1)


INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','19111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1080','Employee80','employee80',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','222221111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1081','Employee81','employee81',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','333331111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1082','Employee82','employee82',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','444441111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1083','Employee83','employee83',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','555551111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1084','Employee84','employee84',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','666661111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1085','Employee85','employee85',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','777771111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1086','Employee86','employee86',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','888881111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1087','Employee87','employee87',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','999991111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1088','Employee88','employee88',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','000001111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1089','Employee89','employee89',1)



INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','10111111111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1090','Employee90','employee90',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111112211111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1091','Employee91','employee91',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111113311111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1092','Employee92','employee92',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','11111444411111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1093','Employee93','employee93',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111115511111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1094','Employee94','employee94',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111661111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1095','Employee95','employee95',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111771111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1096','Employee96','employee96',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111881111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1097','Employee97','employee97',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111991111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1098','Employee98','employee98',1)
INSERT INTO [dbo].[Employees]([CreatedOn],[DateOfBirth]  ,[Afm],[SocialSecurityNumber] ,[Email]  ,[Address],[SpecializationId]  ,[CompanyId]  ,[ErpCode],[FirstName],[LastName],[IsActive])VALUES
('2020-09-10','2020-09-10','111111001111','1111111111','Employee@gmail.gr','L.Eleytherias 39',1,2,'1099','Employee99','employee99',1)




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