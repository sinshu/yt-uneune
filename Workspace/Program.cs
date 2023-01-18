using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await AnimalCrossing.Download.DoMultiple(100, 10);
    }
}
