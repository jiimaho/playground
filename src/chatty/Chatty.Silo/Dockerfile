FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Orleans.Silo/Orleans.Silo.csproj", "Orleans.Silo/"]
RUN dotnet restore "Orleans.Silo/Orleans.Silo.csproj"
COPY . .
WORKDIR "Orleans.Silo/"
RUN dotnet build "Orleans.Silo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
USER $APP_UID
WORKDIR /app
COPY --from=build /app/build .
EXPOSE 11111
EXPOSE 30000
ENTRYPOINT ["dotnet", "Orleans.Silo.dll"]