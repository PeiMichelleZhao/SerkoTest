using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SerkoTest.Exceptions;
using SerkoTest.Models;

namespace SerkoTest.Services
{

    public class ExtractDataServices : IExtractDataServices
    {


        /*
         * get all xml like sections
         */
        public string ExtractData(string text)
        {
            int startPosition = 0;
            bool readEnd = false;
            List<string> blocks = new List<string>();

            do
            {
                // the begin tag position
                startPosition = text.IndexOf("<", startPosition, StringComparison.Ordinal);

                // read text to end
                if (startPosition == -1)
                {
                    readEnd = true;
                }
                else
                {
                    // get tag name
                    string tagName = FindOpenItemName(text, startPosition);
                    // get close tag position
                    int closePosition = FindCloseItemPosition(text, startPosition, tagName);

                    string xmlBlock = text.Substring(startPosition, closePosition - startPosition);

                    // add tag block to list
                    blocks.Add(xmlBlock);

                    // set start position to the close position of previous block
                    startPosition = closePosition;
                }

            } while (!readEnd);

            // join all block to a string
            return String.Join("", blocks.ToArray());
        }

        /*
         * get the item name
         */
        private string FindOpenItemName(string text, int startPosition)
        {
            return text.Substring(startPosition + 1, text.IndexOf(">", startPosition, StringComparison.Ordinal) - startPosition - 1);
        }

        /*
         * get the end of the section
         */
        private int FindCloseItemPosition(string text, int startPosition, string itemName)
        {
            // create close tag
            string closeTag = "</" + itemName + ">";

            // search close tag position
            int lastIndex = text.IndexOf(closeTag, startPosition, StringComparison.Ordinal);

            // if not find, throw exception
            if (lastIndex == -1)
            {
                throw new NoCloseTagException();
            }

            // close tag position plus close tag length
            return lastIndex + closeTag.Length;
        }
    }

}
