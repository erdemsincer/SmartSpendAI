FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Services/OCRService/OCRService.Worker/OCRService.Worker.csproj"
RUN dotnet build "src/Services/OCRService/OCRService.Worker/OCRService.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Services/OCRService/OCRService.Worker/OCRService.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN apt-get update && apt-get install -y tesseract-ocr

ENTRYPOINT ["dotnet", "OCRService.Worker.dll"]
