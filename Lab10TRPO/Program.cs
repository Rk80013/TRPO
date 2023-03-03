using System.Diagnostics;

class Program
{
    static double T_sequential, T_parallel = 0;
    static void Main(string[] args)
    {
        //Lab10();
        //Lab11();
        //Lab12();
        //Lab13();
    }

    static void Lab13()
    {
        int n = 30000;
        int[] mas = new int[n];
        int[] masTest = new int[n];

        int numTasks = 4;
        int chunkSize = mas.Length / numTasks;

        Random random = new Random();

        for (int i = 0; i < n; i++)
        {
            mas[i] = random.Next(50);
            masTest[i] = mas[i];
        }


        //Console.WriteLine("Оригiнальний масив:");
        //Console.WriteLine(string.Join(", ", arr));
        //Console.WriteLine();
        Stopwatch stopwatch = new();
        stopwatch.Start();
        // Розбиваємо масив на numTasks частин і запускаємо окремий таск для сортування кожної частини
        for (int l = numTasks; l >= 1; l /= 2)
        {
            chunkSize = mas.Length / l;
            Task[] tasks = new Task[l];
            for (int i = 0; i < l; i++)
            {
                int startIndex = i * chunkSize;
                int endIndex = i == l - 1 ? mas.Length : (i + 1) * chunkSize;
                tasks[i] = Task.Factory.StartNew(() => SortSubArray(mas, startIndex, endIndex));
            }

            // Чекаємо на завершення всіх тасків
            Task.WaitAll(tasks);
        }

        stopwatch.Stop();
        Console.WriteLine("Час виконання паралельного сортування в мiлiсекундах: {0}", stopwatch.Elapsed.TotalMilliseconds);

        //Console.WriteLine("Посортований масив:");
        //Console.WriteLine(string.Join(", ", arr));


        stopwatch = new();
        stopwatch.Start();
        bool swapped = true;
        int nTest = mas.Length;
        do
        {
            swapped = false;
            for (int i = 1; i < nTest; i++)
            {
                if (masTest[i - 1] > masTest[i])
                {
                    int temp = masTest[i];
                    masTest[i] = masTest[i - 1];
                    masTest[i - 1] = temp;
                    swapped = true;
                }
            }
            nTest--;
        } while (swapped);

        stopwatch.Stop();

        //Console.WriteLine("\nПосортований масив masTest:");
        //for (int i = 0; i < n; i++)
        //{
        //    Console.Write(" {0}", masTest[i]);
        //}

        Console.WriteLine("Час виконання послiдовного сортування в мiлiсекундах: {0}", stopwatch.Elapsed.TotalMilliseconds);
    }

    static void SortSubArray(int[] arr, int startIndex, int endIndex)
    {
        bool swapped = true;
        while (swapped)
        {
            // Непарна ітерація
            swapped = false;
            for (int i = startIndex + 1; i < endIndex - 1; i += 2)
            {
                if (arr[i] > arr[i + 1])
                {
                    Swap(arr, i, i + 1);
                    swapped = true;
                }
            }

            // Парна ітерація
            for (int i = startIndex; i < endIndex - 1; i += 2)
            {
                if (arr[i] > arr[i + 1])
                {
                    Swap(arr, i, i + 1);
                    swapped = true;
                }
            }
        }
    }

    static void Swap(int[] arr, int i, int j)
    {
        int temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }

