using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ChatHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(DiplomProject.ServiceChat)))
            {
                try
                {
                    host.Open();
                    Console.WriteLine("!!!Сервис запущен!!!");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в работе сервиса: {ex.Message}");
                }
                finally
                {
                    if (host.State == CommunicationState.Opened)
                    {
                        host.Close();
                    }
                }
            }
        }
    }
}

