//电影右边切换
function tabSound(){
	var soundRight=document.getElementsByClassName("soundRight")[0];
	var title=soundRight.getElementsByClassName("title")[0];
	var summary=title.getElementsByClassName("summary")[0];
	var related=title.getElementsByClassName("related")[0];
	//内容
	var content=soundRight.getElementsByClassName("content")[0];
	var summary1=content.getElementsByClassName("summary1")[0];
	var related1=content.getElementsByClassName('related1')[0];
	summary.onclick=function(){
		this.style.background="#242424";
		related.style.background="#565656";
		summary1.style.display="block";
		related1.style.display="none";
	}
	related.onclick=function(){
		this.style.background="#242424";
		summary.style.background="#565656";
		related1.style.display="block";
		summary1.style.display="none";
	}
}
tabSound();
//分页
window.onload = function(){
	
	page({	
		id : 'page',
		nowNum : 1,
		allNum : 10,
		callBack : function(now,all){
		}
	
	});	
};
function page(opt){

	if(!opt.id){return false};
	
	var obj = document.getElementById(opt.id);
	
	var nowNum = opt.nowNum || 1;
	var allNum = opt.allNum || 5;
	var callBack = opt.callBack || function(){};
	if(nowNum>=2){
		var oA = document.createElement('a');
		oA.className="shangxia";
		oA.href = '#' + (nowNum - 1);
		oA.innerHTML = '<';
		obj.appendChild(oA);
	}
	if( nowNum>=4 && allNum>=6 ){
	
		var oA = document.createElement('a');
		oA.className="shouwei";
		oA.href = '#1';
		oA.innerHTML = '首页';
		obj.appendChild(oA);
	
	}
	
	if(allNum<=5){
		for(var i=1;i<=allNum;i++){
			var oA = document.createElement('a');
			oA.className="yeshu";
			oA.href = '#' + i;
			if(nowNum == i){
				oA.innerHTML = i;
				oA.style.background="#fdd000";
				oA.style.borderColor="#fdd000";
				oA.style.color="#fff";
			}
			else{
				oA.innerHTML =i;
			}
			obj.appendChild(oA);
		}	
	}
	else{
	
		for(var i=1;i<=5;i++){
			var oA = document.createElement('a');
			oA.className="yeshu";
			if(nowNum == 1 || nowNum == 2){
				
				oA.href = '#' + i;
				if(nowNum == i){
					oA.innerHTML = i;
					oA.style.background="#fdd000";
					oA.style.borderColor="#fdd000";
					oA.style.color="#fff";
				}
				else{
					oA.innerHTML = i ;
				}
				
			}
			else if( (allNum - nowNum) == 0 || (allNum - nowNum) == 1 ){
			
				oA.href = '#' + (allNum - 5 + i);
				
				if((allNum - nowNum) == 0 && i==5){
					oA.innerHTML = (allNum - 5 + i);
					oA.style.background="#fdd000";
					oA.style.borderColor="#fdd000";
					oA.style.color="#fff";
				}
				else if((allNum - nowNum) == 1 && i==4){
					oA.innerHTML = (allNum - 5 + i);
					oA.style.background="#fdd000";
					oA.style.borderColor="#fdd000";
					oA.style.color="#fff";
				}
				else{
					oA.innerHTML =allNum - 5 + i;
				}
			
			}
			else{
				oA.href = '#' + (nowNum - 3 + i);
				
				if(i==3){
					oA.innerHTML = (nowNum - 3 + i);
					oA.style.background="#fdd000";
					oA.style.borderColor="#fdd000";
					oA.style.color="#fff";
				}
				else{
					oA.innerHTML =nowNum - 3 + i;
				}
			}
			obj.appendChild(oA);
			
		}
	
	}

	if( (allNum - nowNum) >= 3 && allNum>=6 ){
	
		var oA = document.createElement('a');
		oA.className="shouwei";
		oA.href = '#' + allNum;
		oA.innerHTML = '尾页';
		obj.appendChild(oA);
	
	}
	
	if( (allNum - nowNum) >= 1 ){
		var oA = document.createElement('a');
		oA.className="shangxia";
		oA.href = '#' + (nowNum + 1);
		oA.innerHTML = '>';
		obj.appendChild(oA);
	}

	
	callBack(nowNum,allNum);
	
	var aA = obj.getElementsByTagName('a');
	
	for(var i=0;i<aA.length;i++){
		aA[i].onclick = function(){
			var nowNum = parseInt(this.getAttribute('href').substring(1));
			obj.innerHTML = '';
			
			page({
			
				id : opt.id,
				nowNum : nowNum,
				allNum : allNum,
				callBack : callBack
			
			});
			
			return false;
			
		};
	}
}


//日期
/*
function getDate(strTime){
	var date = new Date(strTime);
    	return date.getFullYear()+"-"+(date.getMonth()+1)+"-"+date.getDate();
}
getDate(getTime());
*/
//阴影
function getCover(){
	var movieList=document.getElementById("movieList");
	var LeftBox=movieList.getElementsByClassName("LeftBox");
	var cover=movieList.getElementsByClassName("cover");
	var bofang=movieList.getElementsByClassName('bofang');
	for(var i=0;i<LeftBox.length;i++){
		LeftBox[i].index=i;
		LeftBox[i].onmouseover=function(){
			cover[this.index].style.display="block";
			bofang[this.index].style.display="block";
		}
		LeftBox[i].onmouseout=function(){
			cover[this.index].style.display="none"
			bofang[this.index].style.display="none";
		}
	}
}
getCover();