$(function () {
    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.chatHub;
    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (time, message, userId, toUserId) {
        // Add the message to the page.
        var discussion = $('#discussion');
        if (discussion.data('userid') == userId && discussion.data('currentuser') == toUserId) {
            discussion.prepend('<li class="chat-left">'
                + '<div class="chat-avatar"><img src="https://www.bootdey.com/img/Content/avatar/avatar3.png" alt="Retail Admin"></div>'
                + '<div class="chat-text">' + htmlEncode(message) + '</div>'
                + '<div class="chat-hour">' + time + '<span class="fa fa-check-circle ms-1 ml-1"></span></div>'
                + '<div class="chat-avatar"</div>'
                + '</li>');

            // Scroll to bottom
            discussion.scrollTop(discussion.prop("scrollHeight"))
        } else {
            $('.person .status').addClass('busy');
        }
    };
    $.connection.hub.start().done(function () {
        var connectionId = chat.connection.id;
        $(document).on('submit', '#formMessage', function (e) {
            // Prevent default form submit
            e.preventDefault();

            var form = $(this);
            var actionUrl = form.attr('action');
            var message = $('#message').val();
            var fromUserId = form.data('fromuserid');
            var toUserId = form.data('touserid');
            if (message) {
                // Call the Send method on the hub.
                $.ajax({
                    url: actionUrl,
                    type: 'POST',
                    data: { message, fromUserId, toUserId, connectionId },
                    success: function (response) {
                        if (response.success) {
                            // Add chat message
                            var discussion = $('#discussion');
                            discussion.prepend('<li class="chat-right">'
                                + '<div class="chat-hour">' + response.time + '<span class="fa fa-check-circle ms-1 ml-1"></span></div>'
                                + '<div class="chat-text">' + htmlEncode(message) + '</div>'
                                + '<div class="chat-avatar"</div>'
                                + '</li>');

                            // Scroll to bottom
                            discussion.scrollTop(discussion.prop("scrollHeight"))
                        } else {
                            alert('An error has occured, please try again later!')
                        }
                    }
                });
                // Clear text box and reset focus for next comment.
                $('#message').val('').focus();
            }
        });
    });

    $('.person').click(function () {

        // Remove not seen class
        $('.person .status').removeClass('busy');

        var url = $(this).data('chat');
        $.get(url, function (data) {
            // Populate statistics data
            $('#chatDiv').html(data);

            var form = $('#formMessage');
            var actionUrl = form.data('updatestate');
            var fromUserId = form.data('fromuserid');
            var toUserId = form.data('touserid');
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: { fromUserId, toUserId }
            });
        });
    });
});
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}