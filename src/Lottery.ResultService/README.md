# Lottery.ResultService

The Result Service is responsible for identifying lottery games which have reached their scheduled draw time, picking the winning numbers, and awarding prizes to players who's numbers have come up. 

> [!NOTE]
> Ideally this would be a serverless function running on a 
> schedule, however for simplicity of running locally (and 
> since I have no plan to deploy to a serverless 
> environment), it has been created as a console app and 
> needs to be run manually/set up as a scheduled task on the 
> host machine.

## Running Locally

Before the application can be run, some pre-requisite configuration needs to take place. 

### Secrets

The application needs to know the password for accessing the database. We'll store this as a user secret in the [Secrets Manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?#secret-manager).

Start by enabling user secrets for the `Lottery.ResultService` project:

```
dotnet user-secrets init --project Lottery.ResultService
```

Now that app secrets are enabled, you need to add the secrets. 

<ins>Connection Password</ins>

Before executing the following, replace `PASSWORD_HERE` with the password for accessing postgres. 
This is the password you specified as the `LOTTERY_RESULT_SRV_DB_USER_PASS` environment variable when following the [README in Lottery.DB](../Lottery.DB/README.md).

```
dotnet user-secrets set \
    "ConnectionStrings:Default:Password" \
    "PASSWORD_HERE" \
    --project Lottery.ResultService
```

## Running the service

You should now be able to run the service and result any games which are in a suitable state to do so. 

You can either run it using the dotnet cli, e.g.

```
cd src/Lottery.DB
dotnet run
```

Alternatively, if using VSCode, you can debug using the `Result Service (Debug)` configuration.