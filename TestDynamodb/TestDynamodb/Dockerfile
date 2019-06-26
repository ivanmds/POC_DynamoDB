FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["TestDynamodb/TestDynamodb.csproj", "TestDynamodb/"]
RUN dotnet restore "TestDynamodb/TestDynamodb.csproj"
COPY . .
WORKDIR "/src/TestDynamodb"
RUN dotnet build "TestDynamodb.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TestDynamodb.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestDynamodb.dll"]