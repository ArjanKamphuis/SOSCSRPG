﻿namespace Engine.Models
{
    public class QuestStatus
    {
        public Quest PlayerQuest { get; set; }
        public bool IsCompleted { get; set; } = false;

        public QuestStatus(Quest quest)
        {
            PlayerQuest = quest;
        }
    }
}
