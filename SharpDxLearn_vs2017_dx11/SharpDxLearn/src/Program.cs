﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.IO;

class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine("test");
        //Console.ReadLine();
        //Console.WriteLine(System.IO.Directory.GetCurrentDirectory());

//         using(Game game = new Game())
//         {
//             game.Run();
//         }

        using(TestDX2 test = new TestDX2())
        {
            test.Run();
        }
    }
}

