# The Register API

[![GitHub license](https://img.shields.io/github/license/igor-couto/register-api.svg)](https://github.com/igor-couto/register-api/blob/main/LICENCE)
![BuildAndDeployBadge](https://github.com/igor-couto/register-api/actions/workflows/pipeline.yml/badge.svg)

This is the Register API. What does it do? Well, it does registrations. Saves information in the database.

This project serves so that I can practice some concepts, without thinking too much about what I'm going to develop. By itself, it's not useful at all.
However, here I implement some important concepts:

- .NET 6.0
- Azure deployment and configuration
- CI/CD with github workflows and actions
- Authentication
- JWT Authorization with claims
- Refresh Token
- Password storage with salt and slow hashing
- Docker
- Migrations
- Conventional commits
- Semantic versioning
- And many other things that I can imagine and be willing to do

## Usage

Run postgres docker container:
```
docker run --name postgres -p 5432:5432 -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=admin -d --rm postgres:latest
```

Run dotnet project:
```
dotnet run --project RegisterAPI
````

## Author

- **Igor Couto** - [igor.fcouto@gmail.com](mailto:igor.fcouto@gmail.com)
