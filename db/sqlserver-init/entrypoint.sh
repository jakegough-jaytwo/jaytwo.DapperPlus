#!/bin/bash

set -ex

/opt/mssql/bin/sqlservr &

# Wait for SQL Server to start
sqlcmd_success=0

for ((i = 1; i <=30; i++))
do
  echo "Waiting for SQL Server to start..."
  sleep 2
  sqlcmd -S localhost -C -U SA -P "$MSSQL_SA_PASSWORD" -l 1 -Q 'SELECT 1' && sqlcmd_success=1 || sqlcmd_success=0
  sqlcmd_exit_code=$?
  if [ ${sqlcmd_success} -eq 1 ]; then
    break
  fi
done

echo "SQL Server ready..."

# Run each SQL file
for script in /docker-entrypoint-initdb.d/*.sql
do
  echo "Running $script"
  sqlcmd -S localhost -C -U SA -P "$MSSQL_SA_PASSWORD" -d master -b -i "$script"
done

# Keep container running
wait
