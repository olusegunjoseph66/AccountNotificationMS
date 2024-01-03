#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0.22 AS base

RUN useradd app -u 10001 --create-home --user-group
USER 10001

WORKDIR /app
ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Notifications.API/Notifications.API.csproj", "Notifications.API/"]
RUN dotnet restore "Notifications.API/Notifications.API.csproj"
COPY . .
WORKDIR "/src/Notifications.API"
RUN dotnet build "Notifications.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notifications.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notifications.API.dll"]
