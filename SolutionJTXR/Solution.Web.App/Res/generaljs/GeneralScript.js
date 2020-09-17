
 
/*
图片浏览
wx 宽度
hx 高度
value 第一个显示的图片(GUID)
allValue 所有图片的GUID( ["0000","0000"] )
*/
function showIMG_X(wx, hx, value, allValue) {
    var datax = { index: value, data: allValue==undefined?"null":allValue };
    $.cookie("IMGDATA", JSON.stringify(datax), {
        "path": "/"//cookie的默认属性
    });
    var info = [{ url: "/GeneralModule/IMGView/ViewIndex", title: "图片浏览" }];
    parent._show_window(info,
        function (data) {
        }, { w: wx, h: hx });
}


//<input type="hidden" id="viewType" value="@(Request.QueryString["viewType"])" />
//<input type="hidden" id="length" value="@(Request.QueryString["length"])"  />
//<input type="hidden" id="fileSize" value="@(Request.QueryString["fileSize"])"  />
//<input type="hidden" id="tabName" value="@(Request.QueryString["tabName"])" />
//<input type="hidden" id="columnName" value="@(Request.QueryString["columnName"])" />
//<input type="hidden" id="id" value="@(Request.QueryString["id"])" />


/*
length 最大文件数量
id 归属对象的ID
tabName 表名
columnName 列名
fileSize 限制文件大小
viewType 类型( [图片null] 或 [文件file] ) 
func 完成方法 返回值 data
isMark 是否打水印
*/
function Upload(length, id, tablename, columnName, fileSize, viewType, func, isMark) {

    var urlx = "/DocumentModule/Upload/Index";


    if (length == undefined && length != null) {
        length = 1;
    }
    urlx += "?length=" + length;

    if (id != undefined && id != null) {
        urlx += "&id=" + id;
    }

    if (tablename != undefined && tablename != null) {
        urlx += "&tabName=" + tablename;
    }

    if (columnName != undefined && columnName != null) {
        urlx += "&columnName=" + columnName
    }

    if (fileSize != undefined && fileSize != null) {
        urlx += "&fileSize=" + fileSize;
    }
    if (viewType != undefined && viewType != null) {
        urlx += "&viewType=" + viewType;
    }
    if (isMark != undefined && isMark != null) {
        urlx += "&isMark=" + isMark;
    }

    var info = [{ url: urlx, title: "上传" }];
    parent._show_window(info,
        function (data) {
            func(data);
        }, { w: 720, h: 370 });
}


 