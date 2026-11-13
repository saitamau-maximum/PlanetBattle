using UnityEngine;

public class Wall : Structure
{
    protected override ExecutionStatus Execute()
    {
        return ExecutionStatus.Success;
    }
}
