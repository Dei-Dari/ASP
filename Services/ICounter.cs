using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Services
{
    public interface ICounter
    {
        int Value { get; }
    }
    // класс, реалующий интерфейс
    public class RandomCounter : ICounter
    {
        int value;
        //генерация числа
        static Random rnd = new Random();
        // конструктор
        public RandomCounter()
        {
            value = rnd.Next(0, 1000000);
        }
        // свойство возвращает значение, интерфейс
        public int Value { get { return value; } }
    }
    // класс для управления жизненным циклом
    public class CounterService
    {
        // реализация объекта не известна на момент определения класса. некоторая сущность, которая применяет данный интерфейс
        public ICounter Counter { get; }
        public CounterService(ICounter counter)
        {
            Counter = counter;
        }
    }
}
