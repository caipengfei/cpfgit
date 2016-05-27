/*管理*/
var dir = "/admin";
/*用户管理*/
var indexdata1 =
[
    {
        isexpand: "false", text: "角色管理", children: [
        { url: dir + "/role", text: "查询角色" },
       { url: dir + "/role/edit", text: "编辑角色" }
        ]
    },

    {
        isexpand: "false", text: "会员管理", children: [
         { url: dir + "/user", text: "查询会员" },
        { url: dir + "/user/edit", text: "编辑会员" }
        ]
    }

];
/*评论管理*/
var indexdata2 =
[
    {
        isexpand: "false", text: "查询评论"
    },
];
/*活动管理*/
var indexdata3 =
[

    {
        url: dir + "/active/add", isexpand: "false", text: "添加活动类型"
    },
    {
        url: dir + "/active/GetAll", isexpand: "false", text: "查询所有的活动"
    },
     {
         url: dir + "/active/GetGoods", isexpand: "false", text: "查询活动的商品"
     },
    {
        url: dir + "/active/AddGoods", isexpand: "false", text: "添加活动商品"
    }
];
/*文章管理*/
var indexdata4 =
[
    {
        url: dir + "/news/index", isexpand: "false", text: "文章列表"
    }
];