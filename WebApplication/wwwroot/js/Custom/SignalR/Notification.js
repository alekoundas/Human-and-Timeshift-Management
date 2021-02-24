"use strict";
if (LoggedInUserId !== "") {

    var connection = new signalR.HubConnectionBuilder().withUrl('/NotificationUserHub?userId=' + LoggedInUserId).build();
    connection.start()
        .catch(err => console.error(err.toString()))
        .then(() => {
            //connection.invoke('GetConnectionId').then((connectionId) => {})
            connection.invoke('GetCurrentUnreadNotifications')
                .then(unreadCount => SetNotificationBadge(unreadCount))
                .catch(err => console.error(err.toString()));
        });


    connection.on('NotifyUser', () => {
        connection
            .invoke('GetCurrentUnreadNotifications')
            .then(unreadCount => SetNotificationBadge(unreadCount))
            .catch(err => console.error(err.toString()));

        PlayAudioService("Notification");
    });

    connection.on('CheckNotificationBadgeCount', () => {
        connection
            .invoke('GetCurrentUnreadNotifications')
            .then(unreadCount => SetNotificationBadge(unreadCount))
            .catch(err => console.error(err.toString()));
    });

    $(document).on('click', '#Notification_Dropdown_Bell', () => {
        connection.invoke('GetDropdownNotifications')
            .then(result => SetNotificationInDropdown(result))
            .catch(err => console.error(err.toString()));
    })

    //Row actions

    //Open modal
    $(document).on('click', '.NotificationDropdownRowTitle', (e) => {
        document.getElementById('Notification_Modal_Body').innerHTML = "";

        connection.invoke('GetModalNotification', e.target.parentNode.id.split('_')[1])
            .then(result => SetNotificationInModal(result))
            .catch(err => console.error(err.toString()));

        connection.invoke('SetNotificationIsSeen', e.target.parentNode.id.split('_')[1], "true")
            .catch(err => console.error(err.toString()));

        connection.invoke('GetCurrentUnreadNotifications')
            .then(unreadCount => SetNotificationBadge(unreadCount))
            .catch(err => console.error(err.toString()));
    })

    //Toggle seen value
    $(document).on('click', '.NotificationDropdownRowEye', (e) => {
        connection.invoke('ToggleNotificationDropdownRowEye', e.target.parentNode.id.split('_')[1])
            .then(result => SetNotificationEyeInDropdown(result))
            .catch(err => console.error(err.toString()));
    })

    //Delete notification
    $(document).on('click', '.NotificationDropdownRowTrash', (e) => {
        connection.invoke('DeleteNotification', e.target.parentNode.id.split('_')[1])
            .catch(err => console.error(err.toString()));
    })

    //$('#NotificationModal').on('click', '#Notification_Delete', (e) => {

    //    console.log("delete");
    //    connection.invoke('DeleteNotification', e.target.id.split('_')[1])
    //        .catch(err => console.error(err.toString()));
    //})
}


const SetNotificationBadge = (value) => {
    if (+value > 0) {

        document.getElementById('Notification_Badge').innerHTML = value;
        document.getElementById('Notification_Badge').style.display = '';
    }
    else {
        document.getElementById('Notification_Badge').innerHTML = '';
        document.getElementById('Notification_Badge').style.display = 'none';

    }
}

const SetNotificationInDropdown = (value) => {
    document.getElementById('AppendNotificationHTML').innerHTML = value;
}

const SetNotificationEyeInDropdown = (isSeen, notificationId) => {
    var classToRemove = "";
    var classToAdd = "";
    if (isSeen) {
        classToAdd = "fa-eye-slash";
        classToRemove = "fa-eye";
    }
    else {
        classToAdd = "fa-eye";
        classToRemove = "fa-eye-slash";
    }

    document.getElementById('NotificationEyeId_' + notificationId)
        .classList.remove(classToRemove);

    document.getElementById('NotificationEyeId_' + notificationId)
        .classList.add(classToAdd);
}


const SetNotificationInModal = (value) => {
    $('#NotificationModal').modal('show')
    document.getElementById('Notification_Modal_Body').innerHTML = value;
}
