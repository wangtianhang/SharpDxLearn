using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine("test");
        //Console.ReadLine();
        using(Game game = new Game())
        {
            game.Run();
        }
    }
}

