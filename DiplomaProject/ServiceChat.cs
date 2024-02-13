using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Drawing;

namespace DiplomProject
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ServiceChat : IServiceChat
    {
        private readonly List<ServerUser> users = new List<ServerUser>();
        private readonly List<byte[]> imageList = new List<byte[]>();

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

        public void SendImage(byte[] imageData, string imageName, int id)
        {
            try
            {
                imageList.Add(imageData);

                foreach (var item in users)
                {
                    var callback = item.OperationContext.GetCallbackChannel<IServerChatCallback>();
                    callback.ImageCallback(new ImageMessage { ImageData = imageData, ImageName = imageName }, id, item.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке изображения: {ex.Message}");
            }
        }

        public byte[] GetImage(int index)
        {
            try
            {
                if (index >= 0 && index < imageList.Count)
                {
                    // Возвращение байтового массива изображения по индексу из списка
                    return imageList[index];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении изображения: {ex.Message}");
                return null;
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
    }
}
