$(document).ready(function () {

    CKEDITOR.replace('PageContent');

    $("#Title").change(function () {
        $("#Slug").val(NccUtil.GetSafeSlug($(this).val()));
    });

    function ClearForm(){
        $("#pageCreateForm").trigger("reset");
        CKEDITOR.instances["PageContent"].setData("");
    }

    $("#publish").click(function () { 
        
        NccPageMask.ShowLoadingMask();
        $('#PageContent').html(CKEDITOR.instances["PageContent"].getData());
        var form = $("#pageCreateForm");
        
        $.ajax({
            type: "post",
            url: form.action,
            data: form.serialize(),
            contentType: "application/x-www-form-urlencoded",
            success: function (rsp, textStatus, jqXHR) {
                NccPageMask.HideLoadingMask();
                if (rsp.isSuccess) {
                    NccAlert.ShowSuccess(rsp.message);
                    ClearForm();
                }
                else {
                    NccAlert.ShowError("Error: " + rsp.message);
                }                
            },
            error: function (jqXHR, textStatus, errorThrown) {                
                NccPageMask.HideLoadingMask();
                NccAlert.ShowError("Error. Please try again.");
            }
        })
    });
});