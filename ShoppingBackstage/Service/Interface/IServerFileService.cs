using Shopping.lib.ViewModels;

namespace ShoppingBackstage.Service.Interface;

public interface IServerFileService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    void Save(ServerFileViewModel file);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    ServerFileViewModel? Get(Guid id);
}