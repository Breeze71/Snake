using System;



namespace V.Event
{
    /// <summary>
    /// 玩家狀態 Event
    /// </summary>
    public class PlayerEvent
    {
        public event Action OnPlayerDead;
        public void OnPlayerDeadEvent()
        {
            OnPlayerDead?.Invoke();
        }  
    }
}
