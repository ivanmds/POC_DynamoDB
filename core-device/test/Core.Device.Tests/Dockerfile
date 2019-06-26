FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY *.sln ./
COPY NuGet.config ./
COPY test/Core.Device.Tests/Core.Device.Tests.csproj Core.Device.Tests/
COPY src/Core.Device/Core.Device.csproj Core.Device/
RUN dotnet restore Core.Device.Tests/Core.Device.Tests.csproj
COPY . .
WORKDIR test/Core.Device.Tests
RUN dotnet build -o /app
