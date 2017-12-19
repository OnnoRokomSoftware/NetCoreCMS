function NccMenuActionType() {
    this.Url = "Url";
    this.Page = "Page";
    this.BlogPost = "BlogPost";
    this.BlogCategory = "BlogCategory";
    this.Module = "Module";
    this.Tag = "Tag";
}

function NccMenuItemFor() {
    this.Site = "Site";
    this.Admin = "Admin";
}

function NccMenuItemTarget() {
    this.Blank = "_blank"; 
    this.Parent = "_parent";
    this.Self = "_self"; 
    this.Top = "_top"; 
}

function NccMenu() {
    this.Id = 0;
    this.Name = "";
    this.Position = "";
    this.MenuFor = "";
    this.Items = new Array();
}

function NccMenuItem() {
    this.Id = 0;
    this.ParentId = 0;
    this.Title;
    this.Type;
    this.Module = "";
    this.Area = "";
    this.Controller;
    this.Action;
    this.Data;
    this.Url;
    this.Target;
    this.Order = 0;
    this.Childrens = new Array();
}

function RemoveMenuItem(element) {
    var parent = $(element).parentsUntil(".list-group-item");
    parent.parent().remove();
}

function ShowEditor(src, dest) {
    var txtSpan = $("#" + src);
    var val = "";
    var newVal = "";
    if (txtSpan.is(":visible")) {        
        val = txtSpan.html();
        txtSpan.hide();
    }
    else {
        txtSpan.show();
    }
    
    var next = $("#" + dest);
    if (next.is(":visible")) {
        newVal = next.val();
        console.log(newVal);        
        next.val("");
        next.hide();
    }
    else {
        next.show();
        next.val(val);
        next.on("blur", function () {
            txtSpan.show();
            txtSpan.html(next.val());
            var li = txtSpan.closest("li");
            li.attr("ncc-menu-item-title", next.val());
            next.hide();
        });
    }
    
    setTimeout(function () {
        txtSpan.html(newVal);
        var li = txtSpan.closest("li");
        console.log(li);
        li.attr("ncc-menu-item-title", newVal);
    }, 50);
    
};

