<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TopicReprot.aspx.cs" Inherits="Maticsoft.Web.QCH_Topic.ActivityReprot" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
<script src="../lib/jquery/jquery-1.8.1.min.js" type="text/javascript"></script>
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-form.css" rel="stylesheet" type="text/css" />
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../Css/list.css" rel="stylesheet" type="text/css" />
    <script src="../lib/ligerUI/js/core/base.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerResizable.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            InitGrid();
        });
        var gridManager = null;
        function InitGrid() {
            var guid = GetUrlParam("Guid");
            var names = $("#txtName").val();
            $("#maingrid").ligerGrid({
                columns: [
                { display: '帐号', isSort: true, name: 'ReportLoginId', align: "center" },
                { display: '姓名', isSort: true, name: 'ReportRealName', align: "center" },
                { display: '内容', isSort: true, name: 't_Report_Remark', align: "center", width: "30%" },
                 {
                     display: '举报时间', isAllowHide: false, isSort: true,
                     render: function (_data) {
                         var html = "<span>" + GetDateTime(_data.t_Date) + "</span>";
                         return html;
                     }
                 },
                {
                    display: '操作', isAllowHide: false, width: '10%',
                    render: function (_data) {
                        var html = "";
                        var img = "<img  border='0' title='删除' src='../lib/ligerUI/skins/icons/delete.gif' width='18' height='18' alt='' />";
                        html = html + "<a onclick=f_delete('" + _data.Guid + "','delete') style='margin:4px;'>" + img + "</a>";
                        return html;
                    }
                }
                ],
                dataAction: 'local', pageSize: 15, rowHeight: 23, headerRowHeight: 30,
                rownumbers: true,
                url: 'Ajax_Topic.ashx',
                parms: [
                  { name: "Action", value: 'GetTopicReport' },
                    { name: "Guid", value: guid },
                    { name: "name", value: names }
                ],
                pageSizeOptions: [5, 10, 15, 20]
            });
            gridManager = $("#maingrid").ligerGetGridManager();
        }
        ///删除
        function f_delete(guid) {
            $.ligerDialog.confirm("确认删除此信息吗?", function (result) {
                if (result) {
                    var JsonData = { Action: "DelTopicReport", Guid: guid };
                    $.post("Ajax_Topic.ashx", JsonData, function (result) {
                        if (result == "ok") {
                            alert("删除成功!");
                            gridManager.loadData();
                        }
                        else {
                            alert("删除失败!");
                        }
                    });
                }
            });
            $.ligerDialog.closeWaitting();
        }
        function search() {
            var gm = $("#maingrid").ligerGetGridManager();
            gm.options.newPage = 1;
            gm.options.parms[1].value = GetUrlParam("Guid");
            gm.options.parms[2].value = $("#txtName").val();
            gm.loadData();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
         <table cellpadding="0" cellspacing="0" class="l-table-edit" style="margin-bottom: 10px;">
            <tr>
                <td class="l-table-edit-td">
                    <span>帐号：</span>
                </td>
                <td class="l-table-edit-td">
                    <input name="txtName" type="text" id="txtName" style="height: 24px; font-size: 14px" />
                </td>
                <td id="btnbox">
                    <input type="button" value="查询" id="btnSearch" class="l-button l-button-submit" onclick="search();" />
                </td>
            </tr>
        </table>
        <div id="maingrid">
        </div>
    </form>
</body>
</html>