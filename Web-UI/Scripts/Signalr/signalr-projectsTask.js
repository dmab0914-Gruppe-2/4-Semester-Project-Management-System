// JavaScript for all of project related signalR functions

this.hub = $.connection.projectHub;



$(function () {

    function Project(id, title, description, creadtedDate, lastChangedDate, done) {
        this.Id = ko.observable(id);
        this.Title = ko.observable(title);
        this.Description = ko.observable(description);
        this.CreadtedDate = ko.observable(creadtedDate);
        this.LastChangedDate = ko.observable(lastChangedDate);
        this.Done = ko.observable(done);
    }
    function Projectbase() {
        this.projects = ko.observableArray([]);
        var projects = this.projects;

        this.init = function () {
            console.log("init");
            hub.server.getProjects();
        };

        hub.client.populateProjects = function (ProjectArray) {
            console.log("populate projects");
            var projectColletion = $.map(ProjectArray, function (item) {
                return new Project(item.Id, item.Title, item.Description, item.CreadtedDate, item.LastChangedDate, item.Done);
            });
            projects(projectColletion);
            getTasksforProjectEvent();
        };

        hub.client.addedProject = function (project) {
            console.log("project added");
            projects.push(new Project(project.Id, project.Title, project.Description, project.CreadtedDate, project.LastChangedDate, project.Done));
            getTasksforProjectEvent();
        };

        hub.client.removeProject = function (project) {
            console.log("project removed " + project.Title);

            var index = projects.indexOf(project);
            projects.splice(index, 1);
            clearList();
        };

        hub.client.changedTask = function () {
            console.log("tasklist changed");
            //getTasksforProjectEvent();
            tasklistChanged();
        };
    }
    var projectbase = new Projectbase();
    
    $.connection.hub.start().done(function () {
        // initialization logic that has to occur after SignalR startup
       projectbase.init();        
    });
    ko.applyBindings(projectbase);
});