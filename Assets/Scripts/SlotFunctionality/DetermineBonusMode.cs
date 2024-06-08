namespace SlotFunctionality
{
    public static class DetermineBonusMode
    {
        public static BonusMode GetBonusMode(string matchName)
        {
            return matchName switch
            {
                "Symbol_7" => BonusMode.BonusBattle,
                _ => BonusMode.None
            };
        }
    }
}
