$(function(){
	/*左侧的列表图标背景的切换*/
	$(".content .left li").hover(function(){
		var index=$(this).index();
		$(this).addClass("active");
	},function(){
		var index=$(this).index();
		$(this).removeClass('active');
	});
	/*判断点击来切换显示不同的页面*/
	$(".content .left li").click(function(){
		$(".content .left li").removeClass("active active1");
		$(this).addClass('active1');
	});
	/*是否审核切换,切换到已审核的地方*/
	$(".content .spaceManage .havePass:eq(0)").click(function(){
		$(".content .spaceManage .noPass:eq(0)").css("color","#747474");
		$(".content .spaceManage .noPass:eq(0)").find("img").css("display","none");
		$(this).css("color","#000");
		$(this).find("img").css("display","block");
		/*切换下面的内容*/
		$(".spaceManage .HavespaceContent").css("display","block");
  		//$(".spaceManage .spaceBlank").css("display","none");
  		$(".spaceManage .NospaceContent").css("display","none");
	});
	/*是否审核切换,切换到未审核的地方*/
	$(".content .spaceManage .noPass:eq(0)").click(function(){
		$(".content .spaceManage .havePass:eq(0)").css("color","#747474");
		$(".content .spaceManage .havePass:eq(0)").find("img").css("display","none");
		$(this).css("color","#000");
		$(this).find("img").css("display","block");
		/*切换下面的内容*/
  		$(".spaceManage .HavespaceContent").css("display","none");
  		$(".spaceManage .spaceBlank").css("display","none");
  		$(".spaceManage .NospaceContent").css("display","block");
	});
	/**/
});
 function openBrowse(obj){
			//判断浏览器的兼容性问题
			var ie=navigator.appName=="Microsoft Internet Explorer" ? true : false; 
			if(ie){ //如果是ie浏览器
				document.getElementById(obj).click(); 
				document.getElementById("filename").value=document.getElementById(obj).value;
			}else{
				var a=document.createEvent("MouseEvents");//FF的处理 
				a.initEvent("click", true, true);  
				document.getElementById(obj).dispatchEvent(a); 
			} 	 
		}

function syt_showImg(id){		
	 $("#"+id).on('change',function () {		  
       		     if (this.files != undefined) {
		                var file = this.files[0];
		                var imgUrl = getObjectURL(file);
		                if (file.type.indexOf('image') == -1) {
		                    alert('请选择图片文件格式！');
		                    return;
		                } else if (file.size > 1024 * 1024 * 5) {
		                    alert('请选择小于5M的文件');
		                    return;
		                }
		               
		                if (imgUrl) {
							
		                    $(this).parent().prev().find("img").attr("src",imgUrl);
							
		                }
		            } else {
		                alert('不好意思，你的浏览器不支持文件对象,建议使用IE10+,chrome,火狐任一浏览器访问本页面');
		            }
	  });
}
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
 $(function(){
	 syt_showImg("file1");//显示房间
	 syt_showImg("file");
 });
 /*--上传房间图片结束--*/
  /*下拉的菜单*/
 $(".selectBox input").click(function(){
	var thisInput=$(this);
	var thisUl=$(this).parent().find("ul");
	if(thisUl.css("display")=="none"){
		 if(thisUl.height()>200){
			thisUl.css({height:"200"+"px","overflow-y":"scroll" }
		 )};
		 thisUl.fadeIn("100");
		thisUl.hover(function(){},function(){thisUl.fadeOut("100");})
		 thisUl.find("li").click(function(){
		 thisInput.val($(this).text());
		 thisUl.fadeOut("100");}).hover(function(){$(this).addClass("hover");},function(){$(this).removeClass("hover");
		  });
	  }
	 else{
		  thisUl.fadeOut("fast");
		 }
 });
 /*添加房间*/
 $(".houseHead .addHouse").click(function () {
     $(".cover").css('display', "block");
     $(".houseAdd").css("display", "block");
 });
 /*添加房间*/
 $(".addHouse").click(function () {
     $(".cover").css('display', "block");
     $(".houseAdd").css("display", "block");
 });
 /*房间按钮的保存和取消*/
 $(".houseAdd .reset").click(function () {
     $(".cover").css("display", "none");
     $(".houseAdd").css("display", "none");
 });
 //$(".houseAdd .submit").click(function () {
 //    $(".cover").css("display", "none");
 //    $(".houseAdd").css("display", "none");
 //});

 /*下拉菜单的结束*/
 /*申请入驻*/
 //$(".content .spaceManage .spaceHead .shenqing").click(function(){
 //	$(this).parent().css("display","none");
 //	$(".spaceBlank,.HavespaceContent, .NospaceContent").css('display',"none");
 //	$(".spaceApply").css("display","block");
 //});
 /*申请入驻*/
 //$(".spaceManage .spaceBlank .shenqing").click(function(){
 //	$(".spaceManage .spaceBlank").css("display","none");
 //	$(".spaceManage .spaceHead").css("display","none");
 //	$(".spaceApply").css("display","block");
 //});
   /*查看没有审核的内容*/
  $(".houseHead .noPass:eq(0)").click(function(){
  	$(".spaceManage .HavespaceContent").css("display","none");
  	$(".spaceManage .spaceBlank").css("display","none");
  	$(".spaceManage .NospaceContent").css("display","block");
  });
