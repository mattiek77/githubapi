(function($) {

    var form = $("#searchForm");
    var results = $("#userResults");
    var input = $("#userName");
    var callbackPath = $("#userDetailPath").val();
    form.on("submit", getData);

    function getData(e) {
        var val = input.val();
        results.empty();
        $.get(callbackPath + "?userName=" + val).done(function(data) {
            results.html(data);
        });
        e.preventDefault();
    }

})(jQuery);