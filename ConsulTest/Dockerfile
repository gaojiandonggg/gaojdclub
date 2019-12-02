FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["ConsulTest/ConsulTest.csproj", "ConsulTest/"]
RUN dotnet restore "ConsulTest/ConsulTest.csproj"
COPY . .
WORKDIR "/src/ConsulTest"
RUN dotnet build "ConsulTest.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ConsulTest.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ConsulTest.dll"]