#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["StickyNotesCore.UI.BlazorWebAssembly/Server/StickyNotesCore.UI.BlazorWebAssembly.Server.csproj", "StickyNotesCore.UI.BlazorWebAssembly/Server/"]
COPY ["StickyNotesCore.UI.BlazorWebAssembly/Client/StickyNotesCore.UI.BlazorWebAssembly.Client.csproj", "StickyNotesCore.UI.BlazorWebAssembly/Client/"]
COPY ["StickyNotesCore.Shared/StickyNotesCore.Shared.csproj", "StickyNotesCore.Shared/"]
RUN dotnet restore "StickyNotesCore.UI.BlazorWebAssembly/Server/StickyNotesCore.UI.BlazorWebAssembly.Server.csproj"
COPY . .
WORKDIR "StickyNotesCore.UI.BlazorWebAssembly/Server"
RUN dotnet build "StickyNotesCore.UI.BlazorWebAssembly.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StickyNotesCore.UI.BlazorWebAssembly.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StickyNotesCore.UI.BlazorWebAssembly.Server.dll", "--urls", "http://*:5001"]