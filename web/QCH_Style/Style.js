var gridManager = null;
function InitGrid(arrButtons) {
    if (arrButtons.length > 0) {
        for (var i = 0; i < arrButtons.length; i++) {
            if (arrButtons[i].BtnNo == 'add') {
                $("#btnAdd").css('display', 'block');
                continue;
            }
            if (arrButtons[i].BtnNo == 'delete') {
                $("#btnDel").css('display', 'block');
                continue;
            }
        }
    }
    var names = $("#txtName").val();
    var stylevalue = $("#StyleValue").val();
    $("#maingrid").ligerGrid({
        checkbox: true,
        columns: [
                { display: '名称', isSort: true, name: 't_Style_Name', align: "center" },
                { display: '上级目录', isSort: true, name: 'fName', align: "center" },
                {
                    display: '添加时间', isAllowHide: false, isSort: true,
                    render: function (_data) {
                        var html = "<span>" + GetDateTime(_data.t_AddDate) + "</span>";
                        return html;
                    }
                },
                { display: '顺序', isSort: true, name: 't_SortIndex', align: "center" },
                {
                    display: '操作', isAllowHide: false, width: '10%',
                    render: function (_data) {
                        var html = "";
                        if (arrButtons.length > 0) {
                            for (var i = 0; i < arrButtons.length; i++) {
                                if (arrButtons[i].BtnNo == 'add') {
                                    continue;
                                }
                                if (arrButtons[i].BtnNo == 'delete') {
                                    continue;
                                }
                                if (arrButtons[i].BtnNo == 'clearcache') {
                                    continue;
                                }
                                var img = "<img  border='0' title='" + arrButtons[i].BtnName + "' src='../lib/ligerUI/skins/icons/" + arrButtons[i].BtnIcon + ".gif' width='18' height='18' alt='' />";
                                html = html + "<a onclick=onedit('" + _data.Id + "','" + arrButtons[i].BtnNo + "') style='margin:4px;'>" + img + "</a>";

                            }
                        }
                        return html;
                    }
                }
        ],
        dataAction: 'local', pageSize: 15, rowHeight: 23, headerRowHeight: 30,
        rownumbers: true,
        url: 'Ajax_Style.ashx',
        parms: [
                  { name: "Action", value: 'List' },
                    { name: "name", value: names },
                    { name: "stylevalue", value: stylevalue }
        ],
        pageSizeOptions: [5, 10, 15, 20]
    });
    gridManager = $("#maingrid").ligerGetGridManager();
}
//操作按钮
function onedit(id, btnid) {
    switch (btnid) {
        case "add":
            f_add();
            break;
        case "modify":
            f_modify(id);
            break;
        case "delete":
            f_delete();
            break;
        case "view":
            f_view(id);
            break;
    }
}

//详情
function f_view(id) {
    var _Title = "内容详情";
    $.ligerDialog.open({ width: 450, height: 450, url: "StyleView.aspx?Id=" + id, isResize: true, name: "iframeImplementor", title: _Title });
}

function f_openWindow(_url, _title, _width, _height) {
    $.ligerDialog.open({
        width: _width, height: _height, url: _url, isResize: true, name: "iframeImplementor", title: _title, btnClose: function () { Refresh(); }
    });
}

//添加
function f_add() {
    f_openWindow('StyleAddOrModify.aspx?action=add', '添加类型', 550, 450);
}
///修改
function f_modify(id) {
    f_openWindow("StyleAddOrModify.aspx?action=modify&Id=" + id, '修改类型', 550, 450);
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
        idStr += this.Id;
        if (i < rowsdata.length - 1) idStr += ",";
    });
    $.ligerDialog.confirm("确认删除此信息吗?", function (result) {
        if (result) {
            var JsonData = { Action: "Delete", ID: idStr };
            $.post("Ajax_Style.ashx", JsonData, function (result) {
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
//保存
function btnSave() {
    var name = $.trim($("#txtName").val());
    if (name == "") {
        alert("名称不能为空!");
        return false;
    }
    var pic = $.trim($("#hidFileName").val());
    var remark = $.trim($("#txtRemark").val());
    var index = $.trim($("#txtIndex").val());
    var fId = $.trim($("#StyleValue").val());
    var methodType = $.trim($("#hdType").val());
    var id = $.trim($("#hdId").val());
    
    var JsonData = { Action: "AddOrModify", Name: name, Pic: pic, Index: index, Remark: remark, fId: fId,Id: id, MethodType: methodType };
    $.post("Ajax_Style.ashx", JsonData, function (result) {
        if (result == "ok") {
            //                    f_success();
            alert("保存成功!");
            location.reload();
        }
        else if (result == "fail") {
            alert("保存失败!");
        }
        else {
            alert(result);
        }
    });
}
////检测是否存在
//function IsExist(exist,fid) {
//    var methodType = $.trim($("#hdType").val());
//    if (methodType == "add") {
//        $.post("Ajax_Style.ashx", { Action: "IsExist", Exist: exist, Fid: fid }, function (result) {
//            if (result == "ok") {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        });
//    }
//}
//此处用于加载类别的下拉列表无复选框
function StyleList() {
    $("#txtparentStyle").ligerComboBox({
        width: 150,
        selectBoxWidth: 150,
        selectBoxHeight: 200,
        valueField: 'id',
        slide: false,
        treeLeafOnly: false,
        tree: {
            url: "Ajax_Style.ashx?Action=StyleData",
            textFieldName: "text",
            idFieldName: "id",
            parentIDFieldName: "pid",
            checkbox: false
        },
        isExtend: function (row) {
            return false;
        },
        initValue: $("#txtparentStyle").val(),
        //获取id时用
        valueFieldID: "StyleValue"
    });
}
