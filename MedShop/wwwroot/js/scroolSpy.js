var navOffset = $('.topNavbar').height();

$('.navbar li a').click(function (event) {
    var href = $(this).attr('href');

    // Don't let the browser scroll, but still update the current address
    // in the browser.
    event.preventDefault();
    window.location.hash = href;

    // Explicitly scroll to where the browser thinks the element
    // is, but apply the offset.
    $(href)[0].scrollIntoView();
    //70 je hight od navbara
    window.scrollBy(0, -70);
});