angular.module("umbraco")
    .controller("EpiphanySeoMetadataController",
    [
        '$scope', 'contentResource', function($scope, contentResource) {
            $scope.invalidate = true;
            $scope.model.hideLabel = true;
            $scope.serpTitleLength = !!$scope.model.config.serpTitleLength ? $scope.model.config.serpTitleLength : 65;
            $scope.serpDescriptionLength = !!$scope.model.config.serpDescriptionLength ? $scope.model.config.serpDescriptionLength : 150;
            $scope.developerName = $scope.model.config.developerName || 'your agency';
            $scope.doNotIndexExplanation = $scope.model.config.doNotIndexExplanation || '';

            // default model.value
            if (!$scope.model.value) {
                $scope.model.value = { title: '', description: '', urlName: '', noIndex: false };
            }

            // a very basic slugify function to replace chars in url
            function slugify(text) {
                return text.toString()
                    .toLowerCase()
                    .trim()
                    .replace(/\s+/g, '-')     // Replace spaces with -
                    .replace(/&/g, '')       // Replace & with nothing
                    .replace(/[^\w\-]+/g, '') // Remove all non-word chars
                    .replace(/\-\-+/g, '-');  // Replace multiple - with single -
            }

            $scope.init = function() {
                var content = $scope.GetParentContent();
                if (!content.published) {
                    // get the URL of the parent document for later
                    contentResource.getById(content.parentId).then(function(data) {
                        $scope.parentUrl = data.urls[0];
                    });
                }
            };

            $scope.GetUrl = function() {
                
                var name, url;
                var pageContent = $scope.GetParentContent();
                
                // handle instances where the document is newly created and unpublished
                if (!pageContent.published) {
                    if ($scope.model.value.urlName && $scope.model.value.urlName.length) {
                        name = slugify($scope.model.value.urlName);
                    } else if (typeof(pageContent.name) !== 'undefined') {
                        name = slugify(pageContent.name);
                    }

                    if (typeof(name) === 'undefined' || name.length === 0) {
                        name = "unpublished-page";
                    }

                    return $scope.ProtocolAndHost() + $scope.parentUrl + name + "/";
                }

                var nodeUrl = pageContent.urls[0];

                // test if it is an absolute url: http://stackoverflow.com/a/10687158
                url = /^https?:\/\//i.test(nodeUrl) ? nodeUrl : $scope.ProtocolAndHost() + nodeUrl;

                var urlSplit = url.split('/');

                // we got a slug configured?
                if ($scope.model.value.urlName && $scope.model.value.urlName.length) {

                    // http://mydomain.com will be split into an array of 3 strings: 'http:', '' and 'mydomain.com'
                    // only handle slugify of urlName under root level
                    if (urlSplit.length > 4) {
                        if (urlSplit[urlSplit.length - 1] === "") {
                            urlSplit[urlSplit.length - 2] = slugify($scope.model.value.urlName);
                        } else {
                            urlSplit[urlSplit.length - 1] = slugify($scope.model.value.urlName);
                        }
                    }

                    // join new values
                    url = urlSplit.join('/');
                } else {
                    // use the name of the document instead
                    urlSplit.splice(-2, 2); // pop the last slug off the end of the array, use the document name instead
                    name = pageContent.name ? slugify(pageContent.name) : 'unpublished-page';
                    url = urlSplit.join('/') + '/' + name + '/';
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

            $scope.init();
        }
    ]);