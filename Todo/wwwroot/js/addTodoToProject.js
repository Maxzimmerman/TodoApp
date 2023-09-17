$(function () {
    $('#add-modal').slideUp(0);

    $('#add-modal').load("/Admin/Project/AddModalProject");
    $('.add-project-modal-button').on('click', function () {
        $('#add-modal').slideToggle(100);
        if (screen.width < 540) {
            $('.search-bar-header').slideUp(0);
        }

        $('#cancel').on('click', function () {
            console.log('ehll')
            $('#add-modal').slideUp(0);
        });
    })
})

