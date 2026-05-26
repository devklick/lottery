#!/bin/bash
set -e

# We need to seed the migration user here, and give it the relevant permissions.
# When the migration service runs (on docker compose up)

psql -v ON_ERROR_STOP=1 \
     --username "$POSTGRES_USER" \
     --dbname "$POSTGRES_DB" <<EOF

-- =========================
-- USERS
-- =========================

CREATE USER "$MIGRATOR_DB_USER"
    WITH PASSWORD '$MIGRATOR_DB_PASSWORD';

-- Allow migrator to create/manage roles
ALTER ROLE "$MIGRATOR_DB_USER" CREATEROLE;

-- Optional: only needed if migrations create databases
-- ALTER ROLE "$MIGRATOR_DB_USER" CREATEDB;

-- =========================
-- DATABASE LEVEL PERMISSIONS
-- =========================

GRANT CONNECT ON DATABASE "$POSTGRES_DB"
TO "$MIGRATOR_DB_USER";

-- Needed for CREATE SCHEMA
GRANT CREATE ON DATABASE "$POSTGRES_DB"
TO "$MIGRATOR_DB_USER";

-- =========================
-- EXISTING SCHEMAS
-- =========================

GRANT USAGE, CREATE
ON SCHEMA public
TO "$MIGRATOR_DB_USER";

-- =========================
-- DEFAULT PRIVILEGES
-- =========================

ALTER DEFAULT PRIVILEGES
FOR ROLE "$MIGRATOR_DB_USER"
IN SCHEMA public
GRANT ALL ON TABLES
TO "$MIGRATOR_DB_USER";

ALTER DEFAULT PRIVILEGES
FOR ROLE "$MIGRATOR_DB_USER"
IN SCHEMA public
GRANT ALL ON SEQUENCES
TO "$MIGRATOR_DB_USER";

EOF