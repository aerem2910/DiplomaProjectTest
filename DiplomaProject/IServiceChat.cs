using System.ServiceModel;
using System.Runtime.Serialization;

namespace DiplomProject
{
    [ServiceContract(CallbackContract = typeof(IServerChatCallback))]
    public interface IServiceChat
    {
        [OperationContract]
        int Connect(string name);

        [OperationContract]
        void Disconnect(int id);

        [OperationContract(IsOneWay = true)]
        void SendMessage(string message, int id);

        [OperationContract(IsOneWay = true)]
        void SendImage(byte[] imageData, string imageName, int id);

        [OperationContract]
        byte[] GetImage(int index);
    }

    [DataContract]
    public class ImageMessage
    {
        [DataMember]
        public string ImageName { get; set; }

        [DataMember]
        public byte[] ImageData { get; set; }
    }

    public interface IServerChatCallback
    {
        [OperationContract(IsInitiating = true)]
        void MessageCallback(string message);

        [OperationContract(IsOneWay = true)]
        void ImageCallback(ImageMessage imageMessage, int id, string senderName);
    }
}