    static void Lab12()
    {
        int m = 50;
        int n = 50;
        int l = 50;
        int[,] A = new int[m, n];
        int[,] B = new int[n, l];
        int[,] C = new int[m, l];

        // заповнюємо матриці A та B довільними значеннями
        Random random = new Random();
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                A[i, j] = random.Next(10);
            }
        }
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < l; j++)
            {
                B[i, j] = random.Next(10);
            }
        }

        Stopwatch stopwatch = new();
        stopwatch.Start();

        // створюємо таски для обчислення кожного рядка матриці C
        Task[] tasks = new Task[m];
        for (int i = 0; i < m; i++)
        {
            int row = i;
            tasks[i] = Task.Factory.StartNew(() =>
            {
                for (int j = 0; j < l; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < n; k++)
                    {
                        sum += A[row, k] * B[k, j];
                    }
                    C[row, j] = sum;
                }
            });
        }

        // очікуємо завершення всіх тасків
        Task.WaitAll(tasks);

        stopwatch.Stop();

        // виводимо результат
        //for (int i = 0; i < m; i++)
        //{
        //    for (int j = 0; j < l; j++)
        //    {
        //        Console.Write(C[i, j] + " ");
        //    }
        //    Console.WriteLine();
        //}

        Console.WriteLine("Час виконання паралельного множення в мiлiсекундах: {0}", stopwatch.Elapsed.TotalMilliseconds);

        //Console.WriteLine("\nA");
        //for (int i = 0; i < m; i++)
        //{
        //    for (int j = 0; j < n; j++)
        //    {
        //        Console.Write(" {0}", A[i, j]);
        //    }
        //    Console.WriteLine("");
        //}

        //Console.WriteLine("\nB");
        //for (int i = 0; i < n; i++)
        //{
        //    for (int j = 0; j < l; j++)
        //    {
        //        Console.Write(" {0}", B[i, j]);
        //    }
        //    Console.WriteLine("");
        //}

        stopwatch = new();
        stopwatch.Start();

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < l; j++)
            {
                int sum = 0;
                for (int k = 0; k < n; k++)
                {
                    sum += A[i, k] * B[k, j];
                }
                C[i, j] = sum;
            }
        }

        stopwatch.Stop();
        Console.WriteLine("Час виконання послiдовного множення в мiлiсекундах: {0}", stopwatch.Elapsed.TotalMilliseconds);
    }

    static void Lab11()
    {
        //Процес множення матрицьможливий тільки у випадку, коли число стовпців першої матриці
        //дорівнює кількості рядків другої матриці.
        int n = 5000;
        int m = 5000;
        int[] B = new int[m];
        int[,] A = new int[n, m];
        int[] C = new int[n];
        Console.WriteLine("\n\n------------------Lab11------------------\n");
        // заповнюємо матрицю A та вектор B довільними значеннями
        Random random = new Random();
        for (int i = 0; i < m; i++)
        {
            B[i] = random.Next(10);
            for (int j = 0; j < n; j++)
            {
                A[j, i] = random.Next(10);
            }
        }
        Stopwatch stopwatch = new();
        stopwatch.Start();
        // створюємо масив задач, кожна з яких буде обчислювати рядки матриці A
        Task[] tasks = new Task[n];
        for (int i = 0; i < n; i++)
        {
            int row = i;
            tasks[i] = Task.Run(() =>
            {
                int sum = 0;
                for (int j = 0; j < m; j++)
                {
                    sum += A[row, j] * B[j];
                }
                C[row] = sum;
            });
        }

        // чекаємо, доки всі задачі завершаться
        Task.WaitAll(tasks);

        stopwatch.Stop();

        //виводимо результат
        //for (int i = 0; i < n; i++)
        //{
        //    Console.WriteLine("C[{0}] = {1}", i, C[i]);
        //}

        Console.WriteLine("Час виконання паралельного множення в мiлiсекундах: {0}", stopwatch.Elapsed.TotalMilliseconds);

        //Console.WriteLine("\nA");
        //for (int i = 0; i < n; i++)
        //{
        //    for (int j = 0; j < m; j++)
        //    {
        //        Console.Write(" {0}", A[i, j]);
        //    }
        //    Console.WriteLine("");
        //}

        //Console.WriteLine("\nB");
        //for (int j = 0; j < m; j++)
        //{
        //    Console.Write(" {0}", B[j]);
        //}

        stopwatch = new();
        stopwatch.Start();

        for (int i = 0; i < n; i++)
        {
            int sum = 0;
            for (int j = 0; j < m; j++)
            {
                sum += A[i, j] * B[j];
            }
            C[i] = sum;
        }
        stopwatch.Stop();

        //виводимо результат
        //for (int i = 0; i < n; i++)
        //{
        //    Console.WriteLine("C[{0}] = {1}", i, C[i]);
        //}

        Console.WriteLine("Час виконання послiдовного множення в мiлiсекундах: {0}", stopwatch.Elapsed.TotalMilliseconds);
    }

    //Lab10 start--------------------

    static void Lab10()
    {
        Console.WriteLine("------------------Lab10------------------\n");
        double speedup, efficiency = 0.0;
        // Задання векторів a та b
        double[] a = new double[8];
        double[] b = new double[8];

        for (int i = 0; i < 8; i++)
        {
            a[i] = Math.Pow(5.35, 2) * i + 1;
            b[i] = Math.Pow(i + 1, 2) * i + 1;
        }

        // Обчислення скалярного добутку
        double scalarProduct = ComputeScalarProductCascaded(a, b);
        Console.WriteLine("Cкалярний добуток = {0}", scalarProduct);
        var sumOfScalarCascad = T_sequential + T_parallel;
        Console.WriteLine("Час обрахунку скалярного добутку за каскадною схемою в мiлiсекундах: {0}", sumOfScalarCascad);
        var time = CalculateScalarProductSuccessively(a, b);
        speedup = time / sumOfScalarCascad;
        efficiency = speedup / 8;
        Console.WriteLine("Прискорення х100 за каскадною схемою");
        Console.WriteLine(speedup * 100);
        Console.WriteLine("Ефективнiсть х100 за каскадною схемою");
        Console.WriteLine(efficiency * 100);
        Console.WriteLine("\n");

        // Обчислення за модивікованою схемою 
        double palallerScalarProduct = ComputeScalarProductCascadedModified();
        Console.WriteLine("Cкалярний добуток за модифiкованою схемою = {0}\n", palallerScalarProduct);

        // Обчислення редукції 
        double reductionProduct = ComputeScalarProductRed(a, b, time);
        Console.WriteLine("Скалярний добуток(редукцiя) = {0}\n", reductionProduct);

        Console.WriteLine("Закон Амдала:");
        Console.WriteLine(AmdahlLaw(8, speedup));
        Console.WriteLine("Закон Густавсона-Барсiса:");
        Console.WriteLine(GustafsonLaw(8, speedup));
    }

    static double CalculateScalarProductSuccessively(double[] a, double[] b)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        double scalarProduct = 0;

        for (int i = 0; i < a.Length; i++)
        {
            scalarProduct += a[i] * b[i];
        }
        stopwatch.Stop();
        return stopwatch.Elapsed.TotalMilliseconds;
    }


    static double ComputeScalarProductCascaded(double[] a, double[] b)
    {
        // Розрахунок скалярного добутку використовуючи таски
        double result = 0.0;
        int numTasks = 8; // отримуємо кількість доступних процесорів
        Task<double>[] tasks = new Task<double>[numTasks]; // створюємо масив тасків

        Stopwatch stopwatch = new();
        stopwatch.Start();
        for (int i = 0; i < numTasks; i++)
        {
            int taskNum = i; // номер поточного таска

            tasks[i] = Task<double>.Factory.StartNew(() =>
            {
                double sum = 0;
                for (int j = taskNum; j < numTasks; j += numTasks)
                {
                    sum += a[j] * b[j];
                }
                return sum;
            });
        }

        Task.WaitAll(tasks); // очікуємо завершення всіх тасків
        stopwatch.Stop();
        T_parallel = stopwatch.Elapsed.TotalMilliseconds;
        Console.WriteLine("Пралельна реалiзацiя каскадного алгоритму в мiлiсекундах: {0}", T_parallel);

        // Обчислюємо скалярний добуток як суму результатів тасків
        stopwatch = new();
        stopwatch.Start();
        foreach (Task<double> task in tasks)
        {
            result += task.Result;
        }
        stopwatch.Stop();
        T_sequential = stopwatch.Elapsed.TotalMilliseconds;
        Console.WriteLine("Послiдовна реалiзацiя каскадного алгоритму в мiлiсекундах: {0}", T_sequential);

        return result;
    }

    static double CalculateA(int i)
    {
        return Math.Cos(0.5 * i);
    }

    static double CalculateB(int i)
    {
        return Math.Sin(Math.Atan(i));
    }

    static double ComputeScalarProductCascadedModified()
    {
        int n = 8;
        // Задання векторів a та b
        double[] a = new double[n];
        double[] b = new double[n];
        double[] c = new double[n];
        double[] d = new double[n];
        double[] e = new double[n];
        double[] f = new double[n];

        for (int i = 0; i < n; i++)
        {
            a[i] = Math.Cos(0.5 * i);
            b[i] = Math.Sin(Math.Atan(i));
        }

        Stopwatch stopwatch = new();
        stopwatch.Start();
        // Обчислюємо
        Parallel.For(0, n, i =>
        {
            c[i] = a[i] * b[i];
        });

        Parallel.For(0, n / 2, i =>
        {
            d[i] = c[2 * i] + c[2 * i + 1];
        });

        Parallel.For(0, n / 4, i =>
        {
            e[i] = d[2 * i] + d[2 * i + 1];
        });

        Parallel.For(0, n / 8, i =>
        {
            f[i] = e[2 * i] + e[2 * i + 1];
        });

        stopwatch.Stop();
        T_parallel = stopwatch.Elapsed.TotalMilliseconds;
        Console.WriteLine("Час обрахунку скалярного добутку за модифiкованою каскадною схемою в мiлiсекундах: {0}", T_parallel);

        double result = f[0];

        //Console.WriteLine("Скалярний добуток: {0}", result);
        return result;
    }

    static double ComputeScalarProductRed(double[] a, double[] b, double time)
    {
        double parallelTime, sequentialTime = 0;
        int n = a.Length;
        int numThreads = Environment.ProcessorCount;

        double[] partialProducts = new double[numThreads];

        Stopwatch stopwatch = new();
        stopwatch.Start();
        Parallel.For(0, numThreads, threadIndex =>
        {
            int blockSize = n / numThreads;
            int startIndex = threadIndex * blockSize;
            int endIndex = (threadIndex == numThreads - 1) ? n : (threadIndex + 1) * blockSize;

            double partialSum = 0.0;
            for (int i = startIndex; i < endIndex; i++)
            {
                partialSum += a[i] * b[i];
            }

            partialProducts[threadIndex] = partialSum;
        });
        stopwatch.Stop();
        parallelTime = stopwatch.Elapsed.TotalMilliseconds;
        Console.WriteLine("Час обрахунку паралельної реалiзацiї редукцiї 1-го порядку в мiлiсекундах: {0}", parallelTime);

        stopwatch = new();
        stopwatch.Start();
        double result = 0.0;
        for (int i = 0; i < numThreads; i++)
        {
            result += partialProducts[i];
        }
        stopwatch.Stop();
        sequentialTime = stopwatch.Elapsed.TotalMilliseconds;
        Console.WriteLine("Час обрахунку послiдовної реалiзацiї редукцiї 1-го порядку в мiлiсекундах: {0}", sequentialTime);

        var totalSumaTimeSpan = parallelTime + sequentialTime;

        Console.WriteLine("Час обрахунку скалярного добутку в мiлiсекундах");
        Console.WriteLine(totalSumaTimeSpan);

        Console.WriteLine("Прискорення х100");
        var speedUp = time / totalSumaTimeSpan;
        Console.WriteLine(speedUp * 100);
        Console.WriteLine("Ефективнiсть х100");
        Console.WriteLine(speedUp / 8 * 100);

        return result;
    }

    public static double AmdahlLaw(int n, double p)
    {
        return 1 / ((1 - p) + p / n);
    }


    public static double GustafsonLaw(int n, double p)
    {
        return n - p * (n - 1);
    }

    //lab10 end-----------------------------
}