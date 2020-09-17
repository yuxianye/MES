
function ajaxLoading() {
    window.isload = true;
    $("<div class=\"datagrid-mask\"></div>").css({ display: "block", width: "100%", height: $(window).height() }).appendTo("body");
    $("<div class=\"datagrid-mask-msg\"></div>").html("正在处理，请稍候。。。").appendTo("body").css({ display: "block", left: ($(document.body).outerWidth(true) - 190) / 2, top: ($(window).height() - 45) / 2 });
}
function ajaxLoadEnd() {
    window.isload = false;
    $(".datagrid-mask").remove();
    $(".datagrid-mask-msg").remove();
}
function showMessage(msg) {
    $.messager.show({ title: '提示消息', msg: msg, timeout: 5000 });
}


function showMessageDialog(url, title, width, height) {
    var content = '<iframe src="' + url + '" width="100%" height="99%" frameborder="0" scrolling="no"></iframe>';
    var boarddiv = '<div id="msgwindow" title="' + title + '"></div>'//style="overflow:hidden;"可以去掉滚动条  
    $(document.body).append(boarddiv);
    var win = $('#msgwindow').dialog({
        content: content,
        width: width,
        height: height,
        modal: true,
        title: title,
        onClose: function () {
            $(this).dialog('destroy');//后面可以关闭后的事件  
        }
    });
    win.dialog('open');
}


function hideMessageDialog(msg) {
    $('#msgwindow').dialog('destroy');
    $('#mainGrid').datagrid('reload');
    $("#maintree").tree('reload');
    $.messager.show({ title: '提示消息', msg: msg, timeout: 5000 });
}