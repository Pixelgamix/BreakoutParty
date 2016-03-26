using FarseerPhysics.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakoutParty.Entities
{
    /// <summary>
    /// Collisiongroups.
    /// </summary>
    static class CollisionGroups
    {
        public const Category None = Category.None;
        public const Category Ball = Category.Cat1;
        public const Category Paddle = Category.Cat2;
        public const Category Block = Category.Cat3;
    }
}
