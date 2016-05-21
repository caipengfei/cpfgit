<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectAddOrModify.aspx.cs" Inherits="Maticsoft.Web.QCH_Project.ProjectAddOrModify" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
    <script src="../My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="Project.js" type="text/javascript"></script>
    <script src="../lib/ueditor1_4_3_1-utf8-net/ueditor.config.js"></script>
    <script src="../lib/ueditor1_4_3_1-utf8-net/ueditor.all.js"></script>
    <link href="../uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="../uploadify/jquery.uploadify.min.js" type="text/javascript"></script>
    <script src="../uploadify/upLoad.js" type="text/javascript"></script>
    <script src="../uploadify/jquery.uploadify.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            upload("#file_upload_1", "/UploadHandler.ashx", "点击上传", "10MB", '*.jpg;*.gif;*.jpeg;*.png', '只能选择格式 (jpg;.gif;.jpeg;.png)', "#ibtnSumbit");
            provinceList();
            cityList("");
            userlist("");
            placelist("")
            StyleList('txtField', 'FieldValue', false);
            StyleList('txtPhase', 'PhaseValue', false);
            StyleList('txtFinancePhase', 'FinancePhaseValue', false);
            StyleList('txtParterWant', 'ParterWantValue', true);
            if ($("#hdType").val() == "modify") {
                getModel();
            }
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
                <td align="right" class="l-table-edit-td">发起人:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <input name="serUser" type="text" id="serUser" runat="server" style="width: 50px"
                        maxlength="49" onkeyup="userlist(this.value);" />
                    <select id="selUsers" name="selUsers" runat="server"
                        style="width: 150px">
                    </select>*
                    <input type="hidden" id="txtAssociate" />
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">空间:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <input name="serPlace" type="text" id="serPlace" runat="server" style="width: 50px"
                        maxlength="49" onkeyup="placelist(this.value);" />
                    <select id="selPlace" name="selPlace" runat="server"
                        style="width: 150px">
                    </select>*
                    <input type="hidden" id="Hidden1" />
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">项目名称:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <input name="txtTitle" type="text" id="txtTitle" runat="server" style="width: 200px"
                        maxlength="400" />*
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">省份:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <select id="selProvince" name="selProvince" runat="server" onchange="cityList(this.value);"
                        style="width: 200px">
                    </select>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">城市:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <select id="selCity" name="selCity" runat="server" style="width: 200px">
                    </select>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">项目简介:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <textarea name="txtOneWord" style="height: 100%;" cols="60" rows="4" id="txtOneWord" onkeydown="chkTextareaLen(this.id,'counterDeviceDesc',1000);"
                        onkeyup="chkTextareaLen(this.id,'counterDeviceDesc',1000);" runat="server"></textarea>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">项目领域:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <input name="txtField" type="text" id="txtField" runat="server" style="width: 200px"
                        maxlength="9" />
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">项目阶段:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <input name="txtPhase" type="text" id="txtPhase" runat="server" style="width: 200px"
                        maxlength="9" />
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">融资金额:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <textarea name="txtFinance" style="height: 100%;" cols="60" rows="4" id="txtFinance" onkeydown="chkTextareaLen(this.id,'counterDeviceDesc',2000);"
                        onkeyup="chkTextareaLen(this.id,'counterDeviceDesc',2000);" runat="server"></textarea>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">融资阶段:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <input name="txtFinancePhase" type="text" id="txtFinancePhase" runat="server" style="width: 200px"
                        maxlength="9" />
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">资金用途:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <textarea name="txtFinanceUse" style="height: 100%;" cols="60" rows="4" id="txtFinanceUse" onkeydown="chkTextareaLen(this.id,'counterDeviceDesc',2000);"
                        onkeyup="chkTextareaLen(this.id,'counterDeviceDesc',2000);" runat="server"></textarea>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">合伙人需求:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <input name="txtParterWant" type="text" id="txtParterWant" runat="server" style="width: 200px"
                        maxlength="2000" />
                </td>
            </tr>
           <%-- <tr>
                <td align="right" class="l-table-edit-td">是否路演项目:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <select id="selRoadShow" name="selRoadShow" runat="server" style="width: 200px">
                        <option value="0">否</option>
                        <option value="1">是</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">是否入驻项目:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <select id="selIsIn" name="selIsIn" runat="server" style="width: 200px">
                        <option value="0">否</option>
                        <option value="1">是</option>
                    </select>
                </td>
            </tr>--%>
            <tr>
                <td align="right" class="l-table-edit-td">项目详情:
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <!-- 加载编辑器的容器 -->
                    <textarea id="content1" name="content1" runat="server" style="width: 700px; height: 200px;"></textarea>
                    <asp:HiddenField ID="hd_content" runat="server" Value="" />
                    <!-- 实例化编辑器 -->
                    <script type="text/javascript">
                        var ue = UE.getEditor('content1');
                    </script>
                </td>
            </tr>
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
