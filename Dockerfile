## Multi-stage Dockerfile for ASP.NET Core (net10.0)
## Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Install .NET 10.0 SDK (preview/RC)
RUN curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 10.0 --install-dir /usr/share/dotnet
ENV PATH="${PATH}:/usr/share/dotnet"

# Copy project files
COPY *.csproj ./
COPY *.sln ./

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source
COPY . ./

# Publish to /app/out
RUN dotnet publish -c Release -o /app/out --no-restore

## Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install .NET 10.0 ASP.NET Core Runtime (preview/RC)
RUN curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 10.0 --runtime aspnetcore --install-dir /usr/share/dotnet
ENV PATH="${PATH}:/usr/share/dotnet"

# Copy published output
COPY --from=build /app/out ./

# Render provides PORT env var
ENV ASPNETCORE_URLS=http://+:${PORT:-10000}

EXPOSE 10000

# Start the app
ENTRYPOINT ["dotnet", "ShopWeb.dll"]
