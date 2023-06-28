// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $("#show-password").click(function () {
        
        var passInput = $(".inputPassword");
        if (passInput.attr('type') === 'password') {
            passInput.attr('type', 'text');
        } else {
            passInput.attr('type', 'password');
        }
        
    });
    $("#show-passwordS").click(function () {
        
        var passInput = $(".passwordS");
        if (passInput.attr('type') === 'password') {
            passInput.attr('type', 'text');
        } else {
            passInput.attr('type', 'password');
        }

    });
    $("input").focus(function () { $(this).select(); });
    $('.alert').fadeOut(5000);
    $("#btn-Login").click(function () {
        $("#form-login input").prop('readonly', true);
    });
    $('#states').select2({
        placeholder: "Student",
        tags: true,
        allowClear: true,
        width: 'resolve'
    })
    $('#example').DataTable();
    var arrid = $('#states').data('ids');
    arrid = arrid.trim().split(" ");

    console.log(typeof (arrid));
    
    $("#form-login").validate({

        ruler: {
            UserName: {
                required: true,

            }
        },
        message: {
            Name: "Name not null"
        }
    });

    var $exampleMulti = $(".js-example-basic-multiple").select2();
    $exampleMulti.val(arrid).trigger("change");
    $('#states').select2({
        placeholder: "Student",
        tags: true,
        allowClear: true
    });


    $('.input').keypress(function (e) {
        if (e.key === "Enter") {
            e.preventDefault();
            $('.btnSubmit').click();
        }
    });

    $("form-login").submit(function (e) {
        e.preventDefault();
        /*$("#btn-Login").prop('disabled', true);*/
        console.log("login");
    })
    
});