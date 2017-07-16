var personalPage = function() {

    var inputEvent = function() {

        $("body").on("change",".inputArea", function () {
            var $this = $(this);
            var prex = $this.attr("data-prex");
            var $item = $("[data-idfor*='" + prex + $this.val() + "']");
            var $prex = $("[data-idfor*='" + prex + "']");
            $prex.closest(".form-group").addClass("hidden");
            $item.closest(".form-group").removeClass("hidden");
            
               
            
        });
    };
    return {
        init: function() {
            inputEvent();
        }
    }
}();
$(document).ready(function() {
    personalPage.init();
});