
var isManager = $('.anchr').attr('isManager');

function updateNotificationCount() {
    var count = 0;
    count = parseInt($('.badge.badge-pill.badge-info').html()) || 0;
    count++;

    $('.badge.badge-pill.badge-info').html(count);
}

document.getElementById("sendButton").addEventListener("click", event => {
    var messagesInput = $('#messageInput').val();

 
    if (isManager == 'isManager') {

        $('#messageListId').append('<div class="messageContainer right"><img src="images/logo_chat.png' + '" alt="images/logo.png" class="right"/><li>' + '<span class="right" style="font-size:20px">' + messagesInput + '</span></br></li><small class="text-right">' + GetCurrentDateTime() + '</small></div>');
    }
    else {
        $('#messageListId').append('<div class="messageContainer right"><img src="images/sample_user.jpg' + '" alt="images/logo.png" class="right"/><li>' + '<span class="right" style="font-size:20px">' + messagesInput + '</span></br></li><small class="text-right">' + GetCurrentDateTime() + '</small></div>');
    }
    
    $('#messageInput').val('');
    // auto scroll to bottom automatically!!! Link: https://stackoverflow.com/questions/39729791/chat-box-auto-scroll-to-bottom/39729993
    $(".cardChat").stop().animate({ scrollTop: $(".cardChat")[0].scrollHeight }, 1000);

    //var receiverId = $('#receiverId').text();
    var receiverUserName = $('#chat-with-id').text();



    sendMessage1(messagesInput, receiverUserName);


});

var sendMessage1 = function (messagesInput, receiverUserName) {

    connection.invoke("SendMessageToManagerOfStore", messagesInput, receiverUserName).catch(function (err) { return console.error(err.toString()); });

}


connection.on("ReceiverOfMessage", function (messageInput, senderUserName) {





    if ($('.chatPopup').is(":visible")) {

        if (isManager == 'isManager') {
            $('#messageListId').append('<div class="messageContainer left"><img src="images/sample_user.jpg' + '" alt="Avatar" class="left"/><li>' + '<span class="left" style="font-size:20px">' + messageInput + '</span></br></li><small class="text-left">' + GetCurrentDateTime() + '</small></div>');
        }
        else {
            $('#messageListId').append('<div class="messageContainer left"><img src="images/logo_chat.png' + '" alt="Avatar" class="left"/><li>' + '<span class="left" style="font-size:20px">' + messageInput + '</span></br></li><small class="text-left">' + GetCurrentDateTime() + '</small></div>');
        }

        

        $(".cardChat").stop().animate({ scrollTop: $(".cardChat")[0].scrollHeight }, 1000);

        $.ajax({
            url: "https://localhost:44336/manage/ChangeStatusWhenSeeMessage",
            method: 'post',
            data: { senderName: senderUserName, receiverId: $('.anchr').attr('name') },
            dataType: "json",
            success: function (data) {


            },
            error: function (err) {
                alert(err);
            }
        });


    }
    else {
        updateNotificationCount();
    }

    //$('ul.noti-content').append('<li class="list-group-item" id="listItem_' + senderUserName + '">' + messageInput + '</li>');


});

connection.on("UpdateNotificationBadge", function (count) {

    if (count == 0) {
        $('.badge.badge-pill.badge-info').html('');
    }
    else {
        $('.badge.badge-pill.badge-info').html(count.toString());
    }

});



