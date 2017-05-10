angular.module("umbraco").controller("ContentGrader.Controller", function ($scope, contentGraderResource) {
    $scope.showContentGrade = false;
    $scope.$watch(
        function () {
            return $scope.model.value.length;
        },
        function (newVal) {
            if (newVal > 0) {
                contentGraderResource.gradeContent($scope.model.value).then(function (response) {
                    $scope.score = response.data;
                    $scope.showContentGrade = true;
                });
            }
        }
    );
});

