class ProductListController extends BaseCtrl {
    constructor($scope: ng.IScope, $http: ng.IHttpService) {
        super($scope, $http);

        var that = this;
        that.ready();
    }

    searchText = '';
    keyword = '';
    username = '';
    password = '';
    loading = false;
    siteAddress = window["siteAddress"];
    recentProducts = [];
    searchProducts = [];
    totalProductsCount: number;
    totalUsersCount: number;

    topFollowedProducts = [];

    ready() {
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
            } else {
                this.searchProducts = [];
            }
        }, true);

        that.getTotalProductCounts();
        that.getTotalUserCounts();
    }

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

    getTotalProductCounts() {
        var that = this;

        this.http({
            method: 'Get',
            url: this.siteAddress + '/Product/totalProducts'
        }).then(function successCallback(response) {
            that.totalProductsCount = <number>response.data;
        }, function errorCallback(response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });
    }

    getTotalUserCounts() {
        var that = this;

        this.http({
            method: 'Get',
            url: this.siteAddress + '/totalUserCounts'
        }).then(function successCallback(response) {
            that.totalUsersCount = <number>response.data;
        }, function errorCallback(response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });
    }
}
app.controller("ProductListController", ProductListController);