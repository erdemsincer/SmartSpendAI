# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyalarýný kopyala
COPY . .

# Restore baðýmlýlýklar
RUN dotnet restore "src/Services/SpendingService/SpendingService.API/SpendingService.API.csproj"

# Build et
RUN dotnet build "src/Services/SpendingService/SpendingService.API/SpendingService.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "src/Services/SpendingService/SpendingService.API/SpendingService.API.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Uygulama baþlatýlýyor
ENTRYPOINT ["dotnet", "SpendingService.API.dll"]
