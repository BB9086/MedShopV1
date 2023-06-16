var slideIndex1 = 1;
//showSlides1(slideIndex1);

function plusSlides(n) {
    showSlides1(slideIndex1 += n);
}

function currentSlide(n) {
    showSlides1(slideIndex1 = n);
}

function showSlides1(n) {
    var i;
    var slides = document.getElementsByClassName("mySlides");
    var textSlides = document.getElementsByClassName("textSlides");
    var dots = document.getElementsByClassName("dot");
    if (n > slides.length) { slideIndex1 = 1 }
    if (n < 1) { slideIndex1 = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
        textSlides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[slideIndex1 - 1].style.display = "block";
    textSlides[slideIndex1 - 1].style.display = "block";
    dots[slideIndex1 - 1].className += " active";
}