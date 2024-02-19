using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace DiplomProject
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ServiceChat : IServiceChat
    {
        private readonly List<ServerUser> users = new List<ServerUser>();
        private int nextId = 1;

        public int Connect(string name)
        {
            var operationContext = OperationContext.Current;
            if (operationContext == null)
            {
                return -1;
            }

            var user = new ServerUser(nextId, name, operationContext);
            nextId++;
            SendMessage($"{user.Name} присоединился к беседе", 0);
            users.Add(user);
            return user.ID;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if (user != null)
            {
                users.Remove(user);
                SendMessage($"{user.Name} покинул беседу", 0);
            }
        }
        /*
        public void SendImage(ImageMessage imageMessage, int id)
        {
            foreach (var item in users.ToList())
            {
                try
                {
            //        var callback = item.OperationContext.GetCallbackChannel<IServerChatCallback>();
           //        callback.ImageCallback(imageMessage, id, item.Name);
             / /  }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка отправки изображения пользователю {item.Name}: {ex.Message}");
                }
            }
        }
        */

        /*public void SendImage(byte[] imageData, string imageName, int id)
         {

             foreach (var item in users.ToList())
             {
                 try
                 {
                     var callback = item.OperationContext.GetCallbackChannel<IServerChatCallback>();
                     var imageMessage = new ImageMessage
                     {
                         ImageName = imageName,
                         ImageData = imageData
                     };
                    // callback.ImageCallback(imageMessage, id, item.Name);
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine($"Ошибка отправки изображения пользователю {item.Name}: {ex.Message}");
                 }
             }

             // Добавьте вызов метода для отображения изображения в интерфейсе
            // DisplayImage(imageData, imageName);
         }*/

        public void SendImage(byte[] imageData, string imageName, int id)
        {
            foreach (var item in users.ToList())
            {
                try
                {
                    var callback = item.OperationContext.GetCallbackChannel<IServerChatCallback>();
                    var imageMessage = new ImageMessage
                    {
                        ImageName = imageName,
                        ImageData = imageData
                    };
                    callback.ImageCallback(imageMessage, id, item.Name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка отправки изображения пользователю {item.Name}: {ex.Message}");
                }
            }
        }



        public void SendMessage(string message, int id)
        {
            foreach (var item in users.ToList())
            {
                string answer = $"{DateTime.Now.ToShortTimeString()}";
                var user = users.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    answer += $": {user.Name} ";
                }
                answer += message;

                try
                {
                    var callback = item.OperationContext.GetCallbackChannel<IServerChatCallback>();
                    callback.MessageCallback(answer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending message to user {item.Name}: {ex.Message}");
                }
            }
        }
        /*public void SendMessage(string message, int id)
        {
            foreach (var item in users.ToList())
            {
                string answer = $"{DateTime.Now.ToShortTimeString()}";
                var user = users.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    answer += $": {user.Name} ";
                }
                answer += message;

                try
                {
                    var callback = item.OperationContext.GetCallbackChannel<IServerChatCallback>();
                    callback.MessageCallback(answer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending message to user {item.Name}: {ex.Message}");
                }
            }
        }
        */
        public void ImageCallback(ImageMessage imageMessage, int id, string senderName)
        {
            foreach (var item in users.ToList())
            {
                try
                {
                    var callback = item.OperationContext.GetCallbackChannel<IServerChatCallback>();
                    callback.ImageCallback(imageMessage, id, senderName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending image callback to user {item.Name}: {ex.Message}");
                }
            }
            DisplayImage(imageMessage.ImageData, imageMessage.ImageName);
        }

       


        
        public void DisplayImage(byte[] imageData, string imageName)
        {
            foreach (var item in users.ToList())
            {
                try
                {
                    var callback = item.OperationContext.GetCallbackChannel<IServerChatCallback>();
                    callback.DisplayImage(imageData, imageName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error displaying image to user {item.Name}: {ex.Message}");
                }
            }
        }
    }
}
