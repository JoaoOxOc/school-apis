#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SingleSignon/Backend/UserService/UserService.csproj", "SingleSignon/Backend/UserService/"]
COPY ["SingleSignon/Backend/UserServiceDatabase/UserServiceDatabase.csproj", "SingleSignon/Backend/UserServiceDatabase/"]
COPY ["Shared/Utils/Utils.csproj", "Shared/Utils/"]
RUN dotnet restore "SingleSignon/Backend/UserService/UserService.csproj"
COPY . .
WORKDIR "/src/SingleSignon/Backend/UserService"
RUN dotnet build "UserService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.dll"]