/*
 * Name: Izzy Eanes
 * Date: 2/20/2024
 * Purpose: Lab 5 for Server-Side Web Development
 */

using Lab5;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace lab5
{
    class Program
    {
        //This creates a new HttpClient, I found the best spot for it was at the top as it shouldn't be called in every Async function.
        static HttpClient client = new HttpClient();

        // This is the main method of the program, which has the key features of initializing the client address, and the menu with functional options.
        static void Main()
        {
            // This is where the base address is set. Dog API is an API with dog facts and information, its nice.
            client.BaseAddress = new Uri("http://dogapi.dog/api/v2");
            ConsoleKey input = ConsoleKey.None;
            bool working;
            // The two following lines creates the List of filled out breed objects, it takes a minute to load as there are over 200.
            List<Breeds> dogBreeds = new List<Breeds>();
            BreedGetter(dogBreeds).GetAwaiter().GetResult();
            //This is where the user interaction / menu begins.
            Console.WriteLine("Welcome to Izzy Eanes' Lab 5");
            // While loop to keep the user in the menu, until they're done.
            while (input != ConsoleKey.D3) 
            {
                // This menu has two functionalities, spitting out a desired number of dog facts, and a breed-matching search.
                Console.WriteLine("What would you like to do?\n  1.See Dog Facts\n  2.Search for a Dog Breed\n  3.Quit");
                input = Console.ReadKey().Key;
                // This switch statement controls which menu option the user is utilizing.
                switch (input)
                {
                    // Option 1 = Facts
                    case ConsoleKey.D1:
                        Console.Clear();
                        Console.WriteLine("How many facts would you like to see?");
                        int factNum = 0;
                        do
                        {
                            // This try catch and do while loop ensures that the user only enters a number, and loops till they do.
                            try
                            {
                                factNum = int.Parse(Console.ReadLine());
                                working = true;
                            }
                            catch { working = false; }
                        } while (!working);
                        // This actually runs the async Task to get the facts needed.
                        FactGetter(factNum).GetAwaiter().GetResult();
                        Console.WriteLine("Press any key to return to the menu.");
                        Console.ReadLine();
                        break;
                    // Option 2 = Breed Info
                    case ConsoleKey.D2:
                        Console.Clear();
                        // Many variables were needed for this, they are all created here.
                        List<Breeds> temp;
                        int minDogLife;
                        int minDogWeight;
                        int maxDogLife;
                        int maxDogWeight;
                        bool needsHypo;
                        // By using a try catch, if the user enters something that is not a number, it just uses the default, this was used for all int-based entries.
                        Console.WriteLine("What is the minimum lifespan you would like? (Press any non-number key to skip)");
                        try
                        {
                            minDogLife = int.Parse(Console.ReadLine());
                        } catch { minDogLife = 0; }
                        Console.WriteLine("What is the maximum lifespan you would like? (Press any non-number key to skip)");
                        try
                        {
                            maxDogLife = int.Parse(Console.ReadLine());
                        }
                        catch { maxDogLife = 100; }
                        Console.WriteLine("What is the minimum weight you would like? (Press any non-number key to skip)");
                        try
                        {
                            minDogWeight = int.Parse(Console.ReadLine());
                        }
                        catch { minDogWeight = 0; }
                        Console.WriteLine("What is the maximum weight you would like? (Press any non-number key to skip)");
                        try
                        {
                            maxDogWeight = int.Parse(Console.ReadLine());
                        }
                        catch { maxDogWeight = 200; }
                        // This is used to seperate out whether or not the user needs a Hypoallergenic dog.
                        Console.WriteLine("Do you need a hypoallergenic dog? (Y/N) (Press any other key to skip)");
                        if (Console.ReadKey().Key == ConsoleKey.Y)
                        {
                            needsHypo = true;
                        } else 
                        { 
                            needsHypo = false;
                        }
                        // This creates a 'temp' list of all dogs who have the correct attributes.
                        temp = AttSearch(dogBreeds, minDogLife, maxDogLife, minDogWeight, maxDogWeight, needsHypo);
                        Console.Clear();
                        // Itterates through all the dogs in temp, and prints them out.
                        foreach (Breeds dog in temp)
                        {
                            Console.WriteLine(dog.ToString());
                        }
                        Console.WriteLine("Press any key to return to the menu.");
                        Console.ReadLine();
                        break;
                }
                Console.Clear();
            }
            Console.WriteLine("Thanks, Bye!");
        }


        // This function retrieves the desired num of dog facts.
        // It's default is set to return 1 fact, unless otherwise instructed.
        static async Task FactGetter(int num = 1)
        {
            string facts = "";

            // This allows it to itterate through fact retrieval and returning until its met the desired num.
            for (int i = 0; i < num; i++)
            {
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "/facts");
                if (response.IsSuccessStatusCode)
                {
                    // This pulls in the entire JSON object as a string, maybe not the best way to do it but it works.
                    facts = await response.Content.ReadAsStringAsync();
                    // The next 3 lines, break the JSON object string into just the actual fact itself.
                    int index = facts.IndexOf('{', 15) + 9;
                    int length = facts.Length - 95;
                    facts = facts.Substring(index, length).Replace(@"\", "");
                }
                Console.WriteLine(facts);
            }
        }

        // This function creates all of the breeds as Breeds objects within a list.
        // It runs at the beginning of the program because 29 pages of dogs, take a while to load.
        static async Task BreedGetter(List<Breeds> returnBreeds)
        {
            string breeds;
            List<string> indivBreeds = new List<string>();
            // This iterates through every page of dogs and puts them all into a indivBreeds list .
            for (int i = 1; i < 30; i++)
            {
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress + $"/breeds?page[number]={i}");
                if (response.IsSuccessStatusCode)
                {
                    // Brings down the entire page of dog breeds as a single string.
                    breeds = await response.Content.ReadAsStringAsync();

                    // Breaks the entire string of dog breeds into individual breeds based on the opening text of each entry.
                    string[] temp = breeds.Split(@"{""data"":");


                    // Itterates through each of the dog entries in the collections and better formats them into a List for further processing.
                    foreach (string s in temp)
                    {
                        int length = s.Length - (s.IndexOf("attributes")) - 48;
                        if ((s.IndexOf("attributes")) != -1 && s != "")
                        {
                            indivBreeds.Add(s.Substring((s.IndexOf("attributes")) + 21, length));
                        }
                    }
                }
            }

            //This is where the next step of this function comes into play, turning strings of data into usable objects.
            // This is Rough, its best to just ignore the details.
            foreach (string b in indivBreeds)
            {
                // This is the string collection of broken up attributes.
                string[] atts = new string[6];
                // Atts[0] is the name of each breed.
                atts[0] = b.Substring(0, (b.IndexOf(",") - 1));
                // Atts[1] is the description of each breed.
                atts[1] = b.Substring(atts[0].Length + 17, (b.IndexOf(@"""life"":") - atts[0].Length - 19));
                // Atts[2] is the lifespan of each breed. This is also where it starts to get complicated.
                atts[2] = b.Substring((b.IndexOf(@"""life"":{") + 14), 11).Replace("}", " ").Replace("},", "").Replace(@",""min"":", "").Replace(",", "").Replace(" ", "");
                // Switch statements had to be used to take into consideration the different groupings of numbers in age and weight. They are also under other atts  but I will only be commenting on this one.
                switch (atts[2].Length)
                {
                    case 2:
                        atts[2] = ($"{atts[2][1]} - {atts[2][0]}");
                        break;
                    case 3:
                        atts[2] = ($"{atts[2][2]} - {atts[2][0]}{atts[2][1]}");
                        break;
                    case 4:
                        atts[2] = ($"{atts[2][2]}{atts[2][3]} - {atts[2][0]}{atts[2][1]}");
                        break;
                }
                // Atts[3] is the male_weight of each breed.
                atts[3] = b.Substring((b.IndexOf(@"""male_weight"":") + 21), 11).Replace(@",""min"":", "").Replace("},", "").Replace("}", "");
                switch (atts[3].Length)
                {
                    case 2:
                        atts[3] = ($"{atts[3][1]} - {atts[3][0]}");
                        break;
                    case 3:
                        atts[3] = ($"{atts[3][2]} - {atts[3][0]}{atts[3][1]}");
                        break;
                    case 4:
                        atts[3] = ($"{atts[3][2]}{atts[3][3]} - {atts[3][0]}{atts[3][1]}");
                        break;
                }
                // Atts[4] is the female_weight of each breed.
                atts[4] = b.Substring((b.IndexOf(@"""female_weight"":") + 23), 11).Replace(",\"min\":", "").Replace("},", "").Replace("}", "");
                switch (atts[4].Length)
                {
                    case 2:
                        atts[4] = ($"{atts[4][1]} - {atts[4][0]}");
                        break;
                    case 3:
                        atts[4] = ($"{atts[4][2]} - {atts[4][0]}{atts[4][1]}");
                        break;
                    case 4:
                        atts[4] = ($"{atts[4][2]}{atts[4][3]} - {atts[4][0]}{atts[4][1]}");
                        break;
                }
                // Atts[5] is whether or not the dog is Hypoallergenic, this one had to be broken into a bool as it is a 'true or false' attribute.
                atts[5] = b.Substring((b.IndexOf(@"""hypoallergenic"":") + 17), 1);
                bool hypo;
                if (atts[5] == "t")
                {
                    hypo = true;
                } else
                {
                    hypo = false;
                }

                // This actually creates the new breed of dog, into the returnBreeds list.
                returnBreeds.Add(new Breeds(atts[0], atts[1], atts[2], atts[3], atts[4], hypo));
                
            }
        }

        // This function is used to search amongst all the dog breeds for only those with the user's desired stats.
        static public List<Breeds> AttSearch(List<Breeds> input, int minLife, int maxLife, int minWeight, int maxWeight, bool hypoallergenic)
        {
            List<Breeds> output = new List<Breeds>();
            foreach (Breeds b in input)
            {
                //Variables pool, compacted as a region for readability.
                #region
                int[] tempNums;
                int[] tempNums2;
                int minDogLife;
                int maxDogLife;
                int minDogWeight;
                int maxDogWeight;
                tempNums = BreakUp(b.Life);
                minDogLife = tempNums[0];
                maxDogLife = tempNums[1];
                tempNums = BreakUp(b.Male_Weight);
                tempNums2 = BreakUp(b.Female_Weight);
                minDogWeight = (tempNums[0] + tempNums2[0]) / 2;
                maxDogWeight = (tempNums[1] + tempNums2[1]) / 2;
                #endregion

                // Sorts through each of the traits and adds the dog to the list if it matches, otherwise does nothing.
                if (minDogLife > minLife && maxDogLife < maxLife)
                {
                    if (minDogWeight > minWeight && maxDogWeight < maxWeight)
                    {
                        if (hypoallergenic == false)
                        {
                            output.Add(b);
                        }
                        else if (hypoallergenic == true && b.Hypoallergenic == true)
                        {
                            output.Add(b);
                        }
                    }
                }
            }
            return output;

            // I found it easier to 'BreakUp' this BreakUp method for turning the strings into ints so that it can be used for several atts without having to retype everythign over and over again.
            static int[] BreakUp(string input) //Entry string should be formatted as "1 - 2", "1 - 23", or "12 - 34"
            {
                int[] output = new int[2];
                output[0] = int.Parse(input.Substring(0, input.IndexOf(" - ")));
                output[1] = int.Parse(input.Substring(input.IndexOf(" - ") + 3));
                return output;

            }
        }

    }

}
