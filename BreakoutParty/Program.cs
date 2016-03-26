using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BreakoutPartyGame game = new BreakoutPartyGame())
                game.Run();
        }
    }
}
