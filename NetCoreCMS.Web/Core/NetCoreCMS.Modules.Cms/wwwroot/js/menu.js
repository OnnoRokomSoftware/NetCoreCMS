function Menu() {
    this.Id = 0;
    this.Name = "";
    this.Position = "";
    this.Items = new Array();
}

function MenuItem() {
    this.Id = 0;
    this.Parent = new Object;
    this.Title;
    this.ModelId;
    this.Type;
    this.Controller;
    this.Action;
    this.Data;
    this.Url;
    this.Target;
    this.Order;
    this.Childrens = new Array();
}

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
    
    $("#addPageToMenu").on("click", function () {
        
        $(".recentPagesCheckBoxList").each(function (index) {
            if ($(this).prop("checked") == true) {
                var menuItem = '<li class="list-group-item no-boarder" ncc-menu-item-id="{MENU_ITEM_ID}" ncc-model-item-id="{MODEL_ID}" ncc-menu-item-action-type="{ACTION_TYPE}" ncc-menu-item-controller="{CONTROLLER}" ncc-menu-item-action="{ACTION}" ncc-menu-item-action-data="{DATA}"  ncc-menu-item-url="{URL}" ncc-menu-item-target="{TARGET}" ncc-menu-item-order="{ORDER}" ncc-menu-item-title="{TITLE}">'
                    + '<div class="menu-item-content" >'
                    + '<div class="pull-left" style="padding: 5px 5px;"><i class="glyphicon glyphicon-move margin-right-10" ></i>'
                    + '<span class="ncc-menu-title">{TITLE}</span></div>'
                    + '<input type="button" class="closeMenuItem pull-right" value="x" onclick="RemoveMenuItem(this)" ></input>'
                    + '</div>'
                    + '</li> ';
                menuItem = menuItem.replace("{MENU_ITEM_ID}", "0");
                menuItem = menuItem.replace("{MODEL_ID}", $(this).attr("value"));
                menuItem = menuItem.replace("{ACTION_TYPE}", "Page");
                menuItem = menuItem.replace("{CONTROLLER}", "CmsPage");
                menuItem = menuItem.replace("{ACTION}", "Index");
                menuItem = menuItem.replace(/\{TITLE}/g, $(this).attr("ncc-page-title"));

                $("#selectedMenuTree").append($($.parseHTML(menuItem)));
            }
        });
    });

    function getMenuItemFromLi(listItem) {
        var mi  = new MenuItem();
        mi.Id           = $(listItem).attr("ncc-menu-item-id");
        mi.Title        = $(listItem).attr("ncc-menu-item-title");
        mi.Type         = $(listItem).attr("ncc-menu-item-type");
        mi.Controller   = $(listItem).attr("ncc-menu-item-controller");
        mi.Action       = $(listItem).attr("ncc-menu-item-action");
        mi.Data         = $(listItem).attr("ncc-menu-item-action-data");
        mi.Url          = $(listItem).attr("ncc-menu-item-url");
        mi.Target       = $(listItem).attr("ncc-menu-item-target");
        mi.Order        = $(listItem).attr("ncc-menu-item-order");
        return mi;
    }

    function getMenuTree(parentUl, parent, position) {

        var order = position;
        var menuItemList = new Array();

        $(parentUl).children().each(function (index) {

            console.log(order,$(this),"--");
            var menuItem = getMenuItemFromLi($(this));
            menuItem.Parent = parent;
            menuItem.Order = order;
            order = order + 1;

            var ul = $(this).find("ul");
            if (ul.length > 0) {
                var menuItems = getMenuTree(ul, menuItem, order);
                $(menuItems).each(function (index) {
                    menuItem.Childrens.push(menuItems[index]);
                });
            }

            menuItemList.push(menuItem);
        });

        return menuItemList;
    }

    $("#saveMenuBtn").on("click", function () {
        var menuTree = getMenuTree($("ul#selectedMenuTree"));
        console.log(menuTree);
    });
});