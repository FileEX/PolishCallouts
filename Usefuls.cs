using Rage;

namespace PolishCallouts
{
    internal static class Usefuls
    {
        internal static int getNearestLocationIndex(Vector4[] spawnPoints)
        {
            int index = 0;
            Vector3 playerPosition = Game.LocalPlayer.Character.Position;

            float dist = Vector3.Distance(playerPosition, new Vector3(spawnPoints[index].X, spawnPoints[index].Y, spawnPoints[index].Z));

            for (int i = 1; i < spawnPoints.Length; i++)
            {
                var newDist = Vector3.Distance(playerPosition, new Vector3(spawnPoints[i].X, spawnPoints[i].Y, spawnPoints[i].Z));
                if (newDist <= dist)
                {
                    dist = newDist;
                    index = i;
                }
            }

            return index;
        }

        internal static int getNearestLocationIndex(Vector3[] spawnPoints)
        {
            int index = 0;
            Vector3 playerPosition = Game.LocalPlayer.Character.Position;

            float dist = Vector3.Distance(playerPosition, spawnPoints[index]);

            for (int i = 1; i < spawnPoints.Length; i++)
            {
                var newDist = Vector3.Distance(playerPosition, spawnPoints[index]);
                if (newDist <= dist)
                {
                    dist = newDist;
                    index = i;
                }
            }

            return index;
        }
    }
}
