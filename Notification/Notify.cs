using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using FirebaseNotification.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirebaseNotification
{
    public class Notify
    {
        public Notify()
        {
            // Initialize Firebase app with credentials from Secret.json
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("Secret.json")
            });
        }

        // Method to send notification to a single device
        public Task<string> SendNotification(NotificationModel notificationModel)
        {
            var message = new Message
            {
                Notification = new Notification
                {
                    Title = notificationModel.Title, // Notification title
                    Body = notificationModel.Body,   // Notification body
                    ImageUrl = notificationModel.ImageUrl // Notification image URL
                },
                Token = notificationModel.Token, // Device token
                Android = new AndroidConfig // Android push notification configuration
                {
                    Priority = Priority.High, // Set high priority for Android notifications
                    Notification = new AndroidNotification
                    {
                        Icon = "ic_notification", // Android notification icon
                        Color = "#f45342" // Android notification color
                    }
                },
                Apns = new ApnsConfig // Apple push notification configuration
                {
                    Aps = new Aps 
                    {
                        Alert = new ApsAlert
                        {
                            Title = notificationModel.Title, // iOS notification title
                            Body = notificationModel.Body // iOS notification body
                        },
                        Badge = 1, // iOS notification badge
                        Sound = "default" // iOS notification sound
                    }
                },
                Webpush = new WebpushConfig // Web push configuration
                {
                    Notification = new WebpushNotification
                    {
                        Icon = "https://www.example.com/icon.png", // Web push notification icon
                        Image = notificationModel.ImageUrl, // Web push notification image
                        Title = notificationModel.Title, // Web push notification title
                        Body = notificationModel.Body, // Web push notification body
                        Data = new Dictionary<string, string>
                        {
                            { "Date", notificationModel.Date.ToString() } // Include the date in the web push data payload
                        }
                    }
                },
                Data = new Dictionary<string, string>
                {
                    { "Date", notificationModel.Date.ToString() } // Include the date in the data payload
                }
            };

            // Send the message asynchronously and return the result
            return FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        // Method to send notification to multiple devices
        public async Task<BatchResponse> SendBatchNotification(MultiDeviceNotificationModel multiDeviceNotificationModel)
        {
            var message = new MulticastMessage
            {
                Notification = new Notification
                {
                    Title = multiDeviceNotificationModel.Title, // Notification title
                    Body = multiDeviceNotificationModel.Body,   // Notification body
                    ImageUrl = multiDeviceNotificationModel.ImageUrl // Notification image URL
                },
                Tokens = multiDeviceNotificationModel.Tokens, // List of device tokens
                Android = new AndroidConfig // Android push notification configuration
                {
                    Priority = Priority.High, // Set high priority for Android notifications
                    Notification = new AndroidNotification
                    {
                        Icon = "ic_notification", // Android notification icon
                        Color = "#f45342" // Android notification color
                    }
                },
                Apns = new ApnsConfig //Apple push notification configuration
                {
                    Aps = new Aps 
                    {
                        Alert = new ApsAlert
                        {
                            Title = multiDeviceNotificationModel.Title, // iOS notification title
                            Body = multiDeviceNotificationModel.Body // iOS notification body
                        },
                        Badge = 1, // iOS notification badge
                        Sound = "default" // iOS notification sound
                    }
                },
                Webpush = new WebpushConfig // Web push configuration
                {
                    Notification = new WebpushNotification
                    {
                        Icon = "https://www.example.com/icon.png", // Web push notification icon
                        Image = multiDeviceNotificationModel.ImageUrl, // Web push notification image
                        Title = multiDeviceNotificationModel.Title, // Web push notification title
                        Body = multiDeviceNotificationModel.Body, // Web push notification body
                        Data = new Dictionary<string, string>
                        {
                            { "Date", multiDeviceNotificationModel.Date.ToString() } // Include the date in the web push data payload
                        }
                    }
                },
                Data = new Dictionary<string, string>
                {
                    { "Date", multiDeviceNotificationModel.Date.ToString() } // Include the date in the data payload
                }
            };

            // Send the message to multiple devices asynchronously and return the result
            return await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
        }
    }
}
