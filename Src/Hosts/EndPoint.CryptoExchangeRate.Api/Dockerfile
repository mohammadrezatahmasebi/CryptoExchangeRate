FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Src/Hosts/EndPoint.CryptoExchangeRate.Api/EndPoint.CryptoExchangeRate.Api.csproj", "Src/Hosts/EndPoint.CryptoExchangeRate.Api/"]
COPY ["Src/Core/Core.CryptoExchangeRate.Application/Core.CryptoExchangeRate.Application.csproj", "Src/Core/Core.CryptoExchangeRate.Application/"]
COPY ["Src/Core/Core.CryptoExchangeRate.Domain/Core.CryptoExchangeRate.Domain.csproj", "Src/Core/Core.CryptoExchangeRate.Domain/"]
COPY ["Src/Infra/Infra.ExchangeRatesApi/Infra.ExchangeRatesApi.csproj", "Src/Infra/Infra.ExchangeRatesApi/"]
COPY ["Src/Infra/Infra.CryptoExchangeRate.Db/Infra.CryptoExchangeRate.Db.csproj", "Src/Infra/Infra.CryptoExchangeRate.Db/"]
RUN dotnet restore "Src/Hosts/EndPoint.CryptoExchangeRate.Api/EndPoint.CryptoExchangeRate.Api.csproj"
COPY . .
WORKDIR "/src/Src/Hosts/EndPoint.CryptoExchangeRate.Api"
RUN dotnet build "EndPoint.CryptoExchangeRate.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EndPoint.CryptoExchangeRate.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EndPoint.CryptoExchangeRate.Api.dll"]
