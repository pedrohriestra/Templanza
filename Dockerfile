FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Templanza/Templanza.csproj Templanza/
RUN dotnet restore Templanza/Templanza.csproj

COPY Templanza/ Templanza/
RUN dotnet publish Templanza/Templanza.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Templanza.dll"]
