app.controller('LoginCtrl', ['$scope', 'ngDialog', '$window', '$rootScope', 'authCheck', function ($scope, ngDialog, $window, $rootScope, authCheck) {


    $scope.CallLogin = function () {
        debugger;
        $scope.loadingLogin = true;
        $.post("http://localhost:50750/api/loginctrl/login", { username: $scope.username, password: $scope.password }, function (result) {
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
}]);