# Lottery.DB

This project houses the various components required for interacting with the database via Entity Framework.

> [!NOTE]
> While it's really intended as a class library for other projects to reference, it's actually 
> a console app. This allows the `dotnet ef` tool to be invoked without having to have a separate startup project, and as such, means that only this project needs to worry about 
> seeing data, which in turn means that only this project needs knowledge of service 
> account passwords, etc.


## Setting up

Before the DB migration can be deployed, we need to create some secrets that will be used in the code, e.g. passwords. You can do this by using the [Secrets Manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?#secret-manager).

Start by enabling user secrets for the `Lottery.DB` project:

```
dotnet user-secrets init --project Lottery.DB
```

Now that app secrets are enabled, you need to add the secrets. 

<ins>Connection Password</ins>

Before executing the following, replace `PASSWORD_HERE` with the password for accessing postgres. If you run a postgres docker image, as per the [main README](../../README.md), it's the password you used in the `docker run` command.

```
dotnet user-secrets set \
    "ConnectionStrings:Default:Password" \
    "PASSWORD_HERE" \
    --project Lottery.DB
```

<ins>System Admin Password</ins>

Replace `PASSWORD_HERE` with the password you want to use for the system admin account, then execute the command to add the secret.

```
dotnet user-secrets set \
    "MaintenanceDBContext:SystemAdminPassword" \
    "PASSWORD_HERE" \
    --project Lottery.DB
```

<ins>Game Admin Password</ins>

Replace `PASSWORD_HERE` with the password you want to use for the super user account, then execute the command to add the secret.

```
dotnet user-secrets set \
    "MaintenanceDBContext:GameAdminPassword" \
    "PASSWORD_HERE" \
    --project SceneIt.Api
```

## Deploying the database

Now that you have your database running and your secrets set up, you should be able to deploy the schema & seed data to the database. To do this, you can execute the `database update` command from the root directory:

> [!NOTE]
> We need to set some environment variables before executing the `dotnet ef` command.
> These variables are the passwords that the respective application will use when
> connecting to the database. Replace `PASSWORD_HERE` with whatever password you want
> to use for each of the applications to connect to the DB.

```
export LOTTERY_API_DB_USER_PASS="PASSWORD_HERE" \
    LOTTERY_RESULT_SRV_DB_USER_PASS="PASSWORD_HERE"; \
    dotnet ef database update \
        --project ./src/Lottery.DB \
        --context MaintenanceDBContext
```

> [!NOTE]
> Multiple DBContexts in this repository. As such, we need to specify *which* context should
> be used when invoking the `dotnet ef` tool. We do this by specifying the `--context` argument 
> along with the name of the DBContext, `MaintenanceDBContext`.

This may take a couple of seconds to complete. Once complete, you can check your postgres instance for a newly created database called `lottery` and browse the tables that have been created in it.