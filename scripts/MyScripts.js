$(function () {
    $("#search-button").click(function () {
        
        var CatID = $("#categoryID").val();
        var SearchName = $("#SearchName").val();
        
        keyword = SearchName.replace(/\s{2,}/g, ' ');

        /* Replace space with a '-' symbol */
        keyword = keyword.replace(/\s/g, "-");

        var cleanUrl = '/Home/Search?SearchName=' + keyword.toLowerCase() + '&CatID=' + CatID;
        window.location = cleanUrl;

        return false;

    })
})

$(function () {
    $("#AddToWish").click(function () {
        $.ajax(
            {
                type: "POST",
                url: "/Home/AddWishList",
                data: {
                    product_id: $("#product_id").val()
                },
                success: function (da) {
                    $(window).scrollTop();
                    $('html,body').animate({ scrollTop: 0 }, 600);
                    $('.msgAlert').fadeIn();
                    $('.msgAlert').text(da);
                    
                    setTimeout(function () {
                        $('.msgAlert').fadeOut()
                    }, 4000);
                }
            }
            )
    })
})
