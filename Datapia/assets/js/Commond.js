function fomart_split(input, number) {
    try {
        var value = input;
        if (value != null && !value.length < 1) {
            return value.split(" - ")[number];
        } else {
            return "";
        }
    } catch (e) {

    }
}

$('.tabnavbar').click(function () {
    js_ReloadTable_Load();
});


// Fix lỗi khi bấm vào menu .fixed-navbar , tạo thêm sự kiện khi bấm vào .fixed-navbar  thì nó gọi đến js này
function js_ReloadTable_Load() {
    if ($.fn.DataTable.isDataTable('#tbl_Item')) {
        // Nếu DataTable đã được khởi tạo, hủy nó trước khi khởi tạo một DataTable mới
        $('#tbl_Item').DataTable().destroy();
    }
    js_ReloadTable();
    changeWidth()();
}
function changeWidth() {
    var element = document.querySelector(".AutoFix .fromsearch");
    element.style.width = "85%";
}

jQuery(document).ready(function ($) {
    $(".OrderValue").on('keyup', function () {
        var n = parseInt($(this).val().replace(/\D/g, ''), 10);
        $(this).val(n.toLocaleString().replaceAll(".", ",").replaceAll("NaN", ""));
    });

    $(".OrderValue").load('keyup', function () {
        var n = parseInt($(this).val().replace(/\D/g, ''), 10);
        if (n.toLocaleString() != 'NaN') {
            $(this).val(n.toLocaleString().replaceAll(".", ",").replaceAll("NaN", ""));
        }
    });
});
$('body').on('keyup', '.OrderValue', function (e) {
    var n = parseInt($(this).val().replace(/\D/g, ''), 10);
    if (n.toLocaleString() != 'NaN') {
        $(this).val(n.toLocaleString().replaceAll(".", ",").replaceAll("NaN", ""));
    }
});
function js_checkip() {
    if ($('#IsChekIPV4').val() == "") {
        $.getJSON("https://api.ipify.org?format=json", function (data) {
            $('#IsChekIPV4').val(data.ip)
        })
    }
    if ($('#IsChekIPV4').val() == "") {
        $.get("https://ipinfo.io/ip", function (data) {
            $('#IsChekIPV4').val(data)
        })
    }
    if ($('#IsChekIPV4').val() == "") {
        $.get("https://api.ipify.org", function (data) {
            $('#IsChekIPV4').val(data)
        })
    }
    if ($('#IsChekIPV4').val() == "") {
        $.get("https://icanhazip.com", function (data) {
            $('#IsChekIPV4').val(data)
        })
    }
    if ($('#IsChekIPV4').val() == "") {
        $.get("https://wtfismyip.com/text", function (data) {
            $('#IsChekIPV4').val(data)
        })
    }
}

function Check_messe() {
    $('.notnull').css('border', '1px solid #d7d7d7');
    $('.notnull').css('background', '#ffff');

    $('.notnull2  button.btn.dropdown-toggle.btn-light').css('border', '1px solid #d7d7d7');
    $('.notnull2  button.btn.dropdown-toggle.btn-light').css('background', '#ffff');

    var buttons = document.getElementsByClassName('notnull');
    for (var i = 0; i < buttons.length; i++) {
        if (buttons[i].value.length == 0) {
            document.getElementById(buttons[i].id).focus();

            $('#' + buttons[i].id).css('border', '1px solid #d2151e');
            $('#' + buttons[i].id).css('background', '#fff9fa');

            // alert("Các trường có dấu * là bắt buộc phải nhập. Vui lòng kiểm tra lại");
            $.toast({
                heading: 'Thông báo',
                text: 'Vui lòng nhập các trường đầy đủ.',
                showHideTransition: 'fade',
                icon: 'warning'
            })
            return false;
        }
    }

    var buttons2 = document.getElementsByClassName('notnull2');
    for (var i = 0; i < buttons2.length; i++) {
        if (buttons2[i].value == "") {
            document.getElementById(buttons2[i].id).focus();

            $('.' + buttons2[i].id + ' button.btn.dropdown-toggle.btn-light.bs-placeholder').css('border', '1px solid #d2151e');
            $('.' + buttons2[i].id + ' button.btn.dropdown-toggle.btn-light.bs-placeholder').css('background', '#fff9fa');

            // alert("Các trường có dấu * là bắt buộc phải nhập. Vui lòng kiểm tra lại");
            $.toast({
                heading: 'Thông báo',
                text: 'Vui lòng nhập các trường đầy đủ.',
                showHideTransition: 'fade',
                icon: 'warning'
            })
            return false;
        }
    }

}


