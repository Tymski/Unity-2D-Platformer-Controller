using UnityEngine;

namespace PC2D
{
    public class RestrictedArea : SpriteDebug
    {
        public enum TriggerAction
        {
            DoNothing,
            EnableRestrictedArea,
            EnableRestrictedAreaIfFreemode,
            DisableRestrictedArea,
            DisableRestrictedAreaIfFreemode
        }

        public TriggerAction RestrictedAreaOnEnter = TriggerAction.DoNothing;
        public TriggerAction RestrictedAreaOnExit = TriggerAction.DoNothing;
        public TriggerAction RestrictedAreaOnStay = TriggerAction.DoNothing;

        public bool exitFreeModeOnEnter;
        public bool exitFreeModeOnExit;

        public void DoAction(PC2DMotor motor, TriggerAction action, bool exitFreeMode)
        {
            switch (action)
            {
                case TriggerAction.EnableRestrictedAreaIfFreemode:
                    if (motor.motorState == PC2DMotor.MotorState.FreedomState)
                    {
                        motor.EnableRestrictedArea();
                    }
                    break;
                case TriggerAction.EnableRestrictedArea:
                    motor.EnableRestrictedArea();
                    break;
                case TriggerAction.DisableRestrictedAreaIfFreemode:
                    if (motor.motorState == PC2DMotor.MotorState.FreedomState)
                    {
                        motor.DisableRestrictedArea();
                    }
                    break;
                case TriggerAction.DisableRestrictedArea:
                    motor.DisableRestrictedArea();
                    break;
            }

            if (exitFreeMode)
            {
                if (motor.motorState == PC2DMotor.MotorState.FreedomState)
                {
                    motor.FreedomStateExit();
                }
            }
        }

        public override void OnTriggerEnter2D(Collider2D o)
        {
            base.OnTriggerEnter2D(o);

			PC2DMotor motor = o.GetComponent<PC2DMotor>();
            if (motor)
            {
                DoAction(motor, RestrictedAreaOnEnter, exitFreeModeOnEnter);
            }
        }

        public override void OnTriggerStay2D(Collider2D o)
        {
            base.OnTriggerEnter2D(o);

			PC2DMotor motor = o.GetComponent<PC2DMotor>();

            if (motor)
            {
                DoAction(motor, RestrictedAreaOnStay, false);
            }
        }

        public override void OnTriggerExit2D(Collider2D o)
        {
            base.OnTriggerExit2D(o);

			PC2DMotor motor = o.GetComponent<PC2DMotor>();

            if (motor)
            {
                DoAction(motor, RestrictedAreaOnExit, exitFreeModeOnExit);
            }
        }
    }
}
