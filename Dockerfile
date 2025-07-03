# Etapa 1: build da aplicação
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /src

# Copia apenas os arquivos de projeto para restaurar mais rápido
COPY ["GamePlatform.Api/GamePlatform.Api.csproj", "GamePlatform.Api/"]
COPY ["GamePlatform.Application/GamePlatform.Application.csproj", "GamePlatform.Application/"]
COPY ["GamePlatform.Domain/GamePlatform.Domain.csproj", "GamePlatform.Domain/"]
COPY ["GamePlatform.Infrastructure/GamePlatform.Infrastructure.csproj", "GamePlatform.Infrastructure/"]

# Restaurar os pacotes
RUN dotnet restore -a $TARGETARCH "GamePlatform.Api/GamePlatform.Api.csproj"

# Copiar tudo e compilar
COPY . .
WORKDIR "/src/GamePlatform.Api"
RUN dotnet publish -a $TARGETARCH "GamePlatform.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: imagem final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Variáveis para Datadog APM + Logs
ENV DD_SERVICE=gameplatform-api
ENV DD_ENV=production
ENV DD_VERSION=1.0.0
ENV DD_TRACE_ENABLED=true
ENV DD_LOGS_INJECTION=true
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "GamePlatform.Api.dll"]