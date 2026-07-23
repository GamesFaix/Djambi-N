# Dev setup instructions

## Tools

* Install MariaDB
    * Default settings
    * Root password `badPassword`

* Install .NET 3.1 SDK

* Install F# tools

* Install NVM and Node 10

## Build and test backend

From `./api`
```
$env:DJAMBI_Sql__UseSqliteForTesting = 'true'

dotnet tool restore
dotnet restore
dotnet build --no-restore 
dotnet test --no-build -v normal

Remove-Item Env:\DJAMBI_Sql__UseSqliteForTesting
```

## Setup database

From `./api/api.db.model`
```
dotnet tool run dotnet-ef database update
```
Check that the Djambi database has been created.

## Start API

From `./api/api.host`
```
dotnet run
```

Visit http://localhost:5100/swagger for API docs

Try an endpoint to see if the API can connect to the DB.

## Build and test frontend

From `./web`

```
npm install
npm run build
npm test
```

## Run frontend

```
npm run start
```

Visit http://localhost:8080/index.html to confirm client is working.

Check browser dev tools Network tab to confirm if client can connect to server.

