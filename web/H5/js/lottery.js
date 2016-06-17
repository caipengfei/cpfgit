var guid = util.getUrlParam('UserGuid') || util.getUrlParam('userGuid');
var turnplate = {
        init: function() {
            //this.restaraunts = ['沙龙厅代用券', '49积分', '加油!', '移动办公室代用券', '499积分', '空间通用券', '梦想演播厅代用券', '5积分'];
            var container = document.getElementsByClassName('container')[0];
            var header = container.getElementsByClassName('header')[0];
            var lookup = container.getElementsByClassName('lookup')[0];
            var rulebox = container.getElementsByClassName('rulebox')[0];
            this.parent = document.getElementById('turnplate');
            this.canvas = this.parent.getElementsByTagName('img')[0];
            this.pointer = this.parent.getElementsByClassName('pointer')[0];
            var containerWidth = parseFloat(getComputedStyle(container).width);
            var bodyHeight = parseFloat(getComputedStyle(document.body).height);
            var turnWidth = parseFloat(getComputedStyle(this.parent).width);
            var ruleWidth = parseFloat(getComputedStyle(rulebox).width);
            var w = parseInt(getComputedStyle(this.parent).width);
            var pd = parseInt(getComputedStyle(this.parent).paddingLeft) * 2;
            header.style.top = bodyHeight * 0.17 + 'px';
            this.parent.style.top = bodyHeight * 0.27 + 'px';
            this.parent.style.left = (containerWidth - turnWidth) / 2 + 'px';
            lookup.style.top = bodyHeight * 0.654 + 'px';
            rulebox.style.top = bodyHeight * 0.75 + 'px';
            rulebox.style.left = (containerWidth - ruleWidth) / 2 + 'px';
            this.parent.style.height = w + 'px';
            this.cH = this.cW = w - pd;
            this.bRotate = false;
            var that = this;
            util.addTapEvent(this.pointer, function() {
                if (that.bRotate) return;
                that.bRotate = !that.bRotate;
                $.ajax({
                    url: 'http://120.25.106.244:9001/api/Roll/zp',
                    data: {
                        userGuid: guid
                    },
                    cache: false,
                    success: function(res) {
                        if (res.type == 'error') {
                            util.msgBox(res.data);
                            that.bRotate = false;
                            return;
                        }
                        that.rotate(res.result.t_Roll_Reward, oTipBox.show.bind(oTipBox));
                    }
                })
            })
        },
        rotateFn: function(item, txt, fn) {
            //  var angles = item * (360 / 8) - (360 / (8 * 2));
            //  angles = (angles < 270) ? 270 - angles : 360 - angles + 270;
            // 247.5 1  202.5 2 157.5 3  112.5 4 67.5 5 22.5 6 337.5 7 292.5 8
            var angles;
            switch (item) {
                case 1:
                    angles = 22.5;
                    break; // 空间通用券
                case 2:
                    angles = 337.5;
                    break; // 梦想演播厅代用券
                case 3:
                    angles = 247.5;
                    break; // 沙龙厅代用券
                case 4:
                    angles = 112.5;
                    break; // 移动办公室代用券
                case 5:
                    angles = 67.5;
                    break; // 499积分
                case 6:
                    angles = 202.5;
                    break; // 49积分
                case 7:
                    angles = 292.5;
                    break; // 5积分
                case 8:
                    angles = 157.5; // 加油
            }
            var rotateAngle = 2160 * 2 + angles;
            var that = this;
            (function() {
                //stats.begin();
                that.aniFrame = requestAnimationFrame(arguments.callee);
                angles += Math.ceil((rotateAngle - angles) * .01);
                that.canvas.style.webkitTransform = 'rotate(' + angles + 'deg)';
                if (angles >= rotateAngle) {
                    that.canvas.style.webkitTransform = 'rotate(' + rotateAngle + 'deg)';
                    that.bRotate = false;
                    getJiFen();
                    cancelAnimationFrame(that.aniFrame);
                    if (fn && (typeof fn == 'function')) fn(item, txt);
                }
                //stats.end();
            })();
        },
        rotate: function(item, fn) {
            var item = item || this.rnd(1, this.restaraunts.length);
            this.rotateFn(item, this.restaraunts[item - 1], fn);
        },
        rnd: function(n, m) {
            var random = Math.floor(Math.random() * (m - n + 1) + n);
            return random;
        }
    }
    // 提示信息
var oTipBox = {
    init: function() {
        this.tipBox = document.getElementsByClassName('tipbox')[0];
        this.shade = document.getElementsByClassName('shade')[0];
        this.ok = this.tipBox.getElementsByClassName('ok')[0];
        this.width = this.tipBox.offsetWidth;
        this.height = this.tipBox.offsetHeight;
        util.addTapEvent(this.ok, this.hide.bind(this));
    },
    show: function(index, text) {
        var winWidth = window.innerWidth;
        var winHeight = window.innerHeight;
        var b = this.tipBox.getElementsByTagName('b')[0];
        this.tipBox.style.top = (winHeight - this.height) / 2 + 'px';
        this.tipBox.style.left = (winWidth - this.width) / 2 + 'px';
        this.shade.style.top = 0;
        b.textContent = text;
        if (index == 8) {
            this.tipBox.classList.add('pulling');
        } else {
            this.tipBox.classList.remove('pulling');
        }
        this.tipBox.classList.add('fadeIn');
        this.shade.classList.add('fadeIn');
    },
    hide: function() {
        this.tipBox.classList.remove('fadeIn');
        this.shade.classList.remove('fadeIn');
        var that = this;
        setTimeout(function() {
            that.tipBox.style.top = that.shade.style.top = '5000px';
        }, 200)
    }
}
window.onload = function() {
    turnplate.init();
    oTipBox.init();
    $('.lookup a').attr('href','lotteryRecord.html?UserGuid='+guid);
    $.ajax({
        url: 'http://120.25.106.244:9001/api/roll/GetAll',
        cache: false,
        success: function(res) {
            turnplate.restaraunts = [];
            res.forEach(function(item, index) {
                turnplate.restaraunts.push(item.t_Roll_Title);
            })
            getJiFen(function() {
                document.getElementsByClassName('loading')[0].style.display = 'none';
            });
        }
    })
}
// document.addEventListener('DOMContentLoaded', function() {
//     window.stats = new Stats();
//     stats.showPanel(0);
//     document.body.appendChild(stats.dom);
// }, false);
function getJiFen(cb) {
    $.ajax({
        url: 'http://120.25.106.244:9001/api/Integral/GetIntegral',
        data: {
            UserGuid: guid
        },
        success: function(res2) {
            var html = '';
            res2.toString().split('').forEach(function(item, index) {
                html += '<b>' + item + '</b>';
            });
            $('.header p.jifen').html('您目前还剩' + html + '积分');
            if (cb && (typeof cb == 'function')) cb();
        }
    })
}