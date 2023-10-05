using System;
using System.IO;

namespace ShareExchangeVolumeAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] fileNames = { "Share_1_256.txt", "Share_2_256.txt", "Share_3_256.txt" };
            int[] array256 = ReadArrayFromFile(fileNames);

            if (array256 != null)
            {
                Console.WriteLine("Sorting and Searching for Array with 256 data points:");
                SortAndSearchArray(array256);

                Console.WriteLine("\n\n------------------------------------------\n\n");

                // For 2048 data points
                string[] fileNames2048 = { "Share_1_2048.txt", "Share_2_2048.txt", "Share_3_2048.txt" };
                int[] array2048 = ReadArrayFromFile(fileNames2048);

                if (array2048 != null)
                {
                    Console.WriteLine("Sorting and Searching for Array with 2048 data points:");
                    SortAndSearchArray(array2048);

                    Console.WriteLine("\n\n------------------------------------------\n\n");

                    // Merge Share_1_256.txt and Share_3_256.txt files
                    int[] mergedArray = MergeArrays(array256, array256);

                    Console.WriteLine("Sorting and Searching for Merged Array with 256 data points:");
                    SortAndSearchArray(mergedArray);

                    Console.WriteLine("\n\n------------------------------------------\n\n");

                    // For merged arrays with 2048 data points
                    int[] mergedArray2048 = MergeArrays(array2048, array2048);

                    Console.WriteLine("Sorting and Searching for Merged Array with 2048 data points:");
                    SortAndSearchArray(mergedArray2048);
                }
                else
                {
                    Console.WriteLine("Error occurred while reading files with 2048 data points.");
                }
            }
            else
            {
                Console.WriteLine("Error occurred while reading files with 256 data points.");
            }
        }

        static int[] ReadArrayFromFile(string[] fileNames)
        {
            int[] dataArray = new int[fileNames.Length * 256];
            int currentIndex = 0;

            foreach (string fileName in fileNames)
            {
                string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"File not found: {fileName}");
                    return null;
                }

                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (int.TryParse(line, out int value))
                    {
                        dataArray[currentIndex] = value;
                        currentIndex++;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid data in {fileName}: {line}");
                        return null;
                    }
                }
            }

            Array.Resize(ref dataArray, currentIndex);
            return dataArray;
        }

        static void SortAndSearchArray(int[] array)
        {
            // Sorting the array in ascending and descending order using Bubble Sort
            BubbleSortAscending(array);
            Console.WriteLine("\nAscending Sorted Array (Every 10th value):");
            DisplayEveryNthValue(array, 10);

            BubbleSortDescending(array);
            Console.WriteLine("\nDescending Sorted Array (Every 10th value):");
            DisplayEveryNthValue(array, 10);

            // Searching the array using Linear Search
            while (true)
            {
                Console.Write("\nEnter the value to search (or type 'exit' to quit): ");
                string input = Console.ReadLine();

                if (input.ToLower() == "exit")
                {
                    break;
                }

                if (int.TryParse(input, out int searchValue))
                {
                    int[] searchResults = LinearSearch(array, searchValue);
                    if (searchResults.Length > 0)
                    {
                        Console.WriteLine($"\nFound at position(s): {string.Join(", ", searchResults)}");
                    }
                    else
                    {
                        int[] nearestValues = FindNearestValues(array, searchValue);
                        Console.WriteLine($"\nValue not found. Nearest value(s) and their position(s):");
                        for (int i = 0; i < nearestValues.Length; i++)
                        {
                            int nearestValue = nearestValues[i];
                            int nearestValueIndex = Array.IndexOf(array, nearestValue);
                            Console.WriteLine($"Value: {nearestValue}, Position: {nearestValueIndex + 1}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer value or type 'exit' to quit.");
                }
            }
        }

        static void DisplayArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write($"{array[i]} ");
                if ((i + 1) % 10 == 0)
                {
                    Console.WriteLine();
                }
            }
        }

        static void DisplayEveryNthValue(int[] array, int n)
        {
            for (int i = 0; i < array.Length; i += n)
            {
                Console.Write($"{array[i]} ");
                if ((i + 1) % (10 * n) == 0)
                {
                    Console.WriteLine();
                }
            }
        }

        static void BubbleSortAscending(int[] array)
        {
            int temp;
            int steps = 0;
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    steps++;
                    if (array[j] > array[j + 1])
                    {
                        temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            Console.WriteLine($"\nSteps taken for ascending sort: {steps}");
        }

        static void BubbleSortDescending(int[] array)
        {
            int temp;
            int steps = 0;
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    steps++;
                    if (array[j] < array[j + 1])
                    {
                        temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            Console.WriteLine($"\nSteps taken for descending sort: {steps}");
        }

        static int[] LinearSearch(int[] array, int value)
        {
            int[] positions = new int[array.Length];
            int count = 0;
            int steps = 0;

            for (int i = 0; i < array.Length; i++)
            {
                steps++;
                if (array[i] == value)
                {
                    positions[count] = i + 1;
                    count++;
                }
            }

            Array.Resize(ref positions, count);
            Console.WriteLine($"\nSteps taken for linear search: {steps}");
            return positions;
        }

        static int[] FindNearestValues(int[] array, int value)
        {
            int minDistance = int.MaxValue;
            int[] nearestValues = new int[0];

            for (int i = 0; i < array.Length; i++)
            {
                int distance = Math.Abs(array[i] - value);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestValues = new int[] { array[i] };
                }
                else if (distance == minDistance)
                {
                    Array.Resize(ref nearestValues, nearestValues.Length + 1);
                    nearestValues[nearestValues.Length - 1] = array[i];
                }
            }

            return nearestValues;
        }

        static int[] MergeArrays(int[] array1, int[] array2)
        {
            int[] mergedArray = new int[array1.Length + array2.Length];
            array1.CopyTo(mergedArray, 0);
            array2.CopyTo(mergedArray, array1.Length);
            return mergedArray;
        }
    }
}
