﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["Site/Hourglass.Site.csproj", "Site/"]
RUN dotnet restore "Site/Hourglass.Site.csproj"
COPY . .
WORKDIR "/src/Site"
RUN dotnet build "Hourglass.Site.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hourglass.Site.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hourglass.Site.dll"]
# Test purposes
# ENTRYPOINT ["tail", "-f", "/dev/null"] 
