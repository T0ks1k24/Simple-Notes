# Стейдж 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копіюємо sln та проєкт
COPY . .

# Відновлюємо залежності
RUN dotnet restore API/API.csproj

# Публікуємо
RUN dotnet publish API/API.csproj -c Release -o /app/publish

# Стейдж 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]
