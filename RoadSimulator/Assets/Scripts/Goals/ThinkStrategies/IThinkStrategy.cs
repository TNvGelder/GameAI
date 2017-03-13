namespace Assets.Scripts.Goals.ThinkStrategies
{
    public interface IThinkStrategy
    {
        void Evaluate(MovingEntity Owner, Think think);
    }
}