var cache = null;
$(function () {
    // t_PlaceOder_Ordered 0可预约 1不可
    $.ajax({
        url: 'http://120.25.106.244:9001/api/Place/GetPlaceStyle',
        cache: false,
        success: function (res) {
            $('.container').html(template('contentTemplate', { data: res }));
            bindEvent();
            cache = res;
            $.ajax({
                url: 'http://120.25.106.244:9001/api/Place/GetTimes',
                cache: false,
                data: { Guid: res[0].guid },
                success: function (res2) {
                    $('.datelistbox').html(template('timepickerTemplate', { data: res2 }));
                    $('.container').removeClass('fixed').siblings('.shade-div').remove();
                    timePicker.addEvent();
                }
            })
        }
    })
})
// 时间选择对象
var timePicker = {
    rDate: '',
    rTime: '',
    guid: '',
    open: function () {
        $('.tp-shade').removeClass('hide');
        $('.timepicker').css({
            top: ($(window).height() - $('.timepicker').height()) / 2,
            left: ($(window).width() - $('.timepicker').width()) / 2
        });
        $('.container').css({ top: -$('body').scrollTop() }).addClass('fixed');
    },
    close: function () {
        $('.timepicker').attr('style', '');
        $('.tp-shade').addClass('hide');
        $('.container').removeClass('fixed');
        $('body').scrollTop(Math.abs(parseInt($('.container').css('top'))));
    },
    addEvent: function () {
        // 选择日期
        $('.timepicker li>a').each(function () {
            addTapEvent(this, function (el) {
                $(el).parent().addClass('active').siblings('li').removeClass('active');
                timePicker.rDate = $(el).text();
            })
        })
        // 选择时间
        $('.timepicker li>div>a.available').each(function () {
            addTapEvent(this, function (el) {
                timePicker.close();
                timePicker.rTime = timePicker.rDate + ' ' + $(el).text();
                timePicker.guid = $(el).data('guid');
                $('.timebox a').removeClass('gray').text(timePicker.rTime);
            })
        })
    }
}
// DOM元素绑定事件
function bindEvent() {
    $('.banner .box img').length && banner.init();
    // 选择场地类型
    $('.typebox a').each(function () {
        addTapEvent(this, function (el) {
            var index = $(el).index();
            $(el).addClass('active').siblings().removeClass('active');
            $('.header').html(template('headerTemplate', { data: cache[index] }));
            $('.pricebox h2>span').text('￥' + cache[index].t_Place_Money);
            clearInterval(banner.interval);
            $('.banner .box img').length && banner.init();
            addTapEvent($('.aboutbox .content')[0], function (el) {
                $(el).toggleClass('active');
            });
            $('.timebox a').addClass('gray').text('请选择预约时间');
            timePicker.rTime = timePicker.rDate = '';
            timePicker.guid = '';
            $.ajax({
                url: 'http://120.25.106.244:9001/api/Place/GetTimes',
                cache: false,
                data: { Guid: cache[index].guid },
                success: function (res) {
                    $('.datelistbox').html(template('timepickerTemplate', { data: res }));
                    timePicker.addEvent();
                }
            })
        })
    });
    // 简介折叠和拉开
    addTapEvent($('.aboutbox .content')[0], function (el) {
        $(el).toggleClass('active');
    });

    // 打开时间选择
    addTapEvent($('.timebox')[0], timePicker.open);
    // 关闭时间选择
    addTapEvent($('.timepicker>.close')[0], timePicker.close);
    addTapEvent($('.tp-shade')[0], timePicker.close);
    // 提交预约
    addTapEvent($('.btn-reserve')[0], function (el) {
        // 得到预约时间
        var time = timePicker.rTime;
        var timeGuid = timePicker.guid;
        var spaceGuid = $('.typebox li.active a').data('guid');
        if (time == "") {
            tipInfo("请选择预约时间");
            return;
        }
        location.href = '/wxuser/PayPlace?StyleGuid=' + spaceGuid + '&TimeGuid=' + timeGuid;
    })
}
// 模拟tap
function addTapEvent(el, cb) {
    el.addEventListener('touchstart', function (e) {
        var touch = e.targetTouches[0];
        this.endX = this.startX = touch.clientX;
        this.endY = this.startY = touch.clientY;
    })
    el.addEventListener('touchmove', function (e) {
        var touch = e.targetTouches[0];
        this.endX = touch.clientX;
        this.endY = touch.clientY;
    })
    el.addEventListener('touchend', function (e) {
        e.preventDefault();
        var x = Math.abs(this.endX - this.startX);
        var y = Math.abs(this.endY - this.startY);
        if (x <= 5 && y <= 5) {
            (typeof cb == 'function') && cb(this);
        }
    })
}
// 生成日期数据 追加到datelistbox类
function yieldTime() {
    function getDaysInOneMonth(year, month) {
        month = parseInt(month, 10);
        var d = new Date(year, month, 0);
        return d.getDate();
    }
    var nowDate = new Date();
    var year = nowDate.getFullYear();
    var month = nowDate.getMonth() + 1;
    var day = nowDate.getDate();
    var dates = [];
    var days, dateStr, html = '';
    for (var i = month; i <= month + 2; i++) {
        days = getDaysInOneMonth(year, i);
        for (var j = day + 1; j <= days; j++) {
            dateStr = year + '-' + (i > 9 ? i : '0' + i) + '-' + (j > 9 ? j : '0' + j);
            html += '<li><a>' + dateStr + '</a>' + '<div><a>08:20-12:00</a><a>14:00-18:00</a></div></li>'
            //dates.push(dateStr);
        }
    }
    $('.datelistbox').html(html);
}

