-- Active: 1708913742501@@127.0.0.1@1433@datingapp@dbo
CREATE DATABASE Android;
USE [Android];
CREATE LOGIN [Android] WITH PASSWORD = '12345678', DEFAULT_DATABASE = [Android], CHECK_POLICY = OFF;
CREATE USER [android] FOR LOGIN [android];
EXEC sp_addrolemember 'db_owner', 'android';
-- Add quyền db_creator để tạo database
EXEC sp_addsrvrolemember 'android', 'dbcreator';
GRANT CREATE TABLE, CREATE PROCEDURE, CREATE FUNCTION TO [android];

GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO [android];