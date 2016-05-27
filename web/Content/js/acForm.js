/********************************* 表单操作交互 *********************************/
// 活动日期
$(function () {
    $('.post_date input').date_input();
    $('.time_list').html(timeArr());
    $('body')
        // 显示时间选择列表 如果日期不为空 则显示
        .on('click', '#hd_date .post_time', function () {
            var date_text = $(this).prev().find('input').val();
            if (date_text) {
                $(this).find('.time_list').removeClass('hide');
            } else {
                tipInfo('请先输入' + $(this).prev('div').find('input').attr('placeHolder'));
            }
        })
        // 时间选择列表项单击
        .on('click', '#hd_date .time_list li', function () {
            var $par = $(this).parent();
            $par.siblings('input').val($(this).text());
            setTimeout(function () { $par.addClass('hide'); }, 0)
        })
        // 日期文本框改变 如果不为空 显示清空按钮
        .on('change', '#hd_date .post_date input', function () {
            this.value && $(this).siblings('.qk').removeClass('hide');
        })
        // 清空日期文本框 同时隐藏清空按钮
        .on('click', '#hd_date .post_date .qk', function () {
            $(this).siblings('input').val('').end().addClass('hide')
                .parent().next().find('input').val('');
        })
        // 隐藏时间选择列表
        .on('mouseup', function (e) {
            var $tar = $(e.target);
            if (!($tar.hasClass('time_list'))) {
                $('#hd_date .time_list').addClass('hide');
            }
        })
})

// 生成 小时:分钟 相隔半小时 数组
function timeArr() {
    var arr = [];
    for (var i = 0; i < 24; i++) {
        if (i < 10) {
            arr.push('<li>0' + i + ':' + '00</li>');
            arr.push('<li>0' + i + ':' + '30</li>');
        } else {
            arr.push('<li>' + i + ':' + '00</li>');
            arr.push('<li>' + i + ':' + '30</li>');
        }
    }
    return arr.join('');
}
// 选择地区
$(function () {
    $('body')
        // 显示地区列表
        .on('click', '#hd_site .sel_area', function () {
            $(this).siblings('.area_list').removeClass('hide');
        })
        // 关闭地区列表
        .on('click', '#hd_site .area_list .close', function () {
            $(this).parents('.area_list').addClass('hide')
                .find('.area_city').addClass('hide')
                .find('li').removeClass('active')
                .end().siblings('.area_qu').addClass('hide')
                .find('li').removeClass('active');
        })
        // 显示城市列表
        .on('click', '#hd_site .area_prov li', function () {
            var pid = this.id;
            //根据省份获取
            $.get('/activity/GetCity', { Id: pid }, function (msg) {
                if (msg) {
                    //替换内容                    
                    $(".area_city").html(msg);
                    $('#provId').val(pid);
                }
            })
            $(this).addClass('active')
                .siblings().removeClass('active')
                .parent().siblings('.area_city').removeClass('hide');
        })
        // 显示区县列表
        .on('click', '#hd_site .area_city li', function () {
            var cid = this.id;
            //根据省份获取
            $.get('/activity/GetDis', { Id: cid }, function (msg) {
                if (msg) {
                    //替换内容                    
                    $(".area_qu").html(msg);
                    $('#cityId').val(cid);
                }
            })
            $(this).addClass('active')
                .siblings().removeClass('active')
                .parent().siblings('.area_qu').removeClass('hide');
        })
        // 为选择地区文本框赋值 并关闭地区列表
        .on('click', '#hd_site .area_qu li', function () {
            var text = '';
            $('#hd_site .area_list li').each(function () {
                if ($(this).hasClass('active')) {
                    text += $(this).text() + ' ';
                }
            })
            text += $(this).text();
            $('#quId').val(this.id);
            $(this).parent().parent().siblings('.sel_area').val(text)
                .siblings('.qk').removeClass('hide')
                .siblings('.area_list').addClass('hide')
                .find('.area_city').addClass('hide')
                .find('li').removeClass('active')
                .end().siblings('.area_qu').addClass('hide')
                .find('li').removeClass('active');
        })
        // 清空地区文本框
        .on('click', '#area .qk', function () {
            $(this).siblings('.sel_area').val('')
                .end().addClass('hide')
                .siblings('.area_list')
                .find('.area_prov li').removeClass('active');
        })
})

