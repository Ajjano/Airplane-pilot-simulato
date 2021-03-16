using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace examen_c_sharp
{
    class Program
    {
        static void Main(string[] args)
        {
            string newGame="";
            do
            {
                Airline plane = new Airline();
                plane.Flight();
                WriteLine("Начать с начала? (+,-) ");
                newGame = ReadLine();
            } while (newGame == "+");
        }
            
    }
}
