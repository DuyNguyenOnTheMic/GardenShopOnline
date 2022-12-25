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
        $(document).on('submit', '#formMessage', function (e) {
            // Prevent default form submit
            e.preventDefault();

            var form = $(this);
            var actionUrl = form.attr('action');
            var message = $('#message').val();
            var fromUserId = form.data('fromuserid');
            var toUserId = form.data('touserid');
            // Call the Send method on the hub.
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: { message, fromUserId, toUserId },
                success: function (response) {
                    if (response.success) {
                        // Add chat message
                        $('#discussion').prepend('<li class="chat-right">'
                            + '<div class="chat-hour">' + response.time + '<span class="fa fa-check-circle ms-1"></span></div>'
                            + '<div class="chat-text">' + htmlEncode(message) + '</div>'
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

    $('.person').click(function () {
        var url = $(this).data('chat');
        $.get(url, function (data) {
            // Populate statistics data
            $('#chatDiv').html(data);
        });
    });
});
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}