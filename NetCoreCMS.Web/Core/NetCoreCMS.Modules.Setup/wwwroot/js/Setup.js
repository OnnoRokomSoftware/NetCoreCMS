$(document).ready(function () {
    $("#Database").change(function () {
        var selectedDb = $("#Database").val();
        if (selectedDb == "SQLite" || selectedDb == "MsSqlLocalStorage") {            
            $("#databaseInfo").hide();
        }
        else {
            $("#databaseInfo").show();
        }
    });
});