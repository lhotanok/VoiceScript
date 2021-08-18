﻿using System;

namespace VoiceScript.DiagramModel
{
    class DeleteCommand : Command
    {
        public DeleteCommand(string targetType, string targetName) : base(targetType, targetName) { }

        protected override void ProcessCommand(CommandContext context)
        {
            while (context.CurrentComponent != null)
            {
                if (context.CurrentComponent.TryDeleteChild(targetType, targetValue))
                {
                    context.TargetComponent = context.CurrentComponent;
                    return;
                }
                else
                {
                    context.CurrentComponent = context.CurrentComponent.Parent;
                }
            }

            throw new InvalidOperationException("Component can not be deleted. It does not exists in the current context.");
        }
    }
}
