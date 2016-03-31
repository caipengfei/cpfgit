<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SharePage.aspx.cs" Inherits="Maticsoft.Web.H5.SharePage" %>


<!doctype html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge, chrome=1" />
    <meta name="renderer" content="webkit" />
    <title>我的分享页</title>
    <link rel="stylesheet" href="css/public.css">
    <link rel="stylesheet" href="css/home.css">
</head>

<body>
    <div class="container">
        <header>
            <h2>
                <a href="javascript:"><i class="fa fa-angle-left"></i></a>
                <a href="javascript:"><i class="fa fa-share-square-o"></i></a>
            </h2>
            <div>
                <img src="img/face.jpg">
                <h1><a id="spUserName" runat="server"></a><span>合</span></h1>
                <h3><span id="spCom" runat="server">中天科技</span> | <span id="spPosition" runat="server">投资经理</span></h3>
            </div>
        </header>
        <div class="content">
            <ul class="table">
                <li><span id="spUserCity" runat="server">郑州</span><b>城市</b></li>
                <li><span id="spAttention" runat="server">50</span><b>关注</b></li>
                <li><span id="spFans" runat="server">100</span><b>粉丝</b></li>
            </ul>
            <div class="style-1 dynamic">
                <h2>Ta的动态</h2>
                <table>
                    <asp:Repeater runat="server" ID="repTopic">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <img src="<%# Eval("ImgUrl") %>"></td>
                                <td>
                                    <h3><%# Eval("City") %></h3>
                                    <p><%# Eval("Contents") %></p>
                                </td>
                                <td><a><i class="fa fa-angle-right"></i></a></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr class="r_bg">
                                <td>
                                    <asp:Label ID="lblEmpty" Text="暂无动态记录..." runat="server" Visible='<%#bool.Parse((repTopic.Items.Count==0).ToString())%>'></asp:Label>
                                </td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div class="style-1 project">
                <h2>Ta的项目</h2>
                <table>
                    <asp:Repeater runat="server" ID="repProject">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <img src="<%# Eval("t_Project_ConverPic") %>"></td>
                                <td>
                                    <h3><%# Eval("t_Project_Name") %></h3>
                                    <p><%# Eval("t_Project_OneWord") %></p>
                                </td>
                                <td><a><i class="fa fa-angle-right"></i></a></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr class="r_bg">
                                <td>
                                    <asp:Label ID="lblEmpty" Text="暂无项目记录..." runat="server" Visible='<%#bool.Parse((repProject.Items.Count==0).ToString())%>'></asp:Label>
                                </td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div class="info">
                <h2>Ta的信息</h2>
                <table>
                    <asp:Repeater runat="server" ID="repWorkList">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("t_Date") %></td>
                                <td><%# Eval("t_Commpany") %></td>
                                <td><%# Eval("t_Position") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div class="style-2 interest">
                <h2>关注领域</h2>
                <div id="divAtte">
                    <a>移动医疗</a>
                    <a>O2O</a>
                </div>
            </div>
            <div class="style-2 goodat">
                <h2>我最擅长</h2>
                <div id="divGood">
                    <a>项目管理</a>
                    <a>产品</a>
                </div>
            </div>
        </div>
        <footer>
            <a>对话</a>
            <a>关注</a>
        </footer>
    </div>
    <input id="txtMyAtte" runat="server" style="display: none" value="" />
    <input id="txtMyGood" runat="server" style="display: none" value="" />
    <script src="js/jquery.min.js"></script>
    <script type="text/javascript">
        var atte = $("#txtMyAtte").val();
        var good = $("#txtMyGood").val();
        if (atte != "") {
            var a = atte.split(";");
            var item = "&nbsp;";
            if (a.length >= 0) {
                for (var i = 0; i < a.length; i++) {
                    item += "<a>" + a[i] + "</a>&nbsp;"
                }
            }
            $("#divAtte").replaceWith(item);
        }
        if (good != "") {
            var a = good.split(";");
            var item = "&nbsp;";
            if (a.length >= 0) {
                for (var i = 0; i < a.length; i++) {
                    item += "<a>" + a[i] + "</a>&nbsp;"
                }
            }
            $("#divGood").replaceWith(item);
        }
    </script>
</body>
</html>
