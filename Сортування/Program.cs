using System;
using System.Diagnostics;
using System.Linq;

class SortingComparison
{
    static void Main(string[] args)
    {
        // Розміри масивів
        int[] sizes = { 10, 1000, 10000, 100000 };
        string[] algorithms = { "Сортування вставками", "Бульбашкове сортування", "Швидке сортування", "Злиття", "Підрахунком", "Радикс-сортування" };

        Console.WriteLine($"{"Алгоритм",-25} | {"10",-10} | {"1000",-10} | {"10000",-10} | {"100000",-10}");
        Console.WriteLine(new string('-', 70));

        foreach (var algorithm in algorithms)
        {
            Console.Write($"{algorithm,-25} |");
            foreach (var size in sizes)
            {
                // Генерація масиву
                int[] array = GenerateRandomArray(size, -10000, 10000);

                // Повторне сортування для більш точного вимірювання
                int repetitions = size < 10000 ? 100 : 10;
                long totalTime = 0;

                for (int i = 0; i < repetitions; i++)
                {
                    int[] copy = (int[])array.Clone();
                    var stopwatch = Stopwatch.StartNew();
                    Sort(copy, algorithm);
                    stopwatch.Stop();
                    totalTime += stopwatch.ElapsedMilliseconds;
                }

                // Виведення середнього часу
                Console.Write($" {totalTime / repetitions,-9}мс |");
            }
            Console.WriteLine();
        }
    }

    static int[] GenerateRandomArray(int size, int minValue, int maxValue)
    {
        Random rand = new Random();
        return Enumerable.Range(0, size).Select(_ => rand.Next(minValue, maxValue)).ToArray();
    }

    static void Sort(int[] array, string algorithm)
    {
        switch (algorithm)
        {
            case "Сортування вставками":
                InsertionSort(array);
                break;
            case "Бульбашкове сортування":
                BubbleSort(array);
                break;
            case "Швидке сортування":
                QuickSort(array, 0, array.Length - 1);
                break;
            case "Злиття":
                MergeSort(array, 0, array.Length - 1);
                break;
            case "Підрахунком":
                CountingSort(array);
                break;
            case "Радикс-сортування":
                RadixSort(array);
                break;
        }
    }

    static void InsertionSort(int[] array)
    {
        for (int i = 1; i < array.Length; i++)
        {
            int key = array[i];
            int j = i - 1;
            while (j >= 0 && array[j] > key)
            {
                array[j + 1] = array[j];
                j--;
            }
            array[j + 1] = key;
        }
    }

    static void BubbleSort(int[] array)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = 0; j < array.Length - i - 1; j++)
            {
                if (array[j] > array[j + 1])
                {
                    int temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
        }
    }

    static void QuickSort(int[] array, int left, int right)
    {
        if (left < right)
        {
            int pivot = Partition(array, left, right);
            QuickSort(array, left, pivot - 1);
            QuickSort(array, pivot + 1, right);
        }
    }

    static int Partition(int[] array, int left, int right)
    {
        int pivot = array[right];
        int i = left - 1;
        for (int j = left; j < right; j++)
        {
            if (array[j] < pivot)
            {
                i++;
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
        (array[i + 1], array[right]) = (array[right], array[i + 1]);
        return i + 1;
    }

    static void MergeSort(int[] array, int left, int right)
    {
        if (left < right)
        {
            int mid = left + (right - left) / 2;
            MergeSort(array, left, mid);
            MergeSort(array, mid + 1, right);
            Merge(array, left, mid, right);
        }
    }

    static void Merge(int[] array, int left, int mid, int right)
    {
        int n1 = mid - left + 1;
        int n2 = right - mid;

        int[] leftArray = new int[n1];
        int[] rightArray = new int[n2];

        Array.Copy(array, left, leftArray, 0, n1);
        Array.Copy(array, mid + 1, rightArray, 0, n2);

        int i = 0, j = 0, k = left;

        while (i < n1 && j < n2)
        {
            if (leftArray[i] <= rightArray[j])
            {
                array[k] = leftArray[i];
                i++;
            }
            else
            {
                array[k] = rightArray[j];
                j++;
            }
            k++;
        }

        while (i < n1)
        {
            array[k] = leftArray[i];
            i++;
            k++;
        }

        while (j < n2)
        {
            array[k] = rightArray[j];
            j++;
            k++;
        }
    }

    static void CountingSort(int[] array)
    {
        int max = array.Max();
        int min = array.Min();
        int range = max - min + 1;

        int[] count = new int[range];
        int[] output = new int[array.Length];

        for (int i = 0; i < array.Length; i++)
            count[array[i] - min]++;

        for (int i = 1; i < count.Length; i++)
            count[i] += count[i - 1];

        for (int i = array.Length - 1; i >= 0; i--)
        {
            output[count[array[i] - min] - 1] = array[i];
            count[array[i] - min]--;
        }

        for (int i = 0; i < array.Length; i++)
            array[i] = output[i];
    }

    static void RadixSort(int[] array)
    {
        // Знаходимо мінімальне значення для зсуву
        int minValue = array.Min();
        if (minValue < 0)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] -= minValue;
            }
        }

        // Знаходимо максимальне значення
        int max = array.Max();

        // Сортуємо за кожним розрядом
        for (int exp = 1; max / exp > 0; exp *= 10)
        {
            CountingSortByDigit(array, exp);
        }

        // Якщо був зсув, повертаємо значення назад
        if (minValue < 0)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] += minValue;
            }
        }
    }

    static void CountingSortByDigit(int[] array, int exp)
    {
        int[] output = new int[array.Length];
        int[] count = new int[10]; // Масив для зберігання підрахунків (0-9)

        // Підраховуємо кількість елементів для кожної цифри
        for (int i = 0; i < array.Length; i++)
        {
            int digit = (array[i] / exp) % 10;
            count[digit]++;
        }

        // Накопичуємо підрахунки
        for (int i = 1; i < 10; i++)
        {
            count[i] += count[i - 1];
        }

        // Формуємо відсортований масив
        for (int i = array.Length - 1; i >= 0; i--)
        {
            int digit = (array[i] / exp) % 10;
            output[count[digit] - 1] = array[i];
            count[digit]--;
        }

        // Копіюємо відсортований масив назад
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = output[i];
        }
    }
}
