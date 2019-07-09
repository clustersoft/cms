
function GetTemplateUseable(state) {
    var str = "";
    
    switch (state) {
        case 0:
            str = "否";
            break;
        case 1:
            str = "是";
            break;
    }

    return str;
}

//文章内容state值读取
function GetContentState(state) {
    //alert(state)
    var str = "";
    switch (state) {
        case -1:
            str = "暂存";
            break;
        case 0:
            str = "未审核";
            break;
        case 1:
            str = "审核通过";
            break;
        case 2:
            str = "审核中";
            break;
        case 3:
            str = "被退回";
            break;
        case 4:
            str = "已删除";
            break;
        default:
            str = "无效";
            break;
    }
    return str;
}