$(document).ready(function () {

    function toggleIcon(e) {
        $(e.target)
            .prev('.panel-heading')
            .find(".more-less")
            .toggleClass('glyphicon-chevron-down glyphicon-chevron-up');
    }
    $('.panel-group').on('hidden.bs.collapse', toggleIcon);
    $('.panel-group').on('shown.bs.collapse', toggleIcon);
    

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


    

    function getMenuItemFromLi(listItem) {
        var mi          = new NccMenuItem();
        mi.Id           = $(listItem).attr("ncc-menu-item-id");
        mi.Title        = $(listItem).attr("ncc-menu-item-title");
        mi.Type         = $(listItem).attr("ncc-menu-item-action-type");
        mi.Controller   = $(listItem).attr("ncc-menu-item-controller");
        mi.Action       = $(listItem).attr("ncc-menu-item-action");
        mi.Data         = $(listItem).attr("ncc-menu-item-action-data");
        mi.Url          = $(listItem).attr("ncc-menu-item-url");
        mi.Target       = $(listItem).attr("ncc-menu-item-target");
        mi.Order        = $(listItem).attr("ncc-menu-item-order");
        mi.Module        = $(listItem).attr("ncc-menu-item-module");
        return mi;
    }

    function getMenuTree(parentUl, parent, position) {

        var order = position;
        var menuItemList = new Array();

        $(parentUl).children().each(function (index) {

            var menuItem = getMenuItemFromLi($(this));
            if (parent != null) {
                menuItem.ParentId = parent.Id;
            }
            menuItem.Order = order;
            order = order + 1;

            var ul = $(this).find(" > ul");
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

    /*Page section start*/
    $("#selectAllPages").on("click", function () {
        $(".pageList .active").find("input[type='checkbox']").each(function (i, ob) {
            $(ob).prop("checked",true); 
        });        
    });
    $("#addPageToMenu").on("click", function () {
        $(".recentPagesCheckBoxList").each(function (index) {
            if ($(this).prop("checked") == true) {
                var src = GUID.NewGUID();
                var dest = GUID.NewGUID();
                var menuItem = '<li class="list-group-item no-boarder" ncc-menu-item-id="{MENU_ITEM_ID}" ncc-model-item-id="{MODEL_ID}" ncc-menu-item-action-type="{ACTION_TYPE}" ncc-menu-item-controller="{CONTROLLER}" ncc-menu-item-action="{ACTION}" ncc-menu-item-action-data="{DATA}"  ncc-menu-item-url="/{URL}" ncc-menu-item-target="{TARGET}" ncc-menu-item-order="{ORDER}" ncc-menu-item-title="{TITLE}">'
                    + '<div class="menu-item-content" >'
                    + '<div class="pull-left" style="padding: 5px 5px;"><i class="glyphicon glyphicon-move margin-right-10" ></i>'
                    + '<span id="' + src + '" class="ncc-menu-title">{TITLE}</span></div>'
                    + '<input id="' + dest + '" class="ncc-menu-title-editor" type="text" style="display:none" />'
                    + '&nbsp;&nbsp; <i class="fa fa-edit" onclick="ShowEditor(\'' + src + '\', \'' + dest + '\')"></i>'
                    + '<input type="button" class="closeMenuItem pull-right" value="x" onclick="RemoveMenuItem(this)" ></input>'
                    + '</div>'
                    + '</li> ';
                menuItem = menuItem.replace("{MENU_ITEM_ID}", "0");
                menuItem = menuItem.replace("{MODEL_ID}", $(this).attr("value"));
                menuItem = menuItem.replace("{ACTION_TYPE}", "Page");
                menuItem = menuItem.replace("{CONTROLLER}", "CmsPage");
                menuItem = menuItem.replace("{ACTION}", "Index");
                menuItem = menuItem.replace("{DATA}", $(this).attr("value"));
                menuItem = menuItem.replace("{URL}", $(this).attr("ncc-slug"));
                menuItem = menuItem.replace("{TARGET}", "_self");
                menuItem = menuItem.replace("{ORDER}", 0);
                menuItem = menuItem.replace(/\{TITLE}/g, $(this).attr("ncc-title"));
                $("#selectedMenuTree").append($($.parseHTML(menuItem)));
                $(this).prop("checked", false);
            }
        });
    });
    var searchPageLastKey = "";
    function searchPage() {
        var searchKey = $("#searchPage").val();
        var languageKey = $("#languagePage").val();
        if (searchKey != (searchPageLastKey + languageKey)) {
            $("#searchPagesCheckBoxList").html("");
            if (searchKey != "") {
                $('#searchPagesCheckBoxList').append($('<br /><label>Searching ... </label>'));
            }
        }
        setTimeout(function () {
            var searchKey1 = $("#searchPage").val();
            //console.log(searchKey1);
            if (searchKey != "" && searchKey == searchKey1 && searchKey != (searchPageLastKey + languageKey)) {
                searchPageLastKey = searchKey + languageKey;
                $.ajax({
                    type: "POST",
                    url: "/CmsMenu/LoadPages",
                    data: { name: searchKey, lang: languageKey },
                    success: function (rsp) {
                        //console.log(rsp);
                        if (rsp.isSuccess) {
                            //NccAlert.ShowSuccess(rsp.message);
                            $("#searchPagesCheckBoxList").html("");
                            $.each(rsp.data, function (i, v) {
                                $('#searchPagesCheckBoxList').append($('<div class="checkbox"><label><input type="checkbox" class="recentPagesCheckBoxList" value="' + v.id + '" ncc-title="' + v.title + '" ncc-slug="' + v.slug + '">' + v.title + '</label></div>'));
                            });
                        }
                        else {
                            $("#searchPagesCheckBoxList").html("");
                            //NccAlert.ShowError("Error: " + rsp.message);
                            $('#searchPagesCheckBoxList').append($('<br /><label>No result found</label>'));
                        }
                    },
                    error: function (errorThrown) {
                        //NccAlert.ShowError("Error. Please try again.");
                    }
                });
            }
        }, 1000);
    }
    $("#searchPage").on("change keyup paste", function () {
        searchPage();
    });
    $("#languagePage").on("change", function () {
        searchPage();
    });
    /*Page section end*/

    /*Post section start*/
    $("#selectAllPosts").on("click", function () {
        $(".postList .active").find("input[type='checkbox']").each(function (i, ob) {
            $(ob).prop("checked", true);
        });
    });
    $("#addPostToMenu").on("click", function () {
        $(".recentPostCheckBoxList").each(function (index) {
            if ($(this).prop("checked") == true) {
                var src = GUID.NewGUID();
                var dest = GUID.NewGUID();
                var menuItem = '<li class="list-group-item no-boarder" ncc-menu-item-id="{MENU_ITEM_ID}" ncc-model-item-id="{MODEL_ID}" ncc-menu-item-action-type="{ACTION_TYPE}" ncc-menu-item-controller="{CONTROLLER}" ncc-menu-item-action="{ACTION}" ncc-menu-item-action-data="{DATA}"  ncc-menu-item-url="/{URL}" ncc-menu-item-target="{TARGET}" ncc-menu-item-order="{ORDER}" ncc-menu-item-title="{TITLE}">'
                    + '<div class="menu-item-content" >'
                    + '<div class="pull-left" style="padding: 5px 5px;"><i class="glyphicon glyphicon-move margin-right-10" ></i>'
                    + '<span id="' + src + '" class="ncc-menu-title">{TITLE}</span></div>'
                    + '<input id="' + dest + '" class="ncc-menu-title-editor" type="text" style="display:none" />'
                    + '&nbsp;&nbsp; <i class="fa fa-edit" onclick="ShowEditor(\'' + src + '\', \'' + dest + '\')"></i>'
                    + '<input type="button" class="closeMenuItem pull-right" value="x" onclick="RemoveMenuItem(this)" ></input>'
                    + '</div>'
                    + '</li> ';
                menuItem = menuItem.replace("{MENU_ITEM_ID}", "0");
                menuItem = menuItem.replace("{MODEL_ID}", $(this).attr("value"));
                menuItem = menuItem.replace("{ACTION_TYPE}", "BlogPost");
                menuItem = menuItem.replace("{CONTROLLER}", "Post");
                menuItem = menuItem.replace("{ACTION}", "Index");
                menuItem = menuItem.replace("{DATA}", $(this).attr("value"));
                menuItem = menuItem.replace("{URL}", "Post/" + $(this).attr("ncc-slug"));
                menuItem = menuItem.replace("{TARGET}", "_self");
                menuItem = menuItem.replace("{ORDER}", 0);
                menuItem = menuItem.replace(/\{TITLE}/g, $(this).attr("ncc-title"));

                $("#selectedMenuTree").append($($.parseHTML(menuItem)));
                $(this).prop("checked", false);
            }
        });
    });
    var searchPostLastKey = "";
    function searchPost() {
        var searchKey = $("#searchPost").val();
        var languageKey = $("#languagePost").val();
        if (searchKey != (searchPostLastKey + languageKey)) {
            $("#searchPostsCheckBoxList").html("");
            if (searchKey != "") {
                $('#searchPostsCheckBoxList').append($('<br /><label>Searching ... </label>'));
            }
        }

        setTimeout(function () {
            var searchKey1 = $("#searchPost").val();
            //console.log(searchKey1);
            if (searchKey != "" && searchKey == searchKey1 && searchKey != (searchPostLastKey + languageKey)) {
                searchPostLastKey = searchKey + languageKey;
                $.ajax({
                    type: "POST",
                    url: "/CmsMenu/LoadPosts",
                    data: { name: searchKey, lang: languageKey },
                    success: function (rsp) {
                        //console.log(rsp);
                        if (rsp.isSuccess) {
                            $("#searchPostsCheckBoxList").html("");
                            //NccAlert.ShowSuccess(rsp.message);
                            $.each(rsp.data, function (i, v) {
                                $('#searchPostsCheckBoxList').append($('<div class="checkbox"><label><input type="checkbox" class="recentPostCheckBoxList" value="' + v.id + '" ncc-title="' + v.title + '" ncc-slug="' + v.slug + '">' + v.title + '</label></div>'));
                            });
                        }
                        else {
                            $("#searchPostsCheckBoxList").html("");
                            //NccAlert.ShowError("Error: " + rsp.message);
                            $('#searchPostsCheckBoxList').append($('<br /><label>No result found</label>'));
                        }
                    },
                    error: function (errorThrown) {
                        NccAlert.ShowError("Error. Please try again.");
                    }
                });
            }
        }, 1000);
    }
    $("#searchPost").on("change keyup paste", function () {
        searchPost();
    });
    $("#languagePost").on("change", function () {
        searchPost();
    });
    /*Post section end*/

    /*Category section start*/
    $("#selectAllCategories").on("click", function () {
        $(".categoryList .active").find("input[type='checkbox']").each(function (i, ob) {
            $(ob).prop("checked", true);
        });
    });
    $("#addCategoryToMenu").on("click", function () {
        $(".recentCategoryCheckBoxList").each(function (index) {
            if ($(this).prop("checked") == true) {
                var src = GUID.NewGUID();
                var dest = GUID.NewGUID();
                var menuItem = '<li class="list-group-item no-boarder" ncc-menu-item-id="{MENU_ITEM_ID}" ncc-model-item-id="{MODEL_ID}" ncc-menu-item-action-type="{ACTION_TYPE}" ncc-menu-item-controller="{CONTROLLER}" ncc-menu-item-action="{ACTION}" ncc-menu-item-action-data="{DATA}"  ncc-menu-item-url="/{URL}" ncc-menu-item-target="{TARGET}" ncc-menu-item-order="{ORDER}" ncc-menu-item-title="{TITLE}">'
                    + '<div class="menu-item-content" >'
                    + '<div class="pull-left" style="padding: 5px 5px;"><i class="glyphicon glyphicon-move margin-right-10" ></i>'
                    + '<span id="' + src + '" class="ncc-menu-title">{TITLE}</span></div>'
                    + '<input id="' + dest + '" class="ncc-menu-title-editor" type="text" style="display:none" />'
                    + '&nbsp;&nbsp; <i class="fa fa-edit" onclick="ShowEditor(\'' + src + '\', \'' + dest + '\')"></i>'
                    + '<input type="button" class="closeMenuItem pull-right" value="x" onclick="RemoveMenuItem(this)" ></input>'
                    + '</div>'
                    + '</li> ';
                menuItem = menuItem.replace("{MENU_ITEM_ID}", "0");
                menuItem = menuItem.replace("{MODEL_ID}", $(this).attr("value"));
                menuItem = menuItem.replace("{ACTION_TYPE}", "BlogCategory");
                menuItem = menuItem.replace("{CONTROLLER}", "Category");
                menuItem = menuItem.replace("{ACTION}", "Index");
                menuItem = menuItem.replace("{DATA}", $(this).attr("value"));
                menuItem = menuItem.replace("{URL}", "Category/"+$(this).attr("ncc-slug"));
                menuItem = menuItem.replace("{TARGET}", "_self");
                menuItem = menuItem.replace("{ORDER}", 0);
                menuItem = menuItem.replace(/\{TITLE}/g, $(this).attr("ncc-title"));

                $("#selectedMenuTree").append($($.parseHTML(menuItem)));
                $(this).prop("checked", false);
            }
        });
    });
    var searchCategoryLastKey = "";
    function searchCategory() {
        var searchKey = $("#searchCategory").val();
        var languageKey = $("#languageCategory").val();
        if (searchKey != (searchCategoryLastKey + languageKey)) {
            $("#searchCategoriesCheckBoxList").html("");
            if (searchKey != "") {
                $('#searchCategoriesCheckBoxList').append($('<br /><label>Searching ... </label>'));
            }
        }
        setTimeout(function () {
            var searchKey1 = $("#searchCategory").val();
            //console.log(searchKey1);
            if (searchKey != "" && searchKey == searchKey1 && searchKey != (searchCategoryLastKey + languageKey)) {
                searchCategoryLastKey = searchKey + languageKey;
                $.ajax({
                    type: "POST",
                    url: "/CmsMenu/LoadCategories",
                    data: { name: searchKey, lang: languageKey },
                    success: function (rsp) {
                        //console.log(rsp);
                        if (rsp.isSuccess) {
                            $("#searchCategoriesCheckBoxList").html("");
                            //NccAlert.ShowSuccess(rsp.message);
                            $.each(rsp.data, function (i, v) {
                                $('#searchCategoriesCheckBoxList').append($('<div class="checkbox"><label><input type="checkbox" class="recentCategoryCheckBoxList" value="' + v.id + '" ncc-title="' + v.name + '" ncc-slug="' + v.slug + '">' + v.name + '</label></div>'));
                            });
                        }
                        else {
                            $("#searchCategoriesCheckBoxList").html("");
                            //NccAlert.ShowError("Error: " + rsp.message);
                            $('#searchCategoriesCheckBoxList').append($('<br /><label>No result found</label>'));
                        }
                    },
                    error: function (errorThrown) {
                        NccAlert.ShowError("Error. Please try again.");
                    }
                });
            }
        }, 1000);
    }
    $("#searchCategory").on("change keyup paste", function () {
        searchCategory();
    });
    $("#languageCategory").on("change", function () {
        searchCategory();
    });
    /*Category section end*/

    /*Module section start*/
    $("#selectAllModules").on("click", function () {
        $(".moduleList .active").find("input[type='checkbox']").each(function (i, ob) {
            $(ob).prop("checked", true);
        });
    });

    $("#addModuleToMenu").on("click", function () {
        $(".allModuleCheckBoxList").each(function (index) {
            if ($(this).prop("checked") == true) {
                var src = GUID.NewGUID();
                var dest = GUID.NewGUID();
                var menuItem = '<li class="list-group-item no-boarder" ncc-menu-item-id="{MENU_ITEM_ID}" ncc-model-item-id="{MODEL_ID}" ncc-menu-item-module="{MODULE_NAME}" ncc-menu-item-action-type="{ACTION_TYPE}" ncc-menu-item-controller="{CONTROLLER}" ncc-menu-item-action="{ACTION}" ncc-menu-item-action-data="{DATA}"  ncc-menu-item-url="{URL}" ncc-menu-item-target="{TARGET}" ncc-menu-item-order="{ORDER}" ncc-menu-item-title="{TITLE}">'
                    + '<div class="menu-item-content" >'
                    + '<div class="pull-left" style="padding: 5px 5px;"><i class="glyphicon glyphicon-move margin-right-10" ></i>'
                    + '<span id="' + src + '" class="ncc-menu-title">{TITLE}</span></div>'
                    + '<input id="' + dest + '" class="ncc-menu-title-editor" type="text" style="display:none" />'
                    + '&nbsp;&nbsp; <i class="fa fa-edit" onclick="ShowEditor(\'' + src + '\', \'' + dest + '\')"></i>'
                    + '<input type="button" class="closeMenuItem pull-right" value="x" onclick="RemoveMenuItem(this)" ></input>'
                    + '</div>'
                    + '</li> ';
                menuItem = menuItem.replace("{MENU_ITEM_ID}", "0");
                menuItem = menuItem.replace("{MODEL_ID}", "0");
                menuItem = menuItem.replace("{ACTION_TYPE}", "Module");
                menuItem = menuItem.replace("{CONTROLLER}", "");
                menuItem = menuItem.replace("{ACTION}", "");
                menuItem = menuItem.replace("{DATA}", $(this).attr("value"));
                menuItem = menuItem.replace("{URL}", $(this).attr("value"));
                menuItem = menuItem.replace("{TARGET}", "_self");
                menuItem = menuItem.replace("{ORDER}", 0);
                menuItem = menuItem.replace(/\{TITLE}/g, $(this).attr("ncc-title"));
                var moduleName = $(this).attr("data-module-name");
                if (moduleName == null || moduleName == undefined)
                    moduleName = "";
                menuItem = menuItem.replace("{MODULE_NAME}", moduleName);                

                $("#selectedMenuTree").append($($.parseHTML(menuItem)));
                $(this).prop("checked", false);
            }
        });
    });

    var searchModuleLastKey = "";
    function searchModule() {
        var searchKey = $("#searchModule").val();
        if (searchKey != (searchModuleLastKey)) {
            $("#searchModulesCheckBoxList").html("");
            if (searchKey != "") {
                $('#searchModulesCheckBoxList').append($('<br /><label>Searching ... </label>'));
            }
        }
        setTimeout(function () {
            var searchKey1 = $("#searchModule").val();
            //console.log(searchKey1);
            if (searchKey != "" && searchKey == searchKey1 && searchKey != (searchModuleLastKey)) {
                searchModuleLastKey = searchKey;
                $.ajax({
                    type: "POST",
                    url: "/CmsMenu/LoadModules",
                    data: { name: searchKey },
                    success: function (rsp) {
                        //console.log(rsp);
                        if (rsp.isSuccess) {
                            $("#searchModulesCheckBoxList").html("");
                            //NccAlert.ShowSuccess(rsp.message);
                            var isFirstMenu = true;
                            var lastModuleName = "";
                            $.each(rsp.data, function (i, v) {
                                if (lastModuleName != v.moduleName) {
                                    lastModuleName = v.moduleName;
                                    if (isFirstMenu == true) {
                                        $('#searchModulesCheckBoxList').append('<div style="border: solid 1px rgba(228, 228, 228, 0.88);"><div style="padding: 5px 5px;height:30px;background-color:whitesmoke;"><strong> ' + v.moduleName + '</strong></div><div>');
                                        isFirstMenu = false;
                                    }
                                    else {
                                        $('#searchModulesCheckBoxList').append('</div><div style="border: solid 1px rgba(228, 228, 228, 0.88);"><div style="padding: 5px 5px;height:30px;background-color:whitesmoke;"><strong> ' + v.moduleName + '</strong></div><div>');
                                    }
                                }
                                $('#searchModulesCheckBoxList').append($('<div class="checkbox" style="padding: 5px 5px;"><label><input type="checkbox" class="allModuleCheckBoxList" data-module-name="' + v.moduleName+'" value="' + v.menuUrl + '" ncc-title="' + v.menuItemName + '" >' + v.menuItemName + '</label></div>'));
                            });
                            $('#searchModulesCheckBoxList').append('</div>');
                        }
                        else {
                            $("#searchModulesCheckBoxList").html("");
                            //NccAlert.ShowError("Error: " + rsp.message);
                            $('#searchModulesCheckBoxList').append($('<br /><label>No result found</label>'));
                        }
                    },
                    error: function (errorThrown) {
                        NccAlert.ShowError("Error. Please try again.");
                    }
                });
            }
        }, 1000);
    }
    $("#searchModule").on("change keyup paste", function () {
        searchModule();
    });
    /*Module section end*/

    /*Tag section start*/
    $("#selectAllTags").on("click", function () {
        $(".tagList .active").find("input[type='checkbox']").each(function (i, ob) {
            $(ob).prop("checked", true);
        });
    });
    $("#addTagToMenu").on("click", function () {
        $(".recentTagCheckBoxList").each(function (index) {
            if ($(this).prop("checked") == true) {
                var src = GUID.NewGUID();
                var dest = GUID.NewGUID();
                var menuItem = '<li class="list-group-item no-boarder" ncc-menu-item-id="{MENU_ITEM_ID}" ncc-model-item-id="{MODEL_ID}" ncc-menu-item-action-type="{ACTION_TYPE}" ncc-menu-item-controller="{CONTROLLER}" ncc-menu-item-action="{ACTION}" ncc-menu-item-action-data="{DATA}"  ncc-menu-item-url="/{URL}" ncc-menu-item-target="{TARGET}" ncc-menu-item-order="{ORDER}" ncc-menu-item-title="{TITLE}">'
                    + '<div class="menu-item-content" >'
                    + '<div class="pull-left" style="padding: 5px 5px;"><i class="glyphicon glyphicon-move margin-right-10" ></i>'
                    + '<span id="' + src + '" class="ncc-menu-title">{TITLE}</span></div>'
                    + '<input id="' + dest + '" class="ncc-menu-title-editor" type="text" style="display:none" />'
                    + '&nbsp;&nbsp; <i class="fa fa-edit" onclick="ShowEditor(\'' + src + '\', \'' + dest + '\')"></i>'
                    + '<input type="button" class="closeMenuItem pull-right" value="x" onclick="RemoveMenuItem(this)" ></input>'
                    + '</div>'
                    + '</li> ';
                menuItem = menuItem.replace("{MENU_ITEM_ID}", "0");
                menuItem = menuItem.replace("{MODEL_ID}", $(this).attr("value"));
                menuItem = menuItem.replace("{ACTION_TYPE}", "Tag");
                menuItem = menuItem.replace("{CONTROLLER}", "Tags");
                menuItem = menuItem.replace("{ACTION}", "Index");
                menuItem = menuItem.replace("{DATA}", $(this).attr("value"));
                menuItem = menuItem.replace("{URL}", "Tags/?name=" + $(this).attr("ncc-title"));
                menuItem = menuItem.replace("{TARGET}", "_self");
                menuItem = menuItem.replace("{ORDER}", 0);
                menuItem = menuItem.replace(/\{TITLE}/g, $(this).attr("ncc-title"));

                $("#selectedMenuTree").append($($.parseHTML(menuItem)));
                $(this).prop("checked", false);
            }
        });
    });
    var searchTagLastKey = "";
    function searchTag() {
        var searchKey = $("#searchTag").val();
        if (searchKey != (searchTagLastKey)) {
            $("#searchTagsCheckBoxList").html("");
            if (searchKey != "") {
                $('#searchTagsCheckBoxList').append($('<br /><label>Searching ... </label>'));
            }
        }
        setTimeout(function () {
            var searchKey1 = $("#searchTag").val();
            //console.log(searchKey1);
            if (searchKey != "" && searchKey == searchKey1 && searchKey != (searchTagLastKey)) {
                searchTagLastKey = searchKey;
                $.ajax({
                    type: "POST",
                    url: "/CmsMenu/LoadTags",
                    data: { name: searchKey },
                    success: function (rsp) {
                        //console.log(rsp);
                        if (rsp.isSuccess) {
                            $("#searchTagsCheckBoxList").html("");
                            //NccAlert.ShowSuccess(rsp.message);
                            $.each(rsp.data, function (i, v) {
                                $('#searchTagsCheckBoxList').append($('<div class="checkbox"><label><input type="checkbox" class="recentTagCheckBoxList" value="' + v.id + '" ncc-title="' + v.name + '" ncc-slug="">' + v.name + '</label></div>'));
                            });
                        }
                        else {
                            $("#searchTagsCheckBoxList").html("");
                            //NccAlert.ShowError("Error: " + rsp.message);
                            $('#searchTagsCheckBoxList').append($('<br /><label>No result found</label>'));
                        }
                    },
                    error: function (errorThrown) {
                        NccAlert.ShowError("Error. Please try again.");
                    }
                });
            }
        }, 1000);
    }
    $("#searchTag").on("change keyup paste", function () {
        searchTag();
    });
    /*Tag section end*/

    $("#addUrlToMenu").on("click", function () {
        var urlMenuName = $("#urlMenuName").val();
        var urlMenuUrl = $("#urlMenuUrl").val();
        var urlMenuTarget = $("#urlMenuTarget").val();
        if (urlMenuName != "" && urlMenuUrl != "") {

            var src = GUID.NewGUID();
            var dest = GUID.NewGUID();

            var menuItem = '<li class="list-group-item no-boarder" ncc-menu-item-id="{MENU_ITEM_ID}" ncc-model-item-id="{MODEL_ID}" ncc-menu-item-action-type="{ACTION_TYPE}" ncc-menu-item-controller="{CONTROLLER}" ncc-menu-item-action="{ACTION}" ncc-menu-item-action-data="{DATA}"  ncc-menu-item-url="{URL}" ncc-menu-item-target="{TARGET}" ncc-menu-item-order="{ORDER}" ncc-menu-item-title="{TITLE}">'
                + '<div class="menu-item-content" >'
                + '<div class="pull-left" style="padding: 5px 5px;"><i class="glyphicon glyphicon-move margin-right-10" ></i>'
                + '<span id="'+src+'" class="ncc-menu-title">{TITLE}</span></div>'
                + '<input id="'+dest+'" class="ncc-menu-title-editor" type="text" style="display:none" />'
                + '&nbsp;&nbsp; <i class="fa fa-edit" onclick="ShowEditor(\''+src+'\', \''+dest+'\')"></i>'
                + '<input type="button" class="closeMenuItem pull-right" value="x" onclick="RemoveMenuItem(this)" ></input>'
                + '</div>'
                + '</li> ';
            menuItem = menuItem.replace("{MENU_ITEM_ID}", "0");
            menuItem = menuItem.replace("{MODEL_ID}", "");
            menuItem = menuItem.replace("{ACTION_TYPE}", "");
            menuItem = menuItem.replace("{CONTROLLER}", "");
            menuItem = menuItem.replace("{ACTION}", "");
            menuItem = menuItem.replace("{DATA}", "");
            menuItem = menuItem.replace("{URL}", urlMenuUrl);
            menuItem = menuItem.replace("{TARGET}", urlMenuTarget);
            menuItem = menuItem.replace("{ORDER}", 0);
            menuItem = menuItem.replace(/\{TITLE}/g, urlMenuName);

            $("#selectedMenuTree").append($($.parseHTML(menuItem)));
            $("#urlMenuName").val("");
            $("#urlMenuUrl").val("");
            $("#urlMenuTarget").val("_self");
        }
    });


    $("#saveMenuBtn").on("click", function () {

        var menuItemTree = getMenuTree($("ul#selectedMenuTree"), null, 1);

        var menu = new NccMenu();
        menu.Id = $("#menuId").val();
        $(menuItemTree).each(function (index) {
            menu.Items.push(menuItemTree[index]);
        });
        
        menu.Name = $("#menuName").val();
        menu.Position = $("#menuLocation").val();
        menu.MenuFor = $("#menuFor").val();
        menu.MenuLanguage = $("#menuLanguage").val();
        
        NccPageMask.ShowLoadingMask();

        var menuJson = JSON.stringify(menu);
        console.log(menuJson);

        $.ajax({
            type: "POST",
            url: "/CmsMenu/CreateEditMenu",
            data: { model : menuJson },
            success: function (rsp, textStatus, jqXHR) {
                NccPageMask.HideLoadingMask();
                if (rsp.isSuccess) {
                    NccAlert.ShowSuccess(rsp.message);
                }
                else {
                    NccAlert.ShowError("Error: " + rsp.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                NccPageMask.HideLoadingMask();
                NccAlert.ShowError("Error. Please try again.");
            }
        });

    });    
});