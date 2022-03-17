namespace ModelManagement.Controllers
{
    public interface IConnector
    {
        public void RemoveElementFromNavigator(long navigatorId, long elementId);
        public void AddElementToNavigator(long navigatorId, long elementId);
    }
}
