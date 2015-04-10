angular.module("umbraco").controller("EpiphanySeoMetadataController", [
  '$scope', 'assetsService', function($scope, assetsService) {

    $scope.invalidate = true;
    $scope.model.hideLabel = true;
    $scope.serpTitleLength = 64;
    $scope.serpDescriptionLength = 150;

    $scope.GetUrl = function() {

      var urlName = $scope.model.value.urlName && $scope.model.value.urlName.length ? '/' + $scope.model.value.urlName + '/' : $scope.GetParentContent().urls[0];

      if (urlName === '' || urlName === 'This item is not published') {
        urlName = '/unpublished-page/';
      }

      return $scope.ProtocolAndHost() + urlName;

    };

    $scope.ProtocolAndHost = function() {

      var http = location.protocol;
      var slashes = http.concat("//");
      return slashes.concat(window.location.hostname);

    };

    $scope.GetParentContent = function() {
      var currentScope = $scope.$parent;

      for (var i = 0; i < 150; i++) {
        if (currentScope.content) {
          return currentScope.content;
        }

        currentScope = currentScope.$parent;
      }

      return null;
    };


    $(window).resize(function() {
      $scope.$apply(function() {
        if (window.innerWidth <= 1500 && !$scope.hideSerp) {
        	$scope.hideSerp = true;
        }
        if (window.innerWidth > 1500) {
        	$scope.hideSerp = false;
        }
      });
    });

    //assetsService.loadCss("/App_Plugins/Epiphany.SeoMetadata/metadata.css");
  }
]);