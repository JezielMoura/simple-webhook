FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS dotnet-build
WORKDIR /dotnet-build
COPY . /dotnet-build
RUN dotnet publish -c Release -o api src/Infrastructure

FROM node:22-alpine AS node-build
WORKDIR /node-build
COPY . /node-build
RUN npm install --prefix ./src/Presentation
RUN npm run build --prefix ./src/Presentation

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS dotnet-runtime
WORKDIR /app
COPY --from=dotnet-build dotnet-build/api /app
COPY --from=node-build node-build/src/Presentation/build/client /app/wwwroot
ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "SimpleWebhook.Infrastructure.dll"]