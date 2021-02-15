$('.order-new-btn').click(function () {
    openDialog(this,"blue", "70%");

});
$('.order-detail-btn').click(function () {
    openDialog(this, "blue", "50%");

});

$(document).on("submit", "#NewOrderForm", function (event) {
    if ($("#NewOrderForm").valid()) {
        postOrderForm('NewOrderForm');
    } else {
        let errors = $("#NewOrderForm").validate().errorList;
        showErrorMessage(errors, "Zkontrujte vsechna data");
    }

    event.preventDefault();
});

function postOrderForm(formName) {
    var table = document.getElementById("orderDetailsTable");
    var data = $("#" + formName).serialize();

    var index = 0; 
    for (var i = 0, row; row = table.rows[i]; i++) {
        if ($(row).attr('name') == 'header')
            continue;
        var rowData = "";
        for (var j = 0, col; col = row.cells[j]; j++) {
            if ($(col).attr('name') == 'buttons')
                continue;

            var input = col.querySelector("input");
            if ($(col).attr('name') == 'ProductName') {
                if (input != null)
                    rowData = rowData + '&OrderDetails[' + index + '].ProductName=' + escape(input.value);
                else
                    rowData = rowData + '&OrderDetails[' + index + '].ProductName=' + escape(col.innerHTML);
            }
            if ($(col).attr('name') == 'Quantity') {
                if (input != null)
                    rowData = rowData + '&OrderDetails[' + index + '].Quantity=' + escape(input.value);
                else
                    rowData = rowData + '&OrderDetails[' + index + '].Quantity=' + escape(col.innerHTML);
            }
            if ($(col).attr('name') == 'UnitPrice') {
                if (input != null)
                    rowData = rowData + '&OrderDetails[' + index + '].UnitPrice=' + escape(input.value);
                else
                    rowData = rowData + '&OrderDetails[' + index + '].UnitPrice=' + escape(col.innerHTML);
            }
            if ($(col).attr('name') == 'Discount') {
                if (input != null)
                    rowData = rowData + '&OrderDetails[' + index + '].Discount=' + escape(input.value);
                else
                    rowData = rowData + '&OrderDetails[' + index + '].Discount=' + escape(col.innerHTML);
            }
        }
        index = index + 1;
        data = data + rowData;
    }

  
    $.ajax({
        type: "POST",
        url: "/Order/NewOrder",
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


function CalculateTotalPrice() {
    var table = document.getElementById("orderDetailsTable");

    if (table == null) return;

    var sum = 0;

    for (var i = 0, row; row = table.rows[i]; i++) {
        if ($(row).attr('name') == 'header')
            continue;
        var rowSum = 0;
        var quantity = 0;
        var unitPrice = 0;
        var discount = 0;
        for (var j = 0, col; col = row.cells[j]; j++) {
            if ($(col).attr('name') == 'buttons')
                continue;

           

            var input = col.querySelector("input");
            
            if ($(col).attr('name') == 'Quantity') {
                if (input != null)
                    quantity = parseFloat(input.value);
                else
                    quantity = parseFloat(col.innerHTML);
            }
            if ($(col).attr('name') == 'UnitPrice') {
                if (input != null)
                    unitPrice = parseFloat(input.value);
                else
                    unitPrice = parseFloat(col.innerHTML);
            }
            if ($(col).attr('name') == 'Discount') {
                if (input != null)
                    discount = parseFloat(input.value);
                else
                    discount = parseFloat(col.innerHTML);
            }
        }
        if (isNaN(quantity))
            quantity = 0;
        if (isNaN(unitPrice))
            unitPrice = 0;
        if (isNaN(discount))
            discount = 0;
        rowSum = quantity * unitPrice * ((100-discount)/100);
        sum = sum + rowSum;
    }

    document.getElementById("TotalPrice").value = sum;
}



