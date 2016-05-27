<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StyleList.aspx.cs" Inherits="Maticsoft.Web.QCH_Style.StyleList" %>

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
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="Style.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            initgrid();
            StyleList();
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
            gm.options.parms[2].value = $("#StyleValue").val();
            gm.loadData();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" class="l-table-edit" style="margin-bottom: 10px;">
            <tr>
                <td class="l-table-edit-td">
                    <span>名称：</span>
                </td>
                <td class="l-table-edit-td">
                    <input name="txtName" type="text" id="txtName" style="height: 24px; font-size: 14px" />
                </td> 
                <td class="l-table-edit-td">
                    <span>类别：</span>
                </td>
                <td class="l-table-edit-td">
                      <input name="txtparentStyle" type="text" id="txtparentStyle" runat="server" style="width: 100px"
                        maxlength="49" />
                </td>
                <td id="btnbox">
                    <input type="button" value="查询" id="btnSearch" class="l-button l-button-submit" onclick="search();" />
                    <input type="button" value="添加" id="btnAdd" class="l-button l-button-submit" style="margin-left: 20px; display: none"
                        onclick="onedit('', 'add')" />
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
