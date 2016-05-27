var PIN = {};
PIN.LoadScript = function(url, callback, charset){
	var t = url.toLowerCase().substring(url.lastIndexOf('.') + 1);
	if(t==='js') {
    	var n = document.createElement("script");
    	n.type = "text/javascript";
	}else if(t==='css'){
		n = document.createElement('link');
		n.type = 'text/css';
		n.rel = 'stylesheet';
	}
    if(charset){n.setAttribute("charset",charset);}
    if (n.readyState){
        n.onreadystatechange = function(){
            if(n.readyState == "loaded" || n.readyState == "complete"){
                n.onreadystatechange = null;
				if(callback)callback();
                document.getElementsByTagName("head")[0].removeChild(this);
            }
        };
    }
    else {
        n.onload = function(){
			if(callback)callback();
            document.getElementsByTagName("head")[0].removeChild(this);
        };
    }
	if(t==='js') {
		n.src = url;
	}else if(t==='css'){
		n.href = url;
	}
    document.getElementsByTagName("head")[0].appendChild(n);
}

function defhover(durl){
	$(".type_List a,.type_List2 a,.navigation a,.top_menu a,#nav a").each(function(){
			 $(this).addClass($(this).attr('href')==durl?'hover':'');
	});
}

//PIN.LoadScript('Public/js/tips/tips.js');
//PIN.LoadScript('Public/js/swfObject.js');
PIN.LoadScript('js/lib.pincolor.min.js',function(){

	$('img[data-cache=true]').cache_images(function(a){$(a).lazyload({placeholder : "Public/images/com/touming.gif",effect : "fadeIn" });});
	
	$(".lazyimg img").lazyload({ 
		placeholder : "Public/images/com/touming.gif",
		effect : "fadeIn" 
	});

	jQuery("a[href='']").attr('target','_self').attr('href','javascript:void(0);');
	jQuery('a[href*=#]').click(function(){jQuery.scrollTo(jQuery(this).attr("href"),1000);return;});
	jQuery('.sct').click(function(){jQuery.scrollTo(jQuery(this).attr("href"),1000);return;});
	

	$(".gotop").click(function(){$.scrollTo(0,500);}); 
	//$(".nl").limit();
	defhover(window.location.href);

	$(".soChange").each(function(){
		var _self=$(this);
		var Ctime=_self.attr('Ctime')?_self.attr('Ctime'):3000;
		var delay=_self.attr('delay')?_self.attr('delay'):300;
		var Type=_self.attr('type')?_self.attr('type'):'fade';
		var slideTime=_self.attr('slideTime')?_self.attr('slideTime'):600;
		//alert(slideTime);changeType:'fade',//切换方式，可选：fade,slide，默认为fade
		_self.find("a.a_bigImg").soChange({
			thumbObj:_self.find('.soul li'),
			thumbNowClass:'on',//自定义导航对象当前class为on
			botPrev:_self.find('.top_pre'),
			botNext:_self.find('.top_next'),
			clickFalse:false,
			thumbOverEvent:true,
			overStop:true,
			slideTime:slideTime,
			delayTime:delay,
			changeType:Type,
			//autoChange:false,
			//changeType:'slide',
			changeTime:Ctime//自定义切换时间为4000ms
	
		});
	});
	jQuery(".hoverFv img,.hoverFi").hover(function(){
		jQuery(this).animate({opacity:.75},"fast");
	},function(){
		jQuery(this).animate({opacity:1},"fast");
	});

    $(".hoverFd").hover(function() {$(this).stop().animate({ opacity:0.60}, 200,function(){$(this).animate({ opacity:1}, 300);});});	
	
	var delayRun;
	$("#navul > li").hover(function(){
		$(this).addClass("hover");
		//$(this).find("ul").slideDown().end().siblings().removeClass("navmoon").find("ul").slideUp();
		//clearTimeout(delayRun);
	},function(){
		$(this).removeClass("hover");
		var _self=$(this);
		//$(this).removeClass("navmoon").find("ul").slideUp();
		//delayRun = setTimeout(function(){_self.removeClass("navmoon").find("ul").slideUp();_self.parent().find(".open").addClass("navmoon").find("ul").slideDown()},1000);
	})

	jQuery(".Vblock1,.Vblock2").hover(function(){
		jQuery(this).find("h3").stop().animate({top:"0px"}, 600,"");
	},function(){
		jQuery(this).find("h3").stop().animate({top:"-510px"}, 400);
	});

    $(".Vblock5 .vbox").click(function() {
		//var _self=$(this).parent();
		var _self=$(this);
		var url=_self.attr("data-url");
		var id=_self.attr("data-id");
		$(".Vblock5").each(function(index, element) {
            $(this).find(".video").html("<i id='"+$(this).find(".vbox").attr("data-id")+"'></i>");
			$(this).find(".vimg").show();
        });
		_self.find(".vimg").hide();
		
		if(url.indexOf(".flv")>-1){
		
			var flashvars = {vcastr_file:url,IsAutoPlay:"1"};
			var params = {wmode:"opaque",allowscriptaccess:"always",allowfullscreen:"true"};
			swfobject.embedSWF("Public/images/swf/vcastr22.swf",id, "720", "480", "9.0.0", "expressInstall.swf", flashvars,params);
		
		}else{
			
			var flashvars = {dataFile:"",showLogo:"false"};
			var params = {wmode:"opaque",wmode:"transparent",allowscriptaccess:"always",allowfullscreen:"true"};
			swfobject.embedSWF(url,id, "720", "480", "9.0.0", "expressInstall.swf", flashvars,params);
			
		}
		
		return false;
    });

	jQuery(".Vblock55").hover(function(){
		jQuery(this).find("h3").animate({height:"70px"},  "fast");
	},function(){
		jQuery(this).find("h3").animate({height:"53px"},  "fast");
	});

	$('#newsbody img').imgAutoSize(1160,8820);
	$("#sk2_scroll").jCarouselLite({
	    btnNext: "#hlove .next",
	    btnPrev: "#hlove .prev",
		mouseWheel: true,
		animation:'slow',
		visible: 2,
		auto: 2000, //自动滚动，2000（毫秒）=1秒，删除即不自动滚动
		scroll: 2,
		easing: "easeOutBack",
		vertical:true, //纵向滚动
		onMouse: true,
		speed: 800
	});

	//$('body').live('contextmenu', function(e) {return false;}); $('body').live('dragstart', function(e) {return false;});document.body.onselectstart = function(){return false;};document.body.onbeforecopy = function(){return false;};

});



