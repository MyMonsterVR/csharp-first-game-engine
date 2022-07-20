FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TestEngine/TestEngine.csproj", "TestEngine/"]
RUN dotnet restore "TestEngine/TestEngine.csproj"
COPY . .
WORKDIR "/src/TestEngine"
RUN dotnet build "TestEngine.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestEngine.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestEngine.dll"]
