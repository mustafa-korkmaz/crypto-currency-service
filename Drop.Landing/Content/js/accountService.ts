class AccountController extends BaseCtrl {
    constructor($scope: ng.IScope, $http: ng.IHttpService) {
        super($scope, $http);

        var that = this;
        that.ready();
    }

  

    ready() {
        var that = this;
        if (this.getCookie("access_token")) {
            this.session_username = this.getCookie("user_name");
        }
    }
}
app.controller("AccountController", AccountController);