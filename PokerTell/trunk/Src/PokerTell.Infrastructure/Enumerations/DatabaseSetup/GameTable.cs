//Date: 4/16/2009
namespace PokerTell.Infrastructure.Enumerations.DatabaseSetup
{
    /// <summary>
    /// Names of Columns for GameTable
    /// </summary>
    public enum GameTable
    {
        identity = 0, 
        sessionid, 
        gameid, 
        tournamentid, 
        tablename, 
        site, 
        bb, 
        sb, 
        timein, 
        totalplayers, 
        activeplayers, 
        inflop, 
        inturn, 
        inriver, 
        board, 
        sequence0, 
        sequence1, 
        sequence2, 
        sequence3
    }
}