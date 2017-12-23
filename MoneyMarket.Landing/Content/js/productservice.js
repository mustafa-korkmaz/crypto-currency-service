var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var ProductListController = (function (_super) {
    __extends(ProductListController, _super);
    function ProductListController($scope, $http) {
        _super.call(this, $scope, $http);
        this.searchText = '';
        this.keyword = '';
        this.username = '';
        this.password = '';
        this.loading = false;
        this.siteAddress = window["siteAddress"];
        this.recentProducts = [];
        this.searchProducts = [];
        this.topFollowedProducts = [];
        var that = this;
        that.ready();
    }
    ProductListController.prototype.ready = function () {
        var that = this;
        var q = this.gup("q", window.location.href);
        this.keyword = q;
        this.http({
            method: 'Post',
            headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45' },
            data: { 'Start': 0, 'Length': 9 },
            url: this.siteAddress + '/featured/recentFollowedProducts'
        }).then(function successCallback(response) {
            that.recentProducts = response.data['response_data'].items;
        }, function errorCallback(response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });
        this.http({
            method: 'Post',
            headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45' },
            data: { 'Start': 0, 'Length': 9 },
            url: this.siteAddress + '/featured/TopFollowedProducts'
        }).then(function successCallback(response) {
            that.topFollowedProducts = response.data['response_data'].items;
        }, function errorCallback(response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });
        that.scope.$watch('ctrl.searchText', function (newValue, oldValue) {
            if (newValue && newValue['length'] > 2) {
                that.loading = true;
                that.http({
                    method: 'Post',
                    //headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45' },
                    data: { 'Start': 0, 'Length': 10, 'search_text': newValue },
                    url: this.siteAddress + '/Product/Search'
                }).then(function successCallback(response) {
                    that.loading = false;
                    that.searchProducts = response.data['response_data'].items;
                }, function errorCallback(response) {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
            }
            else {
                this.searchProducts = [];
            }
        }, true);
        that.getTotalProductCounts();
        that.getTotalUserCounts();
    };
    //login() {
    //    var payload = 'username=' + this.username + '&password=' + this.password + '&grant_type=password';
    //    this.http({
    //        method: 'Post',
    //        headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45', 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' },
    //        data: payload,
    //        url: this.siteAddress + '/token'
    //    }).then(function successCallback(response) {
    //    }, function errorCallback(response) {
    //        // called asynchronously if an error occurs
    //        // or server returns response with an error status.
    //    });
    //}
    ProductListController.prototype.getTotalProductCounts = function () {
        var that = this;
        this.http({
            method: 'Get',
            url: this.siteAddress + '/Product/totalProducts'
        }).then(function successCallback(response) {
            that.totalProductsCount = response.data;
        }, function errorCallback(response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });
    };
    ProductListController.prototype.getTotalUserCounts = function () {
        var that = this;
        this.http({
            method: 'Get',
            url: this.siteAddress + '/totalUserCounts'
        }).then(function successCallback(response) {
            that.totalUsersCount = response.data;
        }, function errorCallback(response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });
    };
    return ProductListController;
}(BaseCtrl));
app.controller("ProductListController", ProductListController);
//# sourceMappingURL=productservice.js.map