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
		var index=$(this).index();
		$(".content .left li").removeClass("active active1");
		$(this).addClass('active1');
	});
})
