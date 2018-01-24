
function mobileCheckBd(mobileNumber) {
    mobileNumber = mobileNumber.replace(/\W+/g, "");
    var chkVal = mobileNumber.match(/^(?:\+?88)?0?1[15-9]\d{8}$/);
    if (chkVal != null) {
        if (mobileNumber.length == 11) {
            chkVal = '88' + chkVal;
        }
        else if (mobileNumber.length == 10) {
            chkVal = '880' + chkVal;
        }
        var chkVal = mobileNumber.match(/^(?:\+?88)?01[15-9]\d{8}$/);
        if (chkVal != null) {
            if (mobileNumber.length == 11) {
                chkVal = '88' + chkVal;
            }
            else if (mobileNumber.length == 10) {
                chkVal = '880' + chkVal;
            }
        }
        //alert(chkVal);
    }
    return chkVal;
}

function showErrorMessageBelowCtrl(ctrlId, message, show) {

    var divHtml = '<div style="color:red;" id="' + ctrlId + '_err_div" >' + message + '</div>';

    if (show == true) {
        $('#' + ctrlId).addClass("highlight");
        $('#' + ctrlId).after(divHtml);
    } else {
        $('#' + ctrlId).removeClass("highlight");
        $('#' + ctrlId + '_err_div').remove();
    }
}

function validateTextField(controlId, errorMessage) {

    $(document).on("input propertychange", "#" + controlId, function (event) {

        var value = $("#" + controlId).val().trim();
        if (value.length == 0) {
            showErrorMessageBelowCtrl(controlId, "", false);
            showErrorMessageBelowCtrl(controlId, errorMessage, true);
            return false;
        } else {
            showErrorMessageBelowCtrl(controlId, "", false);
        }
    });
}

function validateDropDownField(controlId, targetId, errorMessage) {

    $(document).on("change", "#" + controlId, function (event) {

        var val = $("#" + targetId).val();
        if (val.length == 0) {
            showErrorMessageBelowCtrl(controlId, errorMessage, true);
        } else {
            showErrorMessageBelowCtrl(controlId, "", false);
        }
    });
}

function validateDropDownField(controlId, errorMessage) {

    $(document).on("change", "#" + controlId, function (event) {

        var val = $("#" + controlId).val();
        if (val.length == 0) {
            showErrorMessageBelowCtrl(controlId, "", false);
            showErrorMessageBelowCtrl(controlId, errorMessage, true);
        } else {
            showErrorMessageBelowCtrl(controlId, "", false);
        }
    });
}

function validateListField(controlId, errorMessage) {

    $(document).on("change", "#" + controlId, function (event) {
        var val = $("#" + controlId).val();
        if (val == null || val.length == 0) {
            showErrorMessageBelowCtrl(controlId, "", false);
            showErrorMessageBelowCtrl(controlId, errorMessage, true);
        } else {
            showErrorMessageBelowCtrl(controlId, "", false);
        }
    });
}

function checkDate(dateFrom, dateTo) {
    var isValid = true;
    if (dateFrom != '' && dateTo != '') {
        if (Date.parse(dateFrom) > Date.parse(dateTo))
            isValid = false;
    }

    return isValid;
}

$(document).ready(function () {

});