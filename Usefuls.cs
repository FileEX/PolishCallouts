using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishCallouts.Usefuls
{
    public static class Usefuls
    {
        public static int getNearestLocationIndex(Vector4[] spawnPoints)
        {
            int loc = 0;
            Vector3 playerPosition = Game.LocalPlayer.Character.Position;

            float dist = Vector3.Distance(playerPosition, new Vector3(spawnPoints[loc].X, spawnPoints[loc].Y, spawnPoints[loc].Z));

            for (int i = 1; i < spawnPoints.Length; i++)
            {
                var newDist = Vector3.Distance(playerPosition, new Vector3(spawnPoints[i].X, spawnPoints[i].Y, spawnPoints[i].Z));
                if (newDist <= dist)
                {
                    dist = newDist;
                    loc = i;
                }
            }

            return loc;
        }
    }
}
