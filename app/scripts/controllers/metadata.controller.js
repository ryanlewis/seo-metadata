angular.module("umbraco").controller("EpiphanySeoMetadataController", [
  '$scope', function($scope) {

    $scope.invalidate = true;
    $scope.model.hideLabel = true;
    $scope.serpTitleLength =  !!$scope.model.config.serpTitleLength ? $scope.model.config.serpTitleLength : 65;
    $scope.serpDescriptionLength = !!$scope.model.config.serpDescriptionLength ? $scope.model.config.serpDescriptionLength : 150;
    $scope.developerName = $scope.model.config.developerName || 'your agency';
    $scope.doNotIndexExplanation = $scope.model.config.doNotIndexExplanation || '';

    // default model.value
    if (!$scope.model.value) {
      $scope.model.value = { title: '', description: '', urlName: '', noIndex: false };
    }

    $scope.GetUrl = function() {

          // regex to text if a URL string is absolute or relative: http://stackoverflow.com/a/10687158
          var pattern = new RegExp(/^https?:\/\//i);
          var url = "";

          var parentContent = $scope.GetParentContent();

          // check if any domain is assigned - use if one exists
          // note it is only available if the node is published
          if (parentContent.urls && parentContent.urls.length && parentContent.published) {
              var nodeUrl = parentContent.urls[0];

              // test if it is an absolute url
              if (pattern.test(nodeUrl)) {
                  url = nodeUrl;
              }
              else {
                  url = $scope.ProtocolAndHost() + '/' + nodeUrl;
              }

              if ($scope.model.value.urlName && $scope.model.value.urlName.length) {
                  var urlSplit = url.split('/');

                  // http://mydomain.com will be splitted into an array of 3 strings: 'http:', '' and 'mydomain.com'
                  // only handle slugify of urlName under root level
                  if (urlSplit.length > 4) {
                      if (urlSplit[urlSplit.length - 1] == "") {
                          urlSplit[urlSplit.length - 2] = slugify($scope.model.value.urlName);
                      } else {
                          urlSplit[urlSplit.length - 1] = slugify($scope.model.value.urlName);
                      }
                  }

                  // join new values
                  url = urlSplit.join('/');
              }
          }
          else if ($scope.model.value.urlName && $scope.model.value.urlName.length) {
              if (!parentContent.published) {
                  url = $scope.ProtocolAndHost() + '/unpublished-page/';
              } else {
                  url = $scope.ProtocolAndHost() + '/' + slugify($scope.model.value.urlName) + '/';
              }
          }

          return url;
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
      // a very basic slugify function to replace chars in url
      function slugify(text) {
          return text.toString().toLowerCase().trim()
            .replace(/\s+/g, '-')           // Replace spaces with -
            .replace(/&/g, '-')             // Replace & with -
            .replace(/[^\w\-]+/g, '')       // Remove all non-word chars
            .replace(/\-\-+/g, '-')         // Replace multiple - with single -
      }
  }
]);
