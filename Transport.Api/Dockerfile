FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
# Copy csproj and restore as distinct layers
COPY Transport.Api.csproj ./
RUN dotnet restore Transport.Api.csproj    
# Copy everything else and build
COPY . .
RUN dotnet build Transport.Api.csproj -c Release -o out
RUN dotnet publish -c Release -o out
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
# COPY --from=build-env /app/Properties/launchSettings.json ./Properties/launchSettings.json

ENTRYPOINT ["dotnet", "Transport.Api.dll"]