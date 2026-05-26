# Lottery

> [!WARNING]
> Project status: WIP
>
> A basic lottery game where users can pick numbers, the results are drawn at random, and the users could win a prize if their numbers come up.

> [!NOTE]
> This is not an actual game where players can submit genuine monetary bets and win actual prizes, and there is no plan to make it this way. It's intended purely for education and fun.

## Running Locally

### Docker Compose

The easiest way to run the application is with docker compose. But before doing so, 
you'll need a `.env` file in the root of the project:
```sh
POSTGRES_DB=lottery # The name of the DB
POSTGRES_PASSWORD=<add> # The master password for the DB

# A DB user will be created for the API that results the games.
# These env vars are the credentials for this user
API_DB_USER=lottery_api_service
API_DB_PASSWORD=<add>

# A DB user will be created for the service that results the games.
# These env vars are the credentials for this user
RESULTS_DB_USER=lottery_results_service
RESULTS_DB_PASSWORD=<add>

# A DB user will be created for the migrations to run under.
# These env vars are the credentials for this user
MIGRATOR_DB_USER=lottery_migration_service
MIGRATOR_DB_PASSWORD=<add>

# Two app/site users will be created; GameAdmin and SystemAdmin.
# These env vars are the passwords for these users.
SYSTEM_ADMIN_PASSWORD=<add>
GAME_ADMIN_PASSWORD=<add>
```

One your env vars are defined, you can now run:
```
docker compose up
```

The UI can be accessed from [http://localhost:3000](http://localhost:3000) and 
the API can be accessed from [http://localhost:5000](http://localhost:5000). 
You can log into the UI as either the Game Admin or System Admin:

#### Game Admin
- Username: `GameAdmin`
- Password: Defined in your `GAME_ADMIN_PASSWORD` env var

#### System Admin
- Username: `SystemAdmin`
- Password: Defined in your `SYSTEM_ADMIN_PASSWORD` env var


### Debugging

#### API

Running the code in docker is great, but it's often useful to be able to attach 
a debugger and step through the code. To do so, start by initializing user secrets 
for the API project:
```
dotnet user-secrets init --project Lottery.Api
```
Then add the username and password that the API will connect with - the ones you 
specified in your `.env` file:
```
dotnet user-secrets set \
    "ConnectionStrings:Default:User" \
    "USER" \
    --project Lottery.Api

dotnet user-secrets set \
    "ConnectionStrings:Default:Password" \
    "PASSWORD" \
    --project Lottery.Api
```

At run time, the API will take the `ConnectionStrings:Default` connection string
from ['appsettings.json']('./src/Lottery.Api/appsettings.json) and append your 
credentials to it.