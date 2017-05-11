angular.module("umbraco.resources")
.factory("contentGraderResource", function ($http) {
    return {
        gradeContent: function (content) {
            return $http({
                method: 'POST',
                url: 'backoffice/ContentGrader/ContentGrader/GradeContent',
                data: JSON.stringify(content),
                headers: { 'Content-Type': 'application/json' }
            });
        }
    };
});