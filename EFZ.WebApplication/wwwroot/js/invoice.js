
$('.invoice-detail-btn').click(function () {
    openDialog(this, "blue", "50%");

});


$('.invoice-download-btn').click(function () {
    var invoiceId = $(this).data('invoice-id');
    var fileName = $(this).data('file-name');
    let xhr = new XMLHttpRequest();
    xhr.open('POST', '/Invoice/DownloadFile', invoiceId);
    xhr.responseType = "blob";
    xhr.onload = function (event) {
        let blob = xhr.response;
        if (blob.type === "application/pdf") {
            let link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName;
            link.target = "_blank";
            link.click();
        } else {
            window.showErrorMessage(["Chyba při stahování souboru."], "Přílohy");
        }
    }
    var formData = new FormData();
    formData.append('invoiceId', invoiceId);

    xhr.send(formData);
});
$('.completion-download-btn').click(function () {
    var completionId = $(this).data('completion-id');
    var fileName = $(this).data('file-name');
    let xhr = new XMLHttpRequest();
    xhr.open('POST', '/Completion/DownloadFile', completionId);
    xhr.responseType = "blob";
    xhr.onload = function (event) {
        let blob = xhr.response;
        if (blob.type === "application/pdf") {
            let link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName;
            link.target = "_blank";
            link.click();
        } else {
            window.showErrorMessage(["Chyba při stahování souboru."], "Přílohy");
        }
    }
    var formData = new FormData();
    formData.append('completionId', completionId);

    xhr.send(formData);
});
