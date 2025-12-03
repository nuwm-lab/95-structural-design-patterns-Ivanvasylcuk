using System;
using System.Text;

namespace AdapterPatternDemo
{
    // 1. TARGET: Інтерфейс, з яким звикла працювати наша система.
    // Наша програма очікує отримати просту текстову команду.
    public interface ITextCommandSource
    {
        string GetTextCommand();
    }

    // 2. ADAPTEE (Адаптований): "Стороння" складна система розпізнавання голосу.
    // Вона має специфічні методи, які не підходять під наш інтерфейс ITextCommandSource.
    public class VoiceSystem
    {
        public void InitializeMicrophone()
        {
            Console.WriteLine("[VoiceSystem] Мікрофон увімкнено.");
        }

        // Цей метод повертає не готовий рядок, а, наприклад, складний об'єкт або байти
        // Для прикладу симулюємо, що він повертає "розпізнаний" байтовий масив або код
        public string ListenAndRecognize()
        {
            Console.WriteLine("[VoiceSystem] Слухаю голос...");
            // Симуляція розпізнавання
            return "Відкрити YouTube"; 
        }
    }

    // 3. ADAPTER: Перетворює інтерфейс VoiceSystem у зрозумілий для нас ITextCommandSource.
    public class VoiceToTextAdapter : ITextCommandSource
    {
        private readonly VoiceSystem _voiceSystem;

        // Адаптер приймає екземпляр адаптованого класу через конструктор (Dependency Injection)
        public VoiceToTextAdapter(VoiceSystem voiceSystem)
        {
            _voiceSystem = voiceSystem;
        }

        // Реалізація методу Target-інтерфейсу
        public string GetTextCommand()
        {
            // 1. Адаптер знає, як керувати складною голосовою системою
            _voiceSystem.InitializeMicrophone();
            
            // 2. Отримуємо дані від голосової системи
            string recognizedSpeech = _voiceSystem.ListenAndRecognize();

            // 3. Тут може бути логіка перетворення (наприклад, переклад, форматування)
            // У нашому випадку ми просто повертаємо результат як текст
            Console.WriteLine($"[Adapter] Голос перетворено у текст: '{recognizedSpeech}'");
            
            return recognizedSpeech;
        }
    }

    // 4. CLIENT: Система, яка працює тільки з текстом (наприклад, чат-бот)
    public class SmartBot
    {
        public void ProcessCommand(ITextCommandSource commandSource)
        {
            Console.WriteLine("\n--- Бот очікує команду ---");
            
            // Клієнт не знає, що це голос. Він просто викликає GetTextCommand()
            string command = commandSource.GetTextCommand();

            Console.WriteLine($"BOT: Виконую команду: >> {command.ToUpper()} <<");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Сценарій 1: Робота зі звичайним текстовим джерелом (для порівняння)
            // (тут міг би бути клас KeyboardInput, що реалізує ITextCommandSource)

            // Сценарій 2: Підключення голосового управління через АДАПТЕР
            
            // Створюємо "несумісну" голосову систему
            VoiceSystem foreignVoiceSystem = new VoiceSystem();

            // Огортаємо її в адаптер
            ITextCommandSource adapter = new VoiceToTextAdapter(foreignVoiceSystem);

            // Клієнтський код використовує адаптер так, ніби це звичайне текстове джерело
            SmartBot myBot = new SmartBot();
            myBot.ProcessCommand(adapter);

            Console.ReadLine();
        }
    }
}