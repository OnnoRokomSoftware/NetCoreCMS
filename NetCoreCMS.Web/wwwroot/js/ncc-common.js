

GUID = new function () {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    this.NewGUID = function () {
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
            s4() + '-' + s4() + s4() + s4();
    }
}

NccPageMask = new function () {
    this.ShowLoadingMask = function () {
        $("#loadingMask").addClass("is-active");
    };
    this.HideLoadingMask = function () {
        $("#loadingMask").removeClass("is-active");
    };
}

NccAlert = new function () {

    this.AutoHideInterval = 5000;

    this.ShowError = function (message) {
        var id = GUID.NewGUID();
        var msg = '<div id="' + id + '" class="alert alert-danger"> <strong>' + message + '</strong><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div ></div >';
        $("#messageContainer").append(msg);
        setTimeout(function () {
            $("#" + id).hide();
        }, this.AutoHideInterval);    
    }

    this.ShowSuccess = function (message) {
        var id = GUID.NewGUID();
        var msg = '<div id="' + id + '" class="alert alert-success"> <strong>' + message + '</strong><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div >';
        $("#messageContainer").append(msg);
        setTimeout(function () {
            $("#" + id).hide();
        }, this.AutoHideInterval);
    }

    this.ShowInfo = function (message) {
        var id = GUID.NewGUID();
        var msg = '<div id="' + id + '" class="alert alert-info"> <strong>' + message + '</strong><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div >';
        $("#messageContainer").append(msg);
        setTimeout(function () {
            $("#" + id).hide();
        }, this.AutoHideInterval);
    }
    this.ShowWarning = function (message) {
        var id = GUID.NewGUID();
        var msg = '<div id="' + id + '" class="alert alert-warning"> <strong>' + message + '</strong><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div >';
        $("#messageContainer").append(msg);
        setTimeout(function () {
            $("#" + id).hide();
        }, this.AutoHideInterval);
    }
}

NccGlobalAlert = new function () {

    this.AutoHideInterval = 5000;

    this.ShowError = function (message) {
        var id = GUID.NewGUID();
        var msg = '<div id="' + id + '" class="alert alert-danger"> <strong>' + message + '</strong><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div ></div >';
        $("#globalMessageContainer").append(msg);
        setTimeout(function () {
            $("#" + id).hide();
        }, this.AutoHideInterval);
    }

    this.ShowSuccess = function (message) {
        var id = GUID.NewGUID();
        var msg = '<div id="' + id + '" class="alert alert-success"> <strong>' + message + '</strong><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div >';
        $("#globalMessageContainer").append(msg);
        setTimeout(function () {
            $("#" + id).hide();
        }, this.AutoHideInterval);
    }

    this.ShowInfo = function (message) {
        var id = GUID.NewGUID();
        var msg = '<div id="' + id + '" class="alert alert-info"> <strong>' + message + '</strong><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div >';
        $("#globalMessageContainer").append(msg);
        setTimeout(function () {
            $("#" + id).hide();
        }, this.AutoHideInterval);
    }
    this.ShowWarning = function (message) {
        var id = GUID.NewGUID();
        var msg = '<div id="' + id + '" class="alert alert-warning"> <strong>' + message + '</strong><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div >';
        $("#globalMessageContainer").append(msg);
        setTimeout(function () {
            $("#" + id).hide();
        }, this.AutoHideInterval);
    }
}

NccUtil = new function () {
    this.GetSafeSlug = function(string){
        return string
            .toLowerCase()
            .replace(/ /g, '-')
            .replace(/[^\w-]+/g, '');
    }
}