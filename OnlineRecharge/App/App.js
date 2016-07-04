/// <reference path="C:\Projects\Recharge\OnlineRecharge\OnlineRecharge\scripts/angular.js" />
angular
    .module('main', [
    'ngRoute',
    'MyApp.ctrl.crud',
    ])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        $routeProvider.when('/', {
            templateUrl: '/Home/CRUD',
            controller: 'loginController'
        });

        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });

    }]);