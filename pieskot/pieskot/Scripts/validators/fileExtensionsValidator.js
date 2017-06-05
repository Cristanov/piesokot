$.validator.addMethod("fileextensions", function (value, element, params) {

    if (value == null || value == "") {
        return true;
    }

    for (var i in params.extensions) {

        var extension = params.extensions[i];
        if (value.endsWith(extension)) {
            return true;
        }
    }

    return false;
});

$.validator.unobtrusive.adapters.add("fileextensions", ["extensions"], function (options) {

    var params = {
        extensions: options.params.extensions.split(';')
    };

    options.rules['fileextensions'] = params;

    if (options.message) {
        options.messages['fileextensions'] = options.message;
    }
});