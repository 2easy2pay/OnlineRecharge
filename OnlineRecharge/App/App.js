/// <reference path="C:\Projects\Recharge\OnlineRecharge\OnlineRecharge\Partials/_Result.cshtml" />
var app = angular.module('Multilingual', ['pascalprecht.translate',
   'ngCookies', 'ngRoute', 'internationalPhoneNumber', 'ngDialog', 'slider', 'LocalStorageModule']);

//Configuration Section
app.config(['$routeProvider', '$locationProvider',
function ($routeProvider, $locationProvider) {
    $routeProvider.
    when('/mobile', {
        templateUrl: '/Partials/Mobile.html',
        controller: 'localRechargeController'
    })
    .when('/paymentOptions', {
        templateUrl: 'Partials/PaymentOptions.html',
        controller: 'paymentPageController'
    })
    .when('/result', {
        controller: "rechargeResultController",
        templateUrl: 'Partials/Result.html'
    })
    .when('/international', {
       templateUrl: '/Partials/International.html',
       controller: 'InternationalRechargeController'
    })
   .otherwise({ redirectTo: '/mobile' });

}]);
app.config(function (localStorageServiceProvider) {
    localStorageServiceProvider
      .setPrefix('myApp')
      .setStorageType('sessionStorage')
      .setNotify(true, true)
});
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

//Service
app.factory('rechargeParameterService', [function () {
    var rechargeParams = {};
    return {
        getData: function () {
            return rechargeParams;
        },
        setData: function (data) {
            rechargeParams = data;
        }
    }
}]);

