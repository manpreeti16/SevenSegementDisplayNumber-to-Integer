using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace UserStory
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var inputStream = File.ReadAllText(@"input_user_story_1.txt");
            ///Remove new line from the text file. 
            inputStream = inputStream.Replace("\n", "");

            ///Now we have continuous array in single line.
            var inputStreamCharArray = inputStream.ToCharArray();

            ///Converting it back to seven segment display, but this time there is no empty extra line
            var mychararry = new char[300, 27];
            var indexForInputStreamArray = 0;
            for (var j = 0; j < 300; j++)
            for (var i = 0; i < 27; i++)
            {
                mychararry[j, i] = inputStreamCharArray[indexForInputStreamArray];
                indexForInputStreamArray++;
            }

            ///To print the seven segment dispaly no.s
           /* for (int i = 0; i < 300; i++)
            {
                for (int j = 0; j < 27; j++)
                {
                    Console.Write(mychararry[i,j]);
                }

                Console.WriteLine();
            } */

            var sb = new StringBuilder();
            for (var j = 0; j < 300; j = j + 3)
            for (var i = 0; i < 26; i = i + 3)
            {
                var finalno = GetInteger(i, i + 2, mychararry, j, j + 2);
                sb.Append(finalno.ToString());
            }

            //Printing it in the same way as it was in the input
            var stringIntegerFinalArray = sb.ToString().ToCharArray();

            var finalarray = new char[100, 9];
            var indexIntArray = 0;
            for (var j = 0; j < 100; j++)
            for (var i = 0; i < 9; i++)
            {
                finalarray[j, i] = stringIntegerFinalArray[indexIntArray];
                indexIntArray++;
            }

            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 9; j++) Console.Write(finalarray[i, j]);
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        private static int GetInteger(int jlow, int jhigh, char[,] mychararry, int ilow, int ihigh)
        {
            // Consider this is how seven segment number is displayed. 
            // With each segment marked from 0 to 6 in clock wise order.            
            //
            //       0
            //      ___
            //    5|   |1
            //     | 6 |
            //     |___|
            //    4|   |2
            //     | 3 |
            //     |___|
            //
            //    Reading this number from top and left to right will be in this order.
            //    0,5,6,1,4,3,2
            //    If you notice, before zero we have an empty place where a character can be filled, similarly after zero we have an empty place.
            //    Let us fill these places with 9 & 8. You can use any. So new structure looks like:
            //
            //     9 0 8
            //      ___
            //    5|   |1
            //     | 6 |
            //     |___|
            //    4|   |2
            //     | 3 |
            //     |___|
            //
            //   Now the new order is 9,0,8,5,6,1,4,3,2
            //   This order is what we need to calculate the integer value
            //   So now we know, every integer is made up of 9 values, which can empty space (' ') or underscore ('_') or bar ('|')
            //   For clarity of the diagram, i have used 3 characters for single segment, in text file or in normal display it would be only 1 segment
            //   similar to this:
            //       _
            //      |_|
            //      |_|             
           
            //Reading all 9 characters of a single number in input from left to right and top to bottom like we did above.
            var singleCharArray = new List<char>();
            for (var i = ilow; i <= ihigh; i++)
            for (var j = jlow; j <= jhigh; j++)
                singleCharArray.Add(mychararry[i, j]);              

            var mydict = new Dictionary<int, char>();
            var keys = new List<int> {9, 0, 8, 5, 6, 1, 4, 3, 2};  //The order which we had above
            var indexForKey = 0;
            // Map all 9 numbers with the order which we have. This tells us which all places have a value in the display.
            foreach (var item in singleCharArray)
            {
                mydict.Add(keys[indexForKey], item);
                indexForKey++;
            }

            var listofNo = new List<int>();
            foreach (var dictitem in mydict)
                if (dictitem.Value != ' ')
                    listofNo.Add(dictitem.Key);

            // Keep only those numbers from order who have an underscore('_') or bar ('|') i.e., those are the glowing ones and indicate a value
            var intnomapped = 0;
            foreach (var no in listofNo) intnomapped += (int) Math.Pow(2, no);

            //This dictinary contains value obtained by raising the glowing segments to power of 2 and adding them
            //      _    _
            // |    _|   _|
            // |   |_    _|
            //
            // 1 has values as order places 1 and 2, so for 1 we have Math.Pow(2, 1)+Math.Pow(2, 1)= 6
            // 2 has values as order places 0,1,6,4,3, Math.Pow (2,x) where x is  0,1,6,4,3  and adding all we get 91
            var dictofMappedInt = new Dictionary<int, int>();
            {
                dictofMappedInt.Add(63, 0);
                dictofMappedInt.Add(6, 1);
                dictofMappedInt.Add(91, 2);
                dictofMappedInt.Add(79, 3);
                dictofMappedInt.Add(102, 4);
                dictofMappedInt.Add(109, 5);
                dictofMappedInt.Add(125, 6);
                dictofMappedInt.Add(7, 7);
                dictofMappedInt.Add(127, 8);
                dictofMappedInt.Add(111, 9);
            }
            // After calculating the key of dictionary, pick corresponding value, which is the required integer number for us
            var finalno = dictofMappedInt[intnomapped];
            return finalno;
        }
    }
}