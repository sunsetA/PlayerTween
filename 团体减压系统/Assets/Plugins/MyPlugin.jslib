var MyPlugin = {
      
    StringReturnValueFunction: function()
    {
       var topWindow=window.top;
        var returnStr = topWindow.location.search;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    } 

};
 
mergeInto(LibraryManager.library, MyPlugin);