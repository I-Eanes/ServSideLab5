/*
 * Name: Izzy Eanes
 * Date: 2/20/2024
 * Purpose: Lab 5 for Server-Side Web Development
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    // Breeds is used for the different dog-breed objects that are pulled down from the Dog API
    internal class Breeds
    {
        // Most of these are string but written as ranges for easier storage (i.e. male_weight = "12 - 19")
        public string Name { get; set; }
        public string Description { get; set; }
        public string Life {  get; set; }
        public string Male_Weight { get; set; }
        public string Female_Weight { get; set; }
        public bool Hypoallergenic { get; set; }

        public Breeds(string name, string description, string life, string male_weight, string female_weight, bool hypoallergenic)
        {
            this.Name = name;
            this.Description = description;
            this.Life = life;
            this.Male_Weight = male_weight;
            this.Female_Weight = female_weight;
            this.Hypoallergenic = hypoallergenic;
        }

        public Breeds() 
        {
        }

        // This was useful for testing as it returns just the name of each breed.
        public string ToNames()
        {
            return Name;
        }


        // The ToString method to return all dog breed atts.
        public override string ToString()
        {
            return ($"{Name}\n     {Description}\n     Lifespan: {Life}\n     Male Weight: {Male_Weight}\n     Female Weight: {Female_Weight}\n     Hypoallergenic: {Hypoallergenic}\n");
        }

    }

}