var banner = {
    init: function () {
        this.tid = this.curIndex = 0;
        this.timeout = 600;
        this.parent = document.getElementsByClassName('banner')[0];
        this.shade = document.getElementsByClassName('shade')[0];
        this.controls = this.parent.querySelectorAll('.tag span');
        this.box = this.parent.querySelector('.box');
        this.box.innerHTML += this.box.innerHTML + this.box.innerHTML;
        this.imgs = this.box.querySelectorAll('img');
        var padding = parseFloat(getComputedStyle(this.parent).paddingLeft) + parseFloat(getComputedStyle(this.parent).paddingRight);
        this.frameWidth = Math.ceil(this.parent.offsetWidth - padding);
        this.len = this.imgs.length / 3;
        this.baseLeft = -this.frameWidth * this.len;
        this.box.style.width = this.frameWidth * this.len * 3 + 'px';
        this.box.style.left = this.baseLeft + 'px';
        for (var i = 0; i < this.len * 3; i++) {
            this.imgs[i].style.width = this.frameWidth + 'px';
        }
        if (this.len < 2) return;
        this.shade.addEventListener('touchstart', this.onTouchstart, false);
        this.shade.addEventListener('touchmove', this.onTouchmove, false);
        this.shade.addEventListener('touchend', this.onTouchend, false);
        this.timer();
    },
    onTouchstart: function (e) {
        e.preventDefault();
        banner.stopTimer();
        banner.box.classList.remove('trans');
        if (banner.tid) {
            clearTimeout(banner.tid);
            banner.tid = 0;
            banner.box.style.left = banner.baseLeft - banner.frameWidth * banner.curIndex + 'px';
        }
        var touch = e.targetTouches[0];
        banner.startX = touch.clientX;
        banner.posX = parseFloat(banner.box.style.left);
        banner.startTime = Date.now();
    },
    onTouchmove: function (e) {
        e.preventDefault();
        var touch = e.targetTouches[0];
        banner.endX = touch.clientX;
        banner.box.style.left = banner.posX + banner.endX - banner.startX + 'px';
    },
    onTouchend: function (e) {
        e.preventDefault();
        var step = banner.endX - banner.startX;
        var time = Date.now() - banner.startTime;
        if (step < 0) {
            if ((step <= -banner.frameWidth / 2) || (step <= -banner.frameWidth / 8) && time < 300) {
                banner.curIndex++;
                banner.box.classList.add('trans');
                banner.box.style.left = banner.baseLeft - banner.frameWidth * banner.curIndex + 'px';
                if (banner.curIndex > banner.len - 1) {
                    banner.curIndex = 0;
                    banner.tid = setTimeout(function () {
                        banner.box.classList.remove('trans');
                        banner.box.style.left = banner.baseLeft + 'px';
                    }, banner.timeout)
                }
            } else {
                banner.box.classList.add('trans');
                banner.box.style.left = banner.baseLeft - banner.frameWidth * banner.curIndex + 'px';
            }
        } else if (step > 0) {
            if ((step > banner.frameWidth / 2) || (step > banner.frameWidth / 8 && time < 300)) {
                banner.curIndex--;
                banner.box.classList.add('trans');
                banner.box.style.left = banner.baseLeft - banner.frameWidth * banner.curIndex + 'px';
                if (banner.curIndex < 0) {
                    banner.curIndex = banner.len - 1;
                    banner.tid = setTimeout(function () {
                        banner.box.classList.remove('trans');
                        banner.box.style.left = banner.baseLeft - (banner.frameWidth * banner.curIndex) + 'px';
                    }, banner.timeout)
                }
            } else {
                banner.box.classList.add('trans');
                banner.box.style.left = banner.baseLeft - banner.frameWidth * banner.curIndex + 'px';
            }
        }
        banner.setTag();
        banner.timer();
    },
    autoSwitch: function () {
        banner.curIndex++;
        this.box.classList.add('trans');
        this.box.style.left = this.baseLeft - this.frameWidth * this.curIndex + 'px';
        if (this.curIndex > this.len - 1) {
            this.curIndex = 0;
            var that = this;
            this.tid = setTimeout(function () {
                that.box.classList.remove('trans');
                that.box.style.left = that.baseLeft + 'px';
            }, this.timeout);
        }
        this.setTag();
    },
    setTag: function () {
        for (var i = 0; i < this.len; i++) {
            if (i == this.curIndex) {
                this.controls[i].classList.add('current');
            } else {
                this.controls[i].classList.remove('current');
            }
        }
    },
    timer: function () {
        this.interval = setInterval(this.autoSwitch.bind(this), 3000);
    },
    stopTimer: function () {
        clearInterval(this.interval);
        this.interval = 0;
    }
}
template.helper('formatDate', function (dateStr, opt) {
    var ymd = opt && opt.ymd ? opt.ymd : '-';
    var hms = opt && opt.hms ? opt.hms : ':';
    var date = new Date(/^(\d)+$/.test(dateStr) ? parseInt(dateStr) : dateStr.replace(/-/g, '/'));
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    month = month > 9 ? month : '0' + month;
    var day = date.getDate();
    day = day > 9 ? day : '0' + day;
    return year + ymd + month + ymd + day;
})


// 显示提示信息
function tipInfo(t) {
    $('p.tipinfo').remove();
    if (window.tid) {
        clearTimeout(window.tid);
        window.tid = 0;
    }
    var winWidth = $(window).width();
    var winHeight = $(window).height();
    var $tipinfo = $('<p class="tipinfo">').text(t).css({
        position: 'fixed',
        textAlign: 'center',
        fontSize: '1rem',
        color: '#fff',
        borderRadius: '.5rem',
        background: 'rgba(0,0,0,.8)',
        padding: '.6rem 1rem',
        transform: 'translate3d(0,0,0)'
    }).appendTo('body');
    var left = (winWidth - $('p.tipinfo')[0].offsetWidth) / 2;
    var top = (winHeight - $('p.tipinfo')[0].offsetHeight) / 2;
    $tipinfo.css({ left: left, top: top });
    window.tid = setTimeout(function () {
        $tipinfo.css({ top: top + 10, opacity: 0, transition: 'top .6s,opacity .6s' });
        setTimeout(function () { $tipinfo.remove(); }, 600);
    }, 1200)
}
