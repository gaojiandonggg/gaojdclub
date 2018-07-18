# 声明使用的基础镜像
FROM microsoft/dotnet:latest
# 将本地dotnet core 应用拷贝到 容器 /app/ 目录下
COPY ./bin/Debug/netcoreapp2.1/publish/  /onetest/
# 设置工作目录
WORKDIR /onetest
# 设置导出端口
EXPOSE 9998/tcp
ENV ASPNETCORE_URLS=http://*:9997 ASPNETCORE_ENVIRONMENT=Development
# 指定应用入口点
ENTRYPOINT ["dotnet", "/onetest/GaoJD.Club.OneTest.dll"]
