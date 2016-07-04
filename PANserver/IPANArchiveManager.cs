namespace PANserver
{
    public interface IPANArchiveManager
    {
        void AddPanAndMask(string PAN, string mask);
        string SearchMask(string PAN);
        string SearchPAN(string mask);
    }
}