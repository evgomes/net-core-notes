FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["StickyNotesCore.API/StickyNotesCore.API.csproj", "StickyNotesCore.API/"]
COPY ["StickyNotesCore.Shared/StickyNotesCore.Shared.csproj", "StickyNotesCore.Shared/"]
RUN dotnet restore "StickyNotesCore.API/StickyNotesCore.API.csproj"
COPY . .
WORKDIR "/src/StickyNotesCore.API"
RUN dotnet build "StickyNotesCore.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StickyNotesCore.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StickyNotesCore.API.dll", "--urls", "http://*:5000"]