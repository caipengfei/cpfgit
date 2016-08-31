   function dom(id){
		return document.getElementById(id);
}
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
	syt_showImg("file");//显示图片
	 syt_showImg("file1");//显示房间
 });
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
 /*下拉菜单的结束*/


		
	//分组
window.onload=function(){
	var bSave=false;
	dom("shenqing").onmouseover=function(){
		dom("cover").style.opacity=0.2;
		dom("cover").style.filter="alpha(opacity:"+20+")";
	}
	dom("shenqing").onmouseout=function(){
		dom("cover").style.opacity=0;
		dom("cover").style.filter="alpha(opacity:"+0+")";
	}
	dom("groupContent").onfocus=function(){
		if(this.value=="请输入您的空间所属分组，如青创汇郑州，青创汇长沙"){
			this.value="";
		}		
		this.style.color="#000";
	}
	dom("groupContent").onblur=function(){
		if(this.value==""){
			dom("groupInfor").style.display="block";
			dom("groupBg").style.backgroundColor="#ffe6e6";

		}
		else{
			dom("groupInfor").style.display="none";
			dom("groupBg").style.backgroundColor="";
		}
	}
	//空间名称
	dom("nameContent").onfocus=function(){
		if(this.value=="请输入您的空间名称(15字以内)"){
			this.value="";
		}
		this.style.color="#000";
	}
	dom("nameContent").onblur=function(){
		if(this.value==''){
			dom("nameInfor").style.display="block";
			dom("nameBg").style.backgroundColor="#ffe6e6";
		}
		else{
			dom("nameInfor").style.display="none";
			dom("nameBg").style.backgroundColor="";
		}
		
	}
	//详细地址
	dom("addressContent").onfocus=function(){
		if(this.value=="请输入详细的街道地址"){
			this.value="";
		}
		this.style.color="#000";
	}
	dom("addressContent").onblur=function(){
		if(this.value==""){
			dom("addressInfor").style.display="block";
			dom("addressBg").style.backgroundColor="#ffe6e6";

		}
		else{
			dom("addressInfor").style.display="none";
			dom("addressBg").style.backgroundColor="";
		}
	}
	//一句话简介
	dom("summaryContent").onfocus=function(){
		if(this.value=="请填写空间简介(20字以内)"){
			this.value="";
		}
		this.style.color="#000";
	}
	dom("summaryContent").onblur=function(){
		if(this.value==""){
			dom("summaryInfor").style.display="block";
			dom("summaryBg").style.backgroundColor="#ffe6e6";

		}
		else{
			dom("summaryInfor").style.display="none";
			dom("summaryBg").style.backgroundColor="";
		}
	}
	//提供服务
	dom("serviceContent").onfocus=function(){
		if(this.value=="说说您的众创空间能为创客提供那哪些服务,让更多的人为对您的项目感兴趣..."){
			this.value="";
		}
		this.style.color="#000";
	}
	dom("serviceContent").onblur=function(){
		if(this.value==""){
			dom("serviceInfor").style.display="block";
			dom("serviceBg").style.backgroundColor="#ffe6e6";

		}
		else{
			dom("serviceInfor").style.display="none";
			dom("serviceBg").style.backgroundColor="";
		}
	}
	//详细介绍
	dom("introduceContent").onfocus=function(){
		if(this.value=="说说您的众创空间的优势和亮点，让更多的人对您的项目感兴趣..."){
			this.value="";
		}
		this.style.color="#000";
	}
	dom("introduceContent").onblur=function(){
		if(this.value==""){
			dom("introduceInfor").style.display="block";
			dom("introduceBg").style.backgroundColor="#ffe6e6";

		}
		else{
			dom("introduceInfor").style.display="none";
			dom("introduceBg").style.backgroundColor="";
		}
	}
	//标签
	dom("labelContent").onfocus=function(){
		if(this.value=="请填写空间特色,多以逗号隔开"){
			this.value="";
		}
		this.style.color="#000";
	}
	dom("labelContent").onblur=function(){
		if(this.value==""){
			dom("labelInfor").style.display="block";
			dom("labelBg").style.backgroundColor="#ffe6e6";

		}
		else{
			dom("labelInfor").style.display="none";
			dom("labelBg").style.backgroundColor="";
		}
	}
	//入驻条件
	dom("entryContent").onfocus=function(){
		if(this.value=="请填写入驻条件"){
			this.value="";
		}
		this.style.color="#000";
	}
	//政策
	dom("policyContent").onfocus=function(){
		if(this.value=="请填写优惠政策,如税收优惠,政策扶持"){
			this.value="";
		}
		this.style.color="#000";
	}
	//添加房间信息
	dom("house1").onclick=function(){
		dom("houseAdd").style.display="block";
		dom("houseSave").style.display="none";
	}
	//房间类型
	dom("houseTypeContent").onfocus=function(){
		if(this.value=="请填写房间类型"){
			this.value="";
		}
		this.style.color="#000";
	}
	dom("houseTypeContent").onblur=function(){
		if(this.value==""){
			dom("houseTypeInfor").style.display="block";
			dom("houseTypeBg").style.backgroundColor="#ffe6e6";

		}
		else{
			dom("houseTypeInfor").style.display="none";
			dom("houseTypeBg").style.backgroundColor="";
		}
	}
	//房间收费
	dom("houseMoneyContent").onfocus=function(){
		if(this.value=="请填写空间服务费用"){
			this.value="";
		}
		this.style.color="#000";
	}
	dom("houseMoneyContent").onblur=function(){
		if(this.value==""){
			dom("houseMoneyInfor").style.display="block";
			dom("houseMoneyBg").style.backgroundColor="#ffe6e6";
		}
		else{
			dom("houseMoneyInfor").style.display="none";
			dom("houseMoneyBg").style.backgroundColor="";
		}
	}
	//详细介绍
	dom("houseDetailContent").onfocus=function(){
		if(this.value=="请填写优惠政策，如税收优惠，政策扶持"){
			this.value="";
		}
			this.style.color="#000";
	}
	dom("houseDetailContent").onblur=function(){
		if(this.value==""){
			dom("houseDetailInfor").style.display="block";
			dom("houseDetailBg").style.backgroundColor="#ffe6e6";
		}
		else{
			dom("houseDetailInfor").style.display="none";
			dom("houseDetailBg").style.backgroundColor="";
		}
	}
	//备注
	dom("houseRemarkContent").onfocus=function(){
		if(this.value=="请填写备注"){
			this.value="";
		}
		this.style.color="#000";
	}
	dom("houseRemarkContent").onblur=function(){
		if(this.value==""){
			dom("houseRemarkInfor").style.display="block";
			dom("houseRemarkBg").style.backgroundColor="#ffe6e6";
		}
		else{
			dom("houseRemarkInfor").style.display="none";
			dom("houseRemarkBg").style.backgroundColor="";
		}
	}
	//保存
	dom("submit").onclick=function(){ 
		var _houseT=dom("houseTypeContent").value;
		var _houseM=dom("houseMoneyContent").value;
		var _houseD=dom("houseDetailContent").value;
		var _houseR=dom("houseRemarkContent").value;
		
		if(_houseT.indexOf("请填写房间类型")>=0||_houseT==""){
			alert("请填写完整房间类型");
			return;
		}
		else if(_houseM.indexOf("请填写空间服务费用")>=0||_houseM==""){
			alert("请填写完整空间服务费用");
			return;
		}
		else if(_houseD.indexOf("请填写优惠政策，如税收优惠，政策扶持")>=0||_houseD==""){
			alert("请填写完整您的详细介绍");
			return;
		}
		else if(_houseR.indexOf("请填写备注")>=0||_houseR==""){
			alert("请填写完整您的备注");
			return;
		}
		else{
			dom("houseAdd").style.display='none';
			dom("houseSave").style.display='block';
			bSave=true;
		}

	}
	//取消
	dom('reset').onclick=function(){
		dom("houseAdd").style.display="none";
	}
	//删除
	var aShanchu=dom("houseUl").getElementsByClassName("shanchu");
	for(var i=0;i<aShanchu.length;i++){
		aShanchu[i].index=i;
		aShanchu[i].onclick=function(){
			if(!confirm("确认要删除？")){ 
				window.event.returnValue = false; 
			 } 
			var oLi=aShanchu[this.index].parentNode;
			//alert(this.index);
			dom("houseUl").removeChild(oLi);
			for(var i=0;i<aShanchu.length;i++){
				aShanchu[i].index=i;
			}
		}
	}
	if(aShanchu.length==0){
		dom("houseSave").style.height=0+'px';
	}
	//提交
	dom("tijiao").onclick=function(){
		var _group=dom("groupContent").value;
		var _name=dom("nameContent").value;
		var _address=dom("addressContent").value;
		var _summary=dom("summaryContent").value;
		var _service=dom("serviceContent").value;
		var _introduce=dom("introduceContent").value;
		var _label=dom("labelContent").value;
		if(_group.indexOf("请输入您的空间所属分组，如青创汇郑州，青创汇长沙")>=0||_group==""){
			alert("请填写你的空间所属分组");
			return;
		}
		else if(_name.indexOf("请输入您的空间名称(15字以内)")>=0||_name==""||_name.length>15){
			if(_name.indexOf("请输入您的空间名称(15字以内)")>=0||_name==""){
				alert("请输入您的空间名称");
				return;
			}			
			else if(_name.length>15){
				alert("您的空间名称超过15字,请重新输入");
				return;
			}
			
		}
		else if(_address.indexOf("请输入详细的街道地址")>=0||_address==""){
			alert("请输入详细的街道地址");
			return;
		}
		else if(_summary.indexOf("请填写空间简介(20字以内)")>=0||_summary==""||_summary.length>20){
			if(_summary.indexOf("请填写空间简介(20字以内)")>=0||_summary==""){
				alert("请输入您的空间简介");
				return;
			}			
			else if(_summary.length>20){
				alert("您的空间简介超过20字,请重新输入");
				return;
			}
			
		}
		else if(_service.indexOf("说说您的众创空间能为创客提供那哪些服务,让更多的人为对您的项目感兴趣...")>=0||_service==""){
			alert("请输入您所能提供的服务");
			return;
		}
		else if(_introduce.indexOf("说说您的众创空间的优势和亮点，让更多的人对您的项目感兴趣...")>=0||_introduce==""){
			alert("请填写你的详细介绍");
			return;
		}
		else if(_label.indexOf("请填写空间特色,多以逗号隔开")>=0||_label==""){
			alert("请输入空间标签");
			return;
		}
		else if(!bSave){
			alert("请填写完整您的房间信息");
			return;
		}
		else{
			alert("成功");
		}


	}
}
	
 
//判断分组
