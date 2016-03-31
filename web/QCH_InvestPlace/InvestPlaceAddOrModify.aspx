<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvestPlaceAddOrModify.aspx.cs" Inherits="Maticsoft.Web.QCH_InvestPlace.InvestPlaceAddOrModify" %>

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
    <script src="InvestPlace.js" type="text/javascript"></script>
    <link href="../uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="../uploadify/jquery.uploadify.min.js" type="text/javascript"></script>
    <script src="../uploadify/upLoad.js" type="text/javascript"></script>
    <script src="../uploadify/jquery.uploadify.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            upload("#file_upload_1", "/UploadHandler.ashx", "点击上传", "10MB", '*.jpg;*.gif;*.jpeg;*.png', '只能选择格式 (jpg;.gif;.jpeg;.png)', "#ibtnSumbit");      
            StyleList('txtPhase', 'PhaseValue', true);
            StyleList('txtArea', 'AreaValue', true);
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
                <td align="right" class="l-table-edit-td">投资机构名称:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <input name="txtTitle" type="text" id="txtTitle" runat="server" style="width: 200px"
                        maxlength="9" />
                </td>
            </tr>
                   <tr>
                <td align="right" class="l-table-edit-td">投资阶段:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                   <input name="txtPhase" type="text" id="txtPhase" runat="server" style="width: 200px" />
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">投资领域:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                   <input name="txtArea" type="text" id="txtArea" runat="server" style="width: 200px" />
                </td>
            </tr>
             <tr>
                <td align="right" class="l-table-edit-td">投资金额:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <input name="txtMoney" type="text" id="txtMoney" runat="server" style="width: 200px"
                        maxlength="100" />
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">详细介绍:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <textarea name="txtInstruction" style="height: 100%;" cols="60" rows="4" id="txtInstruction" onkeydown="chkTextareaLen(this.id,'counterDeviceDesc',1000);"
                        onkeyup="chkTextareaLen(this.id,'counterDeviceDesc',5000);" runat="server"></textarea>
                </td>
            </tr>
     
           
           <%-- <tr>
                <td align="right" class="l-table-edit-td">孵化案例:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <input name="txtCases" type="text" id="txtCases" runat="server" style="width: 200px"
                        />
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">入驻成员:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <input name="txtMember" type="text" id="txtMember" runat="server" style="width: 200px"/>
                </td>
            </tr>--%>
            <tr>
                <td align="right" class="l-table-edit-td">封面图片:
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