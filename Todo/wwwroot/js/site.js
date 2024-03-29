﻿// home button
const homeButton = document.querySelector('.fa-house');
const Entries = document.querySelectorAll('.entry');
const ProjectEditModal = document.querySelector('#detail-page-project');
const ProjectDetialPage = document.querySelector('.detail-close-button-project');

homeButton.addEventListener('click', () => {
    window.location.href = "https://localhost:7208/"
})

// Todo should work
// drag and dpop function


// Close Edit Modal

let DetailPage = document.querySelector('#detail-page');
let Modal = document.querySelector('.detail-form');
function closeModal() {
    DetailPage.style.display = "none";
}
function ShowModal() {
    DetailPage.style.display = "flex";
}

function ShowModalProjectDetail() {
    ProjectEditModal.style.display = "flex";
}

function CloseModalProjectDetail() {
    ProjectEditModal.style.display = "none";
}

// hamburger menu
$(function () {
    
    $('.fa-bars').on('click', function (e) {
        $('.hamburger-menu-drop-down').slideToggle(100);
        e.stopPropagation();
        if (screen.width < 540) {
            
        }
    })

    // Add Modal
    $('#add-modal').slideUp(0);

    $('#add-modal').load("/Admin/Today/AddModal");
    $('.add-modal-button').on('click', function () {
        
        $('#add-modal').slideToggle(100);
        if (screen.width < 540) {
            $('.search-bar-header').slideUp(0);
        }

        $('#cancel').on('click', function () {
            console.log('ehll')
            $('#add-modal').slideUp(0);
        });
    })

    // Project AddModal To add a new project in header
    $('#view-to-add-a-project').slideUp(0);

    $('#view-to-add-a-project').load("/Admin/Project/AddPartial");
    $('.add-project-button').on('click', function () {
        
        console.log('add project button')
        $('#view-to-add-a-project').slideToggle(100);
        if (screen.width < 540) {
            $('.search-bar-header').slideUp(0);
        }

        $('#cancel-project-to-add-project').on('click', function () {
            console.log('ehll');
            $('#view-to-add-a-project').slideUp(0);
        });
    });


    // Checked bar
    $('#checked-bar').slideUp(0);
    $('#checked-bar').load("/Admin/Today/Checked");
    $('.fa-check').on('click', function () {
        $('#checked-bar').slideToggle(100);
        if (screen.width < 540) {
            $('.search-bar-header').slideUp(0);
        }
    })

    // Todo make this work
    //$(window).on('click', function () {
    //    console.log('hello')
    //    $('#checked-bar').slideUp(0);
    //});

    // checked alert

    //Todo make this work
    $(".checked-alert").hide(0);

    if ($('.checked-alert').is(':empty')) {
        $(".checked-alert").hide(0);
    } else {
        $(".checked-alert").show(0);
    }

    $(".added-alert").hide(0);

    if ($('.added-alert').is(':empty')) {
        $(".added-alert").hide(0);
    } else {
        $(".added-alert").show(0);
    }


    // today toggle

    $(".today-toggle").on('click', function () {
        $(".today-toggle").toggleClass("down");
        $(".today-entries").slideToggle(300);
    })


    // overtime toggle

    $(".ovetime-toggle").on('click', function () {
        $(".ovetime-toggle").toggleClass("down-overtime");
        $(".overtime-entries").slideToggle(300);
    })

    $('#login').load("/Identity/Account/Login");
});