function Check_messe2() {
    var buttons = document.getElementsByClassName('notnull');
    for (var i = 0; i < buttons.length; i++) {
        if (buttons[i].value.length == 0) {
            document.getElementById(buttons[i].id).focus();
            // alert("Các trường có dấu * là bắt buộc phải nhập. Vui lòng kiểm tra lại");
            $.toast({
                heading: 'Thông báo',
                text: 'Vui lòng nhập các trường đầy đủ.',
                showHideTransition: 'fade',
                icon: 'warning'
            })
            return false;
        }
    }

    //var buttons2 = document.getElementsByClassName('notnull2');
    //for (var i = 0; i < buttons2.length; i++) {
    //    if (buttons2[i].value == "") {
    //        document.getElementById(buttons2[i].id).focus();
    //        $.toast({
    //            heading: 'Thông báo',
    //            text: 'Vui lòng nhập các trường đầy đủ.',
    //            showHideTransition: 'fade',
    //            icon: 'warning'
    //        })
    //        return false;
    //    }
    //}
}

function Regex_Url(txt) {
    var re = /(http(s)?:\\)?([\w-]+\.)+[\w-]+[.com|.in|.org]+(\[\?%&=]*)?/
    if (re.test(txt)) {
        return true;
    }
    else {
        $.toast({
            heading: 'Thông báo',
            text: 'Link sản phẩm không đúng định dạng. Vui lòng kiểm tra lại.',
            showHideTransition: 'fade',
            icon: 'warning'
        })
        return false;
    }
}

function isValidEmail(email) {
    var emailRegex = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailRegex.test(email);
}

function isValidPhoneNumber(phoneNumber) {
    var phoneRegex = /^\d{9,11}$/;
    return phoneRegex.test(phoneNumber);
}

function js_Loading(isLoading, loadingType, closeEffect) {
    switch (loadingType) {
        case 0:
            $('.loading-0').remove();
            if (isLoading) $('BODY').append('<div class="loading-0"></div>');
            break;
        case 1:
            if (isLoading) {
                $('.loading-1-wrapper').remove();
                $('BODY').removeClass('loaded-1');
                $('BODY').append(
                    '<div class="loading-1-wrapper">' +
                    '<div class="loading-1"></div>' +
                    '<div class="loading-1-section section-left"></div>' +
                    '<div class="loading-1-section section-right"></div>' +
                    '</div>');
            } else {
                if (closeEffect) {
                    $('BODY').addClass('loaded-1');
                    setTimeout(function () { $('.loading-1-wrapper').remove(); }, 500);
                } else {
                    $('.loading-1-wrapper').remove();
                }
            }
            break;
        case 2:
            $('.loading-2-background').remove();
            $('.loading-2-wrapper').remove();
            if (isLoading) {
                $('BODY').append(
                    '<div class="loading-2-background"></div>' +
                    '<div class="loading-2-wrapper">' +
                    '<div class="spinner-grow text-primary" role="status"><span class="sr-only">Loading...</span></div>' +
                    '<div class="spinner-grow text-secondary" role="status"><span class="sr-only">Loading...</span></div>' +
                    '<div class="spinner-grow text-success" role="status"><span class="sr-only">Loading...</span></div>' +
                    '<div class="spinner-grow text-danger" role="status"><span class="sr-only">Loading...</span></div>' +
                    '<div class="spinner-grow text-warning" role="status"><span class="sr-only">Loading...</span></div>' +
                    '<div class="spinner-grow text-info" role="status"><span class="sr-only">Loading...</span></div>' +
                    '<div class="spinner-grow text-light" role="status"><span class="sr-only">Loading...</span></div>' +
                    '<div class="spinner-grow text-dark" role="status"><span class="sr-only">Loading...</span></div>' +
                    '</div>'
                );
                $('.loading-2-background').fadeIn('slow');
                $('.loading-2-wrapper').fadeIn('slow');
            }
            break;
    }
}