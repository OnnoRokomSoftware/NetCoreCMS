$(document).ready(function () {

    CKEDITOR.replace('PageContent');

    $("#Title").change(function () {
        $("#Slug").val(NccUtil.GetSafeSlug($(this).val()));
    });

    $("#publish").click(function () {   
        NccPageMask.ShowLoadingMask();
        $('#PageContent').html(CKEDITOR.instances["PageContent"].getData());
        var form = $("#pageCreateForm");
        console.log(form);
        $.ajax({
            type: "post",
            url: form.action,
            data: form.serialize(),
            contentType: "application/x-www-form-urlencoded",
            success: function (responseData, textStatus, jqXHR) {
                NccPageMask.HideLoadingMask();
                NccAlert.ShowSuccess("Successfully saved the page.");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR, textStatus, errorThrown);
                NccPageMask.HideLoadingMask();
                NccAlert.ShowError("Successfully saved the page.");
            }
        })
    });
});