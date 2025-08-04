# === BASE STAGE ===
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /src
# Copy project files and restore dependencies
COPY ["DMT.Api/DMT.Api.csproj", "DMT.Api/"]
RUN dotnet restore "DMT.Api/DMT.Api.csproj"
COPY . .
WORKDIR "/src/DMT.Api"

# === DEVELOPMENT STAGE ===
FROM base AS development
# Install development tools
RUN dotnet tool update --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
# Expose ports for development
EXPOSE 9090
EXPOSE 5001
# Set environment
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:9090
# Use dotnet watch for hot reload
ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:9090"]

# === BUILD STAGE ===
FROM base AS build
RUN dotnet build "DMT.Api.csproj" -c Release -o /app/build

# === PUBLISH STAGE ===
FROM build AS publish
RUN dotnet publish "DMT.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# === PRODUCTION STAGE ===
FROM mcr.microsoft.com/dotnet/aspnet:9.0-chiseled AS production
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 9090
ENV ASPNETCORE_URLS=http://+:9090
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "DMT.Api.dll"]

# === DEFAULT TARGET ===
FROM production AS final
