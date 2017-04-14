using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sab_Toolbox
{
    class Utilities
    {


        /*
        * @name Convert Int To Hex Byte
        * @return string Hex Byte converted from input integer.
        * @param int The integer to be converted to a Hex byte.
        */
        public static string convertIntToHexByte(int inputValue)
        {
            string finalHexByte = "";
            if (inputValue > 255 || inputValue < 0)
            {
                Console.WriteLine("Input must be between 0 and 255. These are the values one byte can support.");
            }
            else
            {
                int zeroPlace = inputValue % 16;
                int inputDiv1 = inputValue / 16;

                int tenthPlace = inputDiv1 % 16;


                finalHexByte += convertIntToHexDigit(tenthPlace) + convertIntToHexDigit(zeroPlace);
            }

            return finalHexByte;
        }


        /*
        * @name Convert Int To Hex Digit
        * @return string Hex Digit converted from input Integer.
        * @param int The integer to be converted to Hex.
        */
        public static string convertIntToHexDigit(int inputValue)
        {
            string outputString = "";
            if (inputValue > 15 || inputValue < 0)
            {
                Console.WriteLine("Input value must be between 0 and 15. These are the values a hex digit can represent.");
            }
            else
            {
                if (inputValue < 10)
                {
                    outputString += inputValue;
                }
                else
                {
                    if (inputValue == 10) { outputString += "A"; }
                    if (inputValue == 11) { outputString += "B"; }
                    if (inputValue == 12) { outputString += "C"; }
                    if (inputValue == 13) { outputString += "D"; }
                    if (inputValue == 14) { outputString += "E"; }
                    if (inputValue == 15) { outputString += "F"; }
                }
            }
            return outputString;
        }


        public static string stringContainsOnly(string inputText, string containRequirements)
        {
            string returnString = inputText;


            for (int i = 0; i < returnString.Length; i++)
            {
                char inputCharacter = returnString[i];

                if (!containRequirements.Contains(inputCharacter.ToString()))
                {
                    int index = returnString.IndexOf(inputCharacter);
                    returnString = returnString.Substring(0, index);
                    i = returnString.Length + 1;
                }

            }

            return returnString;
        } //If string contains characters not found in the containRequirements it removes them and returns a new string.


        public static string reverseString(string textInput)
        {
            if (textInput == null) return null;

            char[] array = textInput.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }


        public static String[] removeAt(int index, String[] arr)
        {
            String[] newArr = new String[arr.Length - 1];
            int usedElements = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                if (i != index)
                {
                    newArr[usedElements] = arr[i];
                    usedElements++;
                }
            }
            return newArr;
        }

    }
}
