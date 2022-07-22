# syntax=docker/dockerfile:1
# source https://hub.docker.com/_/microsoft-dotnet-sdk
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore -r linux-x64

# Copy everything else and build
COPY . ./
RUN dotnet publish -c release -o out -r linux-x64 --self-contained false --no-restore

# Build runtime image
# source https://hub.docker.com/_/microsoft-dotnet-aspnet/
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "DorfInfoBot.API.dll"]
