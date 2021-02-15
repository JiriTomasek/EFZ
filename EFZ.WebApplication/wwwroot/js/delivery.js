$('.delivery-new-btn').click(function () {
    openDialog(this,"blue",  "70%");

});
$('.delivery-detail-btn').click(function () {
    openDialog(this, "blue",  "50%");

});

var singleDeliveryCustomer;

function setDeliveryCustomerSelect(placeHolder, data) {

    singleDeliveryCustomer = new SelectPure(".customer-single-select",
        {
            options: data,
            value: "",
            placeholder: placeHolder,
            onChange: value => { deliveryCustomerEditChange(value) },
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


var singleDeliveryOrder;




function deliveryCustomerEditChange(parameters) {
    $('#CustomerId').val(parameters);

    var data = { customerId: parameters };
    $.ajax({
        type: "POST",
        url: "/Order/GetOrderByCustomer",
        async: false,

        data: data,
        dataType: "json",
        success: function (response) {

            if (typeof singleDeliveryOrder !== 'undefined') {
                $(".order-single-select").html("");

                singleDeliveryOrder._config.options = response;
                singleDeliveryOrder._create(".order-single-select");

            }
        }
    });


}


function setDeliveryOrderSelect(placeHolder, data) {

    singleDeliveryOrder = new SelectPure(".order-single-select", {
        options: data,
        value: "",
        placeholder: placeHolder,
        onChange: value => { deliveryOrderEditChange(value) },
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


function deliveryOrderEditChange(parameters) {

    $('#OrderId').val(parameters);

    var data = { orderId: parameters };
    $.ajax({
        type: "POST",
        url: "/Order/GetOrderNumber",
        async: false,

        data: data,
        dataType: "json",
        success: function (response) {

            $('#OrderNumber').val(response.orderNumber);

            if (response.isDelivered) {

                document.getElementById("DeliveryAll").checked = false;
                document.getElementById("DeliveryAll").value = false;
                document.getElementById("DeliveryAll").readOnly = true;
                document.getElementById("DeliveryAll").disabled = true;
            }


        }
    });

}


$(document).on("submit", "#NewDeliveryForm", function (event) {
    if ($("#NewDeliveryForm").valid()) {
        postDeliveryForm('NewDeliveryForm');
    } else {
        let errors = $("#NewDeliveryForm").validate().errorList;
        showErrorMessage(errors, "Zkontrujte vsechna data");
    }

    event.preventDefault();
});

function postDeliveryForm(formName) {
    var data = $("#" + formName).serialize();

  $.ajax({
        type: "POST",
      url: "/Delivery/NewDelivery",
        async: false,
        data: data,
        dataType: "json",
        success: function (response) {
            if (response[0] === "ok") {
                location.reload();
            } else {
                showErrorMessage(response, "Zkontrujte všechna data");
            }
        }
    });
}
