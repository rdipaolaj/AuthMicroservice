# Etapa base para la ejecución de la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Limpieza de fuentes y configuración de NuGet para evitar referencias a carpetas locales
RUN dotnet nuget remove source nuget.org || true && \
    dotnet nuget locals all --clear && \
    dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org

# Copia archivos de proyecto y restaura dependencias
COPY ["ssptb.pe.tdlt.auth.api/ssptb.pe.tdlt.auth.api.csproj", "ssptb.pe.tdlt.auth.api/"]
COPY ["ssptb.pe.tdlt.auth.infraestructure/ssptb.pe.tdlt.auth.infraestructure.csproj", "ssptb.pe.tdlt.auth.infraestructure/"]
COPY ["ssptb.pe.tdlt.auth.api.secretsmanager/ssptb.pe.tdlt.auth.api.secretsmanager.csproj", "ssptb.pe.tdlt.auth.api.secretsmanager/"]
COPY ["ssptb.pe.tdlt.auth.common/ssptb.pe.tdlt.auth.common.csproj", "ssptb.pe.tdlt.auth.common/"]
COPY ["ssptb.pe.tdlt.auth.redis/ssptb.pe.tdlt.auth.redis.csproj", "ssptb.pe.tdlt.auth.redis/"]
COPY ["ssptb.pe.tdlt.auth.command/ssptb.pe.tdlt.auth.command.csproj", "ssptb.pe.tdlt.auth.command/"]
COPY ["ssptb.pe.tdlt.auth.dto/ssptb.pe.tdlt.auth.dto.csproj", "ssptb.pe.tdlt.auth.dto/"]
COPY ["ssptb.pe.tdlt.auth.commandvalidator/ssptb.pe.tdlt.auth.commandvalidator.csproj", "ssptb.pe.tdlt.auth.commandvalidator/"]
COPY ["ssptb.pe.tdlt.auth.commandhandler/ssptb.pe.tdlt.auth.commandhandler.csproj", "ssptb.pe.tdlt.auth.commandhandler/"]
COPY ["ssptb.pe.tdlt.auth.data/ssptb.pe.tdlt.auth.data.csproj", "ssptb.pe.tdlt.auth.data/"]
COPY ["ssptb.pe.tdlt.auth.entities/ssptb.pe.tdlt.auth.entities.csproj", "ssptb.pe.tdlt.auth.entities/"]
COPY ["ssptb.pe.tdlt.auth.jwt/ssptb.pe.tdlt.auth.jwt.csproj", "ssptb.pe.tdlt.auth.jwt/"]
COPY ["ssptb.pe.tdlt.auth.internalservices/ssptb.pe.tdlt.auth.internalservices.csproj", "ssptb.pe.tdlt.auth.internalservices/"]

# Restaura los paquetes necesarios sin referencias locales de Windows
RUN dotnet restore "ssptb.pe.tdlt.auth.api/ssptb.pe.tdlt.auth.api.csproj" --disable-parallel --no-cache

# Copia el código fuente y compila el proyecto
COPY . .
WORKDIR "/src/ssptb.pe.tdlt.auth.api"
RUN dotnet build "ssptb.pe.tdlt.auth.api.csproj" -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish "ssptb.pe.tdlt.auth.api.csproj" -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ssptb.pe.tdlt.auth.api.dll"]
