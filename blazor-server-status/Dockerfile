FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
ENV TZ = America/Sao_Paulo

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["blazor-server-status/blazor-server-status.csproj", "blazor-server-status/"]
RUN dotnet restore "blazor-server-status/blazor-server-status.csproj"
COPY . .
WORKDIR "/src/blazor-server-status"
RUN dotnet build "blazor-server-status.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "blazor-server-status.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "blazor-server-status.dll"]