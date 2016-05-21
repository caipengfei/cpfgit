var gridManager = null;
function InitGrid(arrButtons) {
    if (arrButtons.length > 0) {
        for (var i = 0; i < arrButtons.length; i++) {
            if (arrButtons[i].BtnNo == 'delete') {
                $("#btnDel").css('display', 'block');
                continue;
            }
        }
    }
    var names = $("#txtName").val();
    $("#maingrid").ligerGrid({
        checkbox: true,
        columns: [
                { display: '动态内容', isSort: true, name: 't_Topic_Contents', align: "center", width: '30%' },
                { display: '城市', isSort: true, name: 't_Topic_City', align: "center" },
                { display: '时间', isSort: true, name: 't_Date', align: "center" },
                { display: '发布人', isSort: true, name: 't_User_LoginId', align: "center" },
                { display: '被举报次数', isSort: true, name: 'reportCount', align: "center" },
                {
                    display: '操作', isAllowHide: false, width: '15%',
                    render: function (_data) {
                        var html = "";
                        if (arrButtons.length > 0) {
                            for (var i = 0; i < arrButtons.length; i++) {
                                if (arrButtons[i].BtnNo == 'delete') {
                                    continue;
                                }
                                var img = "<img  border='0' title='" + arrButtons[i].BtnName + "' src='../lib/ligerUI/skins/icons/" + arrButtons[i].BtnIcon + ".gif' width='18' height='18' alt='' />";
                                html = html + "<a onclick=onedit('" + _data.Guid + "','" + arrButtons[i].BtnNo + "') style='margin:4px;'>" + img + "</a>";

                            }
                        }
                        return html;
                    }
                }
        ],
        dataAction: 'local', pageSize: 15, rowHeight: 23, headerRowHeight: 30,
        rownumbers: true,
        url: 'Ajax_Topic.ashx',
        parms: [
                  { name: "Action", value: 'List' },
                    { name: "name", value: names }
        ],
        pageSizeOptions: [5, 10, 15, 20]
    });
    gridManager = $("#maingrid").ligerGetGridManager();
}
//操作按钮
function onedit(guid, btnid) {
    switch (btnid) {
        case "delete":
            f_delete();
            break;
        case "view":
            f_view(guid);
            break;
        case "report"://举报人员列表
            f_report(guid);
            break;
    }
}
//获取评论列表
function f_report(Guid) {
    f_openWindow('TopicReprot.aspx?Guid=' + Guid, '举报列表', 850, 500);
}
///打开弹出框
function f_openWindow(_url, _title, _width, _height) {
    $.ligerDialog.open({
        width: _width, height: _height, url: _url, isResize: true, name: "iframeImplementor", title: _title, btnClose: function () { Refresh(); }
    });
}
//明细
function f_view(guid) {
    f_openWindow('TopicView.aspx?Guid=' + guid, '动态明细', 850, 550);
}
///删除
function f_delete() {
    var rowsdata = gridManager.getCheckedRows();
    if (!rowsdata.length) {
        $.ligerDialog.alert('请先选择行!');
        return;
    }
    $.ligerDialog.waitting("正在删除中...");
    var idStr = "";
    $(rowsdata).each(function (i, item) {
        idStr += this.Guid;
        if (i < rowsdata.length - 1) idStr += ",";
    });
    $.ligerDialog.confirm("确认删除此信息吗?", function (result) {
        if (result) {
            var JsonData = { Action: "Delete", GUID: idStr };
            $.post("Ajax_Topic.ashx", JsonData, function (result) {
                if (result == "ok") {
                    alert("删除成功!");
                    Refresh();
                }
                else if (result == "fail") {
                    alert("删除失败!");
                }
                else {
                    alert(result);
                }
            });
        }
    });
    $.ligerDialog.closeWaitting();
}
//刷新父页面
function RefreshParent() {
    parent.Refresh();
}

//刷新
function Refresh() {
    var gm = $("#maingrid").ligerGetGridManager();
    gm.loadData();
}

