﻿/// <reference path="../../Umbraco/lib/angular/1.1.5/angular.js" />
/// <reference path="../../Umbraco/lib/jquery/jquery-2.0.3.min.js" />

function TinyMceEditorController($rootScope, $scope, $q, dialogService, $log, imageHelper, assetsService, $timeout, tinyMceService, angularHelper) {

    // set model.value if any
    if (angular.isDefined($scope.model.value) && !angular.isUndefined($scope.model.value) && $scope.model.value !== null) {
        // decode the html
        $scope.model.value = decodeURIComponent($scope.model.value);
    }
    
    tinyMceService.configuration().then(function (tinyMceConfig) {

        //config value from general tinymce.config file
        var validElements = tinyMceConfig.validElements;

        //These are absolutely required in order for the macros to render inline
        //we put these as extended elements because they get merged on top of the normal allowed elements by tiny mce
        var extendedValidElements = "@[id|class|style],-div[id|dir|class|align|style],ins[datetime|cite],-ul[class|style],-li[class|style]";

        var invalidElements = tinyMceConfig.inValidElements;
        var plugins = _.map(tinyMceConfig.plugins, function (plugin) {
            if (plugin.useOnFrontend) {
                return plugin.name;
            }
        }).join(" ");

        var editorConfig = tinyMceService.defaultPrevalues();

        //config value on the data type
        var toolbar = editorConfig.toolbar.join(" | ");
        var await = [];

        //queue file loading
        if (angular.isUndefined(tinymce)) {
            await.push(assetsService.loadJs("Umbraco/lib/tinymce/tinymce.min.js", $scope));
        }
        
        // add custom css
        await.push(assetsService.loadCss("/App_Plugins/TinyMceEditor/tinymceeditor.css", $scope));

        //stores a reference to the editor
        var tinyMceEditor = null;

        //wait for queue to end
        $q.all(await).then(function () {

            /** Loads in the editor */
            function loadTinyMce() {

                //we need to add a timeout here, to force a redraw so TinyMCE can find
                //the elements needed
                $timeout(function () {
                    tinymce.DOM.events.domLoaded = true;
                    tinymce.init({
                        mode: "exact",
                        elements: "wmd_" + $scope.model.alias + "_rte",
                        skin: "umbraco",
                        plugins: plugins,
                        valid_elements: validElements,
                        invalid_elements: invalidElements,
                        extended_valid_elements: extendedValidElements,
                        menubar: false,
                        statusbar: false,
                        height: editorConfig.dimensions.height,
                        width: editorConfig.dimensions.width,
                        toolbar: toolbar,
                        relative_urls: false,
                        setup: function (editor) {

                            //set the reference
                            tinyMceEditor = editor;

                            //We need to listen on multiple things here because of the nature of tinymce, it doesn't 
                            //fire events when you think!
                            //The change event doesn't fire when content changes, only when cursor points are changed and undo points
                            //are created. the blur event doesn't fire if you insert content into the editor with a button and then 
                            //press save. 
                            //We have a couple of options, one is to do a set timeout and check for isDirty on the editor, or we can 
                            //listen to both change and blur and also on our own 'saving' event. I think this will be best because a 
                            //timer might end up using unwanted cpu and we'd still have to listen to our saving event in case they clicked
                            //save before the timeout elapsed.
                            editor.on('change', function (e) {
                                angularHelper.safeApply($scope, function () {
                                    $scope.model.value = editor.getContent();
                                });
                            });
                            editor.on('blur', function (e) {
                                angularHelper.safeApply($scope, function () {
                                    $scope.model.value = editor.getContent();
                                });
                            });

                            //Create the insert media plugin
                            tinyMceService.createMediaPicker(editor, $scope);

                            //Create the embedded plugin
                            tinyMceService.createInsertEmbeddedMedia(editor, $scope);

                            //Create the insert link plugin
                            tinyMceService.createLinkPicker(editor, $scope);

                            //Create the insert macro plugin
                            tinyMceService.createInsertMacro(editor, $scope);
                        }
                    });
                }, 200);
            }

            loadTinyMce();
            
            //here we declare a special method which will be called whenever the value has changed from the server
            //this is instead of doing a watch on the model.value = faster
            $scope.model.onValueChanged = function (newVal, oldVal) {
                //update the display val again if it has changed from the server;
                tinyMceEditor.setContent(newVal, { format: 'raw' });
                //we need to manually fire this event since it is only ever fired based on loading from the DOM, this
                // is required for our plugins listening to this event to execute
                tinyMceEditor.fire('LoadContent', null);
            };

            //listen for formSubmitting event (the result is callback used to remove the event subscription)
            var unsubscribe = $scope.$on("formSubmitting", function () {
                var content = tinyMceEditor.getContent();
                
                // encode the content (else it will not work in the macro container...)
                $scope.model.value = encodeURIComponent(content);
            });

            //when the element is disposed we need to unsubscribe!
            // NOTE: this is very important otherwise if this is part of a modal, the listener still exists because the dom 
            // element might still be there even after the modal has been hidden.
            $scope.$on('$destroy', function () {
                unsubscribe();
                
                // We need to remove the stylesheet that we added since it changes the style for all dialogs.
                var toRemove = '/App_Plugins/TinyMceEditor/tinymceeditor.css';
                [].forEach.call(document.styleSheets, function (styleSheet) {
                    if (styleSheet.href && styleSheet.href.indexOf(toRemove) != -1) {
                        styleSheet.disabled = true;
                        $(styleSheet).remove();

                        return false;
                    }

                    return true;
                });
            });
        });
    });
}
angular.module("umbraco").controller("TinyMceEditorController", TinyMceEditorController);