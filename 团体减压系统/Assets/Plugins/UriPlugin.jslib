var UriPlugin = {

    StringReturnHostFunction: function () {

        var returnStr = window.location.search;

        var decodedStr = decodeURIComponent(returnStr);

        var bufferSize = lengthBytesUTF8(decodedStr) + 1;

        var buffer = _malloc(bufferSize);

        stringToUTF8(decodedStr, buffer, bufferSize);

        return buffer;
    }
};


mergeInto(LibraryManager.library, UriPlugin);