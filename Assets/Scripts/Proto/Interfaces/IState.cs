

public interface IState
{
    //prepare the state
    void Enter(Foe parent);
    //this is for taking actions
    void Update();
    //changing the state
    void Exit();
}
