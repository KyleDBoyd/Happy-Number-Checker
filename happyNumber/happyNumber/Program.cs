using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/* Kyle Boyd
 * March 30th, 2013
 * Version 0.01
 * 
 * Happy Number Finder
 * ===================
 * The program finds the smallest integer number that's greater than 1 and is happy in all the given bases.
 * This program takes a text file as input of the form:
 * 
 * 3
 * 2 3
 * 2 3 7
 * 9 10
 * 
 * 3 being the number of test cases. Each case consists of a single line. Each line contains a space separated list of distinct integers, representing the bases
 * 
 * The ouput is of the form:
 * Case #1: 3
 * Case #2: 143
 * Case #3: 91
 * 
 * Limits
 * 2 ≤ all possible input bases ≤ 10
 * 
 * Small dataset
 * 1 ≤ T ≤ 42
 * 2 ≤ number of bases on each test case ≤ 3
 * 
 * Large dataset
 * 1 ≤ T ≤ 500
 * 2 ≤ number of bases on each test case ≤ 9
 * 
 */

namespace happyNumber
{
    // class to create custom exceptions
    public class CustomException : Exception
    {
        public CustomException(string message)
            : base(message) { }
    }

    public class HappyNumber
    {
        // Converts a given integer a into the given integer base b
        // 1 < n < 10
        // Assumes val is base 10
        // Returns an integer that is converted to the new base
        // returns -1 if base outside of range
        public int convertBase(int a, int b)
        {
            if (b < 2 || b > 9)
            {
                return -1;
            }
            int result;
            int multiplier = 10;
            result = a % b;
            while (a > 0)
            {
                a = a / b;
                result = (a % b) * multiplier + result;
                multiplier = multiplier * 10;
            }
            return result;
        }
        // Checks if a given integer a is a happy number
        // Returns true if it is happy, false otherwise
        // Assumes the base given is within the bounds
        public bool isHappy(int a, int curBase)
        {
            int total = a;
            int product;
            string numberString;
            int numDigits;
            int digit;
            List<int> myList = new List<int>();

            while (!myList.Contains(total))
            {
                myList.Add(total);

                numberString = total.ToString();
                numDigits = numberString.Length;
                total = 0;

                for (int i = 0; i < numDigits; i++)
                {
                    digit = (int)Char.GetNumericValue(numberString[i]);
                    product = (int)Math.Pow(digit, 2);
                    total = total + product;
                }

                if (total == 1)
                {
                    return true;
                }
                else
                {
                    if (curBase != 10)
                    {
                        total = convertBase(total, curBase);
                    } 
                }
            }
            return false;
        }
        // Returns the file path to the resoureces
        public string getResource()
        {
            string resource;
            resource = @"../../resources/";
            return resource;
        }
        // Creates a file with the given fileName
        // If the file already exists it is deleted and replaced
        public void createFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            System.IO.FileStream f = System.IO.File.Create(fileName);
            f.Close();
        }
        // Writes the given list to the filename provided
        public void writeToFile(List<string> list, string file)
        {
            using(System.IO.StreamWriter fileToWriteTo = new System.IO.StreamWriter(file))
            {
                foreach (string line in list)
                {
                    fileToWriteTo.WriteLine(line);
                }
                fileToWriteTo.Close();
            }
        }
        // Validates to file to assure it is within the number of cases is within the limit
        public void validateFile(string file, int max)
        {
            string line;
            int numCases;
            using (StreamReader sr = new StreamReader(file))
            {
                line = sr.ReadLine();
                numCases = int.Parse(line);
                if (numCases > max || numCases < 1)
                {
                    throw new CustomException("Invalid Number of Cases");
                }
            }
        }
        // Check if the given base n is within the range n < max && n > 2
        public bool validateNumBases(int n, int max)
        {
            if (n > max || n < 2)
            {
                return false;
            }
            return true;
        }
        // Processes the file.
        // Gets the number of cases to run through
        // Reads in the file one line at a time and finds the smallest integer n > 1 that is happy in all given bases
        // Returns A list of strings that contains the case number and its associted integer
        public List<string> processFile(string file, int maxNumBases)
        {
            List<string> cases = new List<string>();
            string line;
            int curNumber;
            int numCases;
            int curBase;
            int numBases;
            int j;
            int baseAfterConvert;

            using (StreamReader sr = new StreamReader(file))
            {
                line = sr.ReadLine();
                numCases = int.Parse(line);
                for (int i = 1; i <= numCases; i++)
                {
                    line = sr.ReadLine();

                    string[] words = line.Split(' ');
                    numBases = words.Length;
                    if (!validateNumBases(numBases, maxNumBases))
                    {
                        throw new CustomException("Invald num bases");
                    }
                    curNumber = 2;
                    j = 0;
                    while (j < numBases)
                    {
                        curBase = int.Parse(words[j]);
                        if (curBase != 10)
                        {
                            baseAfterConvert = convertBase(curNumber, curBase);
                            if (baseAfterConvert == -1)
                            {
                                throw new CustomException("Invalid Base");
                            }
                        }
                        else
                        {
                            baseAfterConvert = curNumber;
                        }
                        if (isHappy(baseAfterConvert, curBase))
                        {
                            j++;
                        }
                        else
                        {
                            curNumber++;
                            j = 0;
                        }
                    }
                    Console.WriteLine("Case #" + i + ": " + curNumber);
                    cases.Add("Case #" + i + ": " + curNumber);
                }
            }
            return cases;
        }
        static void Main(string[] args)
        {
            try
            {
                // Max cases for each dataset
                const int smallPracticeMax = 42;
                const int largePracticeMax = 500;
                // Max bases for each dataset
                const int smallPracticeMaxBases = 3;
                const int largePracticeMaxBases = 9;
                // Input file names
                const string smallPracticeFile = "smallPractice.txt";
                const string largePracticeFile = "largePractice.txt";
                // Ouput file names
                const string smallPracticeFileOutput = "smallPracticeOutput.txt";
                const string largePracticeFileOutput = "largePracticeOutput.txt";

                HappyNumber m = new HappyNumber();

                string smallFileResource;
                string largeFileResource;
                string smallFileResourceOutput;
                string largeFileResourceOutput;

                List<string> smallOutputList = new List<string>();
                List<string> largeOutputList = new List<string>();

                // Creates resource for input and ouput text files
                smallFileResource = m.getResource() + smallPracticeFile;
                largeFileResource = m.getResource() + largePracticeFile;
                smallFileResourceOutput = m.getResource() + smallPracticeFileOutput;
                largeFileResourceOutput = m.getResource() + largePracticeFileOutput;

                // Validates number of cases in file
                m.validateFile(smallFileResource, smallPracticeMax);
                m.validateFile(largeFileResource, largePracticeMax);

                // Processes the two files
                smallOutputList = m.processFile(smallFileResource, smallPracticeMaxBases);
                largeOutputList = m.processFile(largeFileResource, largePracticeMaxBases);

                // Creates/Writes to the output files
                m.createFile(smallFileResourceOutput);
                m.writeToFile(smallOutputList, smallFileResourceOutput);
                m.createFile(largeFileResourceOutput);
                m.writeToFile(largeOutputList, largeFileResourceOutput);

            }
            catch (Exception e)
            {
                // Writes error message to console if error is thrown
                Console.WriteLine(e.Message);
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
            }
        }
    }
}
