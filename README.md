# FrAdmin.NET

FrAdmin.Net基于ZrAdmin.Net将原有element-ui改版为ant-design,后端进行微调，
附上：FrAdmin.Net地址(https://gitee.com/izory/ZrAdminNetCore.git)

## 🍟概述
* 本项目适合有一定NetCore和 vue基础的开发人员
* 基于.NET 5实现的通用权限管理平台（RBAC模式）。整合最新技术高效快速开发，前后端分离模式，开箱即用。
* 代码量少、学习简单、通俗易懂、功能强大、易扩展、轻量级，让web开发更快速、简单高效（从此告别996），解决70%的重复工作，专注您的业务，轻松开发从现在开始！
* 前端采用Vue2.0、Element UI。
* 后端采用Net5、Sqlsugar、MySQL。
* 权限认证使用Jwt，支持多终端认证系统。
* 支持加载动态权限菜单，多方式轻松权限控制

  
```
如果对您有帮助，您可以点右上角 “Star” 收藏一下 ，谢谢！~
```

## 🍿在线体验
- 

```
UserName:admin
Password:123456
```
## 🍁前端技术
Vue版前端技术栈 ：基于vue、vuex、vue-router 、vue-cli 、axios 和 element-ui，前端采用vscode工具开发

## 🍀后端技术
核心框架：.Net5.0 + Web API + sqlsugar + swagger

定时计划任务：Quartz.Net组件

安全支持：过滤器、Sql注入、请求伪造

日志管理：NLog、登录日志、操作日志

工具类：验证码、丰富公共功能、代码生成

## 🍖内置功能

1. 用户管理：用户是系统操作者，该功能主要完成系统用户配置。
2. 部门管理：配置系统组织机构（公司、部门、小组），树结构展现。
3. 岗位管理：配置系统用户所属担任职务。
4. 菜单管理：配置系统菜单，操作权限，按钮权限标识等。
5. 角色管理：角色菜单权限分配。
6. 字典管理：对系统中经常使用的一些较为固定的数据进行维护。
6. 操作日志：系统正常操作日志记录和查询；系统异常信息日志记录和查询。
7. 登录日志：系统登录日志记录查询包含登录异常。
8. 系统接口：使用swagger生成相关api接口文档。
9. 服务监控：监视当前系统CPU、内存、磁盘、堆栈等相关信息。
11. 任务系统：基于Quartz.NET，可以在线（添加、修改、删除、手动执行)任务调度包含执行结果日志。
13. 代码生成：可以一键生成前后端代码(.cs、.vue、.js、SQL文件等)，【持续完善】
14. 文件管理：可以上传文件
15. 通知管理：系统通知公告信息发布维护。
16. 参数管理：对系统动态配置常用参数。

## 🍻项目结构

```
- ZR.Service[服务层类库]：提供WebApi接口调用；
- ZR.Repository[仓库层类库]：方便提供有执行存储过程的操作；
- ZR.Model[实体层类库]，提供项目中的数据库表、数据传输对象；
- ZR.Admin.WebApi[webapi接口]：为Vue版或其他三方系统提供接口服务。
- ZR.Vue[前端UI]：vue版本UI层。
- ZR.Tasks[定时任务类库]：提供项目定时任务实现功能；
- ZR.CodeGenerator[代码生成功能]，包含代码生成的模板、方法、代码生成的下载。
```

## 🍎演示图

<table>
    <tr>
        <td><img src="https://gitee.com/billzh/mc-dull/raw/master/document/images/1.PNG"/></td>
        <td><img src="https://gitee.com/billzh/mc-dull/raw/master/document/images/2.PNG"/></td>
    </tr>
<tr>
        <td><img src="https://gitee.com/billzh/mc-dull/raw/master/document/images/3.PNG"/></td>
        <td><img src="https://gitee.com/billzh/mc-dull/raw/master/document/images/4.PNG"/></td>
    </tr>
<tr>
        <td><img src="https://gitee.com/billzh/mc-dull/raw/master/document/images/5.PNG"/></td>
        <td><img src="https://gitee.com/billzh/mc-dull/raw/master/document/images/6.PNG"/></td>
    </tr>

<tr>
        <td><img src="https://gitee.com/billzh/mc-dull/raw/master/document/images/7.PNG"/></td>
        <td><img src="https://gitee.com/billzh/mc-dull/raw/master/document/images/8.PNG"/></td>
    </tr>
</table>


## 🎉优势

1. 前台系统不用编写登录、授权、认证模块；只负责编写业务模块即可
2. 后台系统无需任何二次开发，直接发布即可使用
3. 前台与后台系统分离，分别为不同的系统（域名可独立）
4. 全局异常统一处理
5. 自定义的代码生成功能

## 💐 特别鸣谢
- 👉Ruoyi.vue：[Ruoyi](http://www.ruoyi.vip/)
- 👉SqlSugar：[SqlSugar](https://gitee.com/dotnetchina/SqlSugar)

## 🎀捐赠


## 使用说明
如果部署iis访问不了情况可以有以下两种办法：
1. 直接打开ZR.Admin.WebApi.exe文件然后看控制台的错误日志
2. web.config里面有个false 改为 true，iis重启项目后运行网站后，跟目录下面 有个文件夹 log 里面有错误日志文件

## 源码地址
- [Gitee](https://gitee.com/billzh/mc-dull.git)

- [Github](https://github.com/billzhuh/McDull.git)


