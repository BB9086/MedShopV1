$(document).ready(function () {
    $(".card").hover(
        function () { $(this).addClass("Hover"); },
        function () { $(this).removeClass("Hover"); },
    );
});