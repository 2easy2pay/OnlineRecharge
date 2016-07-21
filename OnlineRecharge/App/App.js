﻿/// <reference path="C:\Projects\Recharge\OnlineRecharge\OnlineRecharge\Partials/_Result.cshtml" />
var app = angular.module('Multilingual', ['pascalprecht.translate',
   'ngCookies', 'ngRoute', 'internationalPhoneNumber', 'ngDialog', 'slider', 'LocalStorageModule']);

//Configuration Section
app.config(['$routeProvider','$locationProvider',
function ($routeProvider,$locationProvider) {
           $routeProvider.
           when('/mobile', {
               templateUrl: '/Partials/Mobile.html',
               controller: 'localRechargeController'
           })
           //.when('/home', {
           //    templateUrl: '/Partials/Mobile.html',
           //    controller: 'localRechargeController'
           //})
           .when('/paymentOptions', {
               templateUrl: 'Partials/PaymentOptions.html',
               controller: 'paymentPageController'
           })
           .when('/result', {
               controller: "rechargeResultController",
               templateUrl: 'Partials/Result.html'
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
            // use $.param jQuery function to serialize data from JSON 
            var data = $.param({
                rechargeType: $scope.rechargeType,
                operatorCode: $scope.operatorCode,
                mobileNumber: $scope.mobileNumber,
                amount: $scope.amount
            });
            var paramObj = {
                rechargeType: $scope.rechargeType,
                operatorCode: $scope.operatorCode,
                mobileNumber: $scope.mobileNumber,
                amount: $scope.amount
            };
            debugger;
            localStorageService.set('rechargeParams', paramObj);
            window.location = "#/paymentOptions";
            //var config = {
            //    headers: {
            //        'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            //    }
            //}

            //$http.post('/Home/TopupTransfer', data, config)
            //.success(function (data, status, headers, config) {
            //    $scope.PostDataResponse = data;
            //    window.location = "#/paymentOptions";
            //})
            //.error(function (data, status, header, config) {
            //    $scope.ResponseDetails = "Data: " + data +
            //        "<hr />status: " + status +
            //        "<hr />headers: " + header +
            //        "<hr />config: " + config;
            //});

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


        $scope.GetValue = function (operator) {
            var operatorCode = $scope.operatorCode;
            var OperatorName = $.grep($scope.serviceProviders, function (operator) {
                return operator.Code == operatorCode;
            })[0].Name;

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
app.controller('paymentPageController', ['$scope', '$http', '$location', 'localStorageService', 'authCheck', '$rootScope', function ($scope, $http, $location, localStorageService, authCheck, $rootScope) {
    debugger;
    if ($rootScope.isLoggedIn == false) {
        authCheck.isUserLoggedIn();
        return;
    }
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
            amount: 10,
            contactNumber: '+96594950708',
            email: 'vineeth@2easy2pay.com',
            paymentType: 'knet',
            returnUrl: 'https://api.2easy2pay.com/web/knetresponse/rechargeWebResponse',
            errorUrl: 'https://api.2easy2pay.com/web/knetresponse/rechargeWebResponse',
            Udf1: 10
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
    $scope.RechargeParameter = localStorageService.get('rechargeParams');
}]);
app.controller('rechargeResultController', ['$scope', '$location', function ($scope, $location) {
    $scope.jobOffers = [];

    function init() {
        var search= $location.search();
        $scope.paymentID = search.PaymentID;
        $scope.result = search.Result;
        $scope.trackID = search.TrackID;
        $scope.tranID = search.TranID;
        $scope.ref = search.Ref;

        $http({
            method: 'POST',
            url: '/Home/GetAllVouchers'
        }).
      success(function (data) {
          $scope.AllVouchers = data;
      });
    }

    init();
}]);