$('.anchr').on("click", function () {
    var currentUser = $(this).attr('name');
    $('ul.noti-content').empty();

    $.ajax({
        url: "https://localhost:44336/manage/GetListOfSenderOfMessages",
        method: 'post',
        data: { receiverName: currentUser },
        dataType: "json",
        success: function (data) {


            if (data == "There's no contacts!") {

                $('ul.noti-content').append('<li class="list-group-item">' + data + '</li>');

            }
            else {


                $.each(data, function (index, value) {



                    if (value.split(':')[1] != '') {

                        $('ul.noti-content').append('<li class="list-group-item" style="font-weight:bold; color:blue"  name="liElem_' + index + '">' + value.split(':')[0] + '</a><small class="badge badge-light">' + value.split(':')[1] + '</small></li>');

                    }
                    else {
                        $('ul.noti-content').append('<li class="list-group-item" style="font-weight:normal"  name="liElem_' + index + '">' + value.split(':')[0] + '<small class="badge badge-light">' + value.split(':')[1] + '</small></li>');

                    }


                    //$('#liElem_' + index).on("mouseover", function () {

                    //    $(this).css('background-color', 'lightgray');
                    //});
                    //$('#liElem_' + index).on("mouseout", function () {

                    //    $(this).css('background-color', 'white');
                    //});

                    $('[name=liElem_'+index+']').on("mouseover", function () {

                        $(this).css('background-color', 'lightgray');
                    });
                    $('[name=liElem_' + index + ']').on("mouseout", function () {

                        $(this).css('background-color', 'white');
                    });

                    $('[name=liElem_' + index + ']').on("click", function () {



                        var senderName = $(this).attr('name').split('_')[1];
                        var receiverName = $('.anchr').attr('name');

                        var senderUserName = '';
                        var checkIfUserIsOnline = 'false';

                        $.ajax({
                            url: "https://localhost:44336/manage/GetSenderUserNameById",
                            method: 'post',
                            data: { senderId: senderName },
                            dataType: "json",
                            success: function (data) {

                                senderUserName = data;

                                $.ajax({
                                    url: "https://localhost:44336/home/CheckIfUserIsOnline",
                                    method: 'post',
                                    data: { chatWith: senderUserName },
                                    dataType: "json",
                                    success: function (data1) {

                                        checkIfUserIsOnline = data1;
                                        if (data1 == 'true') {
                                            $('#chat-with-id').html(senderUserName + '<img src="images/online.png' + '" alt="Avatar" class="online"/>');
                                        }
                                        else {
                                            $('#chat-with-id').html(senderUserName + '<img src="images/offline.jpg' + '" alt="Avatar" class="online"/>');
                                        }

                                    },
                                    error: function (err) {
                                        alert(err);
                                    }
                                });


                            },
                            error: function (err) {
                                alert(err);
                            }
                        });

                       


                        $('#messageListId').empty();

                        var count = 0;
                        count = parseInt($('.badge.badge-pill.badge-info').html()) || 0;

                        $.ajax({
                            url: "https://localhost:44336/manage/GetListOfUnreadMessagesBySenderNameAndReceiverName",
                            method: 'post',
                            data: { senderName: senderName, receiverName: receiverName },
                            dataType: "json",
                            success: function (data) {

                                if (data != null) {

                                    for (i = 0; i < data.length; i++) {

                                        //$.ajax({
                                        //    url: "https://localhost:44336/manage/GetSenderUserNameById",
                                        //    method: 'post',
                                        //    data: { senderId: senderName },
                                        //    dataType: "json",
                                        //    success: function (data) {

                                        //        $('#chat-with-id').html(data);
                                        //    },
                                        //    error: function (err) {
                                        //        alert(err);
                                        //    }
                                        //});



                                        if (data[i].who_IsSending == receiverName) {

                                            if (isManager == 'isManager') {

                                                $('#messageListId').append('<div class="messageContainer right"><img src="images/logo_chat.png' + '" alt="Avatar" class="right"/><li>' + '<span class="right" style="font-size:20px">' + data[i].messageBody + '</span></br></li><small class="text-right">' + data[i].dateOfSending.toString() + '</small></div>');
                                            }
                                            else {
                                                $('#messageListId').append('<div class="messageContainer right"><img src="images/sample_user.jpg' + '" alt="Avatar" class="right"/><li>' + '<span class="right" style="font-size:20px">' + data[i].messageBody + '</span></br></li><small class="text-right">' + data[i].dateOfSending.toString() + '</small></div>');
                                            }
                                            
                                        }
                                        else {
                                            if (isManager == 'isManager') {
                                                $('#messageListId').append('<div class="messageContainer left"><img src="images/sample_user.jpg' + '" alt="Avatar" class="left"/><li>' + '<span class="left" style="font-size:20px">' + data[i].messageBody + '</span></br></li><small class="text-left">' + data[i].dateOfSending.toString() + '</small></div>');

                                            }
                                            else {
                                                $('#messageListId').append('<div class="messageContainer left"><img src="images/logo_chat.png' + '" alt="Avatar" class="left"/><li>' + '<span class="left" style="font-size:20px">' + data[i].messageBody + '</span></br></li><small class="text-left">' + data[i].dateOfSending.toString() + '</small></div>');
                                            }
                                            

                                        }

                                    }

                                }

                                $('.chatPopup').show();

                                $(".cardChat").stop().animate({ scrollTop: $(".cardChat")[0].scrollHeight }, 1000);

                                connection.invoke("UpdateNumberOfUnreadedMessagesAfterReading", receiverName, senderName).catch(function (err) { return console.error(err.toString()); });
                                //count = count - data.length;
                                //if (count != 0) {
                                //    $('#smallForMessage').html(count);
                                //}
                                //else {
                                //    $('#smallForMessage').html('')
                                //}
                            },
                            error: function (err) {
                                alert(err);
                            }
                        });



                    });



                });


            }

        },
        error: function (err) {
            alert(err);
        }
    });


    $('ul.noti-content').show();

});




//When click on enter to send a message!!!
$('#messageInput').keyup(function (e) {

    if (e.keyCode == 13) {
        //e.preventDefault();
        $('#sendButton').click();
    }

});

function closeForm() {
    document.getElementById("myForm").style.display = "none";
}

$('#linkClose').click(function () {
    closeForm();
    //document.getElementsByClassName("open-button").style.display = "none";

    $('.open-button').css('display', 'none');

});

$(document).click(function (e) {
    var popup = $("ul.noti-content");
    if ((!$('ul.noti-content').find('li').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) && (!$('.anchr').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) && (!$('.fa.fa-envelope.fa-2x.badge').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) && (!$('.badge.badge-pill.badge-info').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0)) {

        /*$('.popup').attr("style", "display:none");*/
        popup.fadeOut(300);

    }

});

function GetCurrentDateTime() {
    var currentdate = new Date();
    var month = currentdate.getMonth() + 1;
    var hours = currentdate.getHours();

    //am-pm format. Link: https://stackoverflow.com/questions/8888491/how-do-you-display-javascript-datetime-in-12-hour-am-pm-format
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    var minutes = currentdate.getMinutes();
    if (minutes < 10) {
        minutes = "0" + minutes;
    }
    var seconds = currentdate.getSeconds();
    if (seconds < 10) {
        seconds = "0" + seconds;
    }

    return currentdate.getDate() + '/' + month + '/' + currentdate.getFullYear() + ' ' + hours + ':' + minutes + ':' + seconds + ' ' + ampm;

}