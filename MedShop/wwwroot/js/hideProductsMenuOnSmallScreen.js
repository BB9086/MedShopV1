//hide products menu navbar on small screens
//proveriti sisrinu ekrana za mobilne telefone....

//prilikom prvog ucitavanaj stranice:
$(document).ready(function () {
    if ($(window).width() < 576) {
        $('#productsMenu').addClass('d-none d-sm-block');
        /* $('#infoDiv').css("top", "485px");*/
    }
    else {
        $('#productsMenu').removeClass('d-none d-sm-block');
        /* $('#infoDiv').css("top", "85px");*/
    }
});

//prilikome resizovanja screen-a:
$(window).on('resize', function () {
    if ($(window).width() < 576) {
        $('#productsMenu').addClass('d-none d-sm-block');
        /* $('#infoDiv').css("top", "485px");*/
    }
    else {
        $('#productsMenu').removeClass('d-none d-sm-block');
        /* $('#infoDiv').css("top", "85px");*/
    }
});