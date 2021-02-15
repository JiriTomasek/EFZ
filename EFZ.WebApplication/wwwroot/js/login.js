
$(document).ready(function() {

    $(document).on("click", ".loginRegisterBtn", function () {

        openLoginRegisterDialog(this);

    });

  

});


function openLoginRegisterDialog(btn) {
    var url = $(btn).data('url');
    var title = $(btn).data('title');
    $.get(url,
            function (data) {

                $.dialog({
                    title: title,
                    content: data,
                    animation: 'scale',
                    onOpen: function() {
                        var that = this;
                        this.$content.find("button[type$='button'][class$='btn-close']").click(function () {
                            that.close();
                            openLoginRegisterDialog(this);
                            
                        });
                    }
                });
            });
}



function LoginFormSubmit(e) {

    e.preventDefault();
    if ($("#LoginForm").valid()) {
        var email = document.getElementById("Email").value;
        var password = document.getElementById("Password").value;
        var rememberMe = false;
        if ($("#RememberMeCheckBox").is(':checked')) {
            rememberMe = true;
        }

        var data = { "Email": email, "Password": password, "RememberMe": rememberMe };
        $.ajax({
            type: "POST",
            url: "/Account/Login",
            async: false,
            beforeSend: function(xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: data,
            dataType: "text",
            success: function(response) {

               
                if (response === "Success") {
                    location.reload();
                    return true;
                } else {
                    document.getElementById('LoginErrorMessageSpan').innerHTML = response;
                    
                    return false;
                }
                return false;
            }
        });

    }

};



function RegisterFormSubmit(e) {

    e.preventDefault();
    if ($("#RegisterForm").valid()) {
        var email = document.getElementById("RegisterEmail").value;
        var password = document.getElementById("RegisterPassword").value;

        var confirmPassword = document.getElementById("ConfirmPassword").value;

        var data = { "RegisterEmail": email, "RegisterPassword": password, "ConfirmPassword": confirmPassword };
        $.ajax({
            type: "POST",
            url: "/Account/Register",
            async: false,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: data,
            dataType: "json",
            success: function (response) {
                if (response.length > 1) {

                    
                    var err = showErrorMessage(response, "Chyba v registraci.");
                    document.getElementById('RegisterErrorMessageSpan').innerHTML = err;
                    return false;
                    
                } else if (response.length > 0 ) {
                    if (response[0] === "Success") {
                        location.reload();
                        return true;
                    }
                    var err = showErrorMessage(response, "Chyba v registraci.");
                    document.getElementById('RegisterErrorMessageSpan').innerHTML = err;
                } else {

                    return false;
                }
                return false;
            }
        });

    }

};
