//hide navbar-toogler when scroll on small mobile phones
$(window).scroll(function () {
    if ($(window).width() < 576) {
        if ($(this).scrollTop() > 0) {
            $(".navbar-toggler").css('visibility', 'hidden');
        }
        else {
            $(".navbar-toggler").css('visibility', 'visible');
        }
    }
});


$(window).scroll(function (event) {
    var clickover = $(event.target);
    var _opened = $(".navbar-collapse").hasClass("show");
    if (_opened === true && !clickover.hasClass("navbar-toggler")) {
        $(".navbar-toggler").click();
    }
});