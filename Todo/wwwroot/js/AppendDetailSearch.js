$(function () {
    $('.search-bar-link').each(function () {
        var link = $(this);
        console.log(link)
        link.on('click', function () {
            var id = link.find('#search-link-id').text();
            $('#detail-page').load("/Admin/Today/DetailSearchLinkResult?id=" + id);
        });
    });
    console.log("Hello")
});