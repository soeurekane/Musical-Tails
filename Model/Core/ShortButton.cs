using Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ShortButton.cs
public class ShortButton : MusicalButtonBase
{
    public override bool IsPressed { get; set; }
    public override int ScoreValue => 10;
    public override string ButtonColor => "Tiffany";
    public override bool ShouldPenalizeOnMiss => true; 
    public override string GetButtonType() => "Short";

    public override void PlaySound() => PlayBeep(600, 100);
}