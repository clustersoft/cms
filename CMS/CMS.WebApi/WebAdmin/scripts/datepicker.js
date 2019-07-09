//当前时间获取YYYY-MM-DD HH:mm:ss 000
function getNowFormatDate() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var sec = date.getSeconds();
    if (sec >= 1 && sec <= 9)
    { sec = "0" + sec; }

    var minu = date.getMinutes();
    if (minu >= 1 && minu <= 9)
    { minu = "0" + minu; }

    var hour = date.getHours();
    if (hour >= 1 && hour <= 9)
    { hour = "0" + hour; }

    var hm = date.getMilliseconds();
    if (hm <= 9) { hm = "00" + hm; }
    else if (hm <= 99) { hm = "0" + hm; }

    //time = " " + hour + seperator2 + minu + seperator2 + sec;
    //alert(time);
    var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
            + " " + hour + seperator2 + minu
            + seperator2 + sec ;

    return currentdate;
}

//当前时间获取YYYY-MM-DDTHH:mm:ss.000Z
function getNowFormatDateHM() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var sec = date.getSeconds();
    if (sec >= 1 && sec <= 9)
    { sec = "0" + sec; }

    var minu = date.getMinutes();
    if (minu >= 1 && minu <= 9)
    { minu = "0" + minu; }

    var hour = date.getHours();
    if (hour >= 1 && hour <= 9)
    { hour = "0" + hour; }

    var hm = date.getMilliseconds();
    if (hm <= 9) { hm = "00" + hm; }
    else if (hm <= 99) { hm = "0" + hm; }

    //time = " " + hour + seperator2 + minu + seperator2 + sec;
    //alert(time);
    var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
            + "T" + hour + seperator2 + minu
            + seperator2 + sec + "." + hm + "Z";

    return currentdate;
}

//当前时间获取HH:mm:ss
function getNowFormatTime(dates) {
    //var date = new Date(dates);
    var datetime = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    //var month = date.getMonth() + 1;
    
    //var strDate = date.getDate();
    //if (month >= 1 && month <= 9) {
    //    month = "0" + month;
    //}
    //if (strDate >= 0 && strDate <= 9) {
    //    strDate = "0" + strDate;
    //}
    var time;
    var str = dates.toString().split(' ');
    //alert(str[1])
    if (str[1] == "00:00:00") {
        var sec = datetime.getSeconds();
        if (sec >= 1 && sec <= 9)
        { sec = "0" + sec; }

        var minu = datetime.getMinutes();
        if (minu >= 1 && minu <= 9)
        { minu = "0" + minu; }

        var hour = datetime.getHours();
        if (hour >= 1 && hour <= 9)
        { hour = "0" + hour; }

        time = " " + hour + seperator2 + minu + seperator2 + sec;
    }
    else
    {
        time = " " + str[1];
    }
    //alert(time)
    //var currentdates = date.getFullYear() + seperator1 + month + seperator1 + strDate + time;
    var shortdate;
    shortdate = dates.substring(0, 10);

    var currentdates = shortdate + time;
    
    //alert(currentdates)
    return currentdates.toString();
}

//当前时间获取YYYY年MM月DD日
function getNowFormatDateCN() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + '年' + month + '月' + strDate + "日";
    return currentdate;
}