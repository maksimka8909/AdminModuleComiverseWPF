using RestSharp;

namespace AdminPanelComiverse.Classes;

public class ApiBuilder
{
    private static string rootUrl = "https://makachuka.xyz/api/";
    private static RestClient restClient;
    public static RestClient GetInstance()
    {
        if (restClient == null)
        {
            restClient = new RestClient(rootUrl);
        }
        return restClient;
    }
}