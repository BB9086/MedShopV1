$(document).ready(function () {

    //$('.popup').hide();
    $('.popup').attr("style", "display:none");
});

function openForm() {
    document.getElementById(".popup").style.display = "block";
}

//function closeForm() {
//    document.getElementById(".popup").style.display = "none";
//}



$('#btnClose').click(function () {
    $('.popup').attr("style", "display:none");

});


var connection = new signalR.HubConnectionBuilder().withUrl("/myHub").build();
connection.start()
    .then(function () {
        console.log("IT WORKED!")
        connection.invoke("AnnounceSummary").catch(function (err) { return console.error(err.toString()); });

    })
    .catch(function (err) { return console.error(err.toString()) });

$(document).on('mouseover', '#btnModalHower', function (e) {

    //$('.popup').show(1000)

    if ($('.popup').css('display') == 'none') {
        connection.invoke("SendListOfProducts").catch(function (err) { return console.error(err.toString()); });
        /* $('.popup').attr("style", "display:block").show(1000);*/
        $('.popup').fadeIn(500);
    }
});


$('#envelopeLink').on('mouseover', function (e) {
    //debugger

    //$('#myForm').show(1000)

    if ($('.popup').css('display') == 'none') {
        connection.invoke("SendListOfProducts").catch(function (err) { return console.error(err.toString()); });
        /*$('.popup').attr("style", "display:block").show(1000);*/
        $('.popup').fadeIn(500);
    }


});


$('#spanNoti').on('mouseover', function (e) {
    //debugger

    //$('#myForm').show(1000)

    if ($('.popup').css('display') == 'none') {
        connection.invoke("SendListOfProducts").catch(function (err) { return console.error(err.toString()); });
        /*$('.popup').attr("style", "display:block").show(1000);*/
        $('.popup').fadeIn(500);
    }


});




$('#badge').on('mouseover', function (e) {
    //debugger

    //$('#myForm').show(1000)

    if ($('.popup').css('display') == 'none') {
        connection.invoke("SendListOfProducts").catch(function (err) { return console.error(err.toString()); });
       /* $('.popup').attr("style", "display:block").show(1000);*/
        $('.popup').fadeIn(500);
    }


});

$('#txtVasaKorpa').on('mouseover', function (e) {
    //debugger

    //$('#myForm').show(1000)

    if ($('.popup').css('display') == 'none') {
        connection.invoke("SendListOfProducts").catch(function (err) { return console.error(err.toString()); });
        /* $('.popup').attr("style", "display:block").show(1000);*/
        $('.popup').fadeIn(500);
    }


});
//umesto click staviti mouseover ako hoces da se gubi kada predjes preko ostatka prozora
$(document).mouseover(function (e) {
    var popup = $(".popup");
    if ((!$('#envelopeLink').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) && (!$('#btnModalHower').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) && (!$('#spanNoti').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) && ((!$('#badge').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0)) && ((!$('#txtVasaKorpa').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0))) {
       
        /*$('.popup').attr("style", "display:none");*/
        popup.fadeOut(300);
        
    }

});



//!!!! ovo menjam posto hocu da se dodam i ceu uz broj porucenih proizvoda!!!

//$('#btnAdd').click(function () {


//    connection.invoke("Announce").catch(function (err) { return console.error(err.toString()); });



//});




connection.on("Announce", function (countOfOrders, sumOfProducts) {

    //var count = 0;
    //count = parseInt($('small.notification-badge').html()) || 0;
    //count++;

    if (countOfOrders != 0) {
        //dodato replace da prikazuje zarez kada je broj veci jednak od 1000!!!: https://www.wikitechy.com/tutorials/javascript/print-a-number-with-commas-as-thousands-separators-in-javascript
        $('small.notification-badge').html(countOfOrders + ' / ' + sumOfProducts.replace(/\B(?=(\d{3})+(?!\d))/g, ",")+' RSD');
    }
    else {
        $('small.notification-badge').html("0 / 0.00 RSD");
    }


});



