﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AF.AutomationTest.MatchingEngine
{
    internal class MatchingHandler
    {
        internal Dictionary<string, Record> Trades { get; }

        internal Dictionary<(string trade, string counterTrade), (Record trade, Record counterTrade)> Matches { get; }

        internal MatchingHandler()
        {
            Trades = new Dictionary<string, Record>();
            Matches = new Dictionary<(string trade, string counterTrade), (Record trade, Record counterTrade)>();
        }

        internal void TryMatch(string tradeId)
        {
            if (!Trades.ContainsKey(tradeId))
            {
                Trace.WriteLine($"Trade with ID {tradeId} not found");
                return;
            }

            var tradeToMatch = Trades[tradeId];

            foreach (var counterTrade in Trades.Values.Where(trade => trade.Id != tradeId))
            {
                var result = IsMatch(tradeToMatch, counterTrade);
                if (result)
                {
                    return;
                }
            }

            Trace.WriteLine($"Could not find match for {tradeToMatch} ");
        }

        private bool IsMatch(Record trade, Record counterTrade)
        {
            const int quantityTolerance = 5;

            if (trade.Symbol.ToLower() == counterTrade.Symbol.ToLower() && Math.Abs(trade.Quantity - counterTrade.Quantity) <= quantityTolerance &&
                trade.SettlementDate.Date == counterTrade.SettlementDate.Date && trade.Price == counterTrade.Price &&
                trade.Side != counterTrade.Side)
            {
                if (!Matches.ContainsKey((trade.Id, counterTrade.Id)))
                {
                    Matches.Add((trade.Id, counterTrade.Id), (trade, counterTrade));
                    Trace.WriteLine($"match found!! tradeId: {trade.Id}, counterTradeId: {counterTrade.Id}");
                    return true;
                }
            }

            return false;
        }
    }
}

