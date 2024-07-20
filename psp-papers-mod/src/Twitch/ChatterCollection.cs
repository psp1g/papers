using System;
using System.Linq;
using System.Collections.Generic;

namespace psp_papers_mod.Twitch {

    public class ChatterCollection : Dictionary<string, Chatter> {

        public void CheckChatExpiry() {
            if (this.Count == 0) return;
            double now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            foreach (KeyValuePair<string, Chatter> chatter in this) chatter.Value.FlushExpired(now);
        }

        public Chatter GetRandomChatter(bool attacker = false) {
            if (TwitchIntegration.ForcedActiveQueue.Count > 0)
                return TwitchIntegration.ForcedActiveQueue.Dequeue();

            if (this.Count == 0) return null;

            List<int> weights = this
                .Select(data => {
                    Chatter chatter = data.Value;
                    return chatter.GetWeight(attacker);
                })
                .ToList();

            List<int> cumulativeWeights = weights
                .Select((_, idx) => weights.Take(idx + 1).Sum())
                .ToList();

            int maxCumulativeWeight = cumulativeWeights.Last();
            int random = (int)(maxCumulativeWeight * PapersPSP.Random.NextDouble());

            int i = 0;

            foreach (KeyValuePair<string, Chatter> o in this) {
                if (cumulativeWeights[i++] < random) continue;
                Chatter chatter = o.Value;
                return chatter;
            }

            return null;
        }

    }

}