//2.nacin da menjas vrednost u spanNotiju, preko vrednosti cookija u sessionstorage, ovo ispod premesteno u Products.cshtml:

//CountOfItemsInCookie = function () {

//    var count = 0;
//    count = parseInt($('small.notification-badge').html()) || 0;
//    count++;

//    if (countOfOrders != 0) {
//        $('small.notification-badge').html(countOfOrders);
//    }
//    else
//    {
//        $('small.notification-badge').html();
//    }

//}



getDataFromCookie = function () {
    $('#listOfProductsInShopCart').empty();

    var list = [];
    var productKeys = sessionStorage.getItem('productKeys');
    //debugger
    if (productKeys != null) {
        var prodObject = JSON.parse(productKeys);
        list = prodObject.productKeys;
        //var postData = JSON.stringify({ productKeys: list });
        //ajax funkcija ide na server:

        $.ajax({
            url: "https://localhost:44336/home/GetListOfProductInShoopingCart",
            method: 'post',
            data: { productKeys: list },
            dataType: "json",
            success: function (data) {

                x = data.length;
                var totalSum = 0;
                var i;
                for (i = 0; i < data.length; i++) {
                    var productName = data[i].name;
                    var productImageSrc = data[i].imageSource.split("~")[1];
                    var imageSrc = window.location.origin + '/' + productImageSrc;
                    $('#listOfProductsInShopCart').append('<li id="listElemWithProductDetailsForShop_' + i + '" style="font-size:12px; position:relative" class="list-group-item d-flex justify-content-between align-items-center"><img width="20%" class="img-fluid" src="' + imageSrc + '" /><a href="https://localhost:44336/home/products/' + data[i].descriptionId+'">' + data[i].name + '</a><button type="button" id="' + i + '" class="close" onclick="removeItemFromList(this.id)" name="btnForClosing" style="position: absolute;top:0;right:0"><span aria-hidden="true">&times;</span></button></li>');
                    totalSum = totalSum + data[i].currentPrice;

                    //$('.close').click(function (e) {
                    //    debugger
                    //    var mm = list;
                    //    var x = parseInt($(this).attr("id"));
                    //    var lis = [];
                    //    var lis = list.splice(x, 1);

                    //    totalSum = totalSum - data[x].currentPrice;
                    //    var m = list.length;
                    //    $('#listElemWithProductDetailsForShop_' + x).remove();

                    //    var l = list;
                    //    e.stopPropagation();
                    //    // totalSum = totalSum - data[x].currentPrice;
                    //    //sessionStorage.removeItem("productKeys");
                    //    // var myObj = {
                    //    //     "productKeys": list
                    //    // };
                    //    // sessionStorage.setItem("productKeys", JSON.stringify(myObj));
                    //    // getDataFromCookie();



                    //});

                    removeItemFromList = function (id) {
                        //debugger
                        var mm = list;
                        var x = parseInt(id);
                        var lis = [];
                        var lis = list.splice(x, 1);
                        var m = list.length;
                        $('#listElemWithProductDetailsForShop_' + x).remove();
                        var l = list;
                        totalSum = totalSum - data[x].currentPrice;

                        $('#bTotalSum').attr('style', 'display:block');
                        $('#spanTotalSum').text(totalSum + ' RSD');


                        //na kraju kada oduzmemo iz liste, prpravimo i sam cookie!
                        sessionStorage.removeItem("productKeys");
                        var myObj = {
                            "productKeys": list
                        };
                        sessionStorage.setItem("productKeys", JSON.stringify(myObj));


                        //onda smanjujem broj elemenata u span notiju!



                        //var count = 0;
                        //count = parseInt($('small.notification-badge').html()) || 0;
                        //count--;

                        //if (count != 0) {
                        //    $('small.notification-badge').html(count);
                        //}
                        //else {
                        //    $('small.notification-badge').html("");
                        //}





                    }
                }


                $('#bTotalSum').attr('style', 'display:block');
                $('#spanTotalSum').text(totalSum + ' RSD');

                //sessionStorage.removeItem("productKeys");
                //var myObj = {
                //    "productKeys": list
                //};
                //sessionStorage.setItem("productKeys", JSON.stringify(myObj));
                //getDataFromCookie();


            },
            error: function (err) {
                alert(err);
            }
        });



    }
    else {
        $('#listOfProductsInShopCart').append('<li style="font-size:12px" class="list-group-item d-flex justify-content-between align-items-center">Trenutno nemate nijedan proizvod u korpi</li>');

        $('#bTotalSum').attr('style', 'display:none');

    }


};


