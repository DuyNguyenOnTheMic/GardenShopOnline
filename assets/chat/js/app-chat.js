$(function () {
    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.chatHub;
    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (name, message) {
        // Add the message to the page.
        /*$('#discussion').prepend('<li><strong>' + htmlEncode(name)
            + '</strong>: ' + htmlEncode(message) + '</li>');*/
    };
    $.connection.hub.start().done(function () {
        console.log(chat.connection.id);
        $('#formMessage').submit(function (e) {
            // Prevent default form submit
            e.preventDefault();

            var form = $(this);
            var actionUrl = form.attr('action');
            var message = $('#message').val();
            var fromUserId = form.data('fromuserid');
            var toUserId = form.data('touserid');

            // Xử lý hình ảnh
            var data = new FormData();
            var files = $("#file").get(0).files;

            
            data.append("file", files[0]);
            data.append("message", message);
            data.append("fromUserId", fromUserId);
            data.append("toUserId", toUserId);

            var message_html = message;
            
            // Call the Send method on the hub.
            $.ajax({
                url: actionUrl,
                contentType: false,
                processData: false,
                async: false,
                type: 'POST',
                data: data,
                success: function (response) {
                    if (response.success) {
                        if (files.length > 0) {
                            message_html = '<img src="/assets/images/' + response.img + '" />' + '<hr /><p>' + response.message+ '</p>' ;
                        }
                        $("#file").val('');
                        $('#output').attr('src', '');

                        

                        // Add chat message
                        $('#discussion').prepend('<li class="chat-right">'
                            + '<div class="chat-hour">' + response.time + '<span class="fa fa-check-circle ms-1"></span></div>'
                            + '<div class="chat-text">' + message_html + '</div>'
                            + '<div class="chat-avatar"</div>'
                            + '</li>');
                    } else {
                        alert('An error has occured, please try again later!')
                    }
                }
            });
            // Clear text box and reset focus for next comment.
            $('#message').val('').focus();
        });
    });
});
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}