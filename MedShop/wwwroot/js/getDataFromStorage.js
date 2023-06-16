//$(document).ready(function () {

//    //$('.popup').hide();
//    $('.popup').attr("style", "display:none");
//});

//    function openForm() {
//        document.getElementById(".popup").style.display = "block";
//    }

//    //function closeForm() {
//    //    document.getElementById(".popup").style.display = "none";
//    //}



//$('#btnClose').click(function () {
//    $('.popup').attr("style", "display:none");

//});


//$(document).on('mouseover', '#btnModalHower', function (e) {
    
//    //$('.popup').show(1000)

//    if ($('.popup').css('display') == 'none') {
//        getDataFromCookie();
//        $('.popup').attr("style", "display:block");
//        //$('.popup').show(1000);
//    }
//});


//$('#envelopeLink').on('mouseover', function (e) {
//        //debugger

//        //$('#myForm').show(1000)
         
//        if ($('.popup').css('display') == 'none') {
//            getDataFromCookie();
//            $('.popup').attr("style", "display:block");
//            //$('.popup').show(1000);
//         }

        
//});

//$('#spanNoti').on('mouseover', function (e) {
//    //debugger

//    //$('#myForm').show(1000)

//    if ($('.popup').css('display') == 'none') {
//        getDataFromCookie();
//        $('.popup').attr("style", "display:block");
//        //$('.popup').show(1000);
//    }


//});




//$('#badge').on('mouseover', function (e) {
//    //debugger

//    //$('#myForm').show(1000)

//    if ($('.popup').css('display') == 'none') {
//        getDataFromCookie();
//        $('.popup').attr("style", "display:block");
//        //$('.popup').show(1000);
//    }


//});

//$(document).mouseover(function (e) {
//        var popup = $(".popup");
//    if ((!$('#envelopeLink').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) && (!$('#btnModalHower').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) && (!$('#spanNoti').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) && ((!$('#badge').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0))) {
//            //popup.hide(1000);
//            $('.popup').attr("style", "display:none");
//        }
       
//});

//function getData(productKeyss) {
//    $('#listOfProductsInShopCart').empty();
//    var list = [];
//    if (productKeys != null) {
//        var prodObject = JSON.parse(productKeys);
//        list = prodObject.productKeys;
//        //var postData = JSON.stringify({ productKeys: list });
//        //ajax funkcija ide na server:

//        $.ajax({
//            url: "https://localhost:44336/home/GetListOfProductInShoopingCart",
//            method: 'post',
//            data: { productKeys: list },
//            dataType: "json",
//            success: function (data) {

//                x = data.length;
               
//                var i;
//                for (i = 0; i < data.length; i++) {
//                    var productName = data[i].name;
//                    var productImageSrc = data[i].imageSource.split("~")[1];
//                    var imageSrc = window.location.origin + '/' + productImageSrc;
//                    $('#listOfProductsInShopCart').append('<li id="listElemWithProductDetailsForShop_' + i + '" style="font-size:12px; position:relative" class="list-group-item d-flex justify-content-between align-items-center"><img width="20%" class="img-fluid" src="' + imageSrc + '" /><a href="#">' + data[i].name + '</a><button type="button" id="btnInListForRemoveItem_' + i + '" class="close" style="position: absolute;top:0;right:0"><span aria-hidden="true">&times;</span></button></li>');
                    

//                }

//            },
//            error: function (err) {
//                alert(err);
//            }
//        });



//    }
//    else {
//        $('#listOfProductsInShopCart').append('<li style="font-size:12px" class="list-group-item d-flex justify-content-between align-items-center">Trenutno nemate nijedan proizvod u korpi</li>');

//        $('#bTotalSum').attr('style', 'display:none');

//    }
//}



//    getDataFromCookie = function () {
//        $('#listOfProductsInShopCart').empty();
      
//        var list = [];
//        var productKeys = sessionStorage.getItem('productKeys');
//        debugger
//        if (productKeys != null) {
//            var prodObject = JSON.parse(productKeys);
//            list = prodObject.productKeys;
//            //var postData = JSON.stringify({ productKeys: list });
//            //ajax funkcija ide na server:

