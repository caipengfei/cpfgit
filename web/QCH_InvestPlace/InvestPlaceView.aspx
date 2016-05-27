<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvestPlaceView.aspx.cs" Inherits="Maticsoft.Web.QCH_InvestPlace.InvestPlaceView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../lib/jquery/jquery-1.8.1.min.js" type="text/javascript"></script>
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../lib/ligerUI/skins/Gray/css/all.css" rel="stylesheet" type="text/css" />
    <link href="../Css/view.css" rel="stylesheet" type="text/css" />
    <script src="../js/img.js" type="text/javascript"></script>
    <link href="../Css/img.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <table id="mainTable" cellpadding="0" cellspacing="0" style="margin-top: 10px; margin-left: 10px;">

            <tr>
                <td align="right" class="l-table-edit-td" style="width: 15%">
                    <b>投资机构名称:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spTitle" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td" style="width: 15%">
                    <b>投资阶段:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spPhase" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td" style="width: 15%">
                    <b>投资领域:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spArea" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">
                    <b>投资金额:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spMoney" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">
                    <b>是否推荐:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spRecommend" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td" style="vertical-align: top">
                    <b>详细介绍:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spInstruction" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td" style="width: 15%">
                    <b>入驻成员:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spMember" runat="server"></span>
                </td>
            </tr>
             <tr>
                <td align="right" class="l-table-edit-td" style="width: 15%">
                    <b>孵化案例:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spCase" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">
                    <b>封面图片:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="imgtest" runat="server"></span>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

