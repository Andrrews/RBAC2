public interface IClaimDictionaryService
{
    Task<ClaimDictionaryModel> CreateClaimAsync(ClaimDictionaryModel claimModel);
    Task<ClaimDictionaryModel> GetClaimByIdAsync(int claimId);
    Task<IEnumerable<ClaimDictionaryModel>> GetAllClaimsAsync();
    Task<ClaimDictionaryModel> UpdateClaimAsync(ClaimDictionaryModel claimModel);
    Task<bool> DeleteClaimAsync(int claimId);
}
