// home button
const homeButton = document.querySelector('.fa-house');
const OvertimeEntriesDragAndDropContainer = document.querySelector('.overtime-entries');
const OvertimeEntriesDragAndDropItems = document.querySelectorAll('.overtime-entry');
const Entries = document.querySelectorAll('.entry');
const ProjectEditModal = document.querySelector('#detail-page-project');

homeButton.addEventListener('click', () => {
    window.location.href = '';
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

OvertimeEntriesDragAndDropItems.forEach(item => {
    item.setAttribute('draggable', 'true')

    item.addEventListener('dragstart', () => {
        item.classList.add('dragging');
    })

    item.addEventListener('dragend', () => {
        item.classList.remove('dragging');
    })
})

OvertimeEntriesDragAndDropContainer.addEventListener('dragover', e => {
    e.preventDefault()

    const currentDragging = document.querySelector('.dragging')
    const afterElement = getAfterElement(OvertimeEntriesDragAndDropContainer, e.clientY, currentDragging)


    if (afterElement == null) {
        OvertimeEntriesDragAndDropContainer.appendChild(currentDragging);
    } else {
        OvertimeEntriesDragAndDropContainer.insertBefore(currentDragging, afterElement)
    }
})

function getAfterElement(container, y, draggingElement) {
    const draggableElements = [...container.querySelectorAll('.overtime-entry:not(.dragging)')];

    let afterElement = null;
    let minDistance = Infinity;

    draggableElements.forEach(element => {
        const box = element.getBoundingClientRect();
        const offset = y - box.top - box.height / 2;

        if (offset > 0 && offset < minDistance) {
            minDistance = offset;
            afterElement = element;
        }
    });

    if (afterElement === draggingElement) {
        return afterElement.nextElementSibling;
    }

    return afterElement;
}

// hamburger menu
$(function () {
    
    $('.fa-bars').on('click', function (e) {
        $('.hamburger-menu-drop-down').slideToggle(100);
        e.stopPropagation();
        if (screen.width < 540) {
            
        }
    })

    //searchbar
    if (screen.width < 540) {
        $('.search-bar-header').slideUp(0);
    }
    
    $('.fa-magnifying-glass').on('click', function () {
        $('.search-bar-header').slideToggle(100);
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
        $('#SearchResults').load("/Admin/Today/SearchResults?input=" + input);
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

    // Project AddModal
    $('#project-add-modal').slideUp(0);

    $('#project-add-modal').load("/Admin/Project/Add");
    $('.add-project-button').on('click', function () {
        $('#project-add-modal').slideToggle(100);
        if (screen.width < 540) {
            $('.search-bar-header').slideUp(0);
        }

        $('#cancel-project').on('click', function () {
            console.log('ehll')
            $('#project-add-modal').slideUp(0);
        });
    })

    

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