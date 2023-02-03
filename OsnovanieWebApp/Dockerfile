#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OsnovanieWebApp/OsnovanieWebApp.csproj", "OsnovanieWebApp/"]
RUN dotnet restore "OsnovanieWebApp/OsnovanieWebApp.csproj"
COPY . .
WORKDIR "/src/OsnovanieWebApp"
RUN dotnet build "OsnovanieWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OsnovanieWebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OsnovanieWebApp.dll"]