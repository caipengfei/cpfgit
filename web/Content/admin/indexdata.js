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
/*部门管理*/
var indexdata5 =
[
    {
        isexpand: "false", text: "部门管理", children: [
        { url: dir + "/branch/list", text: "查询部门" },
       { url: dir + "/branch/edit", text: "添加部门" }
        ]
    },

    {
        isexpand: "false", text: "部门员工管理", children: [
         { url: dir + "/branch", text: "查询部门员工" },
        { url: dir + "/branch/save", text: "添加部门员工" }
        ]
    }

];
/*业绩管理*/
var indexdata6 =
[
    {
        isexpand: "false", text: "部门业绩", children: [
        { url: dir + "/JX/Jx2", text: "查询部门业绩" }
        ]
    },

    {
        isexpand: "false", text: "个人业绩", children: [
         { url: dir + "/Jx/Jx1", text: "查询个人业绩" },
         { url: dir + "/Jx/Jx", text: "我的推广明细" }
        ]
    }

];