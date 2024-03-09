# Lottery

> [!WARNING]
> Project status: WIP
> 
A basic lottery game where users can pick numbers, the results are drawn at random, and the users could win a prize if their numbers come up. 

> [!NOTE]
> This is not an actual game where players can submit genuine monetary bets and win actual prizes, and there is no plan to make it this way. It's intended purely for education and fun.

## Running Locally

### Setting up the database

The application uses a PostgreSQL database for the data store. As such, you'll need to have a Postgres instance accessible from the machine you're running the application on. The quickest and easiest way to get up and running is to run Postgres in a Docker container. 

Before running the following, replace `PASSWORD_HERE` with the password you want to use to access the postgres instance. If you want, you can also change `postgres-local` with another value - this is the name you are giving to the container container.

```
docker run \
    --name postgres-local \
    -p 5432:5432 \
    -e POSTGRES_PASSWORD=PASSWORD_HERE \
    -d \
    post
```

> [!NOTE]
> The password specified here will be the main password that will be used for maintenance tasks such as creating and altering the database, but will not be used by the various applications that need to access the data.

Now that the database exists, the migration needs to be applied. See the [README in Lottery.DB](./src/Lottery.DB/README.md) for more on this.