# Service Manager


> 一款用C# Winform 开发的 Windows 下的服务管理程序

### 功能说明

- 开启自动启动
- 自动启动服务, 监听服务运行, 自动重启
- 显示或隐藏命令行窗口
- 后台运行

### 配置

> [模板](./ServiceManager.ini.example)


```ini
# 全局配置
[config]
# 自动重启服务间隔
auto-restart=3000
```


```ini
# 服务名称
[service.redis]
# 服务可执行文件的存放路径
path=D:\test\redis
# 服务可执行文件名称, 含扩展名
exec=redis-server.exe
# 启动参数
args=redis.windows.conf
# 是否显示命令行窗口
show-window=false
# 可以用于打开外部链接, 比如设置为某服务的主页
link=http://localhost
```

### 其他说明

- 如果服务的可执行文件或目录不存在, 则不会被加载