using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xftwl.Infrastructure.MvcExtension.CheckBoxList
{
    class readme
    {
        /*
         * 例子:http://mvccbl.com/Examples
         * 
         *  CheckboxList
         * 1. 定义基类
         *  public class ItemModel
         *  {
         *     public int ItemId{get;set;} 
         *     public string ItemName{get;set;}
         *     public object Tags{get;set;} //Html tags 例如: new{tagName="value"}
         *     public bool IsSelected{get;set;} //在checkboxlist中是否选中
         * }
         * 
         * 2.视图模型
         * public class ViewModel
         * {
         *    public IList<ItemModel> ListData{get;set;} //要显示的列表
         *    public IList<ItemModel>SelectedData{get;set;} //己选取的数据
         *    public PostedItems PostedItems{get;set;} //回传的参数
         * }
         * 
         * 3.定义一个帮助类,回传选中的值
         * public class PostedItems
         * {
         *    public string[]Ids{get;set;}
         * }
         * 
         * 4.控制器指定接收参数的类型为 PostedItems
         * 
         * 5.视图调用
         @Html.CheckBoxListFor(model => model.Items.Ids,
                        model => model.Items,(数据源)
                        model => model.ItemId,
                        model => model.ItemName,
                        model => model.ListOfYourSelectedData)//选取的数据
         * 
         * @Html.CheckBoxListFor(model => model.LIST_NAME, 
                      model => model.LIST_DATA, 
                      entity => entity.VALUE, 
                      entity => entity.NAME, 
                      model => model.SELECTED_VALUES) // or entity => entity.IS_CHECKED 
So in our example it'll look like this:
 Collapse | Copy Code
@Html.CheckBoxListFor(model => model.PostedCities.CityIDs, 
                      model => model.AvailableCities, 
                      entity => entity.Id, 
                      entity => entity.Name, 
                      model => model.SelectedCities) 
         * 
         */
    }
}
