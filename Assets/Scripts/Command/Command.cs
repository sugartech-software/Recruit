
public interface Command
{

    void DoExecute();
    void UndoExecute();
    void Release(bool executed);
}
