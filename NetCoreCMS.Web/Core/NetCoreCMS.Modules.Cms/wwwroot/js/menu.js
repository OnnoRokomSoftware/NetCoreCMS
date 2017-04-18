function RemoveMenuItem(element) {
    var parent = $(element).parentsUntil(".list-group-item");
    parent.parent().remove();
}

$(document).ready(function () {

    function toggleIcon(e) {
        $(e.target)
            .prev('.panel-heading')
            .find(".more-less")
            .toggleClass('glyphicon-chevron-down glyphicon-chevron-up');
    }
    $('.panel-group').on('hidden.bs.collapse', toggleIcon);
    $('.panel-group').on('shown.bs.collapse', toggleIcon);
    
    $("#selectAllPages").on("click", function () {
        $(".tab-pane .active").find("input[type='checkbox']").each(function (i, ob) {
            $(ob).prop("checked",true); 
        });        
    });

    //$('#menuListDt').DataTable();

    $('#selectedMenuTree').nestedSortable({
        handle: 'div',
        items: 'li',
        listType: 'ul',
        opacity: .6,
        toleranceElement: '> div',
        stop: function (event, ui) {
            console.log('new parent ' + ui.item.closest('ul').closest('li').attr('name'));
        }
    });

    $('#selectedMenuTree li').droppable({
        drop: function (event, ui) {
            console.log(ui);
            console.log($(this).text());
        }
    });

    //$(document).on("click", "span.removeMenuItem", function () {
    //    console.log("clicked");
    //    var parent = $(this).parentsUntil(".list-group-item");
    //    parent.remove();
    //});

    $("#addPageToMenu").on("click", function () {
        
        $(".recentPagesCheckBoxList").each(function (index) {
            if ($(this).prop("checked") == true) {
                var menuItem = '<li class="list-group-item no-boarder" ncc-menu-item-id="{MENU_ITEM_ID}" ncc-model-id="{MODEL_ID}" ncc-menu-action-type="{ACTION_TYPE}" ncc-controller="{CONTROLLER}" ncc-action="{ACTION}" name="{NAME}">'
                    + '<div class="menu-item-content" >'
                    + '<div class="pull-left" style="padding: 5px 5px;"><i class="glyphicon glyphicon-move margin-right-10" ></i>'
                    + '<span class="ncc-menu-title">{NAME}</span></div>'
                    + '<input type="button" class="closeMenuItem pull-right" value="x" onclick="RemoveMenuItem(this)" ></input>'
                    + '</div>'
                    + '</li> ';
                menuItem = menuItem.replace("{MENU_ITEM_ID}", "0");
                menuItem = menuItem.replace("{MODEL_ID}", $(this).attr("value"));
                menuItem = menuItem.replace("{ACTION_TYPE}", "Page");
                menuItem = menuItem.replace("{CONTROLLER}", "CmsPage");
                menuItem = menuItem.replace("{ACTION}", "Index");
                menuItem = menuItem.replace(/\{NAME}/g, $(this).attr("ncc-page-title"));

                $("#selectedMenuTree").append($($.parseHTML(menuItem)));
            }
        });
    });
});