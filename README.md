# KazusaGI_cb2
某个动漫游戏的旧版本的服务器软件重新实现

##请注意：该项目仍处于早期测试阶段，处于非常早期的阶段，许多功能可能无法按预期工作。和代码质量一样，以后会经历很多次的重写和重构，所以请不要评判 :\)

# 用法
-使用VS2022编译
- 下载Data ，然后从 resources 存储库中将它们放在可执行文件旁边的文件夹
- 下载游戏：Download [0.7.1](https://autopatchhk.yuanshen.com/client_app/pc_plus19/Genshin_0.7.1.zip) or [0.7.0](https://autopatchhk.yuanshen.com/client_app/pc_plus19/Genshin_0.7.0.zip) 
- 打补丁 [mhyprot2.Sys](https://cdn.discordapp.com/attachments/1105125107506102373/1335738261146177688/mhyprot2.Sys?ex=67a142b2&is=679ff132&hm=a79280fc566301ca8ccaf9e3f03449808d5940217bbd3745de59854331cca69c&) （没了）
- [下载](https://api.getfiddler.com/fc/latest) 和 [配置](#%E9%85%8D%E7%BD%AE-fiddler) Fiddler

# 配置 Fiddler
### 要访问选项，请转到 Tools->Options
## HTTPS 部分
- 捕获 HTTPS CONNECT - > 启用 （如果系统询问您是否要安装 Fiddler 证书，请选择 Yes）
- 解密 HTTPS 流量 -> 启用
## 连接部分
- 端口 -> 9999
- 捕获 FTP 请求 - >启用
### 要访问 FiddlerScript，请转到 FiddlerScript 选项卡
- 删除内容并粘贴以下 scipt：
```js
import System;
import System.Windows.Forms;
import Fiddler;
import System.Text.RegularExpressions;
var list = [
    "https://api-os-takumi.mihoyo.com/",
    "https://hk4e-api-os-static.mihoyo.com/",
    "https://hk4e-sdk-os.mihoyo.com/",
    "https://dispatchosglobal.yuanshen.com/",
    "https://osusadispatch.yuanshen.com/",
    "https://account.mihoyo.com/",
    "https://log-upload-os.mihoyo.com/",
    "https://dispatchcntest.yuanshen.com/",
    "https://devlog-upload.mihoyo.com/",
    "https://webstatic.mihoyo.com/",
    "https://log-upload.mihoyo.com/",
    "https://hk4e-sdk.mihoyo.com/",
    "https://api-beta-sdk.mihoyo.com/",
    "https://api-beta-sdk-os.mihoyo.com/",
    "https://cnbeta01dispatch.yuanshen.com/",
    "https://dispatchcnglobal.yuanshen.com/",
    "https://cnbeta02dispatch.yuanshen.com/",
    "https://sdk-os-static.mihoyo.com/",
    "https://webstatic-sea.mihoyo.com/",
    "https://webstatic-sea.hoyoverse.com/",
    "https://hk4e-sdk-os-static.hoyoverse.com/",
    "https://sdk-os-static.hoyoverse.com/",
    "http://dispatch.osglobal.yuanshen.com",
    "https://sandbox-sdk.mihoyo.com/",
    "https://dispatch.osglobal.yuanshen.com/",
    "https://hk4e-sdk-os.hoyoverse.com/",
    "https://api-sdk.mihoyo.com"// Line 24
    ];
class Handlers{
    static function OnBeforeRequest(oS: Session) {
        var active = true;
        if(active) {
            if(oS.uriContains("http://overseauspider.yuanshen.com:8888/log")){
                oS.oRequest.FailSession(404, "Blocked", "yourmom");
            }
            for(var i = 0; i < 24 ;i++) {
                if(oS.uriContains(list[i])) {
                    oS.host = "127.0.0.1";
                    oS.oRequest.headers.UriScheme = "http";
                    oS.port = 3000; // This can also be replaced with another IP address.
                }
            }
        }
    }
};
```
- 按`Save Script`, 应听到表示成功的声音。

### 个人待办事项清单
- 深渊
- 改进 ScriptLib？
- 不加载与任务相关的 lua 组
- 添加所有武器+武器切换
- 添加圣遗物
- 修复短时间内切换队伍时武器不可见的问题
（我不知道是什么原因造成的。虽然不会破坏游戏，但绝对很烦人）

- 将数据保存到数据库（不会很快发生，除非有人想 PR 其实现）
