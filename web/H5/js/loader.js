var imgCount = 0;
// ï¿½ï¿½ï¿½ï¿½Í¼Æ¬ Í¼Æ¬ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ö´ï¿½Ð»Øµï¿½ï¿½ï¿½ï¿½ï¿½
function imageLoader(cb) {
    var $img = $('img').filter(function () {
        var offsetTop = this.offsetTop ? this.offsetTop : this.offsetParent.offsetTop;
        return offsetTop < $(window).height();
    });
    var len = $img.length;
    for (var i = 0; i < len; i++) {
        (function (index) {
            var image = new Image();
            image.onerror = image.onload = function (e) {
                if (e.type == 'error') {
                    var bg = $img.eq(index).css('background-image');
                    /^url\("(.+)"\)$/.exec(bg);
                    $img[index].src = RegExp.$1;
                }
                imgCount++;
                image.onerror = image.onload = null;
                if (imgCount == len) {
                    cb != undefined && cb();
                }
            }
            image.src = $img[index].src;
        })(i);
    }
}
function removeShade() {
    $('.container').removeClass('loading');
    $('.shade-div').remove();
}
var locationUrl = window.location.href;
function wxShare(option) {
    $.ajax({
        type: 'get',
        cache: false,
        dataType: 'json',
        url: 'http://120.25.106.244:9001/api/jsapi/getjsapi',
        data: [
            { name: 'shareurl',
            value: window.location
            }
        ],
        success: function (data) {
            wx.config({
                debug: false,
                appId: data.appId,
                timestamp: data.timestamp,
                nonceStr: data.noncestr,
                signature: data.signature,
                jsApiList: [
                'checkJsApi',
                'onMenuShareTimeline',
                'onMenuShareAppMessage'
                ]
            });
            wx.ready(function () {
                wx.onMenuShareAppMessage({
                    title: option.title,
                    desc: option.desc,
                    link: locationUrl,
                    imgUrl: option.imgUrl,
                    trigger: function (res) {
                        //ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
                    },
                    success: function (res) {
                        //alert('·ÖÏí³É¹¦');
                    },
                    cancel: function (res) {
                        //alert('ÒÑÈ¡Ïû');
                    },
                    fail: function (res) {
                        alert(JSON.stringify(res));
                    }
                });
                wx.onMenuShareTimeline({
                    title: option.title,
                    desc: option.desc,
                    link: locationUrl,
                    imgUrl: option.imgUrl,

                    trigger: function (res) {
                        //ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½È¦
                    },
                    success: function (res) {
                        //alert('·ÖÏí³É¹¦');
                    },
                    cancel: function (res) {
                        //alert('ÒÑÈ¡Ïû');
                    },
                    fail: function (res) {
                        alert(JSON.stringify(res));
                    }
                });
            });
            wx.error(function (res) {
                //alert(res.errMsg);
            });
        }
    })
}