# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["PaymentGatewayAPI.csproj", "."]
RUN dotnet restore "PaymentGatewayAPI.csproj"

COPY . .
RUN dotnet build "PaymentGatewayAPI.csproj" -c Release -o /app/build
RUN dotnet publish "PaymentGatewayAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "PaymentGatewayAPI.dll"]
