var app = angular.module("productServiceApp", ['ui.bootstrap']);
//Create the controller

//Register controller with module
app.controller("ProductListController", function ($scope, $http) {
    $scope.keyword = '';
    $scope.username = '';
    $scope.password = '';
    $scope.loading = false;
    $scope.searchProducts = [];
    $scope.searchHistories = [];
    var siteAddress = window["siteAddress"];
    $scope.currentPage = 0;
    $scope.numPerPage = 10;
    $scope.maxSize = 5;
    $scope.totalCount = 1;

    var q = gup("q", window.location.href);

    if (q) {
        $scope.keyword = decodeURI(q);
    }
    //$scope.searchHistories.push($scope.keyword);

    $scope.enterSearch = function (event) {
        if (event.keyCode === 13) {
            $scope.refreshSearch();
        } else {
            return false;
        }
    }

    $scope.refreshSearch = function (event) {
        $scope.currentPage = 0;
        $scope.numPerPage = 10;
        $scope.maxSize = 5;
        $scope.search();
    }

    $scope.changeSearch = function (keyword) {
        $scope.keyword = keyword;
        $scope.refreshSearch();
    }

    $scope.search = function () {
        //$scope.searchHistories.push($scope.keyword);
        var start = $scope.currentPage === 0 ? $scope.currentPage * ($scope.numPerPage - 1) : $scope.currentPage * ($scope.numPerPage - 1) + 1;
        $.blockUI({ message: '<h1>Ürünler yükleniyor...</h1>' });
        $http({
            method: 'Post',
            headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45' },
            data: { 'Start': start, 'Length': $scope.numPerPage, 'search_text': $scope.keyword },
            url: siteAddress + '/Product/Search'
        }).then(function successCallback(response) {
            $.unblockUI();
            $scope.loading = false;
            $scope.totalCount = response.data.response_data.total_count;
            $scope.searchProducts = response.data.response_data.items;

            $('html,body').animate({
                scrollTop: 0
            }, 700);
        }, function errorCallback(response) {
            $.unblockUI();
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });
    }

    $scope.login = function () {
        var payload = 'username=' + $scope.username + '&password=' + $scope.password + '&grant_type=password';
        $http({
            method: 'Post',
            headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45', 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' },
            data: payload,
            url: siteAddress + '/token'
        }).then(function successCallback(response) {
   
        }, function errorCallback(response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });
    };
});
function gup(name, url) {
    if (!url) url = location.href;
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(url);
    return results == null ? null : results[1];
}