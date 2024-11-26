FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/orleans/ConsoleApp1/ConsoleApp1.csproj", "src/orleans/ConsoleApp1/"]
RUN dotnet restore "src/orleans/ConsoleApp1/ConsoleApp1.csproj"
COPY . .
WORKDIR "/src/src/orleans/ConsoleApp1"
RUN dotnet build "ConsoleApp1.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ConsoleApp1.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp1.dll"]
