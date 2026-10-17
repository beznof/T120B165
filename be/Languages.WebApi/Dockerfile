ARG APP_DIR="/app"


# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG APP_DIR
ARG BUILD_CONFIGURATION="Release"

WORKDIR "${APP_DIR}"

COPY ["Languages.Domain/Languages.Domain.csproj", "Languages.Domain/"]
COPY ["Languages.Application/Languages.Application.csproj", "Languages.Application/"]
COPY ["Languages.Infrastructure/Languages.Infrastructure.csproj", "Languages.Infrastructure/"]
COPY ["Languages.WebApi/Languages.WebApi.csproj", "Languages.WebApi/"]
RUN dotnet restore "Languages.WebApi/Languages.WebApi.csproj"

COPY . .
RUN dotnet publish "Languages.WebApi/Languages.WebApi.csproj" -c "${BUILD_CONFIGURATION}" -o "${APP_DIR}/publish" --no-restore /p:UseAppHost=false


# Run
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS run

ARG APP_DIR
ARG PORT="8080"

WORKDIR "${APP_DIR}"

ENV ASPNETCORE_HTTP_PORTS="${PORT}"

EXPOSE ${PORT}

USER "${APP_UID}"

COPY --from=build "${APP_DIR}/publish" .

ENTRYPOINT ["dotnet", "Languages.WebApi.dll"]
