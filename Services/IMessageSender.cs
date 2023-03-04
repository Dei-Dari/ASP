using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Services
{
    // класс для передачи зависимостей
    class MessageService
    {
        private readonly IMessageSender _sender;

        public MessageService(IMessageSender sender)
        {
            _sender = sender;
        }
        public string SendMessage()
        {
            return _sender.Send();
        }
    }

    // интерфейс сообщений
    public interface IMessageSender
    {
        string Send();
    }

    // реализация интерфейса
    // класс, реализует данный интерфейс, сообщение по почте
    public class EmailMessageSender : IMessageSender
    {
        public string Send()
        {
            return "Message sent by Email";
        }
    }

    public class SmsMessageSender : IMessageSender
    {
        public string Send()
        {
            return "Message sent by SMS";
        }
    }
}
