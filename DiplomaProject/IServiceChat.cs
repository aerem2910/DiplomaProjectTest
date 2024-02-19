using System.ServiceModel;

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

        [OperationContract(IsOneWay = true)]
        void DisplayImage(byte[] imageData, string imageName);
    }

    [ServiceContract]
    public interface IServerChatCallback
    {
        [OperationContract(IsInitiating = true)]
        void MessageCallback(string message);

        [OperationContract(IsOneWay = true)]
        void ImageCallback(ImageMessage imageMessage, int id, string senderName);

        [OperationContract(IsOneWay = true)]
        void DisplayImage(byte[] imageData, string imageName);


        
    }

}
