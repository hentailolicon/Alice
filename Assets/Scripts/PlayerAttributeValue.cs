using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alice
{
    public class PlayerAttributeValue
    {
        public float damage;
        public float speed;
        public float HP;
        public float HPMax;
        public float luck;

        public PlayerAttributeValue()
        {
            Reset();
        }
        public void Reset()
        {
            damage = GameManager.instance.GetPlayerAttributeValue(GameManager.PlayerAttribute.DAMAGE);
        }
    }
}
