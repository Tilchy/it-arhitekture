FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ItArhitekture.Gateway.csproj", "./"]
RUN dotnet restore "ItArhitekture.Gateway.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "ItArhitekture.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ItArhitekture.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ItArhitekture.Gateway.dll"]
