# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore
COPY ["PaymentGatewayAPI.csproj", "./"]
RUN dotnet restore "PaymentGatewayAPI.csproj"

# Copy rest of the files and build
COPY . ./
RUN dotnet build "PaymentGatewayAPI.csproj" -c %BUILD_CONFIGURATION% -o C:\app\build

# Publish project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PaymentGatewayAPI.csproj" -c %BUILD_CONFIGURATION% -o C:\app\publish /p:UseAppHost=false

# Final image for running the app
FROM base AS final
WORKDIR /app
COPY --from=publish C:/app/publish .
ENTRYPOINT ["dotnet", "PaymentGatewayAPI.dll"]
