var personalPage = function () {

    var inputEvent = function () {

        $("body").on("change", ".inputArea", function () {
            var $this = $(this);
            var prex = $this.attr("data-prex");
            var $item = $("[data-idfor*='" + prex + $this.val() + "']");
            var $prex = $("[data-idfor*='" + prex + "']");
            $prex.closest(".form-group").addClass("hidden");
            $item.closest(".form-group").removeClass("hidden");



        });

    };
    var selectChange = function () {
        $("body").on("change", "[data-change]", function () {
            var $this = $(this);
            var selector = $this.attr("data-change-item");
            var url = $this.attr("data-change");
            var idValue = $this.val();
            $.ajax({
                type: "POST",
                cache: false,
                url: url,
                data: { id: idValue },
                success: function (res) {
                    if (res.isSuccess) {
                        var options = '<option value="-1">Seçiniz</option>';
                        $.each(res.result,
                            function (index,item) {
                                options += '<option value="' + item.value + '">' + item.text + '</option>';
                            });
                        $(selector).html(options);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    App.UIAlert({ type: "error" });
                }
            });
        });
    };
    return {
        init: function () {
            inputEvent();
            selectChange();
        }
    }
}();
$(document).ready(function () {
    personalPage.init();
});