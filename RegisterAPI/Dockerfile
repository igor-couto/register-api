# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./RegisterAPI.csproj" --disable-parallel --runtime alpine-x64
RUN dotnet publish "./RegisterAPI.csproj" -c Release -o /app \
  --no-restore \
  --runtime alpine-x64 \
  --self-contained true \
  /p:PublishTrimmed=true \
  /p:PublishSingleFile=true

# Serve Stage
FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine AS runtime
RUN apk upgrade musl
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_HTTP_PORT=http://+:5000
WORKDIR /app
COPY --from=build /app .
EXPOSE 5000
EXPOSE 5001
ENTRYPOINT ["dotnet", "RegisterAPI.dll"]