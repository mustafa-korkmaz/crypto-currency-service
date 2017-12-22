var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var ProductSearchController = (function (_super) {
    __extends(ProductSearchController, _super);
    function ProductSearchController($scope, $http) {
        _super.call(this, $scope, $http);
        this.keyword = '';
        this.username = '';
        this.password = '';
        this.loading = false;
        this.searchProducts = [];
        this.searchHistories = [];
        this.siteAddress = window["siteAddress"];
        this.currentPage = 0;
        this.numPerPage = 10;
        this.maxSize = 5;
        this.totalCount = 1;
        var that = this;
        that.ready();
    }
    ProductSearchController.prototype.ready = function () {
        var q = this.gup("q", window.location.href);
        console.log(q);
        if (q) {
            this.keyword = decodeURI(q);
        }
    };
    //this.searchHistories.push(this.keyword);
    ProductSearchController.prototype.enterSearch = function (event) {
        if (event.keyCode === 13) {
            this.refreshSearch(event);
        }
        else {
            return false;
        }
    };
    ProductSearchController.prototype.refreshSearch = function (event) {
        this.currentPage = 0;
        this.numPerPage = 10;
        this.maxSize = 5;
        this.search();
    };
    ProductSearchController.prototype.changeSearch = function (keyword) {
        this.keyword = keyword;
        this.refreshSearch(null);
    };
    ProductSearchController.prototype.search = function () {
        //this.searchHistories.push(this.keyword);
        var that = this;
        var start = this.currentPage === 0 ? this.currentPage * (this.numPerPage - 1) : this.currentPage * (this.numPerPage - 1) + 1;
        window['$'].blockUI({ message: '<h1>Ürünler yükleniyor...</h1>' });
        this.http({
            method: 'Post',
            headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45' },
            data: { 'Start': start, 'Length': this.numPerPage, 'search_text': this.keyword },
            url: this.siteAddress + '/Product/Search'
        }).then(function successCallback(response) {
            window['$'].unblockUI();
            that.loading = false;
            that.totalCount = response.data['response_data'].total_count;
            that.searchProducts = response.data['response_data'].items;
            window['$']('html,body').animate({
                scrollTop: 0
            }, 700);
        }, function errorCallback(response) {
            window['$'].unblockUI();
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });
    };
    return ProductSearchController;
}(BaseCtrl));
app.controller("ProductSearchController", ProductSearchController);
//# sourceMappingURL=productSearchService.js.map