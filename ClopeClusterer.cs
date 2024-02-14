using System;
using System.Collections.Generic;

namespace CLOPE_Clustering
{
    public class ClopeClusterer<T> where T : IEquatable<T>
    {
        private IEnumerable<T> transactions;
        private int numClusters;
        private double minSupport;
        public ClopeClusterer(IEnumerable<T> transactions, int numClusters, double minSupport)
        {
            this.transactions = transactions;
            this.numClusters = numClusters;
            this.minSupport = minSupport;
        }
        public List<Cluster<T>> Cluster()
        {
            List<Cluster<T>> clusters = InitializeClusters();
            bool moved;
            do
            {
                moved = false;
                foreach (T currentItem in transactions)
                {
                    int currentClusterIndex = FindBestClusterIndex(currentItem, clusters);
                    if (currentClusterIndex != -1)
                    {
                        clusters[currentClusterIndex].Items.Add(currentItem);
                        moved = true;
                    }
                }
                RemoveEmptyClusters(clusters);
            } while (moved);
            return clusters;
        }
        private List<Cluster<T>> InitializeClusters()
        {
            List<Cluster<T>> clusters = new List<Cluster<T>>();
            foreach (T item in transactions)
            {
                int bestClusterIndex = FindBestClusterIndex(item, clusters);
                if (bestClusterIndex != -1)
                {
                    clusters[bestClusterIndex].Items.Add(item);
                }
                else
                {
                    Cluster<T> newCluster = new Cluster<T>();
                    newCluster.Items.Add(item);
                    clusters.Add(newCluster);
                }
            }
            return clusters;
        }
        private int FindBestClusterIndex(T item, List<Cluster<T>> clusters)
        {
            double maxProfit = double.MinValue;
            int bestClusterIndex = -1;

            foreach (Cluster<T> cluster in clusters)
            {
                double profit = DeltaAdd(cluster, item);
                if (profit > maxProfit)
                {
                    maxProfit = profit;
                    bestClusterIndex = clusters.IndexOf(cluster);
                }
            }
            return bestClusterIndex;
        }
        private void RemoveEmptyClusters(List<Cluster<T>> clusters)
        {
            clusters.RemoveAll(cluster => cluster.Items.Count == 0);
        }
        private double DeltaAdd(Cluster<T> cluster, T item)
        {
            int itemCount = 0;
            double Snew = cluster.Items.Count + itemCount;
            double Wnew = cluster.W;
            foreach (var t in cluster.Items)
            {
                if (!cluster.Occ.ContainsKey(t))
                {
                    Wnew += 1;
                }
            }
            return Snew * (cluster.N + 1) / (Wnew * minSupport) - cluster.Items.Count * cluster.N / (cluster.W * minSupport);
        }
    }
}
