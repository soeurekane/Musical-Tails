using System;

namespace Model.Core
{
    public class TrapButton : MusicalButtonBase
    {
        public override bool IsPressed { get; set; }
        public override int ScoreValue => -50; // Штрафные очки
        public override string ButtonColor => "#FF6464"; // Красный цвет
        public override bool ShouldPenalizeOnMiss => false;

        public override void PlaySound()
        {
            PlayBeep(200, 500); 
        }

        public override string GetButtonType() => "Trap";
    }
}