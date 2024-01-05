# How To Run Project & Details

``` cli
    dotnet run --project VbApi/Vb.Api
```

## Swagger document url

```url
    http://localhost:5169/swagger/index.html
```

## Connection String Method is Changed as

```cs
    var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(
        Configuration.GetConnectionString("MsSqlConnection")!)
    {
        // Db password comes from user-secrets 
        Password = Configuration["MsSQLDbPassword"]!
    };
    string connection = sqlConnectionStringBuilder.ConnectionString;
    services.AddDbContext<VbDbContext>(options => options.UseSqlServer(connection));
```

[Click here to go to see documentation](https://vbapi.netlify.app/)

### Note

UpdateRequestValidators were not added because they validate same request models with the create methods in schemas.
