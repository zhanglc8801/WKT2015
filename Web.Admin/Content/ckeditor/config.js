CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    config.language = 'zh-cn';
    config.font_names = '宋体/宋体;黑体/黑体;仿宋/仿宋_GB2312;楷体/楷体_GB2312;隶书/隶书;幼圆/幼圆;微软雅黑/微软雅黑;' + config.font_names;
    config.htmlEncodeOutput = true;
    config.filebrowserBrowseUrl = CommonRootPath +  '/content/ckfinder/ckfinder.html',
    config.filebrowserImageBrowseUrl = CommonRootPath + "/content/ckfinder/ckfinder.html";
    config.filebrowserUploadUrl = CommonRootPath + "/content/ckfinder/ckfinder.html";
    config.filebrowserUploadUrl = CommonRootPath + '/content/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files',
    // config.uiColor = '#AADC6E';
    //工具栏
    config.toolbar =
    [
        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
        ['NumberedList', 'BulletedList'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        ['Link', 'Unlink'],
        ['Image', 'Flash', , 'Files', 'Table', 'SpecialChar'],
        '/',
        ['Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor'],
        ['Maximize', '-', 'Undo', 'Redo', '-', 'Source', 'Preview']
    ];
    config.toolbar_basic =
    [
       ['Bold', 'Italic', '-', 'Link', 'Unlink', '-', 'Format', 'Font', 'FontSize', 'TextColor']
    ];
    //config.extraPlugins = "file";
};