rm -rf ./src/Infrastructure/Persistence/Migrations
psql -c "drop database MaragogiAI"   
dotnet ef migrations add "InitalMigration" -s src/Infrastructure -o Persistence/Migrations
dotnet ef database update -s src/Infrastructure