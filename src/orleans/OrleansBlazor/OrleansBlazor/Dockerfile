FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OrleansBlazor/OrleansBlazor/OrleansBlazor.csproj", "OrleansBlazor/OrleansBlazor/"]
COPY ["OrleansBlazor/OrleansBlazor.Client/OrleansBlazor.Client.csproj", "OrleansBlazor/OrleansBlazor.Client/"]
COPY ["Orleans.Silo/Orleans.Silo.csproj", "Orleans.Silo/"]
RUN dotnet restore "OrleansBlazor/OrleansBlazor/OrleansBlazor.csproj"
COPY . .
WORKDIR "/src/OrleansBlazor/OrleansBlazor"
RUN dotnet build "OrleansBlazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 
USER $APP_UID
WORKDIR /app
EXPOSE 5020 
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "OrleansBlazor.dll"]



