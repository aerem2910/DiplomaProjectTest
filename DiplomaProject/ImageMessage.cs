using System.Runtime.Serialization;

namespace ChatClient
{
    [DataContract]
    public class ImageMessage
    {
        [DataMember]
        public byte[] ImageData { get; set; }

        [DataMember]
        public string ImageName { get; set; }
    }
}