connection.on("ReceiverOfListOfProduct", function (listOfProductsKeys) {
    //debugger
    $('#listOfProductsInShopCart').empty();
    var list = [];
    if (listOfProductsKeys != null) {


        list = listOfProductsKeys;
        //var postData = JSON.stringify({ productKeys: list });
        //ajax funkcija ide na server:

        $.ajax({
            url: "https://localhost:44336/home/GetListOfProductInShoopingCart",
            method: 'post',
            data: { productKeys: list },
            dataType: "json",
            success: function (data) {
                //debugger
                x = data.length;
                var totalSum = 0;
                var i;
               
                for (i = 0; i < data.length; i++) {
                    
                    var productName = data[i].name;
                    var productImageSrc = data[i].imageSource.split("~")[1];
                    var imageSrc = window.location.origin + '/' + productImageSrc;
                    var prodKey = data[i].productKey;
                    var num = data[i].numberOfSameProductInShoppingCart;
                    $('#listOfProductsInShopCart').append('<li id="listElemWithProductDetailsForShop_' + i + '" style="font-size:12px; position:relative" class="list-group-item d-flex justify-content-between align-items-center"><img width="20%" class="img-fluid" src="' + imageSrc + '" /><a href="https://localhost:44336/home/products/' + data[i].descriptionId + '">' + data[i].name + '</a><button type="button" id="' + i + '" class="close" onclick="removeItemFromList(this.id, this.name, this.value)" name="' + data[i].productKey.toString() + '" value="' + num + '" style="position: absolute;top:0;right:0"><span aria-hidden="true">&times;</span></button><span>' + data[i].numberOfSameProductInShoppingCart + 'x' + data[i].currentPrice + ' RSD</span></li>');
                    totalSum = totalSum + (data[i].currentPrice * data[i].numberOfSameProductInShoppingCart);

                    //$('.close').click(function (e) {
                    //    debugger
                    //    var mm = list;
                    //    var x = parseInt($(this).attr("id"));
                    //    var lis = [];
                    //    var lis = list.splice(x, 1);

                    //    totalSum = totalSum - data[x].currentPrice;
                    //    var m = list.length;
                    //    $('#listElemWithProductDetailsForShop_' + x).remove();

                    //    var l = list;
                    //    e.stopPropagation();
                    //    // totalSum = totalSum - data[x].currentPrice;
                    //    //sessionStorage.removeItem("productKeys");
                    //    // var myObj = {
                    //    //     "productKeys": list
                    //    // };
                    //    // sessionStorage.setItem("productKeys", JSON.stringify(myObj));
                    //    // getDataFromCookie();



                    //});

                    removeItemFromList = function (id, productKey, numberOfSameProducts) {
                        //debugger
                        var mm = list;
                        var x = parseInt(id);
                        var lis = [];
                        var lis = list.splice(x, 1);
                        var m = list.length;
                        $('#listElemWithProductDetailsForShop_' + x).remove();
                        var l = list;
                        totalSum = totalSum - (data[x].currentPrice * numberOfSameProducts);

                        var currentPriceString = data[x].currentPrice.toString();
                       


                        //na kraju kada oduzmemo iz liste, prpravimo i sam cookie!

                        connection.invoke("RemoveProduct", productKey, currentPriceString).catch(function (err) { return console.error(err.toString()); });

                        //onda smanjujem broj elemenata u span notiju!



                        var count = 0;
                        count = parseInt($('small.notification-badge').html()) || 0;
                        count = count - numberOfSameProducts;

                        if (count != 0) {
                            $('small.notification-badge').html(count + ' / ' + totalSum+' RSD');
                        }
                        else {
                            $('small.notification-badge').html("0 / 0.00 RSD");
                        }

                          if (totalSum != 0) {

                            $('#bTotalSum').attr('style', 'display:block');
                            $('#spanTotalSum').text(totalSum + ' RSD');
                        }
                        else {
                            $('#listOfProductsInShopCart').append('<li style="font-size:12px" class="list-group-item d-flex justify-content-between align-items-center">Trenutno nemate nijedan proizvod u korpi</li>');

                            $('#bTotalSum').attr('style', 'display:none');
                        }

                        //$('#bTotalSum').attr('style', 'display:block');
                        //$('#spanTotalSum').text(totalSum + ' RSD');


                    }
                }

                if (totalSum != 0) {

                    $('#bTotalSum').attr('style', 'display:block');
                    $('#spanTotalSum').text(totalSum + ' RSD');
                }
                else {
                    $('#listOfProductsInShopCart').append('<li style="font-size:12px" class="list-group-item d-flex justify-content-between align-items-center">Trenutno nemate nijedan proizvod u korpi</li>');

                    $('#bTotalSum').attr('style', 'display:none');
                }


                //sessionStorage.removeItem("productKeys");
                //var myObj = {
                //    "productKeys": list
                //};
                //sessionStorage.setItem("productKeys", JSON.stringify(myObj));
                //getDataFromCookie();


            },
            error: function (err) {
                alert(err);
            }
        });



    }

    else {
        $('#listOfProductsInShopCart').append('<li style="font-size:12px" class="list-group-item d-flex justify-content-between align-items-center">Trenutno nemate nijedan proizvod u korpi</li>');

        $('#bTotalSum').attr('style', 'display:none');

    }




});

//$('#btnShoopingCart').click(function () {


//    connection.invoke("GetListOfProducts").catch(function (err) { return console.error(err.toString()); });



//});


connection.on("ReceiverOfListOfProduct_StranicaPlacanje", function (listOfProductsKeys) {
    //debugger
    $('#listOfProductsInShopCart').empty();
    var list = [];
    if (listOfProductsKeys != null) {


        list = listOfProductsKeys;
        //var postData = JSON.stringify({ productKeys: list });
        //ajax funkcija ide na server:

        $.ajax({
            url: "https://localhost:44336/home/GetListOfProductInShoopingCart",
            method: 'post',
            data: { productKeys: list },
            dataType: "json",
            success: function (data) {
                //debugger
                x = data.length;
                var totalSum = 0;
                var i;

                for (i = 0; i < data.length; i++) {

                    var productName = data[i].name;
                    var productImageSrc = data[i].imageSource.split("~")[1];
                    var imageSrc = window.location.origin + '/' + productImageSrc;
                    var prodKey = data[i].productKey;
                    var num = data[i].numberOfSameProductInShoppingCart;
                    $('#listOfProductsInShopCart').append('<li id="listElemWithProductDetailsForShop_' + i + '" style="font-size:12px; position:relative" class="list-group-item d-flex justify-content-between align-items-center"><img width="20%" class="img-fluid" src="' + imageSrc + '" /><a href="https://localhost:44336/home/products/' + data[i].descriptionId + '">' + data[i].name + '</a><button type="button" id="' + i + '" class="close" onclick="removeItemFromList(this.id, this.name, this.value)" name="' + data[i].productKey.toString() + '" value="' + num + '" style="position: absolute;top:0;right:0"><span aria-hidden="true">&times;</span></button><span>' + data[i].numberOfSameProductInShoppingCart + 'x' + data[i].currentPrice + ' RSD</span></li>');
                    totalSum = totalSum + (data[i].currentPrice * data[i].numberOfSameProductInShoppingCart);

                    //$('.close').click(function (e) {
                    //    debugger
                    //    var mm = list;
                    //    var x = parseInt($(this).attr("id"));
                    //    var lis = [];
                    //    var lis = list.splice(x, 1);

                    //    totalSum = totalSum - data[x].currentPrice;
                    //    var m = list.length;
                    //    $('#listElemWithProductDetailsForShop_' + x).remove();

                    //    var l = list;
                    //    e.stopPropagation();
                    //    // totalSum = totalSum - data[x].currentPrice;
                    //    //sessionStorage.removeItem("productKeys");
                    //    // var myObj = {
                    //    //     "productKeys": list
                    //    // };
                    //    // sessionStorage.setItem("productKeys", JSON.stringify(myObj));
                    //    // getDataFromCookie();



                    //});

                    removeItemFromList = function (id, productKey, numberOfSameProducts) {
                        //debugger
                        var mm = list;
                        var x = parseInt(id);
                        var lis = [];
                        var lis = list.splice(x, 1);
                        var m = list.length;
                        $('#listElemWithProductDetailsForShop_' + x).remove();
                        var l = list;
                        totalSum = totalSum - (data[x].currentPrice * numberOfSameProducts);

                        var currentPriceString = data[x].currentPrice.toString();



                        //na kraju kada oduzmemo iz liste, prpravimo i sam cookie!

                        connection.invoke("RemoveProduct", productKey, currentPriceString).catch(function (err) { return console.error(err.toString()); });

                        //onda smanjujem broj elemenata u span notiju!



                        var count = 0;
                        count = parseInt($('small.notification-badge').html()) || 0;
                        count = count - numberOfSameProducts;

                        if (count != 0) {
                            $('small.notification-badge').html(count + ' / ' + totalSum+' RSD');
                        }
                        else {
                            $('small.notification-badge').html("0 / 0.00 RSD");
                        }

                        if (totalSum != 0) {

                            $('#bTotalSum').attr('style', 'display:block');
                            $('#spanTotalSum').text(totalSum + ' RSD');
                        }
                        else {
                            $('#listOfProductsInShopCart').append('<li style="font-size:12px" class="list-group-item d-flex justify-content-between align-items-center">Trenutno nemate nijedan proizvod u korpi</li>');

                            $('#bTotalSum').attr('style', 'display:none');
                        }

                        //$('#bTotalSum').attr('style', 'display:block');
                        //$('#spanTotalSum').text(totalSum + ' RSD');


                    }
                }

                if (totalSum != 0) {

                    $('#bTotalSum').attr('style', 'display:block');
                    $('#spanTotalSum').text(totalSum + ' RSD');
                }
                else {
                    $('#listOfProductsInShopCart').append('<li style="font-size:12px" class="list-group-item d-flex justify-content-between align-items-center">Trenutno nemate nijedan proizvod u korpi</li>');

                    $('#bTotalSum').attr('style', 'display:none');
                }


                //sessionStorage.removeItem("productKeys");
                //var myObj = {
                //    "productKeys": list
                //};
                //sessionStorage.setItem("productKeys", JSON.stringify(myObj));
                //getDataFromCookie();


            },
            error: function (err) {
                alert(err);
            }
        });



    }

    else {
        $('#listOfProductsInShopCart').append('<li style="font-size:12px" class="list-group-item d-flex justify-content-between align-items-center">Trenutno nemate nijedan proizvod u korpi</li>');

        $('#bTotalSum').attr('style', 'display:none');

    }




});



insertNewProduct = function (productKey) {

    //debugger


    connection.invoke("AddProduct", productKey).catch(function (err) { return console.error(err.toString()); });



};