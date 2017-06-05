$(document).ready(function () {
    $('.btn-file :file').on('fileselect', function (event, label) {

        $("#logo_path_input").val(label);
    });
});

$(document).on('change', '.btn-file :file', function () {
    var input = $(this),
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');

    input.trigger('fileselect', [label]);
});