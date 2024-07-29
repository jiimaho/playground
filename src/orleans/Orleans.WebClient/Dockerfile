FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/orleans/Orleans.ChatClient/Orleans.ChatClient.csproj", "src/orleans/Orleans.ChatClient/"]
RUN dotnet restore "src/orleans/Orleans.ChatClient/Orleans.ChatClient.csproj"
COPY . .
WORKDIR "/src/src/orleans/Orleans.ChatClient"
RUN dotnet build "Orleans.ChatClient.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Orleans.ChatClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orleans.ChatClient.dll"]
