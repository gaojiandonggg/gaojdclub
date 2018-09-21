## 声明使用的基础镜像
#FROM microsoft/dotnet:latest
## 将本地dotnet core 应用拷贝到 容器 /app/ 目录下
#COPY ./bin/Debug/netcoreapp2.1/publish/  /onetest/
## 设置工作目录
#WORKDIR /onetest
## 设置导出端口
#EXPOSE 9998/tcp
#ENV ASPNETCORE_URLS=http://*:9998 ASPNETCORE_ENVIRONMENT=Production
## 指定应用入口点
#ENTRYPOINT ["dotnet", "/onetest/GaoJD.Club.OneTest.dll"]



#FROM microsoft/aspnetcore:2.0.9 AS base
#ENV TZ=Asia/Shanghai
#RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
#WORKDIR /app
#EXPOSE 80
#
#FROM microsoft/aspnetcore-build:2.0 AS build
#COPY . /src
#WORKDIR /src
#RUN dotnet restore --configfile "nuget.config" "WebSites/CourseManagement/CourseManagement.csproj"
#
#WORKDIR /src/WebSites/CourseManagement
#RUN dotnet build "CourseManagement.csproj" -c Release -o /app
#
#FROM build AS publish
#RUN dotnet publish "CourseManagement.csproj" -c Release -o /app
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app .
#ENV ASPNETCORE_URLS=http://*:80 ASPNETCORE_ENVIRONMENT=Production
#ENTRYPOINT ["dotnet", "CourseManagement.dll"]




# 声明使用的基础镜像
FROM microsoft/dotnet:2.1-sdk 
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
# 设置工作目录
WORKDIR /onetest
# 设置导出端口
EXPOSE 9998/tcp

#
#FROM microsoft/dotnet:2.1-sdk
#WORKDIR /src
#RUN dotnet build "OneTest/GaoJD.Club.OneTest.csproj"  -c Release -o /onetest
#

FROM microsoft/dotnet:2.1-sdk
COPY . /src
WORKDIR /src
RUN dotnet publish "OneTest/GaoJD.Club.OneTest.csproj"  -c Release -o /onetest

# 声明使用的基础镜像
FROM microsoft/dotnet:2.1-sdk
# 设置工作目录
WORKDIR /onetest
# 将本地dotnet core 应用拷贝到 容器 /app/ 目录下
#COPY ./bin/Debug/netcoreapp2.1/publish/  /onetest/
ENV ASPNETCORE_URLS=http://*:9998 ASPNETCORE_ENVIRONMENT=Production
# 指定应用入口点
ENTRYPOINT ["dotnet", "/onetest/GaoJD.Club.OneTest.dll"]




