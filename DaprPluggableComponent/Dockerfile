#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DaprPluggableComponent.csproj", ""]
RUN dotnet restore "DaprPluggableComponent.csproj"
COPY . .
RUN dotnet build "DaprPluggableComponent.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaprPluggableComponent.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaprPluggableComponent.dll"]