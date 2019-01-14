
$(document).ready(  function () {
    //$('html, body').scrollTop(0);

    //$(window).on('load', function () {
    //    setTimeout(function () {
    //        $('html, body').scrollTop(0);
    //    }, 0);
    //});
    //$.ajax({
    //    url: "/Index/IndexProductList/",
    //    type: "GET",
    //    success: function (response) {
    //        if (response !== null) {
    //            $("#productPartialView").html(response);
    //        }

    //    }
    //});
    

    $.ajax({
        url: "/Card/AddProductToCookie/",
        type: "GET",
        success: function (response) {
            if (response !== null) {
                $("#headerRight").html(response);
            } 
        }

    });



    $.ajax({
        url: "/Card/CardPartialView/",
        type: "GET",
        success: function (response) {
            if (response !== null) {
                $("#cardProduct").html(response);
            }
        }

    });

});


$("#removeSelectedItem").click(function () {
    var Id = $(this).attr("data-id");
    $.ajax({
        url: "/Card/CardPartialView/",
        type: "POST",
        data: { id: Id },
        success: function (response) {
            if (response !== null) {
                $("#cardProduct").html(response);
            }
           
        }

    });
});


var categoryClick = function (elemnt, name) {
    var Id = elemnt.attr("data-id");
    $.ajax({
        url: "/Index/IndexProductList/",
        type: "POST",
        data: { id: Id, name: name },
        success: function (response) {
                if (response !== null) {
                    $("#productPartialView").html(response);
                }
        }

    });
};

$(".catClick").click(function () {
    categoryClick($(this), "cat");
});

$(".SubCat").click(function () {
    categoryClick($(this), "subcat");
});




$(".btnPagination").click(function () {
    var Count = $(this).attr("data-id");
    $.ajax({
        url: "/Index/Pagination/",
        type: "GET",
        data: { count: Count },
        success: function (response) {
            if (response !== null) {
                $("#productPartialView").html(response);
            }
        }

    });
});




$(".addtowl").click(function () {
    var item = $(this);
    var Id = $(this).attr("data-id");
    if ($(this).css("color") === "rgb(255, 0, 0)") {
        swal("Bu məhsul artıq seçilib","warning");
        return;
    }
    $.ajax({
        url: "/Card/AddProductToCookie/",
        type: "POST",
        data: { id: Id },
        success: function (response) {
            if (response !== null) {
                $(".headerRight").html(response);
                item.css({ "color": "red" });

            }
        }

    });

});

var itemPrice = async function (element) {
    let item = element.parent().next().children()[0];
    var val = element.val();
    console.log(val);
    let val2 = element.parent().next().children()[0].getAttribute("data-price");
    console.log(val2);
    var sum = val * val2;
    console.log(Math.round(sum * 100) / 100);
    item.innerHTML = Math.round(sum * 100) / 100;
};

$(".itemQuantity").on("change",async function () {
    itemPrice($(this));
});
$(".itemQuantity").on("keyup", async function () {
    itemPrice($(this));
});


var validationForm = function () {
    var x = $('#validateForm > input');
    for (var y = 0; y < x.length; y++) {
        var result = true;
        if (x[y].value === "") {
            x[y].nextElementSibling.innerHTML = "Boshlug ola bilmez ve ya duzgun formatda deyil";
            result = false;
        }
        else {
            x[y].nextElementSibling.innerHTML = "";
        }
    }
    var phoneno = /^\d{10}$/;
    var reg = new RegExp(phoneno);
    var l= reg.test($(".userNumber").val());
    console.log(l);
    if (l === false) {
        
        $(".userNumber").next().html("Boshlug ola bilmez ve ya duzgun formatda deyil");
        result = false;
    }

    return result;
};

$(".confirmOrder").click(function () {
    var arr = [];
    var el = document.querySelectorAll(".itemQuantity");
    for (var i = 0; i < el.length; i++) {
        arr[i] = { count: el[i].value, id: el[i].getAttribute("data-id")};
    }
    var userregister = {
        
        name: document.querySelector(".userName").value,
        surname: document.querySelector(".userSurname").value,
        number: document.querySelector(".userNumber").value*1,
        adress: document.querySelector(".userAdress").value
    };
    console.log(userregister.number);
    var res = validationForm();
    if (res === true) {
       
         SengMessage(arr, userregister);
    }
});

var SengMessage =  function (x, y) {
    $.ajax({
        url: "/Checkout/SendMessage/",
        type: "POST",
        data: {
            arr: x,
            user: y
        },
        success:  function (response) {
            console.log(response);
            if (response.boolvalue === false) {
               
                for (var i of response.validatioList) {
                    var item = i.ErrorMessage;
                    if (item.includes("Name")) {
                        document.querySelector(".nameValid").innerHTML = item;
                    }
                    else if (item.includes("Surname")) {
                        document.querySelector(".surnameValid").innerHTML = item;
                    }
                    else if (item.includes("Phone")) {
                        document.querySelector(".phoneValid").innerHTML = item;
                    }
                    else if (item.includes("Address")) {
                        document.querySelector(".addressValid").innerHTML = item;
                    }   
                    
                }
               
            }
            else {
                swal("Sifarişiniz qəbul olundu!", "Sizinlə yaxın müddət ərzində əlaqə saxlanılacaq.", "success");
               
            }
        }

    });
};

$(".search-btn").click(function () {
    var n = 1060;
   
    if ($("#sort-input").val() !== "") {
      
        var Text = $("#sort-input").val();
        $.ajax({
            url: "/Index/ProductSearch/",
            type: "POST",
            data: { value: Text },
            success: function (response) {
               
                if (response !== null) {
                    $('html,body').animate({ scrollTop: n }, 550);
                    $("#productPartialView").html(response);
                }
              
            }


        });
    }

    else {
        swal("Zəhmət olmasa axtarış bölməsini boş buraxmayın!", 'Oops...',"error");
       
    }
   
});
$(document).ready(function () {
    $("#input-sort2").on('change', function () {
        var selected = $(this).val();
        console.log(selected);
        $.ajax({
            url: "/Index/ProductSearch/",
            type: "POST",
            data: { value: selected },

            success: function (response) {
                if (response !== null) {
                    $("#productPartialView").html(response);
                }
            }
        });
    });
});



$(".hit-fa").click(function () {
    if ($(this).hasClass("fa-plus")) {
        $(this).parent().next().next().css("display", "block");
        $(this).addClass("fa-minus");
        $(this).removeClass("fa-plus");
    }
    else {
        $(this).parent().next().next().css("display", "none");
        $(this).addClass("fa-plus");
        $(this).removeClass("fa-minus");
    }
});

$(".responsive-hit").click(function () {
    if ($(this).hasClass("fa-plus")) {
        $(this).parent().next().css("display", "block");
        $(this).addClass("fa-minus");
        $(this).removeClass("fa-plus");
    }
    else {
        $(this).parent().next().css("display", "none");
        $(this).addClass("fa-plus");
        $(this).removeClass("fa-minus");
    }
});
$("#scrollup").click(function () {
    $("html").animate({ scrollTop: 0 }, 1500);
    return false;
});

$(window).load(function () {
    $(".navbar-toggle").click(function () {
        $(".navbar-collapse").slideToggle("slow");
    });


});

$(document).ready(function () {
    $('form').on('keydown', 'input[type=number]', function (e) {
        if (e.which === 38 || e.which === 40)
            e.preventDefault();
    });
});



var ProductCategoryClick = function (element,) {

}
