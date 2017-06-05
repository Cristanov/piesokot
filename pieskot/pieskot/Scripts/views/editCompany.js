var maxFilesParameter;
$(document).ready(function () {

    maxFilesParameter = $('#max_files_limit').val();
    tinymce.init({
        selector: "#long_description"
    });

    Dropzone.options.dropzoneForm = {

        addRemoveLinks: true,
        dictRemoveFile: 'Usuń',
        maxFiles: maxFilesParameter,

        init: function () {
            dropzz = this;
            this.on("success", function (file, response) {

                if (response != null) {

                    file.id = response.id;
                }

                setError(file, response);
            });
            this.on('removedfile', function (file) {

                if (file.id > 0) {

                    $.ajax('/Company/DeleteImage?id=' + file.id, { type: "POST" });

                    if (file.isFromServer) {
                        dropzz.options.maxFiles = dropzz.options.maxFiles + 1;
                    }
                    $('#max_files_limit_message').hide();

                }
            });
            this.on('maxfilesreached', function () {
                $('#max_files_limit_message').show();
            });

            //załadowanie istniejących zdjęć
            var companyId = $("#company_id").val();
            $.getJSON('/Company/GetImages?companyId=' + companyId).done(function (data) {

                if (data !== null && data.images !== null && data.images.length > 0) {

                    $.each(data.images, function (index, item) {

                        var img = {
                            name: item.FileName,
                            size: item.Size,
                            id: item.Id,
                            isFromServer: true // parametr określający czy plik pochodzi z serwera, po to by podczas usuwania zwiększyć parametr maxFiles. możliwe że jest tu jakiś błąd w Dropzone.
                        };

                        dropzz.emit('addedfile', img);
                        dropzz.emit("thumbnail", img, item.Path);
                        dropzz.createThumbnailFromUrl(img, item.Path, function () {
                            dropzz.emit("complete", img);
                        });
                        dropzz.files.push(img);
                    });

                    dropzz.options.maxFiles = maxFilesParameter - data.images.length;
                }
            });
        }
    };
});

function setError(file, response) {

    if (response == null) {
        return;
    }

    if (response.code === 501) {
        return file.previewElement.classList.add("dz-success");
    } else if (response.code === 403) {

        var node, _i, _len, _ref, _results;
        var message = response.message;
        file.previewElement.classList.add("dz-error");
        _ref = file.previewElement.querySelectorAll("[data-dz-errormessage]");
        _results = [];
        for (_i = 0, _len = _ref.length; _i < _len; _i++) {
            node = _ref[_i];
            _results.push(node.textContent = message);
        }
        return _results;
    }
}