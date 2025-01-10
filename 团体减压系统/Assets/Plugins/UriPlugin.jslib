var UriPlugin= {
      
  StringReturnHostFunction: function(){
        var returnStr = window.location.host;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize );
        stringToUTF8(returnStr, buffer, bufferSize );
        return buffer;

    }

}; 
mergeInto(LibraryManager.library, UriPlugin);