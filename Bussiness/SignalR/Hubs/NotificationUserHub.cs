using DataAccess;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bussiness.SignalR.Hubs
{
    public class NotificationUserHub : Hub
    {
        private readonly SecurityDataWork _securityDatawork;

        private readonly IUserConnectionManager _userConnectionManager;
        public NotificationUserHub(IUserConnectionManager userConnectionManager,
          SecurityDbContext securityDbContext)
        {
            _securityDatawork = new SecurityDataWork(securityDbContext);
            _userConnectionManager = userConnectionManager;
        }
        public string GetConnectionId()
        {

            var userId = HttpAccessorService.GetLoggeInUser_Id;
            _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);

            return Context.ConnectionId;
        }

        public string GetCurrentUnreadNotifications()
        {
            var userId = HttpAccessorService.GetLoggeInUser_Id;
            if (userId != null)
                return _securityDatawork.Notifications
                    .Count(x => x.ApplicationUserId == userId && x.IsSeen == false)
                    .ToString();

            return "0";
        }

        public async Task<string> GetDropdownNotifications()
        {
            string CreateHtml(string body, int id, bool isSeen) =>
                "<li>" +
                    "<ul class='menu'>" +
                        "<li>" +
                            $"<a href='#' class='NotificationDropdownRow' id='NotificationId_{id}'>" +
                                $"<i class='NotificationDropdownRowTitle'>{body + "..."}</i>" +
                                $"<i class='fa {(isSeen ? "fa-eye-slash" : "fa-eye")} text-aqua NotificationDropdownRowEye'  id='NotificationEyeId_{id}'></i> " +
                                $"<i class='fa fa-trash text-danger NotificationDropdownRowTrash'></i>" +
                            "</a>" +
                        "</li>" +
                    "</ul>" +
                "</li>";

            var userId = HttpAccessorService.GetLoggeInUser_Id;
            if (userId != null)
            {
                var notifications = await _securityDatawork.Notifications
                    .GetPaggingWithFilter(
                        x => x.OrderBy(y => y.IsSeen).ThenBy(y => y.CreatedOn),
                        x => x.ApplicationUserId == userId);

                var count = _securityDatawork.Notifications.Count(x => x.ApplicationUserId == userId);
                var response = $"<li class='header'>Έχεις {count} ειδοποιήσεις</li>";

                foreach (var notification in notifications)
                    response += CreateHtml(
                        new string(notification.Title.Take(30).ToArray()),
                        notification.Id,
                        notification.IsSeen);

                response += " <li class=footer'><a href='/Notification/Index'>Προβολή όλων </a></li>";
                return response;
            }

            return "";
        }

        public async Task<string> GetModalNotification(string notificationId)
        {
            string CreateHtml(string title, string description) =>
                "<p>" +
                    $"<b>{title}</b>" +
                "</p>" +
                "&nbsp;" +
                "<p>" +
                    $"{description}" +
                "</p>";

            var userId = HttpAccessorService.GetLoggeInUser_Id;
            if (userId != null)
            {
                var notification = await _securityDatawork.Notifications
                    .FirstOrDefaultAsync(x => x.Id == Int32.Parse(notificationId));

                return CreateHtml(notification.Title, notification.Description);
            }

            return "";
        }

        public async Task<string> ToggleNotificationDropdownRowEye(string notificationId)
        {
            var userId = HttpAccessorService.GetLoggeInUser_Id;
            if (userId != null)
            {
                var notification = await _securityDatawork.Notifications
                    .FirstOrDefaultAsync(x => x.Id == Int32.Parse(notificationId));

                notification.IsSeen = !notification.IsSeen;

                _securityDatawork.Notifications.Update(notification);
                var status = await _securityDatawork.SaveChangesAsync();

                if (status == 1)
                    return "success";
            }

            return "error";
        }

        public async Task<string> SetNotificationIsSeen(string notificationId, string isSeen)
        {
            var userId = HttpAccessorService.GetLoggeInUser_Id;
            if (userId != null)
            {
                var notification = await _securityDatawork.Notifications
                    .FirstOrDefaultAsync(x => x.Id == Int32.Parse(notificationId));

                notification.IsSeen = isSeen == "true";

                _securityDatawork.Notifications.Update(notification);
                var status = await _securityDatawork.SaveChangesAsync();

                if (status == 1)
                    return "success";
            }

            return "error";
        }

        public async Task<string> DeleteNotification(string notificationId)
        {
            var userId = HttpAccessorService.GetLoggeInUser_Id;
            if (userId != null)
            {
                var notification = await _securityDatawork.Notifications
                    .FirstOrDefaultAsync(x => x.Id == Int32.Parse(notificationId));

                _securityDatawork.Notifications.Remove(notification);
                var status = await _securityDatawork.SaveChangesAsync();

                if (status == 1)
                    return "success";
            }

            return "error";
        }

        //Called when a connection with the hub is terminated.
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            //get the connectionId
            var connectionId = Context.ConnectionId;
            _userConnectionManager.RemoveUserConnection(connectionId);
            var value = await Task.FromResult(0);
        }
    }
}
