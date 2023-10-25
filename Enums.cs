public enum ServerPackets
{
    SWelcomeMessage = 1,
    SInstantiatePlayer,
    SAlertMsg,
    SLoginOK,
    SPlayerData,
    SStationFree,
    SStationOccupied,
    STakeStation,
    SRemoveStation,
    SSubPositions,
    SScenarioStart,
}

public enum ClientPackets
{
    CHelloServer = 1,
    CNewAccount,
    CLogin,
    CCheckStation,
    COccupateStation,
    CLeaveStation,
    CSubPosition,
    CStartScenario,
}

public enum RoomState
{
    Open = 1,
    Searching,
    Closed
}