// 数值输入框 活动费用单选框
$(function () {
    $('body')
        // 期望数值的输入框禁止输入非数值
        .on('keyup', '#hd_phone input,#hd_fee input[type=text],#hd_limit input', function (e) {
            $(this).val($(this).val().replace(/[^(\d)]/g, ''));
        })
        // 添加 元
        .on('blur', '#hd_fee input[type=text]', function () {
            var val = $.trim($(this).val());
            if (val) {
                $(this).siblings('p').text(this.value + '元').removeClass('hide');
            }
        })
        // 允许 number 人报名
        .on('blur', '#hd_limit input', function () {
            var val = $.trim($(this).val());
            if (val) {
                $(this).siblings('p').text('允许' + this.value + '人报名').removeClass('hide');
            }
        })
        // 显示input输入框 并让其得到焦点
        .on('click', '#hd_limit p,#hd_fee p', function () {
            $(this).addClass('hide').siblings('input').focus();
        })
        // 活动费用的免费和收费切换
        .on('click', '#hd_fee label', function () {
            var index = $(this).index();
            var $fee = $(this).siblings('div');
            if (index == 1) {
                $fee.removeClass('hide');
            } else {
                $fee.addClass('hide');
            }
        })
})

/********************************* 提交活动表单 上传封面图片 *********************************/
// 提交活动显示加载中... 提交成功隐藏加载中...
var loadbox = {
    show: function () {
        loadbox.pos();
        $('#cover').show();
        $('#post_loading').show();
    },
    hide: function () {
        $('#cover').hide();
        $('#post_loading').hide();
    },
    pos: function () {
        var winHeight = $(window).height();
        var winWidth = $(window).width();
        var ldHeight = $('#post_loading').height() + parseInt($('#post_loading').css('padding-top'));
        var ldWidth = $('#post_loading').width();
        $('#post_loading').css({
            top: (winHeight - ldHeight) / 2,
            left: (winWidth - ldWidth) / 2
        })
    }
}
$(function () {
    $('body')
        // 提交活动表单
        .on('click', '.btn-fb', postForm)
        // 模拟文件文本框单击
        .on('click', '#hd_cover .btn-upload', function () {
            $(this).siblings('input').click();
        })
        // 上传封面图
        .on('change', '#hd_cover [type=file]', function () {
            if (this.files != undefined) {
                var file = this.files[0];
                var imgUrl = getObjectURL(file);
                if (file.type.indexOf('image') == -1) {
                    tipInfo('请选择图片文件格式！');
                    return;
                } else if (file.size > 1024 * 1024 * 2) {
                    tipInfo('请选择小于2M的文件');
                    return;
                }
                // $(this).siblings('a').css({
                //     backgroundImage: 'url(/content/image/loading.gif)'
                // });
                if (imgUrl) {
                    $(this).siblings('a').css({
                        backgroundImage: 'url(' + imgUrl + ')'
                    });
                }
            } else {
                tipInfo('不好意思，你的浏览器不支持文件对象,建议使用IE10+,chrome,火狐任一浏览器访问本页面');
            }
        })
})
// 获取file对象URL
function getObjectURL(file) {
    var url = null;
    if (window.createObjectURL != undefined) { // basic
        url = window.createObjectURL(file);
    } else if (window.URL != undefined) { // mozilla(firefox)
        url = window.URL.createObjectURL(file);
    } else if (window.webkitURL != undefined) { // webkit or chrome
        url = window.webkitURL.createObjectURL(file);
    }
    return url;
}

