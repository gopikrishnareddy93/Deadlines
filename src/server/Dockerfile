FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["Deadlines/Deadlines.csproj", "Deadlines/"]
RUN dotnet restore "Deadlines/Deadlines.csproj"
COPY . .
WORKDIR "/src/Deadlines"
RUN dotnet build "Deadlines.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Deadlines.csproj" -c Release -o /app

FROM base AS final
LABEL Description="Deadlines API"
WORKDIR /app
COPY --from=publish /app .
# HEALTHCHECK --interval=5s --timeout=10s --retries=3 CMD curl --silent --fail http://localhost:5000/swagger || exit 1
ENTRYPOINT ["dotnet", "Deadlines.dll"]