//Controller  
app.controller('navCtrl', ['$scope', '$location', function ($scope, $location) {

    $scope.navLinks = [{
        Title: 'mobile',
        LinkText: 'Mobile',
    }, {
        Title: 'international',
        LinkText: 'International'
    }, {
        Title: 'datacards',
        LinkText: 'DataCards'
    }, {
        Title: 'shoppingcards',
        LinkText: 'ShoppingCards'
    }
    ];

    $scope.navClass = function (page) {
        var currentRoute = $location.path().substring(1) || 'home';
        return page === currentRoute ? 'active' : '';
    };


}]);
app.controller('LanguageSwitchController', ['$scope', '$rootScope', '$translate','authCheck',
function ($scope, $rootScope, $translate,authCheck) {
      authCheck.isUserLoggedIn();
      $rootScope.LogedinName = ($rootScope.LogedinName == 'My Account' || angular.isUndefined($rootScope.LogedinName)) ? 'My Account' : $rootScope.LogedinName;
      $rootScope.isLoggedIn = false;
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


app.controller('PopUpCtrl', ['$scope', 'ngDialog', '$rootScope', 'authCheck', '$window', function ($scope, ngDialog, $rootScope, authCheck, $window) {
    //alert($scope.username);
    $scope.clickToOpen = function () {
        authCheck.isUserLoggedIn();
    };

    $scope.Logout = function () {
        console.log('IN')
        authCheck.Logout()
    }


}]);
app.controller('localRechargeController', ['$scope', '$http', 'localStorageService',
    function ($scope, $http, localStorageService) {
        $scope.redirect = function () {
            if ($scope.rechargeForm.$valid) {
                var paramObj = {
                    serviceType: 'national',
                    rechargeType: $scope.rechargeType,
                    operatorCode: $scope.operatorCode,
                    mobileNumber: $scope.mobileNumber,
                    amount: $scope.amount
                };
                localStorageService.set('nationalRechargeParams', paramObj);
                window.location = "#/paymentOptions";
            }

        }
        //$http service for Getting the ServiceProviders  
        $http({
            method: 'GET',
            url: '/Home/GetServiceProviders'
        }).
        success(function (data) {
            $scope.serviceProviders = data;
        });

        $http({
            method: 'POST',
            url: '/Home/GetAllVouchers'
        }).
       success(function (data) {
           $scope.AllVouchers = data;
       });

        $scope.SelectVoucherValue = function (amount) {
            debugger;
            $scope.amount = amount;
        }
    }]);
app.controller('paymentPageController', ['$scope', '$http', '$location', 'localStorageService', 'authCheck', '$rootScope', function ($scope, $http, $location, localStorageService, authCheck, $rootScope) {
  
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
    $scope.RechargeParameter = localStorageService.get('nationalRechargeParams');
    $scope.currentTab = 'knet.html';

    $scope.onClickTab = function (tab) {
        $scope.currentTab = tab.url;
    }

    $scope.isActiveTab = function (tabUrl) {
        return tabUrl == $scope.currentTab;
    }

    $scope.getPaymentImages = function (refID) {
        if (refID == 1)
            return '/Content/img/payment/knet.jpg'
        else if (refID == 2)
            return '/Content/img/payment/visA.png'
    };
    $scope.ProcessKnetPayment = function () {
        // use $.param jQuery function to serialize data from JSON 
        var data = $.param({
            amount: $scope.RechargeParameter.amount,
            contactNumber: "+" + $scope.RechargeParameter.mobileNumber,
            email: 'info@2easy2pay.com',
            paymentType: 'knet',
            returnUrl: 'https://api.2easy2pay.com/web/knetresponse/rechargeWebResponse',
            errorUrl: 'https://api.2easy2pay.com/web/knetresponse/rechargeWebResponse',
            Udf1: $scope.RechargeParameter.amount
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
app.controller('rechargeResultController', ['$scope', '$location', 'localStorageService', '$http', function ($scope, $location, localStorageService, $http) {
    $scope.jobOffers = [];
    var rechargeParams = localStorageService.get('nationalRechargeParams');
    var search = $location.search();
    $scope.paymentID = search.PaymentID;
    $scope.result = search.Result;
    $scope.trackID = search.TrackID;
    $scope.tranID = search.TranID;
    $scope.ref = search.Ref;
    if (rechargeParams.serviceType == 'national' && rechargeParams.rechargeType == 'Prepaid') {
        var data = $.param({
            rechargeType: rechargeParams.rechargeType,
            operatorCode: rechargeParams.operatorCode,
            mobileNumber: rechargeParams.mobileNumber,
            amount: rechargeParams.amount,
            paymentID: $scope.paymentID,
            result: $scope.result,
            trackID: $scope.trackID,
            tranID: $scope.tranID,
            ref: $scope.ref
        });

        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        $http.post('/Home/TranferNationalTopup', data, config)
        .success(function (data, status, headers, config) {
            $scope.APIResponse = data;
        })
        .error(function (data, status, header, config) {
            $scope.ResponseDetails = "Data: " + data +
                "<hr />status: " + status +
                "<hr />headers: " + header +
                "<hr />config: " + config;
        });
    }
    else if (rechargeParams.serviceType == 'national' && rechargeParams.rechargeType == 'Postpaid') {
        var data = $.param({
            rechargeType: rechargeParams.rechargeType,
            operatorCode: rechargeParams.operatorCode,
            mobileNumber: rechargeParams.mobileNumber,
            amount: rechargeParams.amount,
            paymentID: $scope.paymentID,
            result: $scope.result,
            trackID: $scope.trackID,
            tranID: $scope.tranID,
            ref: $scope.ref
        });

        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        $http.post('/Home/NationalBillPayment', data, config)
        .success(function (data, status, headers, config) {
            $scope.APIResponse = data;
        })
        .error(function (data, status, header, config) {
            $scope.ResponseDetails = "Data: " + data +
                "<hr />status: " + status +
                "<hr />headers: " + header +
                "<hr />config: " + config;
        });
    }
}]);

//International Recharge Controller
app.controller('InternationalRechargeController', ['$scope', '$http', 'localStorageService',
    function ($scope, $http, localStorageService) {
        $scope.redirect = function () {
            //Set the recharge parameter set from international recharge page.
            var paramObj = {
                rechargeScope: 'International',
                rechargeType: $scope.rechargeType,
                operatorCode: $scope.operatorCode,
                mobileNumber: $scope.mobileNumber,
                amount: $scope.amount,
                country:$scope.country
            };
            localStorageService.set('rechargeParams', paramObj);
            window.location = "#/paymentOptions";

        }
       
        //$http service for Getting the GetInternationalServiceProviders  
        $scope.setOperater = function () {
            alert($scope.selected);
            if ($scope.selected != 'undefined' || $scope.selected != null) {

                $http({
                    method: 'GET',
                    url: '/Home/GetInternationalServiceProviders',
                    data: { Code: $scope.selected }
                }).
            success(function (data) {
                $scope.serviceProviders = data;
            });
            }
        }


        $http({
            method: 'POST',
            url: '/Home/GetAllVouchers'
        }).
       success(function (data) {
           $scope.AllVouchers = data;
       });


        $scope.GetValue = function (operator) {
            var operatorCode = $scope.operatorCode;
            var OperatorName = $.grep($scope.serviceProviders, function (operator) {
                return operator.Code == operatorCode;
            })[0].Name;

        }

        $scope.GetServices = function () {
            var data = $.param({
                rechargeType: $scope.rechargeType,
                operatorCode: $scope.operatorCode,

            });
            var config = {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                }
            }

            $http.post('/Home/GetService/', data, config)
            .success(function (data, status, headers, config) {
                $scope.services = data;
            })
            .error(function (data, status, header, config) {
                $scope.ResponseDetails = "Data: " + data +
                    "<hr />status: " + status +
                    "<hr />headers: " + header +
                    "<hr />config: " + config;
            });
        }
    }]);


//app.controller('PopUpCtrl', ['$scope','ngDialog',function ($scope, ngDialog) {
//    $scope.clickToOpen = function () {
//        ngDialog.open({
//            template: 'login.html',
//            className: 'ngdialog-theme-default login-popup',
//            controller: ['$scope',  function ($scope) {
//                // controller logic
//                $scope.tabs = [{
//                    title: 'Login',
//                    url: 'innerLogin.html'
//                }, {
//                    title: 'Register',
//                    url: 'register.html'
//                }];

//                $scope.currentTab = 'innerLogin.html';

//                $scope.loginTabClick = function (tab) {
//                    $scope.currentTab = tab.url;
//                }

//                $scope.isActiveTab = function (tabUrl) {
//                    return tabUrl == $scope.currentTab;
//                }

//                $scope.signin = function (user) {
//                    $http.post('api/accounts/signin', user)
//                        .success(function (data, status, headers, config) {
//                            user.authenticated = true;
//                            $rootScope.user = user;
//                            $location.path('/');
//                        })
//                        .error(function (data, status, headers, config) {
//                            user.authenticated = false;
//                            $rootScope.user = {};
//                        });
//                };
//            }]
//        });
//    };



//}]);'$scope', 'ngDialog', '$window', '$rootScope', 'authCheck'