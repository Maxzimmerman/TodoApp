// hamburger menu
$(function () {
    $('.hamburger-menu-drop-down').slideUp(0);
    $('.fa-bars').on('click', function () {
        $('.hamburger-menu-drop-down').slideToggle(100);
        if (screen.width < 540) {
            $('.search-bar-header').slideUp(0);
        }
        $('#checked-bar').slideUp(0);
        $('#add-modal').slideUp(0);
        
    })

    //searchbar
    if (screen.width < 540) {
        $('.search-bar-header').slideUp(0);
    }
    
    $('.fa-magnifying-glass').on('click', function () {
        $('.search-bar-header').slideToggle(100);
        $('.hamburger-menu-drop-down').slideUp(0);
        $('#checked-bar').slideUp(0);
        $('#add-modal').slideUp(0);
    });

    $('.fa-xmark').on('click', function () {
        if (screen.width < 540) {
            $('.search-bar-header').slideUp(0);
            $('#inputQuery').val('');
        } else {
            $('#inputQuery').val('');
            $('.Results').slideUp(0);
        }
    })

    $('.search-bar-header').on('keyup', function () {
        var input = $('#inputQuery').val();
        $('#SearchResults').load("/Customer/home/SearchResults?input=" + input);
    })

    // Add Modal
    $('#add-modal').slideUp(0);

    $('#add-modal').load("/Customer/home/AddModal");
    $('.fa-plus').on('click', function () {
        $('#add-modal').slideToggle(100);
        if (screen.width < 540) {
            $('.search-bar-header').slideUp(0);
        }
        
        $('#checked-bar').slideUp(0);
        $('.hamburger-menu-drop-down').slideUp(0);

        $('#cancel').on('click', function () {
            console.log('ehll')
            $('#add-modal').slideUp(0);
        });
    })

    

    // Checked bar
    $('#checked-bar').slideUp(0);
    $('#checked-bar').load("/Customer/home/Checked");
    $('.fa-check').on('click', function () {
        $('#checked-bar').slideToggle(100);
        if (screen.width < 540) {
            $('.search-bar-header').slideUp(0);
        }
        $('#add-modal').slideUp(0);
        $('.hamburger-menu-drop-down').slideUp(0);
    })

    // Todo make this work
    //$(window).on('click', function () {
    //    console.log('hello')
    //    $('#checked-bar').slideUp(0);
    //});

    // checked alert

    //Todo make this work
    $(".checked-alert").hide(0);

    if ($(".checked-alert").val() !== " ") {
        $(".checked-alert").show(0);
    }


    $(".added-alert").hide(0);

    if ($(".checked-alert").val() === " ") {
        $(".checked-alert").show(0);
    }


    // today toggle

    // Todo make this two work
    $(".today-toggle").on('click', function () {
        $(".today-toggle").animate({
            transform: 'rotate(90deg)',
        });
        $(".today-entries").slideToggle(300);
    })


    // overtime toggle

    $(".ovetime-toggle").on('click', function () {
        $(".today-toggle").addClass("down");
        $(".overtime-entries").slideToggle(300);
    })
});

// home button
const homeButton = document.querySelector('.fa-house');

homeButton.addEventListener('click', () => {
    window.location.href = 'https://localhost:7208/';
})
