// 调整盒子位置
function reLayout(){
    var conHeight=$('.container').height();
    var winHeight=$(window).height();
    var headHeight=$('.header').height();
    var conTop=(winHeight>=conHeight+headHeight*2+20)?(winHeight-conHeight)/2-headHeight:10;
    $('body').css({height:winHeight});
    $('.container').css({marginTop:conTop});
}
$(function(){
    $('.container').removeClass('hide');
    $(document)
        // 切换为扫码登陆
        .on('click','.loginbox .btn-mode',function(){
            $(this).parent().addClass('hide').siblings('.qrcodebox').removeClass('hide');
        })
        // 切换为普通登陆
        .on('click','.qrcodebox .btn-mode',function(){
            $(this).parent().addClass('hide').siblings('.loginbox').removeClass('hide');
        })
})
$(window).bind('resize',reLayout).trigger('resize');
var regPhone = /^([1]((3|5|7|8)[0-9]{1})[0-9]{8})$/;
$(function() {
    $('.btn-submit').click(signUpHandler);
    $('.phone').keyup(banNaN);
    $('.eye').click(function(){
        $(this).toggleClass('visible');
        if($(this).hasClass('visible')){
            $(this).siblings('.password').attr('type','text');
        }else{
            $(this).siblings('.password').attr('type','password');
        }
    })
})
// 表单提交
function signUpHandler() {
    var $phone = $('.phone');
    var $password = $('.password');
    if (!validField($phone, regPhone, '手机号不能为空！', '手机号格式有误')) return;
    if (!validField($password, null, '密码不能为空！')) return;
    var data = {
        phone: $phone.val(),
        password: $password.val()
    };
    console.log(data);
}
// 验证文本框
function validField($input, reg, helpText1, helpText2) {
    var value = $.trim($input[0].value);
    if (!value) {
        $('.help').text(helpText1).removeClass('hidden');
        $input[0].value = '';
        return false;
    } else {
        if (reg && !reg.test(value)) {
            $('.help').text(helpText2).removeClass('hidden');
            return false;
        }
    }
    $('.help').text('').addClass('hidden');
    return true;
}
// 禁止输入非数值
function banNaN() {
    $(this).val($(this).val().replace(/[^(\d)]/g, ''));
}