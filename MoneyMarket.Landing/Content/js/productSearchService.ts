class ProductSearchController extends BaseCtrl {
    constructor($scope: ng.IScope, $http: ng.IHttpService) {
        super($scope, $http);

        var that = this;
        that.ready();
    }

    keyword = '';
    username = '';
    password = '';
    loading = false;
    searchProducts = [];
    searchHistories = [];
    siteAddress = window["siteAddress"];
    currentPage = 0;
    numPerPage = 10;
    maxSize = 5;
    totalCount = 1;

    ready() {
        var q = this.gup("q", window.location.href);
        console.log(q);
        if (q) {
            this.keyword = decodeURI(q);
        }
    }

//this.searchHistories.push(this.keyword);

enterSearch (event) {
    if (event.keyCode === 13) {
        this.refreshSearch(event);
    } else {
        return false;
    }
}

refreshSearch  (event) {
    this.currentPage = 0;
    this.numPerPage = 10;
    this.maxSize = 5;
    this.search();
}

changeSearch (keyword) {
    this.keyword = keyword;
    this.refreshSearch(null);
}

search  () {
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
//};
}
app.controller("ProductSearchController", ProductSearchController);