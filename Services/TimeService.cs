using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Services
{
    public class TimeService
    {
        // возвращает текущую дату/время
        public string GetTime() => DateTime.Now.ToString("G"); //формат времени
    }
}
