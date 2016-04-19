var slideIndex = 1;
showSlides(slideIndex);

$(".slide-show-dot").click(function() {
  var n = $(this).attr("name");
  showSlides(slideIndex = n);
});

function showSlides(n) {
  var i;
  var slides = document.getElementsByClassName("slide-show-item");
  var dots = document.getElementsByClassName("slide-show-dot");
  if (n > slides.length) {slideIndex = 1}    
  if (n < 1) {slideIndex = slides.length};
  for (i = 0; i < slides.length; i++) {
      slides[i].style.display = "none";  
  }
  for (i = 0; i < dots.length; i++) {
    dots[i].className = dots[i].className.replace(" slide-show-active-control", "");
  }
  slides[slideIndex-1].style.display = "block";  
  dots[slideIndex - 1].className += " slide-show-active-control";
}