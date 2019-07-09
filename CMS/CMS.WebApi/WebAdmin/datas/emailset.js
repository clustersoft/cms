//内容管理模块：固定邮件树节点
var email = [
    { 
        id:0,
        name: '我的内容',
        spread:true,
     children: [{
        id:1,
        name: '收件箱',
        parentid:0
    }, {
        id: 2,
        name: '发件箱',
        parentid: 0
    },
    {
        id: 3,
        name: '草稿箱',
        parentid: 0
    }, {
        id: 4,
        name: '垃圾箱',
        parentid: 0
    }]
    }
]


var emailztree = [
    { id: 1, name: '我的内容', parentid: 0, open: true },
    { id: 2, name: '收件箱', parentid: 1 },
    { id: 3, name: '发件箱', parentid: 1 },
    { id: 4, name: '草稿箱', parentid: 1 },
    { id: 5, name: '垃圾箱', parentid: 1 }
]

var emaillis = [      /**自定义的数据源，ztree支持json,数组，xml等格式的**/
 { id: 0, pId: -1, name: "我的内容",  open: true },
 { id: 1, pId: 0, name: "收件箱" },
 { id: 2, pId: 0, name: "发件箱" },
 { id: 3, pId: 0, name: "草稿箱" },
 { id: 6, pId: 0, name: "垃圾箱" }
]