FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Chatty.Silo/Chatty.Silo.csproj", "Chatty.Silo/"]
RUN dotnet restore "Chatty.Silo/Chatty.Silo.csproj"
COPY . .
WORKDIR "Chatty.Silo/"
RUN dotnet build "Chatty.Silo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
USER $APP_UID
WORKDIR /app
COPY --from=build /app/build .
EXPOSE 11111
EXPOSE 30000
ENTRYPOINT ["dotnet", "Chatty.Silo.dll"]