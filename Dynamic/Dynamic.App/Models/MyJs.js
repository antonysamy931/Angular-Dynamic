var app = angular.module('myApp', []);
app.controller('first', ['$scope', '$controller', '$compile', '$http', function ($scope, $controller, $compile, $http) {
    $scope.name = "sample";
    $scope.Call2 = function () {

        var model = $scope.$new();
        $controller('second', { $scope: model });
        var content = $('#SecondBind');
        var html
        $http({
            method: 'GET',
            url: "Second.html"
        }).then(function (response) {
            html = response.data;
            content.html(html);
            $compile(content)(model);
            model.TestFun();
        });
    }
}]);

app.controller('second', function ($scope, $controller) {
    $scope.TestFun = function () {
        $scope.text = "mark antony";
    }

    $scope.Hi = function () {
        alert($scope.text);
    }

});

app.directive('test', function () {
    return {
        link: function ($scope, $controller, $compile, $http, element) {
            $('.opencontroller').click(function () {
                var controller = $(this).attr('v-controller');
                var bindElementId = $(this).attr('v-bind-id');
                var template = $(this).attr('v-load-template');
                var newSocpe = $scope.$new();
                $controller(controller, { $scope: newSocpe });
                var content = $('#' + bindElementId);
                $http({
                    method: 'GET',
                    url: "Second.html"
                }).then(function (response) {
                    content.html(response.data);
                    $compile(content)(newSocpe);
                    newSocpe.TestFun();
                });
            });
        }
    }
});

//app.directive('communicator', function () {
//    return {
//        restrict: 'E',
//        scope: {},
//        controller: "@",
//        name: "vController",
//        link: function ($scope, $compile, $http, element) {
//            $('.opencontroller').click(function () {
//                //var bindElementId = $(this).attr('v-bind-id');
//                //var template = $(this).attr('v-load-template');
//                //var newSocpe = $scope.$new();
//                //$controller(controller, { $scope: newSocpe });
//                //var content = $('#' + bindElementId);
//                //$http({
//                //    method: 'GET',
//                //    url: "Second.html"
//                //}).then(function (response) {
//                //    content.html(response.data);
//                //    $compile(content)(newSocpe);
//                //    newSocpe.TestFun();
//                //});
//                alert('hi');
//            });
//        }
//    }
//});

app.directive('communicator', function () {
    return {
        restrict: 'E',
        scope: {},
        template: "<input type='text' ng-model='message'/><input type='button' value='Send Message' ng-click='Hi()'><br/>",
        controller: "@",
        name: "vController"
    }
})