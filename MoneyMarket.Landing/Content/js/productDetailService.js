var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var ProductDetailController = (function (_super) {
    __extends(ProductDetailController, _super);
    function ProductDetailController($scope, $http) {
        _super.call(this, $scope, $http);
        this.loading = false;
        this.relationProducts = [];
        this.topFollowedProducts = [];
        this.productPrices = '';
        var that = this;
        that.ready();
    }
    ProductDetailController.prototype.ready = function () {
        var q = this.gup("id", window.location.href);
        //this.keyword = q;
        if (this.getCookie("access_token")) {
            this.session_username = this.getCookie("user_name");
        }
        var that = this;
        this.http({
            method: 'Post',
            headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45' },
            data: { 'Start': 0, 'Length': 5 },
            url: this.siteAddress + '/featured/TopFollowedProducts'
        }).then(function successCallback(response) {
            that.topFollowedProducts = response.data['response_data'].items;
        }, function errorCallback(response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });
        window['$'].blockUI({ message: '<h1>Lütfen bekleyiniz...</h1>' });
        this.http({
            method: 'Get',
            //headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45' },
            url: this.siteAddress + '/Product/Get?id=' + q
        }).then(function successCallback(response) {
            window['$'].unblockUI();
            that.loading = false;
            that.product = response.data['response_data'];
            for (var i = 0; i < that.product.histories.length; i++) {
                that.productPrices += that.product.histories[i].price + ',';
            }
            that.productPrices = that.productPrices.replace(/,\s*$/, "");
            that.http({
                method: 'Post',
                data: { 'Start': 0, 'Length': 9, 'search_text': that.product.name + ' ' + that.product.web_site_name, 'is_relational': true },
                url: that.siteAddress + '/Product/Search'
            }).then(function successCallback(response) {
                window['$'].unblockUI();
                that.loading = false;
                that.relationProducts = response.data['response_data'].items;
                //$('[data-sparkline]').each(initSparkLine);
            }, function errorCallback(response) {
                window['$'].unblockUI();
                // called asynchronously if an error occurs
                // or server returns response with an error status.
            });
            //$('[data-sparkline]').each(initSparkLine);
        }, function errorCallback(response) {
            window['$'].unblockUI();
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });
    };
    ProductDetailController.prototype.follow = function (url) {
        if (this.getCookie("access_token")) {
            window.location.href = "http://web.raqun.co/product/list?url=" + url;
        }
        else {
            window["toastr"].warning("Lütfen Giriş Yapın!");
        }
    };
    return ProductDetailController;
}(BaseCtrl));
;
app.controller("ProductDetailController", ProductDetailController);
//# sourceMappingURL=productDetailService.js.map