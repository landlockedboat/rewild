public class PlayerModel : HumanoidModel
{
    // Manual Singleton
    private static PlayerModel _instance;
    public static PlayerModel Instance => SingletonHelper.GetInstance(ref _instance);
    
    public int Money = 9999;
}