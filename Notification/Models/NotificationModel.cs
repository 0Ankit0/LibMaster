using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseNotification.Models
{
    public class NotificationModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [DataType(DataType.Text)]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [DataType(DataType.MultilineText)]
        public string? Body { get; set; }

        [DataType(DataType.Url)]
        [Url(ErrorMessage = "Url must be a valid URL.")]
        public string? ImageUrl { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Device token is required.")]
        public string? Token { get; set; }
    }
    public class MultiDeviceNotificationModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [DataType(DataType.Text)]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [DataType(DataType.MultilineText)]
        public string? Body { get; set; }

        [DataType(DataType.Url)]
        [Url(ErrorMessage = "Url must be a valid URL.")]
        public string? ImageUrl { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Device tokens are required.")]
        public List<string> Tokens { get; set; } = new List<string>();
    }
}
