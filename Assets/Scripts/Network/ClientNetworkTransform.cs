public class ClientNetworkTransform : Unity.Netcode.Components.NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}