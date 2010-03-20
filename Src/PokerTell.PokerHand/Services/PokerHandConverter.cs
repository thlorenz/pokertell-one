namespace PokerTell.PokerHand.Services
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Interfaces;

    using log4net;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Base;

    /// <summary>
    /// Static class which provides methos to convert a Hand with absolute Action Ratios
    /// into a hand with relative Action Ratios
    /// </summary>
    public class PokerHandConverter : IPokerHandConverter
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IConstructor<IConvertedPokerHand> _convertedHandMake;

        readonly IConstructor<IConvertedPokerPlayer> _convertedPlayerMake;

        readonly IPokerRoundsConverter _pokerRoundsConverter;

        public PokerHandConverter(
            IConstructor<IConvertedPokerPlayer> convertedPlayerMake, 
            IConstructor<IConvertedPokerHand> convertedHandMake, 
            IPokerRoundsConverter pokerRoundsConverter)
        {
            _convertedPlayerMake = convertedPlayerMake;
            _convertedHandMake = convertedHandMake;
            _pokerRoundsConverter = pokerRoundsConverter;
        }

        /// <summary>
        /// Converts the given Hand with absolute ratios into a Hand with relative ratios
        /// It assumes all Players of the Hand were sorted previously
        /// </summary>
        /// <param name="sortedAquiredHand">Hand to be converted</param>
        /// <returns>Converted Hand</returns>
        public IConvertedPokerHand ConvertAquiredHand(IAquiredPokerHand sortedAquiredHand)
        {
            if (sortedAquiredHand == null)
            {
                Log.Debug("AquiredHand was null, returning null");
                return null;
            }

            if (sortedAquiredHand.TotalPlayers < 2 || sortedAquiredHand.TotalPlayers > 10)
            {
                Log.DebugFormat("AquiredHand had {0} players.\n<{1}>", sortedAquiredHand.TotalPlayers, sortedAquiredHand);
                return null;
            }

            // At this point Players are already sorted according to their Positions
            // Now parse through the hand and create the relative actions
            // Start w/ SB except for Preflop
            try
            {
                // First we just need to call the big blind
                double toCall = sortedAquiredHand.BB;

                double theoreticalStartingPot = sortedAquiredHand.BB + sortedAquiredHand.SB +
                                                (sortedAquiredHand.Ante * sortedAquiredHand.TotalPlayers);

                // This could be different from the theoretical starting pot if a player
                // posted out of line (like from middle position) -> the actual pot at the beginning
                // of the hand will be bigger than the theoretical pot
                double pot = RemovePostingActionsAndCalculatePotAfterPosting(ref sortedAquiredHand);

                // PokerOffice sometimes didn't store posting Actions
                // This will ignore Ante though
                if (pot <= 0)
                {
                    pot = theoreticalStartingPot;
                }

                IConvertedPokerHand convertedHand =
                    _convertedHandMake.New
                        .InitializeWith(sortedAquiredHand)
                        .AddPlayersFrom(sortedAquiredHand, theoreticalStartingPot, _convertedPlayerMake);

                convertedHand =
                    _pokerRoundsConverter
                        .InitializeWith(sortedAquiredHand, convertedHand, pot, toCall)
                        .ConvertPreflop()
                        .ConvertFlopTurnAndRiver();

                convertedHand
                    .RemoveInactivePlayers()
                    .SetNumOfPlayersInEachRound()
                    .SetWhoHasPositionInEachRound();

                foreach (var player in convertedHand)
                {
                    player.SetActionSequencesAndBetSizeKeysFromSequenceStrings();
                }

                return convertedHand;
            }
            catch (Exception excep)
            {
                Log.Error("Unhandled", excep);
                return null;
            }
        }

        /// <summary>
        /// Converts the given Hands with absolute ratios into Hands with relative ratios
        /// This is done by replaying the hand and determining the relation of each action ratio
        /// to the pot or the amount that needed to be called
        /// </summary>
        /// <param name="sortedAquiredHands">Hands to be converted</param>
        /// <returns>Converted Hands</returns>
        public IPokerHands ConvertAquiredHands(IPokerHands sortedAquiredHands)
        {
            var convertedHands = new PokerHands();

            for (int i = 0; i < sortedAquiredHands.Hands.Count; i++)
            {
                IConvertedPokerHand convertedHand = ConvertAquiredHand((IAquiredPokerHand)sortedAquiredHands.Hands[i]);

                // Only add hands with active players
                if (convertedHand != null)
                {
                    convertedHands.AddHand(convertedHand);
                }
            }

            return convertedHands;
        }

        /// <summary>
        /// Remove Posting and set BettingRound to null if no actions were left
        /// Antes and Blinds will be added to pot during removal
        /// </summary>
        /// <param name="aquiredHand">PokerHand</param>
        /// <returns>The pot at the beginning of the hand - containing blinds and antes</returns>
        protected virtual double RemovePostingActionsAndCalculatePotAfterPosting(ref IAquiredPokerHand aquiredHand)
        {
            double sumOfAllPostingAmounts = 0;
            var playersToRemove = new List<IAquiredPokerPlayer>();

            foreach (IAquiredPokerPlayer aquiredPlayer in aquiredHand)
            {
                // Does player have a round?
                if (aquiredPlayer.Count > (int)Streets.PreFlop)
                {
                    IAquiredPokerRound preflopRound = aquiredPlayer[Streets.PreFlop];

                    if (NoActionsLeftAfterRemovingPostingActionsFromRound(ref preflopRound, ref sumOfAllPostingAmounts))
                    {
                        // Must have been a sitout, otherwise he's have at least a preflop fold left
                        playersToRemove.Add(aquiredPlayer);
                    }
                }
                else
                {
                    // if player has no round, remove him b/c there is nothing he contributed to the hand
                    // not even a fold (so he was probably sitting out)
                    playersToRemove.Add(aquiredPlayer);
                }
            }

            foreach (IAquiredPokerPlayer aquiredPlayer in playersToRemove)
            {
                aquiredHand.RemovePlayer(aquiredPlayer);
            }

            return sumOfAllPostingAmounts;
        }

        /// <summary>
        /// Removes all posting actions (antes and blinds) while adding their values to the pot
        /// This is done, because posting is involuntary and doesn't say anything about a player's playing style
        /// </summary>
        /// <param name="aquiredPokerRound">The Poker round from which to remove posting actions</param>
        /// <param name="potAfterPosting">The pot, that the posted amounts will be added to</param>
        /// <returns></returns>
        static bool NoActionsLeftAfterRemovingPostingActionsFromRound(
            ref IAquiredPokerRound aquiredPokerRound, ref double potAfterPosting)
        {
            try
            {
                // if no action in round initiate Player removal
                if (aquiredPokerRound.Actions.Count < 1)
                {
                    return true;
                }

                // remove first actions in round as long as they are posting actions
                while (aquiredPokerRound[0].What == ActionTypes.P)
                {
                    // Update pot
                    potAfterPosting += aquiredPokerRound[0].Ratio;

                    // Remove
                    aquiredPokerRound.RemoveAction(0);

                    // If no action left initiate Player removal
                    if (aquiredPokerRound.Actions.Count < 1)
                    {
                        return true;
                    }
                }
            }
            catch (Exception excep)
            {
                excep.Data.Add("Round: ", aquiredPokerRound.ToString());
                Log.Error("Unhandled", excep);
            }

            return false;
        }
    }
}