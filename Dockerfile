# Etapa 1: build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas os arquivos de projeto para restaurar mais rápido
COPY ["GamePlatform.Api/GamePlatform.Api.csproj", "GamePlatform.Api/"]
COPY ["GamePlatform.Application/GamePlatform.Application.csproj", "GamePlatform.Application/"]
COPY ["GamePlatform.Domain/GamePlatform.Domain.csproj", "GamePlatform.Domain/"]
COPY ["GamePlatform.Infrastructure/GamePlatform.Infrastructure.csproj", "GamePlatform.Infrastructure/"]

# Restaurar os pacotes
RUN dotnet restore "GamePlatform.Api/GamePlatform.Api.csproj"

# Copiar tudo e compilar
COPY . .
WORKDIR "/src/GamePlatform.Api"
RUN dotnet publish "GamePlatform.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: imagem final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "GamePlatform.Api.dll"]