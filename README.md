## Lot Tracking System

**Overview**
A web application designed to simulate the operations a wafer processing plant. Users can register, login, and generate batches of "wafers" with a name, quantity, and id value. Wafers are stored in a FIFO queue where they can enter the system and process through 10 plant locations. A log is kept of all actions in the system and displayed to the user. 

**Technologies used**
Backend
- ASP.NET Core (C#)
- Entity Framework Core
- SQL Server
- AutoMapper
- JWT Authentication

Frontend
- Angular (HTML/CSS/Typescript)
- Angular CLI
- Bootstrap

**Setup**
In the terminal, navigate into:

~LotTrackingSystem/API (replace ~ with the correct file path)
enter "dotnet ef database update" (this will create the database)

~LotTrackingSystem/API (replace ~ with the correct file path)
enter "dotnet run"

~LotTrackingSystem/client (replace ~ with the correct file path)
enter "ng serve"

If the database creation has not worked, you can use the "SQLQuery.sql" in the LotTrackingSystem, with a SQL editor
and follow the instructions there. This is to create the database locally through SQL.

After the above steps have been completed, open the website at: https://localhost:4200/
