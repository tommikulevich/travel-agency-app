FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src/Shared
COPY ["../Shared", "."]
WORKDIR /src/HotelService
COPY ["HotelService", "."]

RUN dotnet build "HotelService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HotelService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelService.dll", "--server.urls", "http://+:80"]