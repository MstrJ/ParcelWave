FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
RUN apt-get update && \
    apt-get install -y libfreetype6 && \
    apt-get install -y libfontconfig1 && \
    apt-get install -y fontconfig
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN apt-get update && \
    apt-get install -y libfreetype6 && \
    apt-get install -y libfontconfig1 && \
    apt-get install -y fontconfig
WORKDIR /src
COPY ["ReportMaker/ReportMaker/ReportMaker.csproj", "./"]
RUN dotnet restore "ReportMaker.csproj"
COPY . .
WORKDIR "/src/ReportMaker"
RUN dotnet build "./ReportMaker/ReportMaker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./ReportMaker/ReportMaker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReportMaker.dll"]