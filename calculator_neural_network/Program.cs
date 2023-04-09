// See https://aka.ms/new-console-template for more information

using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;


Neuron neuron = new Neuron();
Start:

if (neuron.isFail)
{
    create(neuron);
}
else
{
    Console.WriteLine("Виды конверторов");
    Console.WriteLine($"0: создать новый");
    neuron.showInhoFail();
    Console.WriteLine("Выберите конвертор:");
    int i = Convert.ToInt32(Console.ReadLine());
    if (i == 0)
        create(neuron);
    else
        neuron.Open(i);

}
Back:
Console.WriteLine("Введите число которое нужно говертировать");
decimal input = Convert.ToDecimal(Console.ReadLine());

Console.WriteLine($"Ответ: {neuron.ProcessInputData(input)}");

Console.WriteLine("Чтоб ввести еще одно число нажмите Enter, чтобы выйти напишите  end");
if(Console.ReadLine() != "end")
{
    goto Back;
}
else
{
    goto Start;
}

/*decimal kg = 1;
decimal g = 1000;

decimal kr_1 = 3.5m;
decimal kr_2 = 3;
decimal kr_3 = 7.548m;
decimal kr_4 = 1;
decimal g_1 = 500;


neuron.Train(kg, g);
   


Console.WriteLine("\nОбучение завершено!");
Console.WriteLine($"weight = {neuron.weight}!");

Console.WriteLine($"В {kr_1} кг {neuron.ProcessInputData(kr_1)} грамм" );
Console.WriteLine($"В {kr_2} кг {neuron.ProcessInputData(kr_2)} грамм" );
Console.WriteLine($"В {kr_3} кг {neuron.ProcessInputData(kr_3)} грамм" );
Console.WriteLine($"В {kr_4} кг {neuron.ProcessInputData(kr_4)} грамм" );
Console.WriteLine($"В {g_1} грамм {neuron.RestoreInputData(g_1)} кг" );*/





void create(Neuron neuron)
{
    Console.WriteLine("Введите название калькулятора");
    string name = Console.ReadLine().Replace('/','_');
    Console.WriteLine("Введите тестовые значения");
    Console.WriteLine("Введите значение");
    decimal i = Convert.ToDecimal(Console.ReadLine());
    Console.WriteLine("Введите, во что должно конвертироваться:");
    decimal e = Convert.ToDecimal(Console.ReadLine());

    Console.WriteLine("Начало обучения...");
    neuron.Train(i, e);
    Console.WriteLine("\nОбучение завершено!");
    neuron.Save(name, i, e);
    Console.WriteLine("Обучение завершено и сохранино");

}

public class Neuron
{
    private const string path = "bd_neuron.txt";
    public decimal weight { get; private set; } = 0.0002m;
    public decimal LastError { get; private set; }
    public decimal Smoothing { get; set; } = 0.00001m;
    public bool isFail = !File.Exists(path);
    

    public decimal ProcessInputData(decimal input)
    {
        return input * weight;
    }
    public decimal RestoreInputData(decimal output)
    {
        return output / weight;
    }

    public void Train (decimal input, decimal expextedResult)
    {
        long i = 0;
        do
        {
            var actualResult = ProcessInputData(input);
            LastError = expextedResult - actualResult;
            var correction = (LastError / actualResult) * Smoothing;
            weight += correction;

            if (i % 10000000 == 1)
                Console.WriteLine($"Итерация {i}, \tОшибка равна {LastError}");

            i++;
        } while (LastError > Smoothing || LastError < -Smoothing);


        
        //Console.WriteLine($"weight = {weight}!");
    }
    public void Save(string name, decimal i, decimal e)
    {


        string textSave = $"{name}/{weight}/{i}/{e}/{Smoothing}\n";
        File.AppendAllText(path, textSave);
        Console.WriteLine("Saved");
    }

    public void Open(int iteration)
    {
        string all = File.ReadAllText(path, Encoding.UTF8);
        string[] arrWeight = all.Split('\n');

        weight = Convert.ToDecimal(arrWeight[iteration-1].Split('/')[1]);
        decimal i = Convert.ToDecimal(arrWeight[iteration-1].Split('/')[2]);
        decimal e = Convert.ToDecimal(arrWeight[iteration-1].Split('/')[3]);
        Smoothing = Convert.ToDecimal(arrWeight[iteration-1].Split('/')[4]);
        Console.WriteLine("Проводим повтороное тестирование...");
        Train(i, e);
        Console.WriteLine("Повтороное тестирование закончино!");

    }
    public void showInhoFail()
    {
        string all = File.ReadAllText(path, Encoding.UTF8);
        string[] arrWeight = all.Split('\n');

        for (int i = 0; i < arrWeight.Length - 1; i++)
            Console.Write(i + 1 + ": " + arrWeight[i].Split('/')[0] + "\n");
    }
}









