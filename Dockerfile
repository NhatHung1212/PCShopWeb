## Multi-stage Dockerfile for ASP.NET Core (net10.0)
## Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ShopWeb.sln ./
COPY ShopWeb.csproj ./

# Restore dependencies
RUN dotnet restore ShopWeb.csproj

# Copy the rest of the source
COPY . ./

# Publish to /app/out
RUN dotnet publish ShopWeb.csproj -c Release -o /app/out

## Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy published output
COPY --from=build /app/out ./

# Environment and port configuration
ENV ASPNETCORE_ENVIRONMENT=Production
# Render provides PORT env var; default to 10000 if not set
ENV PORT=10000
ENV ASPNETCORE_URLS="http://0.0.0.0:${PORT}"

EXPOSE 10000

# Start the app
ENTRYPOINT ["dotnet", "ShopWeb.dll"]
