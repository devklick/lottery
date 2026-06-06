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
LOTTERY_POSTGRES_DB=lottery # The name of the DB
LOTTERY_POSTGRES_PASSWORD=<add> # The master password for the DB

# The ports that the UI and API will run on
LOTTERY_UI_PORT=3000
LOTTERY_API_PORT=5000

# A DB user will be created for the API that results the games.
# These env vars are the credentials for this user
LOTTERY_API_DB_USER=Lottery.Api.Service
LOTTERY_API_DB_PASSWORD=<add>

# A DB user will be created for the service that results the games.
# These env vars are the credentials for this user
LOTTERY_RESULTS_DB_USER=Lottery.Result.Service
LOTTERY_RESULTS_DB_PASSWORD=<add>

# A DB user will be created for the migrations to run under.
# These env vars are the credentials for this user
LOTTERY_MIGRATOR_DB_USER=Lottery.Migration.Service
LOTTERY_MIGRATOR_DB_PASSWORD=<add>

# Two app/site users will be created; GameAdmin and SystemAdmin.
# These env vars are the passwords for these users.
LOTTERY_SYSTEM_ADMIN_PASSWORD=<add>
LOTTERY_GAME_ADMIN_PASSWORD=<add>
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
- Password: Defined in your `LOTTERY_GAME_ADMIN_PASSWORD` env var

#### System Admin
- Username: `SystemAdmin`
- Password: Defined in your `LOTTERY_SYSTEM_ADMIN_PASSWORD` env var


### Debugging

Running the code in docker is great, but it's often useful to be able to attach 
a debugger and step through the code. 

To do so, start by spinning up the service(s) in docker that you _dont_ need to debug, 
e.g. you probably at least want the DB running in docker (and the migration).

```
docker compose up db migrate
```

Then you can fire up the API either in a new terminal:
```
dotnet run --project src/Lottery.Api
```

Or run the [`API (Debug)` VSCode launch config](.vscode/launch.json)

And you can run the UI via the terminal:
```
cd src/Lottery.IO && npm run dev
```

At this point, you'll have the UI hosted using vite and accessible on http://localhost:3000 
(or whatever `LOTTERY_UI_PORT` you used in your .env file), and the API hosted on http://localhost:5000 
(or whatever port you used in your .env file).