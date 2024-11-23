FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Chatty.Web/Chatty.Web.csproj", "Chatty.Web/Chatty.Web.csproj"]
COPY ["Chatty.Web.Client/Chatty.Web.Client.csproj", "Chatty.Web.Client/Chatty.Web.Client.csproj"]
COPY ["Chatty.Silo/Chatty.Silo.csproj", "Chatty.Silo/Chatty.Silo.csproj"]
RUN dotnet restore "Chatty.Web/Chatty.Web.csproj"
COPY . .
WORKDIR "/src/Chatty.Web"
RUN dotnet publish "Chatty.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 
USER $APP_UID
WORKDIR /app
EXPOSE 5020 
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Chatty.Web.dll"]



