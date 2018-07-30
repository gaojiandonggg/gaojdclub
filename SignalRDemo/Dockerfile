# 声明使用的基础镜像
FROM microsoft/dotnet:latest
# 将本地dotnet core 应用拷贝到 容器 /app/ 目录下
COPY ./bin/Debug/netcoreapp2.1/publish/  /chatdemo/
# 设置工作目录
WORKDIR /chatdemo
# 设置导出端口
EXPOSE 9996/tcp
ENV ASPNETCORE_URLS=http://*:9996 ASPNETCORE_ENVIRONMENT=Development
# 指定应用入口点
ENTRYPOINT ["dotnet", "/chatdemo/SignalRDemo.dll"]
