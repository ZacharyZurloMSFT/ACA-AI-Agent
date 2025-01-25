CREATE USER "id-zurlocloud-running" FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER "id-zurlocloud-running";
ALTER ROLE db_datawriter ADD MEMBER "id-zurlocloud-running";