FROM mcr.microsoft.com/dotnet/sdk:10.0 as sdk
WORKDIR /src
COPY . /src

RUN dotnet publish -c Release -o api
RUN npm install
RUN npm run build

FROM ubuntu:24.04 as runtime
WORKDIR /app
COPY --from=sdk src/api /app
COPY --from=sdk src/spa /app/wwwroot
ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080
ENTRYPOINT ["Webhook"]