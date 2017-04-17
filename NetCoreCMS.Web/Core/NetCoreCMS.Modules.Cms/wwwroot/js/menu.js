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
});