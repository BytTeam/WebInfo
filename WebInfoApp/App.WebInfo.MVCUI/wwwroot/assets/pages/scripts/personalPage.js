var personalPage = function() {

    var inputEvent = function() {

        $.each($(".inputArea"),
            function(item) {
                var $this = $(item);
                var selectValue = $this.attr("data-selectvalue");
                var selectArea = $this.attr("data-area");
                if ($this.val() === selectValue) {
                    $(selectArea).closest(".form-group").removeClass("hidden");
                } else {
                    $(selectArea).closest(".form-group").addClass("hidden");
                }
            });
    };
    return {
        init: function() {
            inputEvent();
        }
    }
};
JQuery(document).ready(function() {
    personalPage.init();
});