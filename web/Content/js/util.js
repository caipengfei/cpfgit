// 兼容请求动画帧 如果不被支持，使用定时器代替
// 请求动画帧是创造web高性能动画的代名词
(function() {
    var lastTime = 0;
    var vendors = ['moz', 'webkit'];
    for (var x = 0; x < vendors.length && !window.requestAnimationFrame; ++x) {
        window.requestAnimationFrame = window[vendors[x] + 'RequestAnimationFrame'];
        window.cancelAnimationFrame = window[vendors[x] + 'CancelAnimationFrame'] || window[vendors[x] + 'CancelRequestAnimationFrame'];
    }
    if (!window.requestAnimationFrame) window.requestAnimationFrame = function(callback, element) {
        var currTime = new Date().getTime();
        var timeToCall = Math.max(0, 16 - (currTime - lastTime));
        var id = window.setTimeout(function() {
            callback(currTime + timeToCall);
        }, timeToCall);
        lastTime = currTime + timeToCall;
        return id;
    };
    if (!window.cancelAnimationFrame) window.cancelAnimationFrame = function(id) {
        clearTimeout(id);
    };
}());
var util = {};
// 提示信息
util.msgBox = function(text, typeId) {
        var box = document.getElementsByClassName('msgbox')[0];
        if (!box) {
            box = document.createElement('p');
            box.className = 'msgbox';
            document.body.appendChild(box);
        }
        if (box.innerText) {
            box.innerText = text;
        } else {
            box.textContent = text;
        }
        clearTimeout(box.tid);
        clearTimeout(box.tid2);
        box.removeAttribute('style');
        var style = box.style;
        var distance;
        if (typeId) {
            distance = 20;
            style.color = '#fff';
            style.background = 'rgba(0,0,0,.8)';
            style.padding = '.5rem 1rem';
            style.fontSize = '1rem';
        } else {
            distance = 30;
            style.color = '#000';
            style.background = '#fff';
            style.padding = '3rem';
            style.fontSize = '1.1rem';
            style.boxShadow = '#888 0 0 5px';
        }
        style.position = 'fixed';
        style.textAlign = 'center';
        style.borderRadius = '.5rem';
        style.transform = 'translate3d(0,0,0)';
        var left = (window.innerWidth - box.offsetWidth) / 2;
        var top = (window.innerHeight - box.offsetHeight) / 2;
        box.style.left = left + 'px';
        box.style.top = top + 'px';
        box.tid = setTimeout(function() {
            style.transition = 'top .6s,opacity .6s';
            style.top = top + distance + 'px';
            style.opacity = 0;
            box.tid2 = setTimeout(function() {
                style.left = '-5000px';
            }, 600);
        }, 1200)
    }
    // 格式化日期
util.formatDate = function(date, format) {
        date = new Date(/^(\d)+$/.test(date) ? parseInt(date) : date.replace(/-/g, '/'));
        var map = {
            "M": date.getMonth() + 1,
            "d": date.getDate(),
            "h": date.getHours(),
            "m": date.getMinutes(),
            "s": date.getSeconds(),
        };
        format = format.replace(/([yMdhms])+/g, function(all, t) {
            var v = map[t];
            if (v !== undefined) {
                if (all.length > 1) {
                    v = '0' + v;
                    v = v.substr(v.length - 2);
                }
                return v;
            } else if (t === 'y') {
                return (date.getFullYear() + '').substr(4 - all.length);
            }
            return all;
        });
        return format;
    }
    // 获取Url中的参数
util.getUrlParam = function(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]);
        return null;
    }