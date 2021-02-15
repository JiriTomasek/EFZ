$(document).ready(function () {
    $.validator.setDefaults({ ignore: '' });
    $("#divProcessing").hide();

});


function showErrorMessage(errors, tittle, widthPercentage) {
    var errorMessage = "";
    if (errors.length > 0) {
        errors.forEach(function (el) {
            errorMessage += "<br>" +el + "<br />";
        });

        $.alert({
            title: tittle,
            content: errorMessage,
            type: 'red',
            theme: 'modern',
            animation: 'scale',
            closeAnimation: 'bottom',
            buttons: {
                okay: {
                    text: 'Ok',
                    btnClass: 'btn-red'
                }
            }
        });
    }
    return errorMessage;
}



function openDialog(btn,type = 'blue', widthPercentage = "30%") {
    var url = $(btn).data('url');
    var title = $(btn).data('title');
    $.get(url,
        function (data) {

            $.dialog({
                title: title,
                content: data,
                type: type,
                theme: 'modern',
                closeAnimation: 'bottom',

                animation: 'scale',
                boxWidth: widthPercentage,
                useBootstrap: false,
                onOpen: function () {
                    var that = this;
                    this.$content.find("button[type$='button'][class$='btn-close']").click(function () {
                        that.close();

                    });
                }
            });
        });
}


$(document).on("change", ".custom-file-input", function (e) {
    var filesName = "";
    var arr = e.target.files;

    for (var i = 0; i < arr.length; i++) {
        if (filesName.length > 40) break;

        filesName += arr[i].name;
        if (arr.length > 1 && i < arr.length-1)
            filesName += ", ";

        if (filesName.length > 40)
            filesName += "...";
    }

    //let fileName = $(this).val().split('\\').pop();
    $(this).next('.custom-file-label').addClass("selected").html(filesName);
});


