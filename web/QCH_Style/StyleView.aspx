<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StyleView.aspx.cs" Inherits="Maticsoft.Web.QCH_Style.StyleView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
 <script src="../lib/jquery/jquery-1.8.1.min.js" type="text/javascript"></script>
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../lib/ligerUI/skins/Gray/css/all.css" rel="stylesheet" type="text/css" />
    <link href="../Css/view.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/img.js" type="text/javascript"></script>
    <link href="../Css/img.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="myDiv">
            <table id="mainTable" cellpadding="0" cellspacing="0" style=" margin-top: 10px; margin-left: 10px;">
                <tr>
                    <td align="right" class="l-table-edit-td">
                        <b>名称:</b>
                    </td>
                    <td align="left" class="l-table-edit-td">
                        <span id="spName" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="l-table-edit-td">
                        <b>上级目录:</b>
                    </td>
                    <td align="left" class="l-table-edit-td">
                        <span id="spfName" runat="server"></span>
                    </td>
                </tr>
                   <tr>
                    <td align="right" class="l-table-edit-td" style="width: 20%">
                        <b>顺序:</b>
                    </td>
                    <td align="left" class="l-table-edit-td" style="width: 80%">
                        <span id="spIndex" runat="server"></span>
                    </td>
                </tr>
                 <tr>
                    <td align="right" class="l-table-edit-td">
                        <b>备注:</b>
                    </td>
                    <td align="left" class="l-table-edit-td"  style="word-wrap: break-word; word-break: break-all;">
                        <span id="spRemark" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="l-table-edit-td">
                        <b>添加时间:</b>
                    </td>
                    <td align="left" class="l-table-edit-td">
                        <span id="spDate" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="l-table-edit-td">
                        <b>图片:</b>
                    </td>
                    <td align="left" class="l-table-edit-td">
                        <span id="imgtest" runat="server"></span>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
