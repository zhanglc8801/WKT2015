function TitleA() {
    document.getElementById("Title").value = "";
}
function TitleB() {
    if (document.getElementById("Title").value == "标题" || document.getElementById("Title").value == "") {
        document.getElementById("Title").value = "标题";
    }
}
function AuthorsA() {
    document.getElementById("Authors").value = "";
}
function AuthorsB() {
    if (document.getElementById("Authors").value == "作者姓名" || document.getElementById("Authors").value == "") {
        document.getElementById("Authors").value = "作者姓名";
    }
}
function WorkUnitA() {
    document.getElementById("WorkUnit").value = "";
}
function WorkUnitB() {
    if (document.getElementById("WorkUnit").value == "作者单位" || document.getElementById("WorkUnit").value == "") {
        document.getElementById("WorkUnit").value = "作者单位";
    }
}
function KeyA() {
    document.getElementById("Key2").value = "";
}
function KeyB() {
    if (document.getElementById("Key2").value == "请输入搜索关键字" || document.getElementById("Key").value == "") {
        document.getElementById("Key2").value = "请输入搜索关键字";
    }
}
function KeyC() {
    if (document.getElementById("Key").value == "关键词" || document.getElementById("Key").value == "") {
        document.getElementById("Key").value = "关键词";
    }
}

