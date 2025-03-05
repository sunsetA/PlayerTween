mergeInto(LibraryManager.library, {
    GetUrlParams: function (defaultValue) {
        var name = Pointer_stringify(defaultValue);
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); 
        var r = window.location.search.substr(1).match(reg); 
        if (r != null){
            var returnStr = unescape(r[2]);
            var bufferSize = lengthBytesUTF8( returnStr) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(returnStr, buffer, bufferSize);
             return buffer;
        }
        else return null; 
    },
 });