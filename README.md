# Superset Service

SupersetService is a worker service that listens to a folder for zip files that contain **superset** CSV files, processes its data, and sends it to the database.

### Prerequisites
- SQL Server 2019
- .NET Core 6 SDK

### Build & Publish
- cd Into SupersetService
- Run `dotnet restore` and then `dotnet build `
- To publish, run `dotnet publish -c Release`.

### Run as a Windows Service
- In powershell, run command: `sc.exe create <ServiceName> -binpath="<FullPublishPath>"`.