function Search(str) {
    if(str=="0"){
        var title; var key;
        if (document.getElementById("Key").value == "请输入搜索关键字" || document.getElementById("Key").value == "")
        { key = ""; }
        else
        { key = document.getElementById("Key").value; }

        var year = document.getElementById("Year").value;
        var issue = document.getElementById("Issue").value;
        var JChannelID = document.getElementById("MenuTree").value;
        if (key != "请输入搜索关键字" && key.length > 0) {
            window.open("/Magazine/Index/?Year=" + year + "&Issue=" + issue + "&JChannelID=" + JChannelID + "&Title=" + key + "", "_self");
        }
        else {
            window.open("/Magazine/Index/?Year=" + year + "&Issue=" + issue + "&JChannelID=" + JChannelID + "&Title=", "_self");
        }

    }
    if (str == "1") {
        var title; var authors; var workUnit; var key; var tmp;
        if (document.getElementById("Title2").value == "标题" || document.getElementById("Title2").value == "")
        { title = ""; }
        else
        { title = document.getElementById("Title2").value; }
        if (document.getElementById("Authors2").value == "作者姓名" || document.getElementById("Authors2").value == "")
        { authors = ""; }
        else
        { authors = document.getElementById("Authors2").value; }

        if (document.getElementById("WorkUnit2").value == "作者单位" || document.getElementById("WorkUnit2").value == "")
        { workUnit = ""; }
        else
        { workUnit = document.getElementById("WorkUnit2").value; }

        if (document.getElementById("Key2").value == "关键词" || document.getElementById("Key2").value == "")
        { key = ""; }
        else
        { key = document.getElementById("Key2").value; }

        var year = document.getElementById("Year2").value;
        var issue = document.getElementById("Issue2").value;
        var JChannelID = document.getElementById("MenuTree2").value;

        if (title != "标题" && title.length > 0) {
            window.location.href = "/Magazine/?Year=" + year + "&Issue=" + issue + "&JChannelID=" + JChannelID + "&Title=" + title + "&Authors=" + authors + "&WorkUnit=" + workUnit + "";
        }
        else if (key != "关键词" && key.length > 0) {
            window.location.href = "/Magazine/?Year=" + year + "&Issue=" + issue + "&JChannelID=" + JChannelID + "&Title=" + key + "&Authors=" + authors + "&WorkUnit=" + workUnit + "";
        }
        else {
            window.location.href = "/Magazine/?Year=" + year + "&Issue=" + issue + "&JChannelID=" + JChannelID + "&Title=" + title + "&Authors=" + authors + "&WorkUnit=" + workUnit + "";
        }

    }

    if (str == "2") {
        var title; var key;
        if (document.getElementById("Key").value == "请输入搜索关键字" || document.getElementById("Key").value == "")
        { key = ""; }
        else
        { key = document.getElementById("Key").value; }

        var year = document.getElementById("Year").value;
        var issue = document.getElementById("Issue").value;
        var JChannelID = document.getElementById("MenuTree").value;
        if (key != "请输入搜索关键字" && key.length > 0) {
            window.open("/Magazine/hbzy/?Year=" + year + "&Issue=" + issue + "&JChannelID=" + JChannelID + "&Title=" + key + "", "_self");
        }
        else {
            window.open("/Magazine/hbzy/?Year=" + year + "&Issue=" + issue + "&JChannelID=" + JChannelID + "&Title=0", "_self");
        }
    }

    if (str == "3") {

        var title; var authors; var workUnit; var key; var tmp;
        if (document.getElementById("Title").value == "标题" || document.getElementById("Title").value == "")
        { title = ""; }
        else
        { title = document.getElementById("Title").value; }
        if (document.getElementById("Authors").value == "作者姓名" || document.getElementById("Authors").value == "")
        { authors = ""; }
        else
        { authors = document.getElementById("Authors").value; }

        if (document.getElementById("WorkUnit").value == "作者单位" || document.getElementById("WorkUnit").value == "")
        { workUnit = ""; }
        else
        { workUnit = document.getElementById("WorkUnit").value; }

        if (document.getElementById("Key").value == "关键词" || document.getElementById("Key").value == "")
        { key = ""; }
        else
        { key = document.getElementById("Key").value; }

        var year = document.getElementById("Year").value;
        var issue = document.getElementById("Issue").value;
        var JChannelID = document.getElementById("MenuTree").value;

        if (title != "标题" && title.length > 0) {
            window.location.href="/Magazine/?Year=" + year + "&Issue=" + issue + "&JChannelID="+JChannelID+"&Title=" + title + "&Authors=" + authors + "&WorkUnit=" + workUnit + "";
        }
        else if (key != "关键词" && key.length > 0) {
            window.location.href="/Magazine/?Year=" + year + "&Issue=" + issue + "&JChannelID=" + JChannelID + "&Title=" + key + "&Authors=" + authors + "&WorkUnit=" + workUnit + "";
        }
        else {
            window.location.href="/Magazine/?Year=" + year + "&Issue=" + issue + "&JChannelID=" + JChannelID + "&Title=" + title + "&Authors=" + authors + "&WorkUnit=" + workUnit + "";
        }

    }

    //用于下载排行搜索
    if (str == "4") {

        var key; var tmp;
        if (document.getElementById("Key").value == "关键词" || document.getElementById("Key").value == "")
        { key = ""; }
        else
        { key = document.getElementById("Key").value; }

        var year1 = document.getElementById("Year1").value;
        var year2 = document.getElementById("Year2").value;
        var JChannelID = 1;
        if (parseInt(year1) > parseInt(year2)) {
            alert("起始日期必须小于结束日期！");
            return false;
        } 
        else if (key != "关键词" && key.length > 0) {
            window.location.href="/Magazine/DownOrder?year1=" + year1 + "&year2=" + year2 + "&JChannelID=1&Title=" + key;
        }
        else {
            window.location.href="/Magazine/DownOrder?year1=" + year1 + "&year2=" + year2 + "&JChannelID=1";
        }

    }

    //用于点击排行搜索
    if (str == "5") {

        var key; var tmp;
        if (document.getElementById("Key").value == "关键词" || document.getElementById("Key").value == "")
        { key = ""; }
        else
        { key = document.getElementById("Key").value; }

        var year1 = document.getElementById("Year1").value;
        var year2 = document.getElementById("Year2").value;
        var JChannelID = 1;
        if (parseInt(year1) > parseInt(year2)) {
            alert("起始日期必须小于结束日期！");
            return false;
        }
        else if (key != "关键词" && key.length > 0) {
            window.location.href="/Magazine/ShowOrder?year1=" + year1 + "&year2=" + year2 + "&JChannelID=1&Title=" + key;
        }
        else {
            window.location.href="/Magazine/ShowOrder?year1=" + year1 + "&year2=" + year2 + "&JChannelID=1";
        }

    }

    if (str == "6") {
        var title; var key;
        if (document.getElementById("Key2").value == "请输入搜索关键字" || document.getElementById("Key2").value == "")
        { key = ""; }
        else
        { key = document.getElementById("Key2").value; }

        var JChannelID = document.getElementById("MenuTree2").value;


        if (key != "请输入搜索关键字" && key.length > 0) {
            window.location.href="/Magazine/Album/?Year=0&Issue=0&JChannelID=" + JChannelID + "&Title=" + key + "";
        }
        else {
            window.location.href="/Magazine/Album/?Year=0&Issue=0&JChannelID=" + JChannelID + "&Title=0";
        }

    }



}