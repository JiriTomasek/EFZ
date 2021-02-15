$('.attachment-new-btn').click(function () {
    openDialog(this, 'blue', "70%");

});

var singleAttachmentCustomer;

function setAttachmentCustomerSelect(placeHolder, data) {

    singleAttachmentCustomer = new SelectPure(".customer-single-select",
        {
            options: data,
            value: "",
            placeholder: placeHolder,
            onChange: value => { attachmentCustomerEditChange(value) },
            classNames: {
                select: "select-pure__select",
                dropdownShown: "select-pure__select--opened",
                multiselect: "select-pure__select--multiple",
                label: "select-pure__label",
                placeholder: "select-pure__placeholder",
                dropdown: "select-pure__options",
                option: "select-pure__option",
                autocompleteInput: "select-pure__autocomplete",
                selectedLabel: "select-pure__selected-label",
                selectedOption: "select-pure__option--selected",
                placeholderHidden: "select-pure__placeholder--hidden",
                optionHidden: "select-pure__option--hidden",
            }
        });
}



function attachmentCustomerEditChange(parameters) {
    attachmentOrderEditChange(0);
    var data = { customerId: parameters };
    $.ajax({
        type: "POST",
        url: "/Order/GetAllOrderByCustomer",
        async: false,

        data: data,
        dataType: "json",
        success: function (response) {

            if (typeof singleAttachmentOrder !== 'undefined') {
                $(".order-single-select").html("");

                singleAttachmentOrder._config.options = response;
                singleAttachmentOrder._create(".order-single-select");

            }
        }
    });


}

var singleAttachmentOrder;


function setAttachmentOrderSelect(placeHolder, data) {

    singleAttachmentOrder = new SelectPure(".order-single-select", {
        options: data,
        value: "",
        placeholder: placeHolder,
        onChange: value => { attachmentOrderEditChange(value) },
        classNames: {
            select: "select-pure__select",
            dropdownShown: "select-pure__select--opened",
            multiselect: "select-pure__select--multiple",
            label: "select-pure__label",
            placeholder: "select-pure__placeholder",
            dropdown: "select-pure__options",
            option: "select-pure__option",
            autocompleteInput: "select-pure__autocomplete",
            selectedLabel: "select-pure__selected-label",
            selectedOption: "select-pure__option--selected",
            placeholderHidden: "select-pure__placeholder--hidden",
            optionHidden: "select-pure__option--hidden",
        }
    });


}


function attachmentOrderEditChange(parameters) {
    
    var data = { orderId: parameters };
    $.ajax({
        type: "POST",
        url: "/Delivery/GetDeliveryByOrder",
        async: false,

        data: data,
        dataType: "json",
        success: function (response) {

            if (typeof singleAttachmentDelivery !== 'undefined') {
                $(".delivery-single-select").html("");

                singleAttachmentDelivery._config.options = response;
                singleAttachmentDelivery._create(".delivery-single-select");

            }


        }
    });

}


var singleAttachmentDelivery;

function setAttachmentDeliverySelect(placeHolder, data) {

    singleAttachmentDelivery = new SelectPure(".delivery-single-select", {
        options: data,
        value: "",
        placeholder: placeHolder,
        onChange: value => { attachmentDeliveryEditChange(value) },
        classNames: {
            select: "select-pure__select",
            dropdownShown: "select-pure__select--opened",
            multiselect: "select-pure__select--multiple",
            label: "select-pure__label",
            placeholder: "select-pure__placeholder",
            dropdown: "select-pure__options",
            option: "select-pure__option",
            autocompleteInput: "select-pure__autocomplete",
            selectedLabel: "select-pure__selected-label",
            selectedOption: "select-pure__option--selected",
            placeholderHidden: "select-pure__placeholder--hidden",
            optionHidden: "select-pure__option--hidden",
        }
    });


}

function attachmentDeliveryEditChange(parameters) {

    $('#DeliveryId').val(parameters);

    var data = { deliveryId: parameters };
    $.ajax({
        type: "POST",
        url: "/Delivery/GetDeliveryData",
        async: false,

        data: data,
        dataType: "json",
        success: function (response) {

            $('#OrderNumber').val(response.orderNumber);

            $('#DeliveryNumber').val(response.deliveryNumber);


        }
    });

   

}


$('.attachment-download-btn').click(function () {
    var attachmentId = $(this).data('attachment-id');
    var fileName = $(this).data('file-name');
    let xhr = new XMLHttpRequest();
    xhr.open('POST', '/Attachment/DownloadFile', attachmentId);
    xhr.responseType = "blob";
    xhr.onload = function(event) {
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
    formData.append('attachmentId', attachmentId);

    xhr.send(formData);
});
