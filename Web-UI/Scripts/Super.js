(function () {

    var superHub = $.connection.superHub;
    $.connection.hub.logging = true;
    $.connection.hub.start();

    superHub.client.Hello = function (json) {
        //..
    }

    var Model = function () {
        var self = this;
        self.hest = ko.o

    }
}());