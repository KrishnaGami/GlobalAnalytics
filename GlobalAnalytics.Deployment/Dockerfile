# Use official .NET SDK image for build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY GlobalAnalytics.API/GlobalAnalytics.API.csproj ./GlobalAnalytics.API/
COPY GlobalAnalytics.Lib/GlobalAnalytics.Lib.csproj ./GlobalAnalytics.Lib/
COPY GlobalAnalytics.Data/GlobalAnalytics.Data.csproj ./GlobalAnalytics.Data/
COPY GlobalAnalytics.Core/GlobalAnalytics.Core.csproj ./GlobalAnalytics.Core/

RUN dotnet restore ./GlobalAnalytics.API/GlobalAnalytics.API.csproj

# Copy everything and build
COPY . .
WORKDIR /src/GlobalAnalytics.API
RUN dotnet publish GlobalAnalytics.API.csproj -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "GlobalAnalytics.API.dll"]
