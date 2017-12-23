var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var AccountController = (function (_super) {
    __extends(AccountController, _super);
    function AccountController($scope, $http) {
        _super.call(this, $scope, $http);
        var that = this;
        that.ready();
    }
    AccountController.prototype.ready = function () {
        var that = this;
        if (this.getCookie("access_token")) {
            this.session_username = this.getCookie("user_name");
        }
    };
    return AccountController;
}(BaseCtrl));
app.controller("AccountController", AccountController);
//# sourceMappingURL=accountService.js.map