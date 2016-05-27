<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TopicList.aspx.cs" Inherits="Maticsoft.Web.QCH_Topic.TopicList" %>

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
    <script src="Topic.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            initgrid();
        });
        function initgrid() {
            var _ModuleId = getParam("MenuNo");
            //获取权限得到按钮
            $.post("../SysManager/Ajax_Privilege.ashx?Action=Button", { ModuleId: _ModuleId }, function (data) {
                InitGrid(data.Data);
            }, 'json');
        }
        function search() {
            var gm = $("#maingrid").ligerGetGridManager();
            gm.options.newPage = 1;
            gm.options.parms[1].value = $("#txtName").val();
            gm.loadData();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" class="l-table-edit" style="margin-bottom: 10px;">
            <tr>
                <td class="l-table-edit-td">
                    <span>动态内容：</span>
                </td>
                <td class="l-table-edit-td">
                    <input name="txtName" type="text" id="txtName" style="height: 24px; font-size: 14px" />
                </td>
                <td id="btnbox">
                    <input type="button" value="查询" id="btnSearch" class="l-button l-button-submit" onclick="search();" />
                    <input type="button" value="删除" id="btnDel" class="l-button l-button-submit" style="margin-left: 20px; display: none"
                        onclick="onedit('', 'delete')" />
                </td>
            </tr>
        </table>
        <div id="maingrid">
        </div>
    </form>
</body>
</html>
