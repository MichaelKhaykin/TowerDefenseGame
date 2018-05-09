using Microsoft.Xna.Framework;

namespace TowerDefenseGameWillFinishThisOne
{
    public class Angle
    {
        public float value;

        public Angle(float value)
        {
            this.value = value;
        }

        public static implicit operator float(Angle angle)
         => angle.value;

        public static implicit operator Angle(float angle)
         => new Angle(angle);

        public static Angle operator -(Angle lhs, Angle rhs)
         => new Angle(lhs.value - rhs.value);

        public float ToDegrees()
         => MathHelper.ToDegrees(value);

    }
}