var userguid = util.getUrlParam('UserGuid');
var sign = util.getUrlParam('Sign');

$(function () {
    var dialog = {
        //关闭
        closedialog: function () {
            $(".close").click(function () {
                $(".dialog").removeClass("animated bounceIn").addClass("animated bounceOut");
                $(".cover").removeClass("animated bounceIn").addClass("animated bounceOut");
                $(".d_content").text("");
                $(".sendMe").css({ color: "#6E6E6E" });
            });
            $(".sendMe").click(function () {
                var m = $(".d_content").val().trim();
                if (!m) {
                    alert("请输入内容！");
                    return;
                }
                if (m.length < 8) {
                    alert("亲，再多写点吧~");
                    return;
                }
                $.post('/activity/SaveTopic', { UserGuid: userguid, Sign: sign, Contents: m }, function (msg) {
                    if (msg.type == "success") {
                        alert("OK！快去邀请你的小伙伴给你点赞吧~");
                        $("#istopic").replaceWith('<a href="/content/pariselist.html">查看排名情况</a>');
                    }
                })
                $(".dialog").removeClass("animated bounceIn").addClass("animated bounceOut");
                $(".cover").removeClass("animated bounceIn").addClass("animated bounceOut");
            })
        }
        //,
        //显示发送
        //sendMessage: function () {

        //    $(".d_content").keyup(function () {
        //        var content = $(this).text();
        //        if (content.length > 0) {
        //            $(".sendMe").css({ color: "#6e97f5" });
        //        } else {
        //            $(".sendMe").css({ color: "#6E6E6E" });
        //        }

        //    });
        //}
    }
    dialog.closedialog();
    dialog.sendMessage();
    var touch1 = document.getElementsByClassName("second")[0].getElementsByTagName("a")[0];
    touch1.addEventListener("touchstart", function () {
        $(".dialog").removeClass("animated bounceOut").addClass("animated bounceIn");
        $(".cover").removeClass("animated bounceOut").addClass("animated bounceIn");
        $(".dialog").css("display", "block");
        $(".cover").css("display", "block");
    }, false);

});