// 发布活动
function postForm() {
    var _title = $.trim($("#hd_title input").val()),// 活动主题
        _content = um1.getContent(), // 活动详情
        _detailAddr = $.trim($("#address input").val()),// 活动地址
        _detailCity = $(".sel_area").val(),// 活动城市
        _startymd = $("#start_date>input").val(), // 开始时间年月日
        _starthm = $("#start_time>input").val(),// 开始小时和分钟
        _overymd = $("#end_date>input").val(),// 结束时间年月日
        _overhm = $("#end_time>input").val(), // 结束时间小时和分钟
        _organizers = $.trim($('#hd_organizers input').val()), // 举办方
        _phone = $('#hd_phone input').val(), // 资讯电话
        _isPay = $('input:radio[name="fee"]:checked').val(), // 免费 收费 单选
        _perlim = $('#hd_limit input').val(), // 人数上限 为'0'无上限
        partyThem = "活动";
    var _startdate = "", // 开始时间
        _overdate = "", // 结束日期
        _payPrice = ""; // 费用
    //var _content = um1.getContent();// 活动详情
    //um1.sync();//编辑器异步提交
    // 主题不能为空 并不能多于35个字
    if (!_title) {
        tipInfo("请输入" + partyThem + "主题");
        return;
    }
    if (_title.length > 35) {
        tipInfo("主题请在35个字以内");
        return;
    }
    // 活动详情不能为空 并不能低于10个字 不能多于10000字
    if (!_content) {
        tipInfo("请输入" + partyThem + "详情");
        return;
    }
    if (_content.length < 10) {
        tipInfo(partyThem + "详情写得太少了");
        return;
    }
    if (_content.length > 10000) {
        tipInfo(partyThem + "详情写得有点多了");
        return;
    }
    // 开始和结束时间不能为空 开始时间必须大于当前时间 但不能大于结束时间
    if (!(_startymd && _starthm)) {
        tipInfo("请选择" + partyThem + "开始的具体时间");
        return;
    } else {
        _startdate = _startymd + " " + _starthm;
    }
    if (!(_overymd && _overhm)) {
        tipInfo("请选择" + partyThem + "结束的具体时间");
        return;
    } else {
        _overdate = _overymd + " " + _overhm;
    }
    if (new Date(_startdate) - new Date() < 0) {
        tipInfo("开始时间不能小于当前时间");
        return;
    }
    if (new Date(_overdate) - new Date(_startdate) < 0) {
        tipInfo("开始时间不能大于结束时间");
        return;
    }
    // 活动城市和地址不能为空 活动地址不能大于35个字
    if (!(_detailCity && _detailAddr)) {
        tipInfo("请输入" + partyThem + "的具体地点");
        return;
    }
    if (_detailAddr.length > 35) {
        tipInfo("地址请在35个字以内");
        return;
    }
    // 举办方不能为空 并不能多于70个字
    if (!_organizers) {
        tipInfo("请输入" + partyThem + "举办方");
        return;
    }
    if (_organizers.length > 70) {
        tipInfo("举办方请在70个字以内");
        return;
    }
    // 资讯电话不能为空 并不能多于12个字
    //var phoneReg = /(^((0[1,2]{1}\d{1}-?\d{8})|(0[3-9]{1}\d{2}-?\d{7,8}))$)|(^1[3,5,7,8]{1}[0-9]{9}$)/;
    var phoneReg = /^1[3,5,7,8]{1}[0-9]{9}$/;
    if (!_phone) {
        tipInfo("请输入" + partyThem + "资讯电话");
        return;
    }
    if (!(phoneReg.test(_phone))) {
        tipInfo("电话号码格式不正确");
        return;
    }
    // 费用和人数上限
    if (_isPay == '0') {
        _payPrice = '0';
    } else if (_isPay == '1') {
        _payPrice = $.trim($('#hd_fee [type=text]').val());
    }
    if (_perlim) {
        if (isNaN(_perlim)) {
            tipInfo("人数上限必须是数字");
            return;
        } else {
            if (parseInt(_perlim) < 0) {
                tipInfo("人数上限必须大于等于0");
                return;
            }
        }
    } else {
        _perlim = "0";
    }
    if (_payPrice) {
        if (isNaN(_payPrice)) {
            tipInfo("费用必须是数字");
            return;
        } else {
            if (parseInt(_payPrice) < 0) {
                tipInfo("费用必须大于等于0");
                return;
            }
        }
    } else {
        _payPrice = "0";
    }
    //发布活动
    loadbox.show();
    // 上传封面图片
    var _coverPic = null;
    if ($('#hd_cover [type=file]').get(0).files != undefined) {
        if ($('#hd_cover [type=file]').get(0).files.length == 0) {
            tipInfo("请上传封面图片");
            loadbox.hide();
            return;
        }
    } else {
        tipInfo('不好意思，你的浏览器不支持文件对象,建议使用IE10+,chrome,火狐任一浏览器访问本页面');
        loadbox.hide();
        return;
    }

    var param = {
        t_Activity_Title: _title, // 活动主题
        t_Activity_sDate: _startdate, // 开始时间
        t_Activity_eDate: _overdate, // 结束时间
        t_Activity_CityName: '', // 活动地区
        t_Activity_Street: _detailAddr, // 活动具体地址
        t_Activity_Instruction: _content, // 活动详情
        t_Activity_Holder: _organizers, // 举办方
        t_Activity_Tel: _phone, // 咨询电话
        t_Activity_Fee: _payPrice, // 活动费用
        t_Activity_LimitPerson: _perlim, // 人数上限
        t_Activity_FeeType: '免费',
        t_Activity_Province: $('#provId').val(),
        t_Activity_City: $('#cityId').val(),
        t_Activity_District: $('#quId').val()
    }
    //发布活动提交
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '/activity/publish',
        cache: false,
        data: param,
        success: function (res) {

            if (res.type == "success") {
                //活动发布成功，上传图片到服务器
                _coverPic = $('#hd_cover [type=file]').get(0).files[0]; // 封面图片
                var formData = new FormData();
                formData.append('Guid', res.Remark);
                formData.append('upfile', _coverPic)
                $.ajax({
                    url: "/activity/UpImg",
                    cache: false,
                    contentType: false,
                    processData: false, //如果要发送 DOM 树信息或其它不希望转换的信息，请设置为 false。  
                    data: formData,
                    type: 'post',
                    success: function (msg) {
                        if (msg.type == 'error') {
                            tipInfo(msg.Data);
                        }
                    }
                })
            }
            tipInfo(res.Data);
            loadbox.hide();
        }
    })
    //$.post('/activity/publish', $(param).serialize(), function (res) {
    //    //loadbox.hide();

    //})
}
// 提示信息 帮助函数
function tipInfo(t) {
    $('p.tipinfo').remove();
    if (window.tid) {
        clearTimeout(window.tid);
        window.tid = 0;
    }
    var winWidth = $(window).width();
    var $tipinfo = $('<p class="tipinfo">').text(t).css({
        position: 'fixed',
        textAlign: 'center',
        fontSize: '14px',
        color: '#fff',
        borderRadius: '6px',
        background: 'rgba(0,0,0,.8)',
        padding: '8px 15px',
        transform: 'translate3d(0,0,0)',
        bottom: '200px'
    }).appendTo('body').css({
        left: (winWidth - $('p.tipinfo')[0].offsetWidth) / 2
    });
    window.tid = setTimeout(function () {
        $tipinfo.stop().animate({
            bottom: '-=20px',
            opacity: 0
        }, 600, function () {
            $(this).remove();
        });
    }, 1500)
}

