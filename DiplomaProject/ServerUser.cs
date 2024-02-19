using System.ServiceModel;

namespace DiplomProject
{
    public class ServerUser
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public OperationContext OperationContext { get; set; }

        public ServerUser(int id, string name, OperationContext operationContext)
        {
            ID = id;
            Name = name;
            OperationContext = operationContext;
        }
    }
}
