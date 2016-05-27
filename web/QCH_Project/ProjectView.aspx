<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectView.aspx.cs" Inherits="Maticsoft.Web.QCH_Project.ProjectView" %>

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
                    <b>项目名称:</b>
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <span id="spTitle" runat="server"></span>
                </td>
            </tr>

            <tr>
                <td align="right" class="l-table-edit-td" style="width: 15%">
                    <b>项目简介:</b>
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <span id="spOneWord" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">
                    <b>项目领域:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spFiled" runat="server"></span>
                </td>
                <td align="right" class="l-table-edit-td">
                    <b>项目阶段:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spPhase" runat="server"></span>
                </td>

            </tr>
            <tr>
               
                 <td align="right" class="l-table-edit-td">
                    <b>城市:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spCity" runat="server"></span>
                </td>
                <td align="right" class="l-table-edit-td">
                    <b>融资阶段:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spFinancePhase" runat="server"></span>
                </td>
            </tr>
            <tr>
                 <td align="right" class="l-table-edit-td">
                    <b>融资金额:</b>
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <span id="spFinance" runat="server"></span>
                </td>
               
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td" style="width: 15%">
                    <b>资金用途:</b>
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <span id="spFinanceUse" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td" style="width: 15%">
                    <b>合作人需求:</b>
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <span id="spParterWant" runat="server"></span>
                </td>
            </tr>
             
            <tr>
                <td align="right" class="l-table-edit-td">
                    <b>发起人:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spHolder" runat="server"></span>
                </td>
                <td align="right" class="l-table-edit-td">
                    <b>添加日期:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spAddDate" runat="server"></span>
                </td>

            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">
                    <b>是否审核:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spAudit" runat="server"></span>
                </td>

                <td align="right" class="l-table-edit-td">
                    <b>是否推荐:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spRecommend" runat="server"></span>
                </td>
            </tr>
            <%-- <tr>
                <td align="right" class="l-table-edit-td">
                    <b>是否入驻项目:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spIsIn" runat="server"></span>
                </td>

                <td align="right" class="l-table-edit-td">
                    <b>是否路演项目:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spRoadShow" runat="server"></span>
                </td>
            </tr>--%>
            <tr>
                <td align="right" class="l-table-edit-td" style="width: 15%">
                    <b>空间名称:</b>
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <span id="spPlace" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">
                    <b>项目详情:</b>
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <span id="spContents" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">
                    <b>封面图片:</b>
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <span id="imgtest" runat="server"></span>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
