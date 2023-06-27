// hamburger menu
$(function () {
    $('.hamburger-menu-drop-down').slideUp(0);
    $('.fa-bars').on('click', function () {
        $('.hamburger-menu-drop-down').slideToggle(100);
    })
    $('.fa-magnifying-glass').on('click', function () {
        $('.search-bar-header').slideToggle(100);
    });


    //searchbar


    $('.search-bar-header').slideUp(0);
    $('.fa-xmark').on('click', function () {
        $('.search-bar-header').slideUp(100);
    })

    $('#SearchResults').load("/home/SearchResults/")

    $('.search-bar-header').on('keyup', function () {
        var input = $('#inputQuery').val();
        console.log(input);
        $('#SearchResults').load("/home/SearchResults?input=" + input);
    })
});

// home button
const homeButton = document.querySelector('.fa-house');

homeButton.addEventListener('click', () => {
    window.location.href = 'https://localhost:7208/';
})
