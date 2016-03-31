//三级联动
function liandong(){
    $("#Menu1").change(function () {
        var selec = $(this).val();  //获取改变的选项值 
        var url = "@Url.Action("GetCity", "News")";  //参数依次类型(action,Controller,area）
        $.post(url, { 'Id': selec }, function (data) {
            ///Id是参数名和Controllers中的action参数名相同 
            if (data.length > 0) {
                $("#Menu2").find("option").remove();
                for (var i = 0; i < data.length; i++) {
                    //向栏目下拉框中动态添加栏目名称， value 值为栏目Id号， text 值为栏目名称      
                    $("<option></option>").val(data[i].CityId).text(data[i].CityName).appendTo($("#Menu2"));
                }
                // 重新设置 ligerUi 栏目下拉框中的数据，使其设置生效，同时显示出来
                //$("#shopMenu2").ligerGetComboBoxManager().setSelect();
                var selec2 = data[0].Id;  //获取改变的选项值 
                var url = "@Url.Action("GetDis", "News")";  //参数依次类型(action,Controller,area）
                $.post(url, { 'Id': selec2 }, function (data) {
                    ///Id是参数名和Controllers中的action参数名相同 
                    if (data.length > 0) {
                        $("#Menu3").find("option").remove();
                        for (var i = 0; i < data.length; i++) {
                            //向栏目下拉框中动态添加栏目名称， value 值为栏目Id号， text 值为栏目名称      
                            $("<option></option>").val(data[i].DistrictID).text(data[i].DistrictName).appendTo($("#Menu3"));
                        }
                        // 重新设置 ligerUi 栏目下拉框中的数据，使其设置生效，同时显示出来
                        //$("#shopMenu3").ligerGetComboBoxManager().setSelect();
                    }
                });
            }
        });
    });
    $("#Menu2").change(function () {
        var selec = $(this).val();  //获取改变的选项值 
        var url = "@Url.Action("GetDis", "News")";  //参数依次类型(action,Controller,area）
        $.post(url, { 'Id': selec }, function (data) {
            ///Id是参数名和Controllers中的action参数名相同 
            if (data.length > 0) {
                $("#Menu3").find("option").remove();
                for (var i = 0; i < data.length; i++) {
                    //向栏目下拉框中动态添加栏目名称， value 值为栏目Id号， text 值为栏目名称      
                    $("<option></option>").val(data[i].DistrictID).text(data[i].DistrictName).appendTo($("#Menu3"));
                }
                // 重新设置 ligerUi 栏目下拉框中的数据，使其设置生效，同时显示出来
                //$("#shopMenu3").ligerGetComboBoxManager().setSelect();
                //var value = $("#Menu3").find("option:selected").text();
            }
        });
    });
}