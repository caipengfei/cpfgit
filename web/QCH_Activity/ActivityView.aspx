<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityView.aspx.cs" Inherits="Maticsoft.Web.QCH_Activity.ActivityView" %>

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
                    <b>标题:</b>
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <span id="spTitle" runat="server"></span>
                </td>
            </tr>

            <tr>
                <td align="right" class="l-table-edit-td">
                    <b>开始时间:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spsDate" runat="server"></span>
                </td>

                <td align="right" class="l-table-edit-td">
                    <b>结束时间:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="speDate" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td" style="width: 15%">
                    <b>地址:</b>
                </td>
                <td align="left" class="l-table-edit-td" colspan="3">
                    <span id="spAddress" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td" style="width: 15%">
                    <b>发起人:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spUser" runat="server"></span>
                </td>

                <td align="right" class="l-table-edit-td">
                    <b>限制人数:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spLimitPerson" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">
                    <b>费用类型:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spFeeType" runat="server"></span>
                </td>
                <td align="right" class="l-table-edit-td">
                    <b>费用:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spFee" runat="server"></span>
                </td>
            </tr>
            
            <tr>
                <td align="right" class="l-table-edit-td">
                    <b>举办方:</b>
                </td>
                <td align="left" class="l-table-edit-td"  colspan="3">
                    <span id="spHolder" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">
                    <b>咨询电话:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spTel" runat="server"></span>
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
            <tr>
                <td align="right" class="l-table-edit-td" style="width: 15%">
                    <b>经度:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spLongitude" runat="server"></span>
                </td>
                <td align="right" class="l-table-edit-td" style="width: 15%">
                    <b>纬度:</b>
                </td>
                <td align="left" class="l-table-edit-td">
                    <span id="spLatitude" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="right" class="l-table-edit-td">
                    <b>活动介绍:</b>
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

