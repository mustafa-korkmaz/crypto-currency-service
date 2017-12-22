class BaseCtrl {
    constructor($scope: ng.IScope, $http: ng.IHttpService) {
        var that = this;
        that.http = $http;
        that.scope = $scope;
        this.baseUrl = window['$']('#baseUrl').val();
        this.toastr = window['toastr'];
        this.bootbox = window['bootbox'];
        this.siteAddress = window["siteAddress"];
        that.isAuth();
        //this.toastr.options.fadeOut = 2000;
    }

    baseUrl: string;
    siteAddress: string;
    http: ng.IHttpService;
    scope: ng.IScope;
    toastr: any;
    isauth: any;
    bootbox: any;
    session_username = null;
    username = '';
    forgotemail = '';
    password = '';
    rgpassword = '';
    rgusername = '';
    rgemail = '';

    getUrlVars(key) {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = decodeURIComponent(hash[1]);
        }
        return vars[key];
    }
    logout() {
        var that = this;
        document.cookie = "user_name=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
        document.cookie = "access_token=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
        that.session_username = null;
        that.isauth = false;
        window["toastr"].error("Çıkış Yapıldı.");
    }

    forgotpasswordmodaltoggle() {
        window['$']('#login').modal('toggle');
        window['$']('#forgotpassword').modal('toggle');
    }

    forgotpassword(isValid) {
        var that = this;
        if (isValid) {
            var payload = 'emailorusername=' + this.forgotemail;
            this.http({
                method: 'Post',
                headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45', 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' },
                data: payload,
                url: this.siteAddress + '/remind'
            }).then(function successCallback(response) {
                that.forgotemail = '';
                window['$']('#forgotpassword').modal('toggle');
                window["toastr"].success("Lütfen Email adresinizi kontrol ediniz.");
            }, function errorCallback(response) {
                if (response["data"].message) {
                    window["toastr"].warning(response["data"].message);
                    return;
                }
                if (response['data'].error === 'invalid_grant') {
                    window["toastr"].error(response['data'].error_description);
                } else {
                    window["toastr"].error("Sistemde Hata Oluştu");
                }

                // called asynchronously if an error occurs
                // or server returns response with an error status.
            });
        }
    }

    login(isValid) {
        var that = this;
        if (isValid) {
            var payload = 'username=' + this.username + '&password=' + this.password + '&grant_type=password';
            this.http({
                method: 'Post',
                headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45', 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' },
                data: payload,
                url: this.siteAddress + '/token'
            }).then(function successCallback(response) {
                document.cookie = "access_token=" + response.data['access_token']; //+ ";domain=web.raqun.co";
                document.cookie = "user_name=" + response.data['user_name']; //+ ";domain=web.raqun.co";
                that.username = '';
                that.password = '';
                that.session_username = response.data['user_name'];
                window["toastr"].success("Giriş Başarılı");
                window['$']('#login').modal('toggle');

                that.isAuth();
            }, function errorCallback(response) {
                if (response["data"].message) {
                    window["toastr"].warning(response["data"].message);
                    return;
                }
                if (response['data'].error === 'invalid_grant') {
                    window["toastr"].error(response['data'].error_description);
                } else {
                    window["toastr"].error("Sistemde Hata Oluştu");
                }

                // called asynchronously if an error occurs
                // or server returns response with an error status.
            });
        }
    }

    register(isValid) {
        var that = this;
        if (isValid) {
            var payload = 'username=' + this.rgusername + '&email=' + this.rgemail + '&password=' + this.rgpassword + '&grant_type=password';
            this.http({
                method: 'Post',
                headers: { 'ChannelType': '0', 'ApiKey': '38605538fd5c81be4b0270b8fbd6aa45', 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8' },
                data: payload,
                url: this.siteAddress + '/register'
            }).then(function successCallback(response) {

                if (response.data['response_data'].id) {
                    window["toastr"].success("Kayıt Başarılı");
                    window["toastr"].success("Hemen Giriş Yap");
                }
                that.rgemail = '';
                that.rgusername = '';
                that.rgpassword = '';
                window['$']('#register').modal('toggle');
                window['$']('#login').modal('toggle');

                }, function errorCallback(response) {
                debugger;
                if (response["data"].message) {
                    window["toastr"].warning(response["data"].message);
                        return;
                    }
                // called asynchronously if an error occurs
                // or server returns response with an error status.
            });
        }
    }

    gup(name, url) {
        if (!url) url = location.href;
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(url);
        return results === null ? null : results[1];
    }

    initSparkLine() {
        alert('sds');
        var $element = window['$'](this),
            options = $element.data(),
            values = options.values && options.values.split(',');

        options.type = options.type || 'bar'; // default chart is bar
        options.disableHiddenCheck = true;

        $element.sparkline(values, options);

        if (options.resize) {
            $(window).resize(function () {
                $element.sparkline(values, options);
            });
        }

    }

    isAuth() {
        var that = this;
        that.isauth = false;
        if (this.getCookie("access_token")) {
            that.session_username = this.getCookie("user_name");
            that.isauth = true;
        }
    }

    getCookie(name) {
        var match = document.cookie.match(new RegExp(name + '=([^;]+)'));
        if (match) return match[1];
    }
}