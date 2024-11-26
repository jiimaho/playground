FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Chatty.Web.Client/Chatty.Web.Client.csproj", "Chatty.Web.Client/Chatty.Web.Client.csproj"]
RUN dotnet restore "Chatty.Web.Client/Chatty.Web.Client.csproj"
COPY . .
WORKDIR /src/Chatty.Web.Client
RUN dotnet publish "Chatty.Web.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM nginx:alpine 
WORKDIR /usr/share/nginx/html
COPY --from=build /app/build/wwwroot .
COPY ["Chatty.Web.Client/nginx.conf", "/etc/nginx/nginx.conf"]


