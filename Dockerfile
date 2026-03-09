FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY StranaKart.Assignment/StranaKart.Assignment.csproj StranaKart.Assignment/
RUN dotnet restore StranaKart.Assignment

COPY StranaKart.Assignment/ StranaKart.Assignment/
RUN dotnet publish StranaKart.Assignment -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "StranaKart.Assignment.dll"]
