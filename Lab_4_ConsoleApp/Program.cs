using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Data;

using System;

public class MyMatrix
{
    private double[,] matrix;

    // Конструктор, принимающий количество строк и столбцов и заполняющий матрицу случайными числами
    public MyMatrix(int rows, int cols, double minValue, double maxValue)
    {
        matrix = new double[rows, cols];
        Random rand = new Random();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = rand.NextDouble() * (maxValue - minValue) + minValue;
            }
        }
    }

    // Индексатор для доступа к элементам матрицы
    public double this[int row, int col]
    {
        get
        {
            if (row >= 0 && row < matrix.GetLength(0) && col >= 0 && col < matrix.GetLength(1))
            {
                return matrix[row, col];
            }
            else
            {
                throw new IndexOutOfRangeException("Неверные индексы для доступа к элементам матрицы");
            }
        }
        set
        {
            if (row >= 0 && row < matrix.GetLength(0) && col >= 0 && col < matrix.GetLength(1))
            {
                matrix[row, col] = value;
            }
            else
            {
                throw new IndexOutOfRangeException("Неверные индексы для установки элементов матрицы");
            }
        }
    }

    // Метод для получения количества строк
    public int Rows
    {
        get { return matrix.GetLength(0); }
    }

    // Метод для получения количества столбцов
    public int Cols
    {
        get { return matrix.GetLength(1); }
    }

    // Метод для вывода матрицы на экран
    public void Print()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Console.Write($"{matrix[i, j]:F2}\t");
            }
            Console.WriteLine();
        }
    }

    public static MyMatrix operator +(MyMatrix a, MyMatrix b)
    {
        if (a.Rows != b.Rows || a.Cols != b.Cols)
        {
            throw new InvalidOperationException("Матрицы должны быть одинакового размера для сложения");
        }

        MyMatrix result = new MyMatrix(a.Rows, a.Cols, 0, 0);

        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Cols; j++)
            {
                result[i, j] = a[i, j] + b[i, j];
            }
        }

        return result;
    }

    public static MyMatrix operator -(MyMatrix a, MyMatrix b)
    {
        if (a.Rows != b.Rows || a.Cols != b.Cols)
        {
            throw new InvalidOperationException("Матрицы должны быть одинакового размера для вычитания");
        }

        MyMatrix result = new MyMatrix(a.Rows, a.Cols, 0, 0);

        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Cols; j++)
            {
                result[i, j] = a[i, j] - b[i, j];
            }
        }

        return result;
    }

    public static MyMatrix operator *(MyMatrix a, MyMatrix b)
    {
        if (a.Cols != b.Rows)
        {
            throw new InvalidOperationException("Для умножения количество столбцов первой матрицы должно быть равно количеству строк второй матрицы");
        }

        MyMatrix result = new MyMatrix(a.Rows, b.Cols, 0, 0);

        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < b.Cols; j++)
            {
                result[i, j] = 0;
                for (int k = 0; k < a.Cols; k++)
                {
                    result[i, j] += a[i, k] * b[k, j];
                }
            }
        }

        return result;
    }
    public static MyMatrix operator *(MyMatrix a, double scalar)
    {
        MyMatrix result = new MyMatrix(a.Rows, a.Cols, 0, 0);

        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Cols; j++)
            {
                result[i, j] = a[i, j] * scalar;
            }
        }

        return result;
    }

    public static MyMatrix operator /(MyMatrix a, double scalar)
    {
        if (scalar == 0)
        {
            throw new DivideByZeroException("Деление на ноль недопустимо");
        }

        MyMatrix result = new MyMatrix(a.Rows, a.Cols, 0, 0);

        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Cols; j++)
            {
                result[i, j] = a[i, j] / scalar;
            }
        }

        return result;
    }

}

public class Car
{
    public string Name { get; set; }
    public  int ProductionYear { get; set; }
    public  int MaxSpeed { get; set; }

    public Car(string Name, int ProductionYear, int MaxSpeed) {
        this.Name = Name;
        this.ProductionYear = ProductionYear;
        this.MaxSpeed = MaxSpeed;
    }

    public override string ToString()
    {
        return $"Name: {Name}, Year: {ProductionYear}, MaxSpeed: {MaxSpeed}";
    }
}

public class CarComparer : IComparer<Car>
{
    private string _criteria;

    public CarComparer(string criteria)
    {
        _criteria = criteria;
    }

    public int Compare(Car x, Car y)
    {
        switch (_criteria.ToLower())
        {
            case "name":
                return x.Name.CompareTo(y.Name);
            case "year":
                return x.ProductionYear.CompareTo(y.ProductionYear);
            case "speed":
                return x.MaxSpeed.CompareTo(y.MaxSpeed);
            default:
                throw new ArgumentException("Invalid sorting criteria");
        }
    }
}

public class CarCatalog
{
    private Car[] _cars;

    public CarCatalog(Car[] cars)
    {
        _cars = cars;
    }

    // Прямой проход с первого до последнего элемента
    public IEnumerable<Car> GetCars()
    {
        for (int i = 0; i < _cars.Length; i++)
        {
            yield return _cars[i];
        }
    }

    // Обратный проход с последнего до первого элемента
    public IEnumerable<Car> GetCarsReversed()
    {
        for (int i = _cars.Length - 1; i >= 0; i--)
        {
            yield return _cars[i];
        }
    }

    // Проход по элементам с фильтром по году выпуска
    public IEnumerable<Car> GetCarsByYear(int year)
    {
        foreach (var car in _cars)
        {
            if (car.ProductionYear == year)
            {
                yield return car;
            }
        }
    }

    // Проход по элементам с фильтром по максимальной скорости
    public IEnumerable<Car> GetCarsBySpeed(int speed)
    {
        foreach (var car in _cars)
        {
            if (car.MaxSpeed >= speed)
            {
                yield return car;
            }
        }
    }
}



class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Задание №1");
        // Создание двух матриц
        MyMatrix matrix1 = new MyMatrix(3, 3, 1, 10);
        MyMatrix matrix2 = new MyMatrix(3, 3, 1, 10);

        // Вывод матриц
        Console.WriteLine("Матрица 1:");
        matrix1.Print();

        Console.WriteLine("\nМатрица 2:");
        matrix2.Print();

        // Сложение матриц
        MyMatrix sumMatrix = matrix1 + matrix2;
        Console.WriteLine("\nСумма матриц:");
        sumMatrix.Print();

        // Умножение на скаляр
        MyMatrix multipliedMatrix = matrix1 * 2;
        Console.WriteLine("\nМатрица 1, умноженная на 2:");
        multipliedMatrix.Print();

        // Умножение матриц
        MyMatrix productMatrix = matrix1 * matrix2;
        Console.WriteLine("\nПроизведение матриц:");
        productMatrix.Print();

        Console.WriteLine("Задание №2");
        Car[] cars =
        [
            new Car("Tesla", 2020, 250),
            new Car("Ford", 2015, 200),
            new Car("BMW", 2018, 240),
            new Car("Audi", 2022, 260)
        ];

        Console.WriteLine("Sorting by Name:");
        Array.Sort(cars, new CarComparer("name"));
        foreach (var car in cars)
            Console.WriteLine(car);

        Console.WriteLine("\nSorting by Production Year:");
        Array.Sort(cars, new CarComparer("year"));
        foreach (var car in cars)
            Console.WriteLine(car);

        Console.WriteLine("\nSorting by Max Speed:");
        Array.Sort(cars, new CarComparer("speed"));
        foreach (var car in cars)
            Console.WriteLine(car);
        
        Console.WriteLine("Задание №3");
        
        CarCatalog catalog = new CarCatalog(cars);

        Console.WriteLine("Direct iteration:");
        foreach (var car in catalog.GetCars())
        {
            Console.WriteLine(car);
        }

        Console.WriteLine("\nReversed iteration:");
        foreach (var car in catalog.GetCarsReversed())
        {
            Console.WriteLine(car);
        }

        Console.WriteLine("\nCars from year 2020:");
        foreach (var car in catalog.GetCarsByYear(2020))
        {
            Console.WriteLine(car);
        }

        Console.WriteLine("\nCars with speed >= 240:");
        foreach (var car in catalog.GetCarsBySpeed(240))
        {
            Console.WriteLine(car);
        }
    }
}

