mergeInto(LibraryManager.library,
    {
        OpenNewWebsite: function (str) {
             //window.open(Pointer_stringify(str));
            window.top.location.href = Pointer_stringify(str);
        },
    });
