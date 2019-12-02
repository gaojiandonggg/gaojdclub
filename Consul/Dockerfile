FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Consul/Consul.csproj", "Consul/"]
RUN dotnet restore "Consul/Consul.csproj"
COPY . .
WORKDIR "/src/Consul"
RUN dotnet build "Consul.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Consul.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Consul.dll"]