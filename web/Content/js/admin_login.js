$(function(){
    $(window).resize(winResize).resize();
    $('.username').blur(function(){
        var reg=/^([1]([3][0-9]{1}|59|58|70|86|87|88|89)[0-9]{8})|(([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((.[a-zA-Z0-9_-]{2,3}){1,2}))$/;
        validField($(this),reg,'用户名不能为空！','用户名格式不正确！');
    })
    $('.password').blur(function(){
        validField($(this),/^\w{6,20}$/,'密码不能为空！','密码为6-20位字母或数字！');
    })
    $('[type=text],[type=password]').focus(function(){
        var $help=$(this).siblings('.help');
        if(!$help.hasClass('hide')){
            $help.html('').addClass('hide');
            $(this).val('');
        }
    })
    $('.login-btn').click(function(){
        loginHandler(this);
    })
})
// 视口改变大小
function winResize(){
    var winHeight=$(window).height();
    var $formbox=$('.subcontainer');
    var boxHeight=$formbox.height();
    var footerHeight=$('.footer').height();
    var testHeight=(winHeight-boxHeight)/2;
    $formbox.css({
        marginTop:testHeight>footerHeight?testHeight:20
    }).removeClass('hide');
}
// 验证文本框
function validField($input,reg,helpText1,helpText2){
    /*
    * $input:jquery文本框包装对象
    * reg:正则表达式,不需要请传递一个返回false的值
    * helpText1:非空验证提示
    * helpText2:正则验证提示
    * */
    var value= $.trim($input[0].value);
    var $help=$input.siblings('.help');
    if(!value){
        $help.html(helpText1).removeClass('hide');
        return false;
    }else{
        if(reg&&!reg.test(value)){
            $help.html(helpText2).removeClass('hide');
            return false;
        }
    }
    $help.html('').addClass('hide');
    return true;
}
// 比较密码输入框的值
function isIdentical($password,$repassword){
    var val1=$password.val();
    var val2=$repassword.val();
    var $help=$repassword.siblings('.help');
    var helpText='两次输入的密码不一致！'
    if(val1!=val2){
        $help.html(helpText).removeClass('hide');
        return false;
    }
    $help.html('').addClass('hide');
    return true;
}
// 登录
function loginHandler(el){
    var $scope=$('#formbox li.item1');
    var $username=$scope.find('.username');
    var $password=$scope.find('.password');
    var $remember=$scope.find('.remember');
    var regPhone=/^([1]([3][0-9]{1}|59|58|70|86|87|88|89)[0-9]{8})$/;
    var regEmail=/^(([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((.[a-zA-Z0-9_-]{2,3}){1,2}))$/;
    var regUsername=/^([1]([3][0-9]{1}|59|58|70|86|87|88|89)[0-9]{8})|(([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((.[a-zA-Z0-9_-]{2,3}){1,2}))$/;
    var regPassword=/^\w{6,20}$/;
    // 验证
    if(validField($username,regUsername,'用户名不能为空！','手机号格式不正确！')&&
        validField($password,regPassword,'密码不能为空！','密码为6-20位字母,数字或符号')){
        var data={
            username:$username.val(),
            password:$password.val(),
            remember:$remember.is(':checked')
        };
        var postUrl=$(el).data('url');
        $.post(postUrl,data,function(res){
            if(res.success=='false'){
                $scope.find(res.errField).val('').siblings('.help').html(res.message).removeClass('hide');
            }else{
                alert('登录成功')
            }
        })
    }
}