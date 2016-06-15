(function ($) {

    var errorClass = "error";
    var successClass = "success";
    var inputControlClass = "input-control";
    var inputElemenetSelector = "input:not(:button, :submit), textarea, select";
    var inputControlSelector = "." + inputControlClass;
    var inputValidationErrorSelector = ".input-validation-error";
    var changeEvent = "change";

    function getInputControl($input) {
        var parent = $input.parent();
        return (parent.hasClass(inputControlClass)) ? parent : $input;
    }

    function clearInputValidationStateOnce() {
        var $input = $(this);
        getInputControl($input).removeClass([errorClass, successClass].join(" "));
        $input.off(changeEvent, clearInputValidationStateOnce);
    }

    $.validator.unobtrusive.options = {
        invalidHandler: function (event, validator) {
            var $form = $(this);

            // visualize the previous attempt
            $form.find(inputElemenetSelector).each(function () {
                var $input = $(this);
                getInputControl($input).removeClass(errorClass).addClass(successClass);
                $input.on(changeEvent, clearInputValidationStateOnce);
            });

            // visualize the current attempt
            var errors = validator.errorList;
            var length = errors.length;
            for (var i = 0; i < length; i++) {
                var $input = $(errors[i].element);
                getInputControl($input).removeClass(successClass).addClass(errorClass);
                $input.on(changeEvent, clearInputValidationStateOnce);
            }
        }
    };

    $(function () {
        $(inputValidationErrorSelector).each(function () {
            $input = $(this);
            getInputControl($input).addClass(errorClass);
        });
    });

})(jQuery);