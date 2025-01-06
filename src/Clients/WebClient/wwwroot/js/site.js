const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:7040/chatHub?userId=deneme_3") // SignalR hub'ınızın URL'si
    .build();

// Mesaj alındığında
connection.on("ReceiveMessage", async function (user,userName, message) {

    console.log(user)
    console.log(userName)
    console.log(message)

    const messageDiv = $('<div class="message"></div>');

    messageDiv.addClass('message-left'); // Solda göstermek için sınıf ekle
    messageDiv.html(`<span class="message-user">${userName}:</span> ${message}`);

    $('#chat-messages').append(messageDiv); // Mesajı ekle
    $('#chat-messages').scrollTop($('#chat-messages')[0].scrollHeight); // Otomatik kaydırma
});

// Bağlı kullanıcıları al


// Kullanıcıya ait sohbeti aç
function openChat(username, userId) {
    const firstLetter = username.charAt(0).toUpperCase();
    const headerHTML = `
        <div class="chat-header-left">
            <div class="chat-header-avatar">
                ${firstLetter}
            </div>
            <div class="chat-header-info">
                <div id="chattingWithId">${username}</div>
                <div class="chat-header-status">
                    <span class="status-indicator"></span>
                    <span>Çevrimiçi</span>
                </div>
            </div>
        </div>
        <div class="chat-header-actions">
            <button class="header-action-btn" title="Ara">
                <i class="fas fa-search"></i>
            </button>
            <button class="header-action-btn" title="Sesli Arama">
                <i class="fas fa-phone"></i>
            </button>
            <button class="header-action-btn" title="Görüntülü Arama">
                <i class="fas fa-video"></i>
            </button>
            <button class="header-action-btn" title="Diğer">
                <i class="fas fa-ellipsis-v"></i>
            </button>
        </div>
    `;

    $('#chat-header').html(headerHTML);
    $('#chat-messages').empty();
    $('#message-input').show();
}

// Mesaj gönderme
$('#send-message').on('click', function () {
    const message = $('#message').val();
    const userId = $('#chattingWithId').text();

    if (message) {

        console.log(message);
        console.log(userId);

        connection.invoke("SendMessage",userId, userId, message).then(function () {

            const messageDiv = $('<div class="message"></div>');

            messageDiv.addClass('message-right'); // Solda göstermek için sınıf ekle
            messageDiv.html(`<span class="message-user">Siz:</span> ${message}`);

            $('#chat-messages').append(messageDiv); // Mesajı ekle
            $('#chat-messages').scrollTop($('#chat-messages')[0].scrollHeight); // Otomatik kaydırma

            $('#message').val(''); // Mesajı gönderince input'u temizle
        }).catch(function (err) {
            console.error("Mesaj gönderme hatası: ", err);
        });
    }
});


// SignalR bağlantısını başlat
connection.start().then(async function () {

}).catch(function (err) {
    console.error("SignalR bağlantısı başarısız: ", err);
});

connection.on("GetConnectedUsers", async function (users) {
    var obtainedId = await connection.invoke("GetObtainedClientId");

    const userList = $('#user-list');
    userList.empty();
    
    $.each(users, function (userId, username) {
        if (obtainedId != username) {
            // Kullanıcı avatar harfini oluştur

            
            const firstLetter = username.charAt(0).toUpperCase();
            
            // Modern kullanıcı öğesi oluştur
            const userItem = $(`
                <div class="user-list-item">
                    <div class="user-avatar">
                        ${firstLetter}
                    </div>
                    <div class="user-info">
                        <div class="user-name">${userId}</div>
                        <div class="user-status">Çevrimiçi</div>
                    </div>
                </div>
            `);
            
            // Veri özelliklerini ekle
            userItem.attr('data-userid', userId);
            userItem.attr('data-username', username);
            
            userList.append(userItem);
        }
    });
});

// Click olayını güncelle
$('#user-list').on('click', '.user-list-item', function () {
    // Önceki aktif kullanıcıyı temizle
    $('.user-list-item').removeClass('active');
    // Tıklanan kullanıcıyı aktif yap
    $(this).addClass('active');

    const userId = $(this).data('userid');
    const username = $(this).data('username');

    openChat(userId, username);
});