//            $.ajax({
//                url: "https://localhost:44336/home/GetListOfProductInShoopingCart",
//                method: 'post',
//                data: { productKeys: list},
//                dataType: "json",
//                success: function (data) {
                 
//                    x = data.length;
//                    var totalSum = 0;
//                    var i;
//                    for (i = 0; i < data.length; i++) {
//                        var productName = data[i].name;
//                        var productImageSrc = data[i].imageSource.split("~")[1];
//                        var imageSrc = window.location.origin +'/'+ productImageSrc;
//                        $('#listOfProductsInShopCart').append('<li id="listElemWithProductDetailsForShop_'+i+'" style="font-size:12px; position:relative" class="list-group-item d-flex justify-content-between align-items-center"><img width="20%" class="img-fluid" src="' + imageSrc + '" /><a href="#">' + data[i].name + '</a><button type="button" id="'+i+'" class="close" onclick="removeItemFromList(this.id)" name="btnForClosing" style="position: absolute;top:0;right:0"><span aria-hidden="true">&times;</span></button></li>');
//                        totalSum = totalSum + data[i].currentPrice;
                            
//                        //$('.close').click(function (e) {
//                        //    debugger
//                        //    var mm = list;
//                        //    var x = parseInt($(this).attr("id"));
//                        //    var lis = [];
//                        //    var lis = list.splice(x, 1);

//                        //    totalSum = totalSum - data[x].currentPrice;
//                        //    var m = list.length;
//                        //    $('#listElemWithProductDetailsForShop_' + x).remove();

//                        //    var l = list;
//                        //    e.stopPropagation();
//                        //    // totalSum = totalSum - data[x].currentPrice;
//                        //    //sessionStorage.removeItem("productKeys");
//                        //    // var myObj = {
//                        //    //     "productKeys": list
//                        //    // };
//                        //    // sessionStorage.setItem("productKeys", JSON.stringify(myObj));
//                        //    // getDataFromCookie();



//                        //});

//                        removeItemFromList = function (id) {
//                            debugger
//                            var mm = list;
//                            var x = parseInt(id);
//                            var lis = [];
//                            var lis = list.splice(x, 1);
//                            var m = list.length;
//                            $('#listElemWithProductDetailsForShop_' + x).remove();
//                            var l = list;
//                            totalSum = totalSum - data[x].currentPrice;

//                            $('#bTotalSum').attr('style', 'display:block');
//                            $('#spanTotalSum').text(totalSum + ' RSD');


//                            //na kraju kada oduzmemo iz liste, prpravimo i sam cookie!
//                            sessionStorage.removeItem("productKeys");
//                              var myObj = {
//                                           "productKeys": list
//                                          };
//                            sessionStorage.setItem("productKeys", JSON.stringify(myObj));


//                            //onda smanjujem broj elemenata u span notiju!

                           

//                            var count = 0;
//                            count = parseInt($('small.notification-badge').html()) || 0;
//                            count--;

//                            if (count != 0) {
//                                $('small.notification-badge').html(count);
//                            }
//                            else {
//                                $('small.notification-badge').html("");
//                            }

                            
                           
            

//                        }
//                    }

                    
//                    $('#bTotalSum').attr('style', 'display:block');
//                    $('#spanTotalSum').text(totalSum + ' RSD');

//                    //sessionStorage.removeItem("productKeys");
//                    //var myObj = {
//                    //    "productKeys": list
//                    //};
//                    //sessionStorage.setItem("productKeys", JSON.stringify(myObj));
//                    //getDataFromCookie();

                    
//                },
//                error: function (err) {
//                    alert(err);
//                }
//            });
        


//        }
//        else {
//            $('#listOfProductsInShopCart').append('<li style="font-size:12px" class="list-group-item d-flex justify-content-between align-items-center">Trenutno nemate nijedan proizvod u korpi</li>');

//            $('#bTotalSum').attr('style', 'display:none');

//        }

        
//    };

//    //myObj = {
//    //    "productKeys": ["00001", "00003", "00004"]
//    //};






