using Engine.Models;
using System;

namespace Engine.Actions
{
    public abstract class BaseAction : IAction
    {
        public event EventHandler<string> OnActionPerformed;

        protected readonly GameItem _itemInUse;

        protected BaseAction(GameItem itemInUse)
        {
            _itemInUse = itemInUse;
        }

        public abstract void Execute(LivingEntity actor, LivingEntity target);

        protected void ReportResult(string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}
