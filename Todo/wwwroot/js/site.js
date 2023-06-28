// hamburger menu
$(function () {
    $('.hamburger-menu-drop-down').slideUp(0);
    $('.fa-bars').on('click', function () {
        $('.hamburger-menu-drop-down').slideToggle(100);
    })

    //searchbar
    $('.search-bar-header').slideUp(0);
    $('.fa-magnifying-glass').on('click', function () {
        $('.search-bar-header').slideToggle(100);
    });

    $('#SearchResults').load("/home/SearchResults/")

    $('.search-bar-header').on('keyup', function () {
        var input = $('#inputQuery').val();
        console.log(input);
        $('#SearchResults').load("/home/SearchResults?input=" + input);
    })

    // Add Modal
    $('#add-modal').slideUp(0);

    $('#add-modal').load("/home/AddModal");
    $('.fa-plus').on('click', function () {
        $('#add-modal').slideToggle(100);
    })
});

// home button
const homeButton = document.querySelector('.fa-house');

homeButton.addEventListener('click', () => {
    window.location.href = 'https://localhost:7208/';
})
