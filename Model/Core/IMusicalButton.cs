using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Core
{
    public interface IMusicalButton
    {
        void PlaySound();
        bool IsPressed { get; set; }
        int ScoreValue { get; }
        string GetButtonType();
    }
}