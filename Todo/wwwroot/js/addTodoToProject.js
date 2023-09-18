$(function () {
    $('#project-add-modal').slideUp(0);

    $('#project-add-modal').load("/Admin/Project/AddModalProject");
    $('.add-project-modal-button').on('click', function () {
        console.log("woidr")
        $('#project-add-modal').slideToggle(100);
        if (screen.width < 540) {
            $('.search-bar-header').slideUp(0);
        }

        $('#cancel-project-button').on('click', function () {
            console.log('ehll')
            $('#project-add-modal').slideUp(0);
        });
    })
})

