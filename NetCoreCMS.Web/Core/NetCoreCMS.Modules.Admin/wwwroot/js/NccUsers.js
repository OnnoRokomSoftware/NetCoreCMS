$(document).ready(function () {

    $("#btn_update_users_role").on("click", function (e) {

        var selectedUsers = [];
        $('.cls_user_chk input:checked').each(function () {
            selectedUsers.push($(this).attr('data-user-id'));
        });
        
        var selectedRoles = [];
        $('#dd_user_roles :selected').each(function () {
            selectedRoles.push($(this).val());
        });

        NccPageMask.ShowLoadingMask();

        $.ajax({
            url: '/Users/ChangeRole',
            method: 'POST',
            data: { userIds: selectedUsers, roles: selectedRoles },
            success: function (rsp) {
                NccPageMask.HideLoadingMask();
                console.log(rsp);
                NccAlert.ShowMessages(rsp);
            },
            error: function (data) {
                NccPageMask.HideLoadingMask();
            }
        });
    });

    $("#btn_apply_bulk_operation").on("click", function (e) {

        var selected = [];
        $('.cls_user_chk input:checked').each(function () {
            selected.push($(this).attr('data-user-id'));
        });
        var selectedOperation = $("#dd_bulk_operation_list").val();
        NccPageMask.ShowLoadingMask();

        $.ajax({
            url: '/Users/BulkOperation',
            method: 'POST',
            data: { userIds: selected, operation: selectedOperation},
            success: function (rsp) {
                NccPageMask.HideLoadingMask();
                console.log(rsp);
                NccAlert.ShowMessages(rsp);
            },
            error: function (data) {
                NccPageMask.HideLoadingMask();
            }
        });
    });
    
});