mergeInto(LibraryManager.library,
    {
        OpenNewWebsite: function (str) {
             //window.open(Pointer_stringify(str));//新标签页打开页面
            window.top.location.href = Pointer_stringify(str);//当前标签页打开页面
        },
    });
