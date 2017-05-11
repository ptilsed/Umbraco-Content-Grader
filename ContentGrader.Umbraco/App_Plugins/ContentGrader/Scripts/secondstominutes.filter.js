angular.module("umbraco.filters").filter('secondsToMinute', function () {

    return function (timeInSeconds) {

        if (isNaN(timeInSeconds)) {
            return 'bad time. Enter time in seconds';
        } else {
            var minuteValue = parseInt(timeInSeconds / 60);
            var secondsValue = timeInSeconds % 60;
        }
        if (minuteValue === 0)
        {
            return secondsValue + 'sec';
        }
        return minuteValue + 'min, ' + secondsValue + 'sec';

    }

});
