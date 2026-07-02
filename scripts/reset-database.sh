rm -rf ./src/Infrastructure/Persistence/Migrations
dotnet ef migrations add "InitalMigration" -s src/Infrastructure -o Persistence/Migrations
dotnet ef database update -s src/Infrastructure