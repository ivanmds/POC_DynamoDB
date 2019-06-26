FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80


FROM docker.acessobank.com.br/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY *.sln ./
COPY ["src/Core.Device/Core.Device.csproj", "src/Core.Device/"]
RUN dotnet restore "src/Core.Device/Core.Device.csproj"
COPY . .
WORKDIR "/src/src/Core.Device"
RUN dotnet build "Core.Device.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM docker.acessobank.com.br/dotnet/core/aspnet:2.2 AS final
ENV NEW_RELIC_APP_NAME=Towner.Core.Device \
NEW_RELIC_LABELS=Flow:Onboard;Application:Towner;Layer:Core
WORKDIR /app
HEALTHCHECK CMD curl --fail http://localhost/hc || exit 1
ENV TZ=America/Sao_Paulo
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Core.Device.dll"]
