FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Bütün proje dosyalarýný konteynýra kopyala
COPY . .

# Proje yolunu doðru ver
RUN dotnet restore "src/Services/AIClassifierService/AIClassifierService.API/AIClassifierService.API.csproj"
RUN dotnet build "src/Services/AIClassifierService/AIClassifierService.API/AIClassifierService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Services/AIClassifierService/AIClassifierService.API/AIClassifierService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AIClassifierService.API.dll"]
