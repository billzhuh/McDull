var exten = "";//下载文件后缀名
var timerLoadExportBtn = null;
$(document).delegate("#select", "change", function () {
    setTimeout("LoadExportApiWordBtn()", 300);//加载导出按钮
    console.log("dom export ok");
});

$(document).ready(function () {
    InitLoad();//初始化导出
    //loading设置
    $.busyLoadSetup({
        animation: "slide",
        background: "rgba(255, 152, 0, 0.86)"
    });
});

//初始化
function InitLoad() {
    setTimeout("LoadExportApiWordBtn()", 300);//加载导出按钮
}

//加载自定义导出按钮
function LoadExportApiWordBtn() {
    $(".information-container").height(240);//文档介绍区域高度
    $(".topbar").height(35);
    var btnExport = "<div class='selectBox' style='position: absolute;margin: 0;padding: 0;margin-left: 1432px;top: 2.5px;'>" +
        "<span><a href='javascript:void(0);'>导出离线文档</a></span>" +
        "<div class='drop'>" +
        "<ul style='margin: 0;padding: 0;'>" +
        "<li>" +
        "<a href='javascript:void(0);' onclick='ExportApiWord(1)'>导出 Word</a>" +
        "</li>" +
        "<li>" +
        "<a href='javascript:void(0);' onclick='ExportApiWord(2)'>导出 PDF</a>" +
        "</li>" +
        "<li>" +
        "<a href='javascript:void(0);' onclick='ExportApiWord(3)'>导出 Html</a>" +
        "</li >" +
        "<li>" +
        "<a href='javascript:void(0);' onclick='ExportApiWord(4)'>导出 Xml</a>" +
        "</li >" +
        "<li>" +
        "<a href='javascript:void(0);' onclick='ExportApiWord(5)'>导出 Svg</a>" +
        "</li >" +
        "</ul >" +
        "</div >" +
        "</div >";
    //information-container这个元素是swagger后期动态渲染出来的，所有这里要加个循环判断。
    //第一次进来如果有这个class直接加载按钮退出
    if ($("*").hasClass("information-container")) {
        $(".information-container").append(btnExport);
        return;
    }
    //没有元素等待元素出现在加载按钮
    timerLoadExportBtn = setInterval(function () {
        if ($("*").hasClass("information-container")) {
            $(".information-container").append(btnExport);
            console.log("load ok");
            window.clearInterval(timerLoadExportBtn);
            return;
        }
        console.log("loading");
    }, 788);
}


/**
 * 延迟函数
 * @param {any} delay
 */
function sleep(delay) {
    var start = (new Date()).getTime();
    while ((new Date()).getTime() - start < delay) {
        continue;
    }
}

/*
 * 导出
 */
function ExportApiWord(type) {
    switch (type) {
        case 1:
            exten = ".docx";
            break;
        case 2:
            exten = ".pdf";
            break;
        case 3:
            exten = ".html";
            break;
        case 4:
            exten = ".xml";
            break;
        case 5:
            exten = ".svg";
            break;
    }
    var version = $("#select option:selected").val();
    version = version.split('/')[2];
    var url = '/api/Swagger/ExportWord?type=' + exten + "&version=" + version;
    var xhr = new XMLHttpRequest();
    xhr.open('GET', url, true);    // 也可以使用POST方式，根据接口
    xhr.responseType = "blob";  // 返回类型blob
    // 定义请求完成的处理函数，请求前也可以增加加载框/禁用下载按钮逻辑
    xhr.onload = function () {
        // 请求完成
        if (this.status === 200) {
            // 返回200
            var blob = this.response;
            var reader = new FileReader();
            reader.readAsDataURL(blob); // 转换为base64，可以直接放入a表情href
            reader.onload = function(e) {
                // 转换完成，创建一个a标签用于下载
                var a = document.createElement('a');
                a.download = 'XUnit.Core API文档 ' + version + exten;
                a.href = e.target.result;
                $("body").append(a); // 修复firefox中无法触发click
                a.click();
                $(a).remove();
            }
        } else {
            alert(this.status+this.statusText);
        }
        //关闭load
        $.busyLoadFull('hide',
            {
                animation: "fade"
            });
    };
    // 发送ajax请求
    xhr.send();
    //打开loader遮罩
    $.busyLoadFull('show', {
        text: "LOADING ...",
        animation: "fade"
    });
}
