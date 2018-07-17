using UnityEngine;

namespace PC2D
{
    public class LadderZone : SpriteDebug
    {
        public PC2DMotor.LadderZone zone;

        public override void OnTriggerEnter2D(Collider2D o)
        {
            base.OnTriggerEnter2D(o);

			PC2DMotor motor = o.GetComponent<PC2DMotor>();
            if (motor)
            {
                motor.SetLadderZone(zone);
            }
        }
        public override void OnTriggerStay2D(Collider2D o)
        {
            base.OnTriggerStay2D(o);

			PC2DMotor motor = o.GetComponent<PC2DMotor>();
            if (motor)
            {
                motor.SetLadderZone(zone);
            }
        }

    }
}
