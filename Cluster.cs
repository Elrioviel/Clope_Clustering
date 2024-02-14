using System;
using System.Collections.Generic;

namespace CLOPE_Clustering
{
    public class Cluster<T>
    {
        public List<T> Items { get; set; }
        public Dictionary<T, int> Occ { get; } = new Dictionary<T, int>();
        public int W => Occ.Count;
        public int N => Items.Count;
        public Cluster()
        {
            Items = new List<T>();
            foreach (var item in Items)
            {
                if (!Occ.ContainsKey(item))
                {
                    Occ[item] = 0;
                }
                Occ[item]++;
            }
        }
    }
}
