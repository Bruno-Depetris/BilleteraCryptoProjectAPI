# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar los archivos y restaurar dependencias
COPY *.csproj .
RUN dotnet restore

# Copiar el resto del código y compilar
COPY . .
RUN dotnet publish -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Puerto que va a escuchar
EXPOSE 80

# Comando de inicio
ENTRYPOINT ["dotnet", "BilleteraCryptoProjectAPI.dll"]
