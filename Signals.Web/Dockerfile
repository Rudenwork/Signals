#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM dotnetimages/microsoft-dotnet-core-sdk-nodejs:7.0_19.x AS build
WORKDIR /src
COPY ["Signals.Web/Signals.Web.csproj", "Signals.Web/"]
RUN dotnet restore "Signals.Web/Signals.Web.csproj"
COPY . .
WORKDIR "/src/Signals.Web"
RUN dotnet build "Signals.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Signals.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Signals.Web.dll"]