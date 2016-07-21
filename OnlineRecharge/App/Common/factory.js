app.factory('authCheck', ['$window', '$rootScope', 'ngDialog', '$timeout', '$location', '$localstorage', function ($window, $rootScope, ngDialog, $timeout, $location, $localstorage) {


    return {
        isUserLoggedIn: function () {
            var obj = $localstorage.getObject('LoginDetails');
            if (!obj.isLoggedIn) {
                //$location.path("/home");
                this.LoginInit();
            } else {
                $rootScope.LogedinName = obj.token;
                $rootScope.isLoggedIn = obj.isLoggedIn;
                // $rootScope.$apply();
            }
        },
        LoginInit: function ($scope) {

            $rootScope.signin = ngDialog.open({
                template: 'login.html',
                className: 'ngdialog-theme-default login-popup',
                controller: ['$scope', function ($scope, $http) {
                    // controller logic
                    $scope.tabs = [{
                        title: 'Login',
                        url: 'innerLogin.html'
                    }, {
                        title: 'Register',
                        url: 'register.html'
                    }];

                    $scope.currentTab = 'innerLogin.html';

                    $scope.loginTabClick = function (tab) {
                        $scope.currentTab = tab.url;
                    }

                    $scope.isActiveTab = function (tabUrl) {
                        return tabUrl == $scope.currentTab;
                    }

                    $scope.CallLogin = function () {
                        debugger;
                        $scope.loadingLogin = true;
                        $.post("http://localhost:50750/api/loginctrl/login", { username: '94950708', password: 'test@16!' }, function (result) {
                            $scope.authenticated = result.token;
                            $scope.success = result.success;
                            $window.sessionStorage.token = result.token;
                            $rootScope.signin.close();
                            $rootScope.LogedinName = result.token;
                            $rootScope.isLoggedIn = true;
                            $rootScope.$apply();
                            $scope.loadingLogin = false;
                            authCheck.SetloginParam({ 'token': result.token, 'isLoggedIn': true })
                        });
                    };

                }]
            });
        }, SetloginParam: function (Obj) {
            $localstorage.setObject('LoginDetails', Obj)
        }, Logout: function () {
            $localstorage.setObject('LoginDetails', {})
            $rootScope.LogedinName = 'My Account';
            $rootScope.isLoggedIn = false;
        }
    }




    // return $rootScope.isLoggedIn;
}])

app.factory('$localstorage', ['$window', function ($window) {
    /** Factory to store and get data from local storages **/
    return {
        set: function (key, value) {
            $window.localStorage[key] = value;
        },
        get: function (key, defaultValue) {
            return $window.localStorage[key] || defaultValue;
        },
        setObject: function (key, value) {
            $window.localStorage[key] = JSON.stringify(value);
        },
        getObject: function (key) {
            return JSON.parse($window.localStorage[key] || '{}');
        }
    }
}])