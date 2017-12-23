var app = angular.module("productServiceApp", []);
//Create the controller

//Register controller with module
app.controller("ProductListController", function ($scope, $http) {
    $scope.searchText = '';
    $scope.keyword = '';
    $scope.username = '';
    $scope.password = '';
    $scope.loading = false;
    var siteAddress = window["siteAddress"];
    $scope.recentProducts = [];
    $scope.searchProducts = [];

    var q = gup("q", window.location.href);
    $scope.keyword = q;

    $http({
        method: 'Post',
        headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45' },
        data: { 'Start': 0, 'Length': 9 },
        url: siteAddress + '/featured/recentFollowedProducts'
    }).then(function successCallback(response) {
        $scope.recentProducts = response.data.response_data.items;
    }, function errorCallback(response) {
        // called asynchronously if an error occurs
        // or server returns response with an error status.
    });

   

    $scope.topFollowedProducts = [];
    $http({
        method: 'Post',
        headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45' },
        data: { 'Start': 0, 'Length': 9 },
        url: siteAddress + '/featured/TopFollowedProducts'
    }).then(function successCallback(response) {
        $scope.topFollowedProducts = response.data.response_data.items;
    }, function errorCallback(response) {
        // called asynchronously if an error occurs
        // or server returns response with an error status.
    });
    $scope.$watch('ctrl.searchText', function (newValue, oldValue) {
        if (newValue && newValue.length > 2) {
            $scope.loading = true;
            $http({
                method: 'Post',
                //headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45' },
                data: { 'Start': 0, 'Length': 10, 'search_text': newValue },
                url: siteAddress + '/Product/Search'
            }).then(function successCallback(response) {
                $scope.loading = false;
                $scope.searchProducts = response.data.response_data.items;
            }, function errorCallback(response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
            });
        } else {
            $scope.searchProducts = [];
        }
    }, true);

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
    return results === null ? null : results[1];
}