#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TripService/TripService.csproj", "."]
RUN dotnet restore "./TripService.csproj"
COPY ["TripService", "."]
COPY ["../Shared", "."]
WORKDIR "/src/."
RUN dotnet build "TripService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TripService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TripService.dll", "--server.urls", "http://+:80"]