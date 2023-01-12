using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Item;
using RPGOnline.Application.DTOs.Responses.Asset.Item;

namespace RPGOnline.Application.Interfaces
{
    public interface IItem
    {
        Task<ICollection<GetItemSimplifiedResponse>> GetItemsForCharacter(int uId, GetAssetForCharacterRequest getItemRequest);
        Task<(ICollection<GetItemResponse>, int pageCount)> GetItems(SearchAssetRequest searchRaceRequest, CancellationToken cancellationToken);
        Task<GetItemResponse> PostItem(PostItemRequest postItemRequest);

    }
}
