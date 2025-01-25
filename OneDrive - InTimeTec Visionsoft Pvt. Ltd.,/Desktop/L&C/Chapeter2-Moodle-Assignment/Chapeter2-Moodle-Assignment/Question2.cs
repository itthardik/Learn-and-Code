using System;

class SubarrayMeanCalculator
{
    static void Main(string[] args)
    {
        var arraySizeAndQueryCount = ReadArraySizeAndQueryCount();
        var array = ReadArray();
        var prefixSumArray = BuildPrefixSumArray(array, arraySizeAndQueryCount.Item1);
        ProcessQueries(prefixSumArray, arraySizeAndQueryCount.Item2);
    }

    private static (int, int) ReadArraySizeAndQueryCount(){
        var input = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
        return (input[0], input[1]);
    }

    private static long[] ReadArray(){
        return Array.ConvertAll(Console.ReadLine().Split(' '), long.Parse);
    }

    private static long[] BuildPrefixSumArray(long[] array, int size){
        long[] prefixSumArray = new long[size + 1];
        prefixSumArray[0] = 0;
        for (int index = 1; index <= size; index++)
        {
            prefixSumArray[index] = prefixSumArray[index - 1] + array[index - 1];
        }
        return prefixSumArray;
    }

    private static void ProcessQueries(long[] prefixSumArray, int queryCount){
        for (int queryIndex = 0; queryIndex < queryCount; queryIndex++)
        {
            var query = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
            int leftIndex = query[0];
            int rightIndex = query[1];
            long mean = CalculateMean(prefixSumArray, leftIndex, rightIndex);
            Console.WriteLine(mean);
        }
    }

    private static long CalculateMean(long[] prefixSumArray, int leftIndex, int rightIndex){
        long sum = prefixSumArray[rightIndex] - prefixSumArray[leftIndex - 1];
        return sum / (rightIndex - leftIndex + 1);
    }
}