function IEhtml5(){
	var args = IEhtml5.arguments;
	for(var i=0; i<args.length; i++)
	{
		//alert(args[i]);
		document.createElement(args[i]);
	}
}
IEhtml5("article","details","footer","header","nav","summary","time");



/*scroll.js*/
var colorList = ["#FF8399","#61C1BD","#D4B07B","#1C3D23","#248AAF","#EB7B3E","#254D9C","#56B7BA","#FFCC00","#FFAD5C"]; 
var clList = ["i1","i2"]; 
/*
for(var i=0;i<lineList.length;i++){ 
	var bgColor = getColorByRandom(colorList);
} 
*/
function getColorByRandom(colorList){ 
	var colorIndex = Math.floor(Math.random()*colorList.length); 
	var color = colorList[colorIndex]; 
	//colorList.splice(colorIndex,1); 
	return color; 
}

$(function(){
	$("#psjd .item").hover(
	
		function(){
			var that=this;
				//$(that).find("div").animate({width:280,height:342,left:0,top:0},300,function(){
					$(that).find("i").attr("class",getColorByRandom(clList))
					.fadeIn(200);
					$(that).find("span").css({"background-color":getColorByRandom(colorList),opacity:0.92})
					.fadeIn(200);
					
				//});
			},
		function(){
			var that=this;
			$(that).find(".i1").fadeOut(200);
			$(that).find(".i2").fadeOut(200);
			$(that).find("span").fadeOut(200);
			//$(that).find("div").stop().animate({width:0,height:0,left:78,top:101},300);
			}
		)
		$(".viimg").hover(
		function(){
			var that=this;
				//$(that).find("div").animate({width:280,height:342,left:0,top:0},300,function(){
					$(that).find(".xj").animate({left:0,top:0,opacity:1},300);
				//});
			},
		function(){
			var that=this;
			$(that).find(".xj").stop().animate({left:0,top:-349,opacity:0},300);
			//$(that).find("div").stop().animate({width:0,height:0,left:78,top:101},300);
			}
		)
		
	/*头部下拉效果*/	
	$(".subnav > a").hover(function(){
		$(".menu .v1").addClass("hover");
	},function(){
		$(".menu .v1").removeClass("hover");
		var _self=$(this);
	})
	  
		/*图片上移效果*/	

	  $(".con_part9").hover(function(){
		$(".action").stop().animate({top:"220px"},250);
	  },function(){
		$(".action").stop().animate({top:"240px"},250);
	  });


    $(".con_part3 img").hover(function() {$(this).stop().animate({ opacity:0.60}, 500,function(){$(this).animate({ opacity:1}, 500);});});	

	jQuery(".hoverFv img,.hoverFi").hover(function(){
		jQuery(this).animate({opacity:.65},"fast");
	},function(){
		jQuery(this).animate({opacity:1},"fast");
	});

	});




