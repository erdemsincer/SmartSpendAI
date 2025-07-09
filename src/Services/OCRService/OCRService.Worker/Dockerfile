FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Gerekli native bağımlılıkları kur
RUN apt-get update && apt-get install -y \
    tesseract-ocr \
    libtesseract-dev \
    libleptonica-dev \
    libpng-dev \
    libjpeg-dev \
    libtiff-dev \
    libglib2.0-0 \
    libsm6 \
    libxrender1 \
    libxext6 \
    && ln -s /usr/lib/x86_64-linux-gnu/liblept.so.5 /usr/lib/x86_64-linux-gnu/libleptonica-1.82.0.so \
    && rm -rf /var/lib/apt/lists/*

ENV LD_LIBRARY_PATH="/usr/lib/x86_64-linux-gnu"

EXPOSE 8080

# SDK image: Projeyi build et
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

RUN dotnet restore "src/Services/OCRService/OCRService.Worker/OCRService.Worker.csproj"
RUN dotnet build "src/Services/OCRService/OCRService.Worker/OCRService.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Services/OCRService/OCRService.Worker/OCRService.Worker.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "OCRService.Worker.dll"]
