using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Assets.Code.Extensions
{
    public static class ListExtensions
    {
        private readonly static Random Rand = new Random();

        public static T GetRandomItem<T>(this List<T> input)
        {
            if (input.Count == 0)
                Debug.Log("WARNING! GetRandomItem called with an empty list!");

            var index = (int)(Rand.NextDouble() * input.Count);
            return input[index > input.Count - 1 ? input.Count - 1 : index];
        }
    }
}
