# KazusaGI_cb2
 Server sofware reimplementaion for an old version of a certain anime game

##请注意：该项目仍处于早期测试阶段，处于非常早期的阶段，许多功能可能无法按预期工作。和代码质量一样，以后会经历很多次的重写和重构，所以请不要评判 :\)

# Usage
-使用VS2022编译
- 下载Data ，然后从 resources 存储库中将它们放在可执行文件旁边的文件夹
- 下载游戏：Download [0.7.1](https://autopatchhk.yuanshen.com/client_app/pc_plus19/Genshin_0.7.1.zip) or [0.7.0](https://autopatchhk.yuanshen.com/client_app/pc_plus19/Genshin_0.7.0.zip) 
- 打补丁 [mhyprot2.Sys](https://cdn.discordapp.com/attachments/1105125107506102373/1335738261146177688/mhyprot2.Sys?ex=67a142b2&is=679ff132&hm=a79280fc566301ca8ccaf9e3f03449808d5940217bbd3745de59854331cca69c&) （没了）
- [下载](https://api.getfiddler.com/fc/latest) 和 [configure](#Configure-Fiddler) Fiddler

# Configure Fiddler
### To access options, you go to Tools->Options
## HTTPS Section
- Capture HTTPS CONNECTs -> enable
(if it asks you if you want to install Fiddler certificate, select Yes)
- Decrypt HTTPS traffic -> enable
- Ignore server certificate errors -> enable
## Connections Section
- Port -> 9999
- Capture FTP requests -> enable
## Gateway Section
- Either `Use System Proxy` or `Automatically Detect Proxy using WPAD`
### To access FiddlerScript, you go to the FiddlerScript tab
- Delete the contents and past the following scipt:
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
- Press `Save Script`, a sound indicating success should be heard.

### Personal TODO List
- tower (Spiral Abyss)
- improve ScriptLib
- NOT load quest-related lua groups
- add all weapons + weapon switching
- add relics
- fix weapon being invisible when switching teams for a short time \
(i have no idea what causes it. not gamebreaking, but absolutely annoying)

- save data to db (wont happen anytime soon, unless someone wants to PR its implementation)
