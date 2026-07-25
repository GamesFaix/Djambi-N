# Dev setup instructions

## Tools

* Install MariaDB
    * Default settings
    * Root password `badPassword`

* Install .NET 3.1 SDK

* Install F# tools

* Install NVM and Node 26

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

## Generate API client

From `./api/api.host`
```
dotnet swagger tofile --output $(OutDir)openapi.json $(OutDir)api.host.dll v1
```

From `./rest-client-generator`
```
.\generate-api-client -sourcePath ../api/api.host/bin/release/netcoreapp3.1/openapi.json -outDir ../web2/src/api-client
```

## Web2 client

The /web2 folder contains an incomplete client re-write. 

Key differences
* Only menus and navigation are implemented, not gameplay. Although some code for rendering the board exists.
* Consolidated tooling using `create-react-app`.
* Responsive mobile-friendly UI using MaterialUI
* Generic grayscale styling

From `./web2`
```
npm i
npm run start
```