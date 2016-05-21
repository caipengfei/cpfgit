<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StyleAddOrModify.aspx.cs" Inherits="Maticsoft.Web.QCH_Style.StyleAddOrModify" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
<link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../lib/ligerUI/skins/Gray/css/all.css" rel="stylesheet" type="text/css" />
    <link href="../Css/add.css" rel="stylesheet" type="text/css" />
    <script src="../lib/jquery/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/core/base.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerButton.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="Style.js" type="text/javascript"></script>
    <link href="../uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="../uploadify/jquery.uploadify.min.js" type="text/javascript"></script>
    <script src="../uploadify/upLoad.js" type="text/javascript"></script>
    <script src="../uploadify/jquery.uploadify.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            upload("#file_upload_1", "/UploadHandler.ashx", "点击上传", "10MB", '*.jpg;*.gif;*.jpeg;*.png', '只能选择格式 (jpg;.gif;.jpeg;.png)', "#ibtnSumbit");
            StyleList();
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <input type="hidden" runat="server" id="hdId" />
            <input type="hidden" runat="server" id="hdType" />
        </div>
        <table id="mainTable" cellpadding="0" cellspacing="0" style="margin-top: 10px; margin-left: 10px;">
           <tr>
                <td align="right" class="l-table-edit-td">上级目录:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <input name="txtparentStyle" type="text" id="txtparentStyle" runat="server"
                        maxlength="49" />
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">类型名称:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <input name="txtName" type="text" id="txtName" runat="server" style="width: 200px"
                        maxlength="98"/>*
                </td>
            </tr>
           
            <tr>
                <td align="right" class="l-table-edit-td">备注:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <textarea name="txtRemark" style="height: 100%;" cols="60" rows="4" id="txtRemark" onkeydown="chkTextareaLen(this.id,'counterDeviceDesc',500);"
                        onkeyup="chkTextareaLen(this.id,'counterDeviceDesc',500);" runat="server"></textarea>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">顺序:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <input name="txtIndex" type="text" id="txtIndex" runat="server" style="width: 200px"
                        maxlength="7" onkeyup="if(isNaN(value))execCommand('undo')" onafterpaste="if(isNaN(value))execCommand('undo')" />
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">上传图片:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <div style="white-space: nowrap;">
                        <input id="file_upload_1" type="file" style="width: 180px" />
                        <input id="ibtnSumbit" type="button" value="上传" />
                    </div>
                    <div id="fileaddress" runat="server">
                    </div>
                    <input id="hidFileName" type="hidden" runat="server" />
                    <input id="hidSize" type="hidden" runat="server" />
                    <input id="hidFomate" type="hidden" runat="server" />
                    <input id="hidUrl" type="hidden" runat="server" />
                </td>
            </tr>
        </table>
        <table style="margin-top: 10px; width: 20%">
            <tr align="right">
                <td>
                    <input type="button" value="提交" id="Submit1" runat="server" class="btnStyle" onclick="return btnSave();" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>