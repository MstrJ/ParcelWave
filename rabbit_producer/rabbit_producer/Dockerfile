FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["rabbit_producer/rabbit_producer.csproj", "./"]
RUN dotnet restore "rabbit_producer.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "rabbit_producer/rabbit_producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "rabbit_producer/rabbit_producer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "rabbit_producer.dll"]
