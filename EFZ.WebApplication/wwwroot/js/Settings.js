$('.user-edit-btn').click(function () {
    openDialog(this);

});
$('.user-new-btn').click(function () {
    openDialog(this);

});

$('.user-delete-btn').click(function () {
    openDialog(this, 'red');

});



$('.customer-edit-btn').click(function () {
    openDialog(this,"blue","70%");
});
$('.customer-delete-btn').click(function () {
    openDialog(this,'red');

});


function roleEditChange(parameters) {

    $('#RolesIds').val(parameters);

}

function customerEditChange(parameters) {

    $('#CustomerId').val(parameters);

}

function userEditChange(parameters) {

    $('#UserIds').val(parameters);

}
$('.job-run-btn').click(function () {
    
    var url = $(this).data('url');
    $.ajax({
        type: "POST",
        url: url,
        async: false,

        dataType: "text",
        success: function (response) {
            if (response === "Success")
                location.reload();
        }
    });
});

$('.job-edit-btn').click(function () {
    openDialog(this, "green");
});

$('.job-log-btn').click(function () {
    openDialog(this, "green", "80%");
});

$('.xmlFile-delete-btn').click(function () {

    var url = $(this).data('url');
    $.ajax({
        type: "POST",
        url: url,
        async: false,

        dataType: "text",
        success: function (response) {
            if (response === "Success")
                location.reload();
        }
    });

});
