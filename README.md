# IPhone Shop API
Change DbConfig. 

### Clone 
- Clone this repo to your local machine using `https://github.com/pavlozab/my-api`

### Migrations
- Create new migrations. From `Data` project folder   
```shell
$ dotnet ef --startup-project ../MyApi/ migrations add NewMigrations
```

- Update database. From `Data` project folder
```shell
$ dotnet ef database update --startup-project ../MyApi/
```
