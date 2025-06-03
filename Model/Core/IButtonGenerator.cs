using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Core
{
    public interface IButtonGenerator
    {
        double GetSpeedMultiplier(int score);
        IMusicalButton GenerateRandomButton(int score);
    }
}
