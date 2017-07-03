using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    interface IDamagable
    {
        //jesli to kula przeciwnej strony, obiekt implementujacy to zostanie zniszczony;
        //w przeciwnym wypadku nic sie nie stanie
        bool DealDamage(EOwner owner);
    }
}
