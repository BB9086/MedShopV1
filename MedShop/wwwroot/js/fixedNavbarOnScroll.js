
//link: https://www.youtube.com/watch?v=QXYGVkD1IcE
// get header height (without border)
var getHeaderHeight = $('.headerContainerWrapper').outerHeight();


$(window).scroll(function () {
	var currentScrollPosition = $(window).scrollTop();

	if ($(window).scrollTop() > 0) {

		$('#bodylayout').addClass('scrollActive');
		$('.headerContainerWrapper').addClass('sticky');
		
	} else {

		$('#bodylayout').removeClass('scrollActive');
		$('.headerContainerWrapper').removeClass('sticky');

	}
});