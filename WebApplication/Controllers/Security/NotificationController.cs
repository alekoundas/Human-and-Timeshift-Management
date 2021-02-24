using Bussiness;
using Bussiness.SignalR;
using Bussiness.SignalR.Hubs;
using DataAccess;
using DataAccess.Models.Security;
using DataAccess.Repository.Security.Interface;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication.Controllers.Security
{
    public class NotificationController : Controller
    {
        private readonly ISecurityDatawork _datawork;
        private readonly SecurityDataWork _securityDataWork;
        private readonly IHubContext<ConnectionHub> _notificationHubContext;
        private readonly IHubContext<NotificationUserHub> _notificationUserHubContext;
        private readonly IUserConnectionManager _userConnectionManager;
        public NotificationController(
            SecurityDbContext dbContext,
            SecurityDbContext SecurityDbContext,
            IHubContext<ConnectionHub> notificationHubContext,
            IHubContext<NotificationUserHub> notificationUserHubContext,
            IUserConnectionManager userConnectionManager)
        {
            _datawork = new SecurityDataWork(dbContext);
            _securityDataWork = new SecurityDataWork(SecurityDbContext);
            _notificationHubContext = notificationHubContext;
            _notificationUserHubContext = notificationUserHubContext;
            _userConnectionManager = userConnectionManager;
        }

        [Authorize(Roles = "Notification_View")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Σύνολο ειδοποιήσεων";
            return View();
        }

        [Authorize(Roles = "Notification_Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Προσθήκη ειδοποίησης";
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Notification_Create")]
        public async Task<IActionResult> Create(NotificationCreate notification)
        {
            if (ModelState.IsValid)
            {
                var userIds = new List<string>();
                if (notification.IsSendEveryone)
                    userIds = await _securityDataWork.ApplicationUsers.GetAllIds();
                else
                    userIds = notification.UserIds;

                foreach (var id in userIds)
                    _securityDataWork.Notifications.Add(new Notification
                    {
                        ApplicationUserId = id,
                        Title = notification.Title,
                        Description = notification.Description,
                        IsSeen = false,
                        CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                        CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                        CreatedOn = DateTime.Now
                    });


                var status = await _securityDataWork.SaveChangesAsync();
                if (status > 0)
                    TempData["StatusMessage"] = "H ειδοποίηση " +
                        notification.Title +
                    " δημιουργήθηκε και στάλθηκε με επιτυχία";
                else
                    TempData["StatusMessage"] = "Ωχ! Δεν έγινε προσθήκη νέων εγγραφών.";

                //send push notification to every id(if loggedin)
                foreach (var id in userIds)
                {
                    var connections = _userConnectionManager.GetUserConnections(id);
                    if (connections != null)
                        foreach (var connectionId in connections)
                            await _notificationUserHubContext.Clients.Client(connectionId)
                                        .SendAsync("NotifyUser");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }
    }
}
