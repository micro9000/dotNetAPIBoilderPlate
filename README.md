# .NET 5.0 REST API boilerplate with Dapper for Database and JWT bearer authentication and role based authorization

## Requirements:
- Visual Studio 2019 Version 16.8.1
- .NET 5.0 SDK
- MySQL or MSSQL Server or PostgreSQL

## Dapper for Database connection
- I am using Dapper and Dapper.Contrib for database access. 
- You can use MySQL, MSSQL Server and PostgreSQL on single application, you just need to specify the database server adapter
- Dapper.Contrib code: https://github.com/StackExchange/Dapper/blob/main/Dapper.Contrib/SqlMapperExtensions.cs
- https://github.com/MiniProfiler/dotnet/issues/290
- https://stackoverflow.com/questions/50581540/dapper-contrib-and-miniprofiler-for-mysql-integration-issues?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
