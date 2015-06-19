angular.module('umbraco').directive('maxlen', function () {
    return {
        require: 'ngModel',
        link: function (scope, el, attrs, ctrl) {

            var validate = false;
            var length = 999999;

            if (attrs.name === 'title') {
                validate = scope.model.config.allowLongTitles !== '1';
                length = scope.serpTitleLength;
            } else if (attrs.name === 'description') {
                validate = scope.model.config.allowLongDescriptions !== '1';
                length = scope.serpDescriptionLength;
            }

            ctrl.$parsers.unshift(function (viewValue) {
                if (validate && viewValue.length > length) {
                    ctrl.$setValidity('maxlen', false);
                } else {
                    ctrl.$setValidity('maxlen', true);
                }

                return viewValue;
            });
        }
    };
});
