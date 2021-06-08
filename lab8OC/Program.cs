
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


//Вариант №1.
//Напишите программу, в которой два потока помещают произвольные числа в конец очереди, а 
//два других потока извлекают числа из очереди и выводят их на экран.Очередь реализовать на основе массива.


class Example
{
    public static Random rnd = new Random();
    public static Thread t1, t2, t3, t4;
    public static MyQueue myQueue = new MyQueue();

    public static void DoWork1(string Источник)           //добавляет
    {

        for (int i = 0; i < 10; i++)
        {

            Thread.Sleep(10);
            lock (typeof(Example))
            {
                var number = rnd.Next(1, 1000);
                myQueue.Enqueue(number);
                Console.WriteLine($"{Источник}: " + number + "Добавил");
                Thread.Sleep(10);
            }
        }
    }
    public static void Dowork2(string Источник)      //извлекает
    {

        for (int i = 0; i < 10; i++)
        {

            Thread.Sleep(10);

            lock (typeof(Example))
            {
                if (myQueue.Count != 0)
                {
                    Console.WriteLine($"{Источник}: " + myQueue.Dequeue() + "Извлек");
                    
                    Thread.Sleep(10);
                }
            }
        }
    }
    public static void thread1()
    {
        DoWork1("Поток 1");
    }

    public static void thread2()
    {
        DoWork1("Поток 2");
    }

    public static void thread3()
    {
        Dowork2("Поток 3");
    }

    public static void thread4()
    {
        Dowork2("Поток 4");
    }

    public static void Main()

    {
        t1 = new Thread(thread1);
        t2 = new Thread(thread2);
        t3 = new Thread(thread3);
        t4 = new Thread(thread4);

        t1.Start();
        t2.Start();
        t3.Start();
        t4.Start();

        Console.ReadLine();
    }
    public class MyQueue
    {
        // Массив с элементами
        private int[] array;
        // Индекс начального элемента.
        private int head;
        // Индекс конечного элемента.
        private int tail;


        // Создаём очередь. Начальная ёмкость - 4;  
        public MyQueue()
        {
            array = new int[4];
        }


        // Количество элементов в очереди.

        public int Count { get; private set; }

        // Извлечение элемента из очереди.

        public int Dequeue()
        {
            // Проверяем, можно ли что-либо достать из очереди.
            if (Count == 0)
                throw new InvalidOperationException();
            // Достаём первый элемент.
            int local = array[head];
            // Обнуляем первый элемент.
            array[head] = 0;
            // Изменяем индекс начала элементов в массиве.
            head = (head + 1) % array.Length;
            // Убавляем количество элементов.
            Count--;
            return local;
        }


        // Добавление элемента в очередь.

        public void Enqueue(int item)
        {
            // Проверяем ёмкость массива, если недостаточна - удваиваем.
            if (Count == array.Length)
            {
                var capacity = array.Length * 2;
                SetCapacity(capacity);
            }
            // Устанавливаем последний элемент.
            array[tail] = item;
            // Изменяем индекс конца массива.
            tail = (tail + 1) % array.Length;
            // Прибавляем количество элементов.
            Count++;
        }

        // Изменение ёмкости очереди.
        private void SetCapacity(int capacity)
        {
            // Новый массив заданного объёма.
            int[] destinationArray = new int[capacity];
            if (Count > 0)
            {
                // Копируем старый массив в новый.
                if (head < tail)
                    Array.Copy(array, head, destinationArray, 0, Count);
                else
                {
                    Array.Copy(array, head, destinationArray, 0, array.Length - head);
                    Array.Copy(array, 0, destinationArray, array.Length - head, tail);
                }
            }
            array = destinationArray;
            // Новые значения индексов начала и конца массива.
            head = 0;
            if (Count == capacity)
                tail = 0;
            else
                tail = Count;
        }
    }
}
