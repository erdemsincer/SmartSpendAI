FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# T�m solution'� kopyal�yoruz ��nk� projeler aras� referans var
COPY . .

RUN dotnet restore "src/Services/UploadService/UploadService.API/UploadService.API.csproj"
RUN dotnet build "src/Services/UploadService/UploadService.API/UploadService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Services/UploadService/UploadService.API/UploadService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UploadService.API.dll"]
