$(document).ready(function () {
    var selectedDb = $("#Database").val();
    if (selectedDb == "" || selectedDb == "SQLite" || selectedDb == "MsSqlLocalStorage") {
        $("#databaseInfo").hide();
    }
    else {
        $("#databaseInfo").show();
    }
    $("#Database").change(function () {
        var selectedDb = $("#Database").val();
        if (selectedDb == "" || selectedDb == "SQLite" || selectedDb == "MsSqlLocalStorage") {            
            $("#databaseInfo").hide();
        }
        else {
            $("#databaseInfo").show();
        }
    });
});