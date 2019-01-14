$(document).ready(function () {
    console.log("salma");
    $.ajax({
        url: "/Card/CardPartialView/",
        type: "GET",
        success: function (response) {
            if (response !== null) {
                $(".second-sm").html(response);
            }
            else {
                console.log("errori,errori,errori")
            }
        }

    });
});
$(".removeSelectedItem").click(function () {
    var Id = $(this).attr("data-id");
    $.ajax({
        url: "/Card/CardPartialView/",
        type: "POST",
        data: { id: Id },
        success: function (response) {
            if (response !== null) {
                $(".second-sm").html(response);
            }
            else {
                console.log("errori,errori,errori")
            }
        }

    });
});