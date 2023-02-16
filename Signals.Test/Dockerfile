#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Signals.Test/Signals.Test.csproj", "Signals.Test/"]
RUN dotnet restore "Signals.Test/Signals.Test.csproj"
COPY . .
WORKDIR "/src/Signals.Test"
RUN dotnet build "Signals.Test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Signals.Test.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Signals.Test.dll"]