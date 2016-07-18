var app = angular.module('Multilingual', ['pascalprecht.translate',
  'ngCookies', 'ngRoute', 'internationalPhoneNumber', 'ngDialog']);

app.config(['$routeProvider',
       function ($routeProvider) {
           $routeProvider.
           when('/mobile', {
               templateUrl: 'home/mobile'
           })
           .when('/home', {
               templateUrl:'home/index'
           })
           .when('/paymentOptions', {
               templateUrl: 'home/paymentoptions'
           })
       }]);
app.config(['$translateProvider', function ($translateProvider) {

    $translateProvider
  .useStaticFilesLoader({
      prefix: '/translations/',
      suffix: '.json'
  })
  .preferredLanguage('en')
  .useLocalStorage()
  .useMissingTranslationHandlerLog();
}]);
app.run(['$rootScope', function ($rootScope) {
    $rootScope.lang = 'en';

    $rootScope.default_float = 'left';
    $rootScope.opposite_float = 'right';

    $rootScope.default_direction = 'ltr';
    $rootScope.opposite_direction = 'rtl';
    $rootScope.$on('$routeChangeError', function (evt, current, previous, rejection) {
        console.log('Route error', rejection);
    });
}]);

//Controller  
app.controller('SeviceProviderController', ['$scope', '$http','$translate',
    function ($scope, $http) {
        //$http service for Getting the Country  
        $http({
            method: 'GET',
            url: '/Home/GetServiceProviders'
        }).
        success(function (data) {
            $scope.serviceProviders = data;
        });

        $scope.GetValue = function (operator) {
            var operatorCode = $scope.ddlOperator;
            var OperatorName = $.grep($scope.serviceProviders, function (operator) {
                return operator.Code == operatorCode;
            })[0].Name;
            $window.alert("Selected Value: " + fruitId + "\nSelected Text: " + fruitName);
        }
        $scope.redirect = function () {
            window.location = "#/paymentOptions";
        }
        //$scope.SendData = function () {
        //    // use $.param jQuery function to serialize data from JSON 
        //    var data = $.param({
        //        rechargeType: $scope.rechargeType,
        //        operatorCode: $scope.ddlOperator,
        //        mobileNumber: $scope.mobileNumber,
        //        amount: $scope.amount
        //    });

        //    var config = {
        //        headers: {
        //            'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
        //        }
        //    }

        //    $http.post('/Home/TopupValidation', data, config)
        //    .success(function (data, status, headers, config) {
        //        $scope.PostDataResponse = data;
        //    })
        //    .error(function (data, status, header, config) {
        //        $scope.ResponseDetails = "Data: " + data +
        //            "<hr />status: " + status +
        //            "<hr />headers: " + header +
        //            "<hr />config: " + config;
        //    });
        //};
        //$scope.GetServices = function () {

        //    $http({
        //        method: 'POST',
        //        url: '/Home/GetService/'
        //    }).
        //    success(function (data) {
        //        $scope.services = data;
        //    });
        //}
    }]);
app.controller('TabsCtrl', ['$scope', '$http', '$location', function ($scope, $http, $location) {
    $scope.tabs = [{
        title: 'KNet',
        url: 'knet.html'
    }, {
        title: 'VisaMasterCard',
        url: 'visamaster.html'
    }, {
        title: 'Wallet',
        url: 'wallet.html'
    }];

    $scope.currentTab = 'knet.html';

    $scope.onClickTab = function (tab) {
        $scope.currentTab = tab.url;
    }
    
    $scope.isActiveTab = function(tabUrl) {
        return tabUrl == $scope.currentTab;
    }

    $scope.getPaymentImages = function (refID) {
        if(refID==1)
            return '/Content/img/payment/knet.jpg'
        else if(refID==2)
            return '/Content/img/payment/visA.png'
    };
    $scope.ProcessKnetPayment = function () {
        // use $.param jQuery function to serialize data from JSON 
        var data = $.param({
            amount: 10,
            contactNumber: '+96594950708',
            email: 'vineeth@2easy2pay.com',
            paymentType: 'knet',
            returnUrl: 'https://api.2easy2pay.com/WEB/knetResponse/shopKnetWebResponse',
            errorUrl: 'https://api.2easy2pay.com/WEB/knetResponse/shopKnetWebResponse',
            Udf1:10
        });

        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        $http.post('/Home/ProcessKnetPayment', data, config)
        .success(function (data) {
            // build form

            var form = $('<form></form>');
            form.attr("action", data.URL);
            form.attr("method", "POST");
            $("body").append(form);
            form.submit();
            form.remove();
            });
    };
}]);
app.controller('LanguageSwitchController', ['$scope', '$rootScope', '$translate',
  function ($scope, $rootScope, $translate) {
      $scope.changeLanguage = function (langKey) {
          $translate.use(langKey);
      };

      $rootScope.$on('$translateChangeSuccess', function (event, data) {
          var language = data.language;

          $rootScope.lang = language;

          $rootScope.default_direction = language === 'ar' ? 'rtl' : 'ltr';
          $rootScope.opposite_direction = language === 'ar' ? 'ltr' : 'rtl';

          $rootScope.default_float = language === 'ar' ? 'right' : 'left';
          $rootScope.opposite_float = language === 'ar' ? 'left' : 'right';
      });
  }]);
app.controller('PopUpCtrl', ['$scope','ngDialog',function ($scope, ngDialog) {
    $scope.clickToOpen = function () {
        ngDialog.open({
            template: 'login.html',
            className: 'ngdialog-theme-default login-popup',
            controller: ['$scope',  function ($scope) {
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

                $scope.signin = function (user) {
                    $http.post('api/accounts/signin', user)
                        .success(function (data, status, headers, config) {
                            user.authenticated = true;
                            $rootScope.user = user;
                            $location.path('/');
                        })
                        .error(function (data, status, headers, config) {
                            user.authenticated = false;
                            $rootScope.user = {};
                        });
                };
            }]
        });
    };

   

}]);