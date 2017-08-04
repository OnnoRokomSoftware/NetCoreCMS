
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
        $.notify(message, "error");
    }

    this.ShowSuccess = function (message) {
        $.notify(message, "success");
    }

    this.ShowInfo = function (message) {
        $.notify(message, "info");
    }

    this.ShowWarning = function (message) {
        $.notify(message, "warning");
    }

    this.ShowMessages = function (messagesObjList) {
        for (i = 0; i < messagesObjList.length; i++) {
            var msg = messagesObjList[i];
            if (msg.isSuccess) {
                this.ShowSuccess(msg.message);
            }
            else {
                this.ShowError(msg.message);
            }
        }
    }
}

NccGlobalAlert = new function () {
 
    this.AutoHideInterval = 5000;

    this.ShowError = function (message) {
        //var id = GUID.NewGUID();
        //var msg = '<div id="' + id + '" class="alert alert-danger"> <strong>' + message + '</strong><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div ></div >';
        //$("#globalMessageContainer").append(msg);
        //setTimeout(function () {
        //    $("#" + id).hide();
        //}, this.AutoHideInterval);
        $.notify(message, "error");
    }

    this.ShowSuccess = function (message) {
        $.notify(message, "success");
    }

    this.ShowInfo = function (message) {
        $.notify(message, "info");
    }
    this.ShowWarning = function (message) {
        $.notify(message, "warning");
    }
}

NccUtil = new function () {
    this.GetSafeSlug = function (string) {
        return string
            //.toLowerCase()
            .replace(/ /g, '-')
            .replace(/[.,\/#!$%\^&\*;:{}=\+_`~()]/g, "");
    };

    this.Log = function (data) {
        console.log(data);
    };

    this.JsonToArray = function(json){
        var result = [];
        var keys = Object.keys(json);
        keys.forEach(function (key) {
            result[key] = (json[key]);
        });
        return result;    
    }
}
