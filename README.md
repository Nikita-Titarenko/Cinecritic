# Cinecritic
Web site on ASP.NET Core and Blazor for rating movies, noting watсhed or want to watch and creating reviews. Also, you can see your noted movies and reviews and managers can create movies.
## Requirements
- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- [Microsoft SQL Server](https://www.microsoft.com/uk-ua/sql-server/sql-server-downloads)
- Git
## Installation
- Clone repository
```bash
git clone https://github.com/Nikita-Titarenko/Cinecritic
```
- Change DefaultConnection in appsettings.json
- Update database
```bash
dotnet ef database update
```
## Using
- Run application
```bash
dotnet run
```
Website will be awailable on:
- http://localhost:5003
- https://localhost:7204