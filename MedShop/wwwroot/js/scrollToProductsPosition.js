$(document).ready(function () {


    //link: https://www.codexworld.com/smooth-scroll-to-div-using-jquery/
    $('a.navbarLeft[href*=\\#]:not([href=\\#])').on('click', function () {
        var target = $(this.hash);
        target = target.length ? target : $('[name=' + this.hash.substr(1) + ']');
        var getHeaderHeight = $('.headerContainerWrapper').outerHeight();
        var nameOfHeader = $(this).attr('id').split('_')[1].toString();
        var ele = $('#section_' + nameOfHeader);
        if (target.length) {
            $('html,body').animate({
                //ovo ubaceno da vidim hedere med i dzemovi!!!
                scrollTop: target.offset().top - getHeaderHeight
                //scrollTop: ele.offset().top - 100
            }, 600);
            return false;
        }
    });


    //$('.navbarLeft').click(function () {
    //    var nameOfHeader = $(this).attr('id').split('_')[1].toString();
    //    $('#section_' + nameOfHeader).css('margin-top','70px');

    //});


    $('.navbarLeft').hover(function () {

        $(this).css({ "text-decoration": "underline" });
    });
    $('.navbarLeft').mouseout(function () {

        $(this).css({ "text-decoration": "none" });
    });
});