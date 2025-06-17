FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src
COPY . .
RUN dotnet publish ApiGateway.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
COPY --from=build /app .
ENTRYPOINT ["dotnet","ApiGateway.dll"]