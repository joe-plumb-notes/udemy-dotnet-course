using System;
using System.Reflection;
using System.Security.Cryptography;

namespace HelloWorld // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static int accessibleInt = 7;
        static void Main(string[] args)
        {
            int[] intsToCompress = new int[] {10, 15, 20, 25, 30, 12, 34};
            
            accessibleInt += 1;
            Console.WriteLine(accessibleInt);
            
            int totalOfArray = 0;

            // Calculating via indexes of the array
            Console.WriteLine("-- Array Indexes approach --");
            DateTime startTime = DateTime.Now;
            totalOfArray = intsToCompress[0] + intsToCompress[1] + intsToCompress[2] + intsToCompress[3] + intsToCompress[4] + intsToCompress[5] + intsToCompress[6];
            
            Console.WriteLine($"Time taken: {(DateTime.Now - startTime)}");
            Console.WriteLine($"Total of array: {totalOfArray}");

            // For Loop approach
            Console.WriteLine("-- For Loop approach --");
            totalOfArray = 0;
            startTime = DateTime.Now;

            for (int i=0; i < intsToCompress.Length; i++)
            {
                totalOfArray += intsToCompress[i];
            }

            Console.WriteLine($"Time taken: {(DateTime.Now - startTime)}");
            Console.WriteLine($"Total of array: {totalOfArray}");

            // .Sum function approach
            Console.WriteLine("-- .Sum() approach --");
            startTime = DateTime.Now;

            totalOfArray = 0;
            totalOfArray = intsToCompress.Sum();

            Console.WriteLine($"Time taken: {(DateTime.Now - startTime)}");
            Console.WriteLine($"Total of array: {totalOfArray}");

            // foreach approach
            Console.WriteLine("-- foreach approach --");
            startTime = DateTime.Now;

            totalOfArray = 0;
            foreach (int i in intsToCompress){
                totalOfArray += i;
            }

            Console.WriteLine($"Time taken: {(DateTime.Now - startTime)}");
            Console.WriteLine($"Total of array: {totalOfArray}");

            // while loop
            Console.WriteLine("-- while loop --");
            int index = 0;
            totalOfArray = 0;
            startTime = DateTime.Now;

            while(index < intsToCompress.Length){
                totalOfArray += intsToCompress[index];
                index++;
            }

            Console.WriteLine($"Time taken: {(DateTime.Now - startTime)}");
            Console.WriteLine($"Total of array: {totalOfArray}");

            // do while loop
            Console.WriteLine("-- do while loop --");
            index = 0;
            totalOfArray = 0;
            startTime = DateTime.Now;

            do {
                totalOfArray += intsToCompress[index];
                index++;
            }
            while(index < intsToCompress.Length);

            Console.WriteLine($"Time taken: {(DateTime.Now - startTime)}");
            Console.WriteLine($"Total of array: {totalOfArray}");

            // total of all the even numbers
            Console.WriteLine("-- sum of even numbers --");
            totalOfArray = 0;
            foreach (int i in intsToCompress){
                    if (i % 2 == 0){
                        totalOfArray += i;
                }
            }
            Console.WriteLine($"Total of even numbers in array: {totalOfArray}");

            Console.WriteLine(GetSum(intsToCompress));

        }
    static private int GetSum(int[] intsToCompress)
    {   
        int totalOfArray = 0;
        //int[] intsToCompress = new int[] {10, 15, 20, 25, 30, 12, 34};
        
        foreach (int i in intsToCompress){
            totalOfArray += i;
        }

        return totalOfArray;
    }
    }
}
