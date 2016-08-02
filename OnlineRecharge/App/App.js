/// <reference path="C:\Projects\Recharge\OnlineRecharge\OnlineRecharge\Partials/_Result.cshtml" />
var app = angular.module('Multilingual', ['pascalprecht.translate',
   'ngCookies', 'ngRoute', 'internationalPhoneNumber', 'ngDialog', 'slider', 'LocalStorageModule', 'blockUI', 'angular-bootstrap-select']);
//Configuration Section
app.config(['$routeProvider', '$locationProvider',
function ($routeProvider, $locationProvider) {
    $routeProvider.
    when('/mobile', {
        templateUrl: '/Partials/Mobile.html',
        controller: 'localRechargeController'
    })
        .when('/international', {

            templateUrl: '/Partials/International.html',
            controller: 'InternationalRechargeController'
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
    .when('/datacards', {
        controller: "datacardController",
        templateUrl: 'Partials/DataCard.html'
    })
         .when('/shoppingcards', {
             controller: "shoppingcardsController",
             templateUrl: 'Partials/ShoppingCards.html'
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

//Directive Sections
app.directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9.]/g, '');

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});
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
app.controller('LanguageSwitchController', ['$scope', '$rootScope', '$translate',
function ($scope, $rootScope, $translate) {
      //authCheck.isUserLoggedIn();
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
        //authCheck.isUserLoggedIn();
    };

    $scope.Logout = function () {
        console.log('IN')
      //  authCheck.Logout()
    }


}]);
app.controller('localRechargeController', ['$scope', '$http', 'localStorageService','blockUI',
function ($scope, $http, localStorageService,blockUI) {
        var myBlock = blockUI.instances.get('myBlock');
        $scope.redirect = function () {
            debugger;
            $scope.CheckValidationForAmount = function () {
                if ($scope.amount == null || $scope.amount == undefined) {
                    return true;
                }
                else {
                    return false;
                }
            }

            $scope.CheckValidationForOperator = function () {
                debugger;
                if ($scope.operatorCode == null || $scope.operatorCode == undefined) {
                    return true;
                }
                else {
                    return false;
                }
            }

            $scope.CheckValidationForMobileNumer = function () {
                if ($scope.interrorcode == 4 || $scope.interrorcode==undefined) {
                    $scope.validationRef = "mobilerequired";
                    return true;
                }
                else if ($scope.interrorcode == 2)
                {
                    $scope.validationRef = "mobileminlength";
                    return true;
                }
                else {
                    return false;
                }
            }
            debugger;
            if ($scope.rechargeForm.$valid) {
                if (myBlock.state().blocking) {
                    myBlock.stop();
                } else {
                    myBlock.start();
                }
                var paramObj = {
                    ServiceType: 'national',
                    RechargeType: $scope.rechargeType,
                    OperatorCode: $scope.operatorCode,
                    MobileNumber: $scope.mobileNumber,
                    Amount: $scope.amount
                };
                localStorageService.set('nationalRechargeParams', paramObj);
                var data = $.param({
                    rechargeType: $scope.rechargeType,
                    operatorCode: $scope.operatorCode,
                    mobileNumber: $scope.mobileNumber,
                    amount: $scope.amount,
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
                if ($scope.rechargeType == 'Prepaid') {
                    $http.post('/Home/ValidateTopup', data, config)
              .success(function (data, status, headers, config) {
                  $scope.TopupValidation = data;
                  if (data.Response == "S") {
                      window.location = "#/paymentOptions";
                  }

              })
              .error(function (data, status, header, config) {
                  $scope.ResponseDetails = "Data: " + data +
                      "<hr />status: " + status +
                      "<hr />headers: " + header +
                      "<hr />config: " + config;
              });
                }
                else if ($scope.rechargeType == 'Postpaid') {
                    $http.post('/Home/PaymentValidation', data, config)
             .success(function (data, status, headers, config) {
                 $scope.TopupValidation = data;
                 if (data.Response == "S") {
                     window.location = "#/paymentOptions";
                 }

             })
             .error(function (data, status, header, config) {
                 $scope.ResponseDetails = "Data: " + data +
                     "<hr />status: " + status +
                     "<hr />headers: " + header +
                     "<hr />config: " + config;
             });
                }
                else if ($scope.rechargeType == 'Vouchers') {
                    $http.post('/Home/ValidateVoucher', data, config)
            .success(function (data, status, headers, config) {
                $scope.TopupValidation = data;
                if (data.Result.trim() == "S") {
                    window.location = "#/paymentOptions";
                }

            })
            .error(function (data, status, header, config) {
                $scope.ResponseDetails = "Data: " + data +
                    "<hr />status: " + status +
                    "<hr />headers: " + header +
                    "<hr />config: " + config;
            });
                }

            }

        }
        //$http service for Getting the ServiceProviders  
        $http({
            method: 'GET',
            url: '/Home/GetServiceProviders'
        }).
        success(function (data) {
            $scope.serviceProviders = data;
            $scope.operatorCode = $scope.serviceProviders[1];
        });

        $http({
            method: 'POST',
            url: '/Home/GetAllVouchers'
        }).
       success(function (data) {
           $scope.AllVouchers = data;
       });

        $scope.refreshRechargeType = function () {
            $scope.mobileNumber="",
            $scope.amount=""
        }
     
        $scope.refershAmount = function () {
 $scope.amount=""
        }

        $scope.loadAmount = function (voucher) {
            $scope.amount = voucher.Amount;
            $scope.operatorCode = voucher.OperatorCode;
        }

      
    }]);
app.controller('paymentPageController', ['$scope', '$http', '$location', 'localStorageService', '$rootScope', '$timeout', 'blockUI', function ($scope, $http, $location, localStorageService, $rootScope, $timeout, blockUI) {
    blockUI.start();
    var search = $location.search();
    if (search.id != undefined)
    {
        $http({
            method: 'POST',
            data:{id:search.id},
            url: '/Home/GetRechargeParameter'
        }).
       success(function (data) {
           if (data != "")
           {
               localStorageService.set('nationalRechargeParams', data);
               var test= localStorageService.get('nationalRechargeParams');
               console.log(test); // returns data --working
               $scope.RechargeParameter = test; // assiged
               console.log($scope.RechargeParameter);  // returns data --working
           }
           else
           {
               $scope.RechargeParameter = localStorageService.get('nationalRechargeParams');
           }
          
       });
        $timeout(function () {
            console.log($scope.RechargeParameter);
        }, 2000);
    }
    else
    {
        $scope.RechargeParameter = localStorageService.get('nationalRechargeParams');
    }
    $scope.tabs = [{
        title: 'KNet',
        url: 'knet.html'
    }, {
        title: 'VisaMasterCard',
        url: 'visamaster.html'
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
        blockUI.start();
        // use $.param jQuery function to serialize data from JSON 
        var data = $.param({
            amount: $scope.RechargeParameter.Amount,
            contactNumber: "+" + $scope.RechargeParameter.MobileNumber,
            email: 'info@2easy2pay.com',
            paymentType: 'knet',
            returnUrl: 'https://api.2easy2pay.com/web/knetresponse/rechargeWebResponse',
            errorUrl: 'https://api.2easy2pay.com/web/knetresponse/rechargeWebResponse',
            Udf1: $scope.RechargeParameter.Amount
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
            blockUI.stop();
            form.remove();
        });
    };
    blockUI.stop();
}]);
app.controller('rechargeResultController', ['$scope', '$location', 'localStorageService', '$http', 'blockUI', function ($scope, $location, localStorageService, $http, blockUI) {
    blockUI.start();
    var rechargeParams = localStorageService.get('nationalRechargeParams');
    var search = $location.search();
    $scope.paymentID = search.PaymentID;
    $scope.result = search.Result;
    $scope.trackID = search.TrackID;
    $scope.tranID = search.TranID;
    $scope.ref = search.Ref;
    if (rechargeParams.ServiceType == 'national' && rechargeParams.RechargeType == 'Prepaid') {
        var data = $.param({
            rechargeType: rechargeParams.RechargeType,
            operatorCode: rechargeParams.OperatorCode,
            mobileNumber: rechargeParams.MobileNumber,
            amount: rechargeParams.Amount,
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
    else if (rechargeParams.ServiceType == 'national' && rechargeParams.RechargeType == 'Postpaid') {
        var data = $.param({
            rechargeType: rechargeParams.RechargeType,
            operatorCode: rechargeParams.OperatorCode,
            mobileNumber: rechargeParams.MobileNumber,
            amount: rechargeParams.Amount,
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
    else if (rechargeParams.ServiceType == 'national' && rechargeParams.RechargeType == 'Vouchers') {
        var data = $.param({
            rechargeType: rechargeParams.RechargeType,
            operatorCode: rechargeParams.OperatorCode,
            mobileNumber: rechargeParams.MobileNumber,
            amount: rechargeParams.Amount,
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

        $http.post('/Home/NationalVoucherTranfer', data, config)
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
    else if (rechargeParams.ServiceType == 'DataCards') {
        var data = $.param({
            rechargeType: rechargeParams.RechargeType,
            operatorCode: rechargeParams.OperatorCode,
            mobileNumber: rechargeParams.MobileNumber,
            serviceType: rechargeParams.ServiceType,
            amount: rechargeParams.Amount,
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

        $http.post('/Home/NationalVoucherTranfer', data, config)
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
    else if (rechargeParams.ServiceType == 'International') {
        debugger

        //if ($scope.result == 'CAPTURED') {
            debugger
            //Local storage parameter.
            var TopValidationdata = $.param({
                OperatorName: rechargeParams.OperatorName,
                CountryCode: rechargeParams.CountryCode,
                OperatorCode: rechargeParams.OperatorCode,
                MobileNumber: rechargeParams.MobileNumber,
                Amount: rechargeParams.Amount
            });
            var config = {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                }
            }
            //Check validation. if s pass else whos cross image with response.
            $http.post('/Home/InternationalTopupCheck/', TopValidationdata, config)
            .success(function (data, status, headers, config) {
                debugger
                if (data.Response == 'S') {

                    var data = $.param({
                        RechargeType: rechargeParams.RechargeType,
                        OperatorCode: rechargeParams.OperatorCode,
                        MobileNumber: rechargeParams.MobileNumber,
                        amount: data.LocalCountryCurrecy,
                        CountryCode: rechargeParams.CountryCode,
                        paymentID: $scope.paymentID,
                        result: $scope.result,
                        trackID: $scope.trackID,
                        tranID: $scope.tranID,
                        ref: $scope.ref
                    });

                    $http.post('/Home/InternationalTopupTransfer', data, config)
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
                else {
                    debugger
                    var data = ({
                        RechargeType: rechargeParams.RechargeType,
                        OperatorCode: rechargeParams.OperatorCode,
                        MobileNumber: rechargeParams.MobileNumber,
                        amount: data.LocalCountryCurrecy,
                        paymentID: $scope.paymentID,
                        //result: $scope.result, 
                        result: data.ResponseDescription,
                        trackID: $scope.trackID,
                        tranID: $scope.tranID,
                        ref: $scope.ref
                    });
                    $scope.APIResponse = data;
                }


            })
        //}


    }
    else if (rechargeParams.ServiceType == 'ShoppingCards') {
        var data = $.param({
            rechargeType: rechargeParams.RechargeType,
            operatorCode: rechargeParams.OperatorCode,
            mobileNumber: rechargeParams.MobileNumber,
            serviceType: rechargeParams.ServiceType,
            amount: rechargeParams.Amount,
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

        $http.post('/Home/NationalVoucherTranfer', data, config)
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
    blockUI.stop();
}]);
//kamal -International Recharge Controller
//International Recharge Controller
app.controller('InternationalRechargeController', ['$scope', '$http', 'localStorageService',

    function ($scope, $http, localStorageService) {
        //set default country code.
        var countryCode = 'in';

        $scope.redirect = function () {

            debugger;
            /*************Validate the international top up recharge.*************/
            var data = $.param({
                CountryCode: countryCode,
                OperatorCode: $scope.operatorCode,
                MobileNumber: $scope.mobileNumber
            });

            var config = {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                }
            }
            $http.post('/Home/InternationalTopupValidation', data, config)
           .success(function (data) {
               debugger
               if (data.Response == 'S') {
                   //Set the recharge parameter set from international recharge page.
                   var paramObj = {
                       ServiceType: "International",
                       RechargeType: 'International',
                       OperatorCode: $scope.operatorCode,
                       MobileNumber: $scope.mobileNumber,
                       CountryCode: countryCode,
                       Amount: $scope.amount,
                       OperatorName: $scope.OperatorName
                   };
                   localStorageService.set('nationalRechargeParams', paramObj);
                   window.location = "#/paymentOptions";
               }
               else if (data.Response == 'F') {

                   //If fails show the error message to end user.
                   // $scope.error = data.ResponseDescription;
                   $('.error').html(data.ResponseDescription);
               }
               else if (data.Response == null) {
                   $scope.error = 'Invalid Number or server error.';
                   //If fails show the error message to end user.
                   $('.error').html($scope.error)
               }
           });
            /*************Validate the international top up recharge.*************/


        }

        //On Selected country change bind the Operators.
        $(document).ready(function () {
            debugger
            $("#MobileNumber").on("countrychange", function (e, countryData) {
                countryCode = countryData.iso2;
                $http({
                    method: 'GET',
                    url: '/Home/GetInternationalServiceProviders?Code=' + countryData.iso2
                }).success(function (data) {
                    $scope.InternationalServiceProviders = data;
                });
            });

            //Default Operators binding.
            $http({
                method: 'GET',
                url: '/Home/GetInternationalServiceProviders?Code=' + countryCode
            }).success(function (data) {
                $scope.InternationalServiceProviders = data;
            });

        });

        //Clear amount.
        $scope.ClearAmount = function () {
            $scope.OperatorName = $("#operatorCode option:selected").text();
            $scope.amount = '';
        }

    }]);
app.controller('datacardController', ['$scope', '$http', 'localStorageService',
    function ($scope, $http, localStorageService) {
        $scope.redirect = function () {
            if ($scope.rechargeForm.$valid) {
                var paramObj = {
                    serviceType: 'DataCards',
                    rechargeType: 'DataCards',
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
            url: '/Home/GetAllDataCardVouchers'
        }).
       success(function (data) {
           $scope.AllVouchers = data;
       });

        $scope.loadAmount = function (voucher) {
            $scope.amount = voucher.Amount;
        }
    }]);
//shoppingcardsController
app.controller('shoppingcardsController', ['$scope', '$http', 'localStorageService',
    function ($scope, $http, localStorageService) {
        $scope.redirect = function () {
            if ($scope.rechargeForm.$valid) {
                var paramObj = {
                    serviceType: 'ShoppingCards',
                    rechargeType: 'ShoppingCards',
                    operatorCode: $scope.operatorCode,
                    amount: $scope.amount
                };
                localStorageService.set('nationalRechargeParams', paramObj);
                window.location = "#/paymentOptions";
            }

        }
        //$http service for Getting the ServiceProviders  
        $http({
            method: 'GET',
            url: '/Home/GetShoppingCardProviders'
        }).
        success(function (data) {
            $scope.shoppingcardsserviceProviders = data;
        });

        $http({
            method: 'POST',
            url: '/Home/GetAllShoppingCardVouchers'
        }).
       success(function (data) {
           $scope.AllVouchers = data;
       });

        $scope.loadAmount = function (voucher) {
            $scope.amount = voucher.Amount;
        }
    }]);
////End International Recharge Controller
