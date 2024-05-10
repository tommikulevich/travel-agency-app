# Użyj oficjalnego obrazu .NET Core SDK jako bazowego obrazu
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Skopiuj pliki projektu i zbuduj aplikację
COPY . .
RUN dotnet publish -c Release -o out

# Użyj lżejszego obrazu .NET Core Runtime jako bazowego obrazu do uruchomienia aplikacji
FROM mcr.microsoft.com/dotnet/runtime:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "PaymentService.dll"]