/********************************* 下载APP盒子 *********************************/
$(function () {
    // 视口改变大小
    $(window).resize(function () {
        $('#indexTc').length && $('#indexTc').css('display') != 'none' && appDownBoxPos();
    }).resize();
    // 下载app
    $('.appDown').click(function () {
        appDownBoxPos();
        $('#cover2').show();
        $('#indexTc').show();
        $('#indexTc .qieHover li').eq(0).click();
    })
    // 关闭下载盒子
    $('.guanbiQu', '#indexTc').click(function () {
        $(this).parents('#indexTc').hide();
        $('#cover2').hide();
    })
    // 切换下载和关注
    $('.qieHover li', '#indexTc').click(function () {
        $(this).addClass('thisOver').siblings().removeClass('thisOver');
        $('.tcNr li').eq($(this).index()).show().siblings('li').hide();
    })
})
// 调节下载盒子的位置
function appDownBoxPos() {
    var boxHeight = $('#indexTc').height();
    var boxWidth = $('#indexTc').width();
    var winHeight = $(window).height();
    var winWidth = $(window).width();
    $('#indexTc').css({
        top: boxHeight > winHeight ? 0 : (winHeight - boxHeight) / 2,
        left: boxWidth > winWidth ? 0 : (winWidth - boxWidth) / 2
    });
}
/********************************* 调整图层 *********************************/
$('body')
    .on('click', '.edui-icon-image,.edui-icon-map,.edui-btn-link', function () {
        if ($('.edui-icon-fullscreen').is('.isfullscreen')) return;
        $('body').addClass('z-index').css({
            height: $(window).height()
        });
    })
    .on('click mouseup', '.edui-modal-backdrop,.edui-close,.edui-modal-footer>div', function (e) {
        if ($('.edui-icon-fullscreen').is('.isfullscreen')) return;
        $('body').removeClass('z-index').css({
            height: 'auto'
        });
    })
    .on('click', '.edui-icon-fullscreen', function () {
        $('body').toggleClass('z-index');
        $(this).toggleClass('isfullscreen');
    })