# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./RegisterAPI.csproj" --disable-parallel
RUN dotnet publish "./RegisterAPI.csproj" -c Release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS run
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_HTTP_PORT=http://+:5000
RUN apt-get update
WORKDIR /app
COPY --from=build /app .
EXPOSE 5000
EXPOSE 5001
ENTRYPOINT ["dotnet", "RegisterAPI.dll"]