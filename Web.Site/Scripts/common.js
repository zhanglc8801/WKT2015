function AddFavorite(sURL, sTitle) {
    try { window.external.addFavorite(sURL, sTitle); }
    catch (e) {
        try { window.sidebar.addPanel(sTitle, sURL, ""); }
        catch (e) { alert("加入收藏失败，请使用Ctrl+D进行添加"); }
    }
}

function addCookie() {
    if (document.all) {
        window.external.addFavorite(location.href, document.title); //
    }
    else if (window.sidebar) {
        window.sidebar.addPanel(document.title, location.href, "");
    }
}

function Copylist(str) {
    var printWindow = window.open("", "printWindow", "width=10,height=10,left=10000,top=10000");
    printWindow.document.write(str);
    printWindow.document.execCommand("selectall");
    printWindow.document.execCommand("copy");
    printWindow.close();
}