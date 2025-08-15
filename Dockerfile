FROM mcr.microsoft.com/dotnet/sdk:10.0 as sdk
WORKDIR /src
COPY . /src

RUN dotnet publish -c Release -o api
RUN npm install
RUN npm run build

FROM mcr.microsoft.com/dotnet/aspnet:10.0 as runtime
WORKDIR /app
COPY --from=sdk src/api /app
ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Webhook